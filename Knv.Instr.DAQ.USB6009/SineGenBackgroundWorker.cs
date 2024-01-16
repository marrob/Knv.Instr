
namespace Knv.Instr.DAQ.USB6009
{
    using NationalInstruments.DAQmx;
    using System;
    using System.ComponentModel;
    using System.Threading;

    /// <summary>
    /// Ez a megoldás BackgorundWorkert használ, amibe kiszámol egy hold tíme-ot aminek a minimális értéke 1ms.
    /// A frekvencia ezért 1/(1ms * Samples)  
    /// pl: 1/(1ms * 1) = 1000Hz
    /// pl: 1/(1ms * 10) = 100Hz
    /// pl: 1/(1ms * 100) = 10Hz
    /// pl: 1/(1ms * 1000) = 1Hz
    /// 
    /// Nem használható nagyfrekvencián , mivel az ütemezőnek az alap frissítési ideje 20ms és 100ms között változik... 
    /// </summary>
    sealed public class SineGenBackgroundWorker : IDisposable
    {
        string _resourceName;
        bool _disposed = false;
        BackgroundWorker _bw { get; set; }
        readonly AutoResetEvent _waitForDoneEvent;

        class BackroundWorkerArg
        {
            public readonly BackgroundWorker Worker;
            public readonly Task NiTask;
            public readonly AnalogSingleChannelWriter Writer;

            public double Offset { get; set; }
            public double Amplitude { get; set; }
            public double Frequency { get; set; }
            public int Samples { get; set; }

            public BackroundWorkerArg(BackgroundWorker worker, Task niTask, AnalogSingleChannelWriter writer, double offset, double amplitude, double freq, int samples)
            {
                Worker = worker;
                NiTask = niTask;
                Writer = writer;
                Offset = offset;
                Amplitude = amplitude;
                Frequency = freq;
                Samples = samples;
            }
        }

        public SineGenBackgroundWorker(string resourceName)
        {
            _resourceName = resourceName;
            _bw = new BackgroundWorker();
            _waitForDoneEvent = new AutoResetEvent(false);
            _bw.DoWork += DoWork;
        }

        private void DoWork(object sender, DoWorkEventArgs e)
        {
            long timestamp = DateTime.Now.Ticks;

            var arg = (e.Argument as BackroundWorkerArg);
            BackgroundWorker bw = arg.Worker;
            Task niTask = arg.NiTask;
            AnalogSingleChannelWriter writer = arg.Writer;
            double offset = arg.Offset;
            double amplitude = arg.Amplitude;
            double freq = arg.Frequency;
            int samples = arg.Samples;

            bw.WorkerReportsProgress = true;
            bw.WorkerSupportsCancellation = true;

            int counter = 0;
            int holdTimeMs = (int)(1 / freq * 1000) / samples;

            if (holdTimeMs == 0)
                Console.WriteLine("A holdTimeMs értéke 0, igy semmilyen befolyása nincs a jel frekveciájára...");
            try
            {
                while (true)
                {
                    if (niTask == null)
                        throw new ApplicationException("Ni Task Not running...");
                    double data = amplitude * Math.Sin(Math.PI / 180.0 * 1 / samples * 360 * (counter++ % samples)) + offset;
                    writer.WriteSingleSample(true, data);
                    if (counter > samples)
                        counter = 0;
                    Console.WriteLine($"Update Time {(DateTime.Now.Ticks - timestamp) / 10000}ms");
                    timestamp = DateTime.Now.Ticks;
                    Thread.Sleep(holdTimeMs);
                    if (bw.CancellationPending)
                    {
                        e.Cancel = true;
                        break;
                    }
                }

                writer.WriteSingleSample(true, 0);
                niTask.Stop();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _waitForDoneEvent.Set();
            }
        }

        /// <summary>
        ///Nagy mintaszámmal már rosszul mükdöik
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="offset"></param>
        /// <param name="amplitude"></param>
        /// <param name="freq"></param>
        /// <param name="samples"></param>
        public void Start(string channel, double offset, double amplitude, double freq, int samples)
        {
            var niTask = new Task();
            string physicalChannel = $"{_resourceName}/{channel}";
            niTask.AOChannels.CreateVoltageChannel(physicalChannel, "aoChannel", 0, 5, AOVoltageUnits.Volts);
            var writer = new AnalogSingleChannelWriter(niTask.Stream);           
            _bw.RunWorkerAsync(new BackroundWorkerArg(_bw, niTask , writer, offset, amplitude, freq, samples));
        }

        public void Abort() 
        {
            if (_bw.IsBusy)
            {
                _bw.CancelAsync();
                _waitForDoneEvent.WaitOne();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                if (_bw.IsBusy)
                {
                    _bw.CancelAsync();
                    _waitForDoneEvent.WaitOne(100);
                }
            }
            _disposed = true;
        }
    }
}

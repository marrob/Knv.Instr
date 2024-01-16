
namespace Knv.Instr.DAQ.USB6009
{

    using NationalInstruments.DAQmx;
    using System;
    using System.Timers;


    /// <summary>
    /// Ez a megoldás a windows ütemezőjét használja
    /// 
    /// Nem használható nagyfrekvencián , mivel az ütemezőnek az alap frissítési ideje 20ms és 100ms között változik... 
    /// </summary>
    sealed public class SineGenSwTiming:IDisposable
    {

        string _resourceName;
        Task _myTask;
        AnalogSingleChannelWriter _writer;
        Timer _timer;
        int _counter = 0;
        bool _disposed = false;
        int _samples =  0;
        double _offset = 0;
        double _amplitude;

        public SineGenSwTiming(string resourceName)
        {
            _resourceName = resourceName;
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
            _myTask = new Task();
            string physicalChannel = $"{_resourceName}/{channel}";
            _myTask.AOChannels.CreateVoltageChannel(physicalChannel, "aoChannel", 0, 5, AOVoltageUnits.Volts);
            _writer = new AnalogSingleChannelWriter(_myTask.Stream);           
            _counter = 0;
            _samples = samples;
            _offset = offset;
            _amplitude = amplitude;


            /*
             * A timer ütemében nő a counter változó értéke.
             * A counter értékvel felosztunk egy teljes kört 360/samples részre
             * Ha frekvencia 50Hz, akkor a teljes kör periódusa: 20ms, ha ezt felosztjuk 100 részre, akkor 20ms/100 = 0.2ms
             * 
             * 2024.01.16-UnitTestből debug módban az interval 1ms esetén a tényleges firssítés 20ms volt úgy hogy az udpate nem csinált semmit.
             */
            _timer = new Timer();
            _timer.Interval = 100;//((1/freq) * 1000)/samples; //az interval ms-ben
             _timer.Elapsed += ValueUpdate;
            _timer.Start();
        }

        /// <summary>
        /// A 360-fokos kört a samples mennyiségű részre osztjuk.
        /// Az idő ütemében növljük a szöget
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        long timestamp = DateTime.Now.Ticks;
        private void ValueUpdate(object sender, ElapsedEventArgs e)
        {
            if (_myTask == null)
                return;
              double data = _amplitude * Math.Sin(Math.PI / 180.0 * 1/_samples * 360 * (_counter++ % _samples)) + _offset;                     
             _writer.WriteSingleSample(true, data);
            if(_counter > _samples)
                _counter = 0;
            Console.WriteLine($"Update Time {(DateTime.Now.Ticks - timestamp) / 10000}ms");
            timestamp = DateTime.Now.Ticks;
        }

        public void Stop() 
        {
            _timer.Stop();
            _timer.Elapsed -= ValueUpdate;
            _timer.Dispose();
            _writer.WriteSingleSample(true, 0);
            _myTask.Stop();
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
                _timer?.Dispose();
                _myTask?.Dispose();
            }
            _disposed = true;
        }
    }
}

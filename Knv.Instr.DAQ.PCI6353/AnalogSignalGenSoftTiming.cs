
namespace Knv.Instr.DAQ.PCI6353
{

    using NationalInstruments.DAQmx;
    using System;
    using System.Timers;


    /// <summary>
    /// Ez a megoldás a windows ütemezőjét használja
    /// </summary>
    sealed public class AnalogSignalGenSoftTiming:IDisposable
    {

        string _deviceName;
        Task _myTask;
        AnalogSingleChannelWriter _writer;
        Timer _timer;
        int _counter = 0;
        bool _disposed = false;
        int _samples =  0;
        double _amplitude;

        public AnalogSignalGenSoftTiming(string deviceName)
        {
            _deviceName = deviceName;

        }

        /// <summary>
        ///Nagy mintaszámmal már rosszul mükdöik
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="amplitude"></param>
        /// <param name="freq"></param>
        /// <param name="samples"></param>
        public void Start(string channel, double amplitude, double freq, int samples)
        {
            _myTask = new Task();
            string physicalChannel = $"{_deviceName}/{channel}";
            _myTask.AOChannels.CreateVoltageChannel(physicalChannel, "aoChannel", -10, 10, AOVoltageUnits.Volts);
            _writer = new AnalogSingleChannelWriter(_myTask.Stream);           
            _counter = 0;
            _samples = samples;
            _amplitude = amplitude;


            /*
             * A timer ütemében nő a counter változó értéke.
             * A counter értékvel felosztunk egy teljes kört 360/samples részre
             * 
             * Ha frekvencia 50Hz, akkor a teljes kör periódusa: 20ms, ha ezt felosztjuk 1000 részre, akkor 20ms/1000 = 
             * 
             */

            _timer = new Timer();
            _timer.Interval = (1/freq)/samples * 1000; //ms-ben várja
            _timer.Elapsed += ValueUpdate;
            _timer.Start();
        }

        /// <summary>
        /// A 360-fokos kört a samples mennyiségű részre osztjuk.
        /// Az idő ütemében növljük a szöget
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ValueUpdate(object sender, ElapsedEventArgs e)
        {
            if (_myTask == null)
                return;

              double data = _amplitude * Math.Sin(Math.PI / 180.0 * 1/_samples * 360 * (_counter++ % _samples));                     
             _writer.WriteSingleSample(true, data);
            if(_counter > _samples)
                _counter = 0;
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

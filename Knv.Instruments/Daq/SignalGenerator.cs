
namespace Knv.Instruments.Daq
{

    using NationalInstruments.DAQmx;
    using System;
    using System.CodeDom;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Timers;


    public class SignalGenerator:IDisposable
    {

        string _deviceName;
        Task _myTask;
        AnalogSingleChannelWriter _writer;
        Timer _timer;
        int _counter = 0;
        bool _disposed = false;

        public SignalGenerator(string deviceName)
        {
            _deviceName = deviceName;
            _timer = new Timer(100);
            _timer.Elapsed += ValueUpdate;
        }

        public void Start(string channel)
        {
            _myTask = new Task();
            string physicalChannel = $"{_deviceName}/{channel}";
            _myTask.AOChannels.CreateVoltageChannel(physicalChannel, "aoChannel", -10, 10, AOVoltageUnits.Volts);
            _writer = new AnalogSingleChannelWriter(_myTask.Stream);
            _timer.Interval = 50;
            _timer.Start();
            _counter = 0;
        }
        bool toogle;

        private void ValueUpdate(object sender, ElapsedEventArgs e)
        {
            if (_myTask == null)
                return;

            if (toogle)
            {
                _writer.WriteSingleSample(true, 1);
                toogle = false;
            }
            else
            {
                _writer.WriteSingleSample(true, -1);
                toogle = true;
            }

            //Ha ezerig elszámol az egy teljes kör
          //  double data = Math.Sin(Math.PI / 180.0 * 0.001 * 360 * ((_counter++) % 1000)); // Calculate sine wave (-1V to 1 V)                     
          //  _writer.WriteSingleSample(true, data);
        }

        public void Stop() 
        {
            _timer.Stop();
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
                _timer.Dispose();
                if(_myTask != null)
                   _myTask.Dispose();
            }
            _disposed = true;
        }
    }
}

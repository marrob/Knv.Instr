


namespace Knv.Instruments.Daq
{
    using NationalInstruments.DAQmx;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Daq
    {
        string _deviceName;

        Task _myTask;
        private AnalogSingleChannelWriter _writer;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceName"></param>
        /// <param name="isSim"></param>
        public Daq(string deviceName, bool isSim) 
        {
        

        
        }

        void AnalogOutputSetVoltage(string channel)
        { 
        

        }



    }
}

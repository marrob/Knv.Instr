

namespace Knv.Instr.DMM.PXI4082
{

    /* 
     *
     * Ivi.Driver.dll
     * C:\Program Files (x86)\IVI Foundation\IVI\Microsoft.NET\Framework32\v2.0.50727\IviFoundationSharedComponents 2.0.0\Ivi.Driver.dll
     * ..\lib\IviFoundationSharedComponents 2.0.0\Ivi.Driver.dll
     * 
     * Ivi.Dmm.dll
     * C:\Program Files (x86)\IVI Foundation\IVI\Microsoft.NET\Framework32\v2.0.50727\IviFoundationSharedComponents 2.0.0\Ivi.Dmm.dll
     * ..\lib\IviFoundationSharedComponents 2.0.0\Ivi.Dmm.dll
     *  
     * NationalInstruments.Common.dll
     * C:\Program Files (x86)\National Instruments\Measurement Studio\DotNET\v4.0\AnyCPU\NationalInstruments.Common 19.1.40\NationalInstruments.Common.dll
     * 
     * NationalInstruments.ModularInstruments.Common.dll
     * C:\Program Files (x86)\IVI Foundation\IVI\Microsoft.NET\Framework32\v4.5.50709\NationalInstruments.Common(x86) 19.1.40\NationalInstruments.ModularInstruments.Common.dll
     * ..\lib\NationalInstruments.Common(x86) 19.1.40\NationalInstruments.Common.dll
     * 
     * 
     * NationalInstruments.ModularInstruments.NIDmm.Fx45
     * C:\Program Files (x86)\IVI Foundation\IVI\Microsoft.NET\Framework32\v4.5.50709\NationalInstruments.ModularInstruments.NIDmm 21.3.0\NationalInstruments.ModularInstruments.NIDmm.Fx45.dll
     * ..\lib\NationalInstruments.ModularInstruments.NIDmm(x86) 22.5.0\NationalInstruments.ModularInstruments.NIDmm.Fx45.dll
     *
     */

    using NationalInstruments.ModularInstruments.NIDmm;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    public class PXI4082 : IDigitalMultiMeter, IDisposable
    {
        public Dictionary<string, string[]> Ranges = new Dictionary<string, string[]>()
        {
            { "DCV", new string[]{ "100mV", "1V", "10V", "100V", "300V" } },
            { "2WR", new string[]{ "100R", "1K00", "100K", "1M00", "10M0", "100M" } },
            { "4WR", new string[]{ "100R", "1K00", "100K", "1M00", "10M0", "100M" } },
            { "DCC", new string[]{ "20mA", "200mA", "1A" } },
            { "ACV", new string[]{ "50mV", "500mV", "5V", "50V", "300V" } },
            { "ACC", new string[]{ "10mA", "100mA", "1A" } },
            { "CAP", new string[]{ "300pF", "1nF", "10nF", "100nF", "1uF", "10uF", "1000uF", "10000UF" } },
        };

        readonly NIDmm _session = null;
        bool _disposed = false;
        readonly bool _simulation = false;

        /// <summary>
        /// PXI4082
        /// </summary>
        public PXI4082(string visaName, bool simulation)
        {
            _simulation = simulation;
            if(!_simulation)
                _session = new NIDmm(visaName, idQuery: true, resetDevice: true);
        }

        /// <summary>
        /// Configuration of the NI PXI-4082
        /// </summary>
        public void Config(string function, string rangeName, double digits, int powerlineFreq)
        {
            var func = DmmMeasurementFunction.ACVolts;
            double range = 0;
            string[] rangeItems;

            if(!Ranges.TryGetValue(function, out rangeItems))
                throw new ArgumentException($" The {function} is not supported. Supported functions: {string.Join(",", Ranges.Keys)}");

            if(!rangeItems.Contains(rangeName))
                throw new ArgumentException($" The {function} is not supported this range {rangeName}. Supported ranges: {string.Join(",", rangeItems)}");

            switch (function)
            {
                case "DCV":
                    {
                        func = DmmMeasurementFunction.DCVolts;
                        switch (rangeName)
                        {
                            case "100mV": range = 0.100; break;
                            case "1V": range = 1.0; break;
                            case "10V": range = 10.0; break;
                            case "100V": range = 100.0; break;
                            case "300V": range = 300.0; break;
                        }
                        break;
                    }
                case "2WR":
                    {
                        func = DmmMeasurementFunction.TwoWireResistance;
                        switch (rangeName)
                        {
                            case "100R": range = 100.0; break;
                            case "1K00": range = 1000.0; break;
                            case "100K": range = 100000.0; break;
                            case "1M00": range = 1000000.0; break;
                            case "10M0": range = 10000000.0; break;
                            case "100M": range = 100000000.0; break;
                        }
                        break;
                    }
                case "4WR":
                    {
                        func = DmmMeasurementFunction.FourWireResistance;
                        switch (rangeName)
                        {
                            case "100R": range = 100.0; break;
                            case "1K00": range = 1000.0; break;
                            case "100K": range = 100000.0; break;
                            case "1M00": range = 1000000.0; break;
                            case "10M0": range = 10000000.0; break;
                            case "100M": range = 100000000.0; break;
                        }
                        break;
                    }
                case "DCC":
                    {
                        func = DmmMeasurementFunction.DCCurrent;
                        switch (rangeName)
                        {
                            case "20mA": range = 0.02; break;
                            case "200mA": range = 0.200; break;
                            case "1A": range = 1.0; break;
                        }
                        break;
                    }
                case "ACV":
                    {
                        func = DmmMeasurementFunction.ACVolts;
                        switch (rangeName)
                        {
                            case "50mV": range = 0.05; break;
                            case "500mV": range = 0.500; break;
                            case "5V": range = 5.0; break;
                            case "50V": range = 50.0; break;
                            case "300V": range = 300.0; break;
                        }
                        break;
                    }
                case "ACC":
                    {
                        func = DmmMeasurementFunction.ACVolts;
                        switch (rangeName)
                        {
                            case "10mA": range = 0.01; break;
                            case "100mA": range = 0.100; break;
                            case "1A": range = 1.0; break;
                        }
                        break;
                    }
                case "CAP":
                    {
                        func = DmmMeasurementFunction.Capacitance;
                        switch (rangeName)
                        {
                            case "300pF":   range = 0.0000000003; break;
                            case "1nF":     range = 0.000000001; break;
                            case "10nF":    range = 0.00000001; break;
                            case "100nF":   range = 0.0000001; break;
                            case "1uF":     range = 0.000001; break;
                            case "10uF":    range = 0.00001; break;
                            case "1000uF":  range = 0.001; break;
                            case "10000uF": range = 0.01; break;
                        }
                        break;
                    }
            }

            if (!_simulation && _session != null)
            {
                _session.ConfigureMeasurementDigits(func, range, digits);
                _session.Advanced.PowerlineFrequency = powerlineFreq;
            }
        }

        public void Config(string function, string rangeName)
        {
            Config(function, rangeName, digits: 5.5, powerlineFreq: 50);
        }

        public void Config(string function, string rangeName, double digits )
        {
            Config(function, rangeName, digits: digits, powerlineFreq: 50);
        }

        public double Read()
        {
            if(!_simulation)
                return _session.Measurement.Read();
            else
                return new Random().NextDouble();
        }

        public string Identify()
        {
            return $"{_session.DriverIdentity.InstrumentManufacturer}, {_session.DriverIdentity.InstrumentModel}, {_session.DriverIdentity.SerialNumber} ";
        }

        public string GetErrors()
        {

            return "Not Supported.";
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
                _session?.Dispose();
            }
            _disposed = true;

        }

    }
}

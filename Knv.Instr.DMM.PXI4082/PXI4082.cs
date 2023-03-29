

namespace Knv.Instr.DMM.PXI4082
{

    /* Ivi.Dmm.dll
     * C:\Program Files (x86)\IVI Foundation\IVI\Microsoft.NET\Framework32\v2.0.50727\IviFoundationSharedComponents 1.4.1\Ivi.Dmm.dll
     * 
     * Ivi.Driver.dll
     * C:\Program Files (x86)\IVI Foundation\IVI\Microsoft.NET\Framework32\v2.0.50727\IviFoundationSharedComponents 1.4.1\Ivi.Driver.dll
     *   
     * NationalInstruments.Common.dll
     * C:\Program Files (x86)\National Instruments\Measurement Studio\DotNET\v4.0\AnyCPU\NationalInstruments.Common 19.1.40\NationalInstruments.Common.dll
     * 
     * NationalInstruments.ModularInstruments.Common.dll
     * C:\Program Files (x86)\IVI Foundation\IVI\Microsoft.NET\Framework32\v4.5.50709\NationalInstruments.ModularInstruments.Common 21.8.0\NationalInstruments.ModularInstruments.Common.dll
     * 
     * NationalInstruments.ModularInstruments.ModularInstrumentsSystem.dll
     * C:\Program Files (x86)\IVI Foundation\IVI\Microsoft.NET\Framework32\v4.5.50709\NationalInstruments.ModularInstruments.ModularInstrumentsSystem 1.4.45\NationalInstruments.ModularInstruments.ModularInstrumentsSystem.dll
     * 
     * NationalInstruments.ModularInstruments.NIDmm.Fx45
     * C:\Program Files (x86)\IVI Foundation\IVI\Microsoft.NET\Framework32\v4.5.50709\NationalInstruments.ModularInstruments.NIDmm 21.3.0\NationalInstruments.ModularInstruments.NIDmm.Fx45.dll
     * 
     * 
     * 
     */

    using NationalInstruments.ModularInstruments.NIDmm;
    using System;

    public class PXI4082 : IDigitalMultiMeter, IDisposable
    {

        bool _disposed = false;

        readonly NIDmm _session = null;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="visaName">J3_DMM</param>
        /// <param name="isSim"></param>
        public PXI4082(string visaName, bool isSim)
        {
            _session = new NIDmm(visaName, idQuery: true, resetDevice: true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="function">Supported function: DCV, DCC, 2WR, 4WR, CAP </param>
        /// <param name="range">DCV: 100mV  </param>
        /// <param name="resoultionDigits"> Supported:  </param>
        /// <param name="powerlineFreq">50Hz, 60Hz</param>
        /// <exception cref="ArgumentException"></exception>
        public void Config(string function, double range, double resoultionDigits, int powerlineFreq)
        {     
            var selectedFunction = DmmMeasurementFunction.ACVolts;

            switch (function)
            {
                case "DCV": selectedFunction = DmmMeasurementFunction.DCVolts; break;
                case "DCC": selectedFunction = DmmMeasurementFunction.DCCurrent; break;
                case "2WR": selectedFunction = DmmMeasurementFunction.TwoWireResistance; break;
                case "4WR": selectedFunction = DmmMeasurementFunction.FourWireResistance; break;
                case "CAP": selectedFunction = DmmMeasurementFunction.Capacitance; break;
                default: throw new ArgumentException("Nem támogatott funkció: Ezek támogatottak: DCV, DCC, 2WR, 4WR");
            }

            _session.ConfigureMeasurementDigits(selectedFunction, range, resoultionDigits);
            _session.Advanced.PowerlineFrequency = powerlineFreq;
        }

        public void Config(string function, double range)
        {
            Config(function, range, resoultionDigits: 5.5, powerlineFreq: 50);
        }

        public double Read()
        {
           var value = _session.Measurement.Read();
            return value;
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

/* 
 * 
 * Ivi.Driver.dll
 * C:\Program Files (x86)\IVI Foundation\IVI\Microsoft.NET\Framework32\v2.0.50727\IviFoundationSharedComponents 2.0.0\Ivi.Driver.dll
 * ..\lib\IviFoundationSharedComponents 2.0.0\Ivi.Driver.dll
 *  
 * Ivi.DCPwr
 * C:\Program Files (x86)\IVI Foundation\IVI\Microsoft.NET\Framework32\v2.0.50727\IviFoundationSharedComponents 2.0.0\Ivi.DCPwr.dll
 * ..\lib\IviFoundationSharedComponents 2.0.0\Ivi.DCPwr.dll
 *  
 * NationalInstruments.Common.dll
 * C:\Program Files (x86)\National Instruments\Measurement Studio\DotNET\v4.0\AnyCPU\NationalInstruments.Common 19.1.40\NationalInstruments.Common.dll
 * ..\lib\NationalInstruments.Common(x86) 19.1.40\NationalInstruments.Common.dll
 * 
 * NationalInstruments.ModularInstruments.Common.dll
 * C:\Program Files (x86)\IVI Foundation\IVI\Microsoft.NET\Framework32\v4.5.50709\NationalInstruments.ModularInstruments.Common 22.5.0\NationalInstruments.ModularInstruments.Common.dll
 * ..\lib\NationalInstruments.ModularInstruments.Common(x86) 22.5.0\NationalInstruments.ModularInstruments.Common.dll
 * 
 * NationalInstruments.ModularInstruments.ModularInstrumentsSystem.dll
 * C:\Program Files (x86)\IVI Foundation\IVI\Microsoft.NET\Framework32\v4.5.50709\NationalInstruments.ModularInstruments.ModularInstrumentsSystem 1.4.45\NationalInstruments.ModularInstruments.ModularInstrumentsSystem.dll
 * ..\lib\NationalInstruments.ModularInstruments.Common(x86) 22.5.0\NationalInstruments.ModularInstruments.Common.dll
 * 
 * NationalInstruments.ModularInstruments.NIDCPower.Fx45.dll
 * C:\Program Files (x86)\IVI Foundation\IVI\Microsoft.NET\Framework32\v4.5.50709\NationalInstruments.ModularInstruments.NIDCPower 19.0.0\NationalInstruments.ModularInstruments.NIDCPower.Fx45.dll
 * ..\lib\NationalInstruments.ModularInstruments.NIDCPower(x86) 19.0.0\
 * 
 * 
 */

using NationalInstruments.ModularInstruments.NIDCPower;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knv.Instr.SMU.PXI4139
{
    public class PXI4139 : ISourceMeasureUnits
    {

        readonly NIDCPower _session = null;
        bool _disposed = false;
        readonly bool _simulation = false;

        /// <summary>
        /// 
        /// </summary>
        public PXI4139(string visaName, bool simulation)
        {
            _simulation = simulation;
            if (!_simulation)
                _session = new NIDCPower(visaName, idQuery: true, resetDevice: true);
        }

        public string Identify()
        {
            return $"{_session.Identity.InstrumentManufacturer}, {_session.Identity.InstrumentModel}, {_session.Identity.Identifier} ";
        }


        public void Config()

        {

          //  DCPowerOutputMeasurement.SamplesToAverage
          //  _session.Outputs.
        
        
        }

        public double Measure(string channel)
        {
            if (!_simulation)
                return _session.Measurement.Measure(channel).VoltageMeasurements[0];
            else
                return new Random().NextDouble();
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

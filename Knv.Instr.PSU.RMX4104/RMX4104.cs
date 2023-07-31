/* 
 * .NET Framework: 4.8.04084
 * Visual Studio: 2022 Community(64 - bit) Version 17.2.6
 *  TestStnad: TestStand Version 2017 (17.0.0.184) 32 - bit
 *
 *---NI VISA-- -
 *NationalInstruments.Visa
 * C:\Program Files(x86)\IVI Foundation\VISA\Microsoft.NET\Framework32\v4.0.30319\NI VISA.NET 19.0\NationalInstruments.Visa.dll
 * C:\Program Files\IVI Foundation\VISA\Microsoft.NET\Framework64\v4.0.30319\NI VISA.NET 19.0\NationalInstruments.Visa.dll 
 * ..\lib\NationalInstruments.Visa(amd64) 4.0.30319\NationalInstruments.Visa.dll
 *
 *
 *
 * Ivi.Visa
 * C:\Program Files(x86)\IVI Foundation\VISA\Microsoft.NET\Framework32\v2.0.50727\VISA.NET Shared Components 5.8.0\Ivi.Visa.dll
 * C:\Program Files\IVI Foundation\VISA\Microsoft.NET\Framework64\v2.0.50727\VISA.NET Shared Components 5.11.0\Ivi.Visa.dll
 * ..\lib\Ivi.Visa(amd64) 2.0.50727\Ivi.Visa.dll
 */

namespace Knv.Instr.PSU.RMX4104
{
    using System;
    using System.Collections.Generic;
    using Ivi.Visa;
    using NationalInstruments.Visa;
    public class RMX4104 : IPowerSupply, IDisposable
    {

        bool _disposed = false;

        readonly ResourceManager _resourceManager;
        readonly MessageBasedSession _session;
        bool _simulation;

        /// <summary>
        /// RMX4104, 
        /// Megvalósítja az IPowerSupply-t.
        /// Használd hozzá az  class GenericPowerSupply-t
        /// 
        /// Ha nem volt kipacsolva va táp akkor itt kinn van a feszültsége a kimeneten
        /// 
        /// </summary>
        /// <param name="resourceName">J24_PPS_1,J24_PPS_2,J24_PPS_3</param>
        /// <param name="simulation"></param>
        public RMX4104(string resourceName, bool simulation)
        {
            _simulation = simulation;
            if (_simulation)
                return;
            _resourceManager = new ResourceManager();
            _session = (MessageBasedSession)_resourceManager.Open(resourceName);
            Write("*RST;");
            Write(":SYST:ERR:ENAB;");
        }

        public string Identify()
        {
            if (_simulation)
                return( "I am a simulated NI RMX4104"); 
            else
                return(Query("*IDN?;"));
        }

        public void SetOutput(double volt, double current)
        {
            if (_simulation)
                return;
            Write($":VOLT {volt:g}; :CURR {current:g};");
        }

        public void OnOff(bool enable)
        {
            if (_simulation)
                return;
            Write($":OUTP:STAT {(enable ? "ON" : "OFF")};");
        }

        public void SetOutput(double volt, double current, bool onOff)
        {
            if (_simulation)
                return;
            Write($":VOLT {volt:g};:CURR {current:g};:OUTP:STAT {(onOff ? "ON" : "OFF")};");
        }

        public double SetOutputGetActualVolt(double volt, double current)
        {
            string resp;
            if (_simulation)
                resp = Query($":VOLT {volt:g};:CURR {current:g};:MEAS:VOLT?;");
            else
                resp = volt.ToString();
            return double.Parse(resp, System.Globalization.CultureInfo.GetCultureInfo("en-US"));
        }

        public double GetActualVolt()
        {
            string resp;
            if (!_simulation)
                resp = Query(":MEAS:VOLT?;");
            else
                resp = "0";
            return double.Parse(resp, System.Globalization.CultureInfo.GetCultureInfo("en-US"));
        }

        public double GetActualCurrent()
        {
            string resp;
            if (!_simulation)
                resp = Query(":MEAS:CURR?");
            else
                resp = "0";
            return double.Parse(resp, System.Globalization.CultureInfo.GetCultureInfo("en-US"));
        }

        public List<string> GetErrors()
        {
            var errors = new List<string>();
            if (_simulation)
                return errors;

            for (int i = 0; i < 10; i++)
            {
                var resp = Query(":SYST:ERR?;");
                if (resp.ToUpper().Contains("NO ERROR"))
                    break;
                else
                    errors.Add(resp);
            }
            return errors;
        }

        public string Query(string request)
        {
            _session.RawIO.Write(request + "\n");
            var response = _session.RawIO.ReadString();
            return response; 
        }

        public void Write(string request)
        {
            _session.RawIO.Write(request + "\n");
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
                _resourceManager?.Dispose();
                _session?.Dispose();
            }
            _disposed = true;

        }

    }
}

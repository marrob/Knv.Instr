

namespace Knv.Instr.PSU.RMX4104
{

    using System;
    /* C:\Program Files\IVI Foundation\VISA\Microsoft.NET\Framework64\v2.0.50727\VISA.NET Shared Components 5.11.0\Ivi.Visa.dll*/
    using Ivi.Visa;
    /* C:\Program Files\IVI Foundation\VISA\Microsoft.NET\Framework64\v4.0.30319\NI VISA.NET 19.0\NationalInstruments.Visa.dll*/
    using NationalInstruments.Visa;
    public class RMX4104 : Log, IPowerSupply, IDisposable
    {

        bool _disposed = false;

        readonly ResourceManager _resourceManager;
        readonly MessageBasedSession _session;
        bool _isSim;

        /// <summary>
        /// RMX4104, 
        /// Megvalósítja az IPowerSupply-t.
        /// Használd hozzá az  class GenericPowerSupply-t
        /// 
        /// Ha nem volt kipacsolva va táp akkor itt kinn van a feszültsége a kimeneten
        /// 
        /// </summary>
        /// <param name="visaName">J24_PPS_1,J24_PPS_2,J24_PPS_3</param>
        /// <param name="isSim"></param>
        public RMX4104(string visaName, bool isSim)
        {
            _isSim = isSim;

            if (!_isSim)
            {
                _resourceManager = new ResourceManager();
                _session = (MessageBasedSession)_resourceManager.Open(visaName);
                LogWriteLine("Instance created.");

                Write("*RST;");
                Write(":SYST:ERR:ENAB;");
            }
        }

        public string Identify()
        {
            string resp;
            if (!_isSim)
                resp = Query("*IDN?");
            else
                resp = "I am a simulated NI RMX4104";
            return resp;
        }

        public void SetOutput(double volt, double current)
        {
            if (!_isSim)
                Write($":VOLT {volt:g}; :CURR {current:g};");
        }

        public void SetOutput(double volt, double current, bool onOff)
        {
            if (!_isSim)
                Write($":VOLT {volt:g};:CURR {current:g};:OUTP:STAT {(onOff ? "ON" : "OFF")};");
        }

        public double SetOutputGetActualVolt(double volt, double current)
        {
            string resp;
            if (!_isSim)
                resp = Query($":VOLT {volt:g};:CURR {current:g};:MEAS:VOLT?;");
            else
                resp = volt.ToString();
            return double.Parse(resp);
        }

        public double GetActualVolt()
        {
            string resp;
            if (!_isSim)
                resp = Query(":MEAS:VOLT?;");
            else
                resp = "0";
            return double.Parse(resp);
        }

        public double GetActualCurrent()
        {
            string resp;
            if (!_isSim)
                resp = Query(":MEAS:CURR?");
            else
                resp = "0";
            return double.Parse(resp);
        }

        public string GetErrors()
        {
            string resp;
            if (!_isSim)
            {
                string request = ":SYST:ERR?";
                resp = Query(request);
            }
            else
            {
                resp = "None";
            }
            return resp;
        }

        public string Query(string request)
        {
            LogWriteLine($"Tx:{request}");
            _session.RawIO.Write(request + "\n");
            var response = _session.RawIO.ReadString();
            LogWriteLine($"Rx:{response}");
            return response; 
        }

        public void Write(string request)
        {
            LogWriteLine($"Tx:{request}");
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
                LogWriteLine("Instance disposed.");
            }
            _disposed = true;

        }

    }
}

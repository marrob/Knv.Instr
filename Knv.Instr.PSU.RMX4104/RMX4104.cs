﻿

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
            _resourceManager = new ResourceManager();
            _session = (MessageBasedSession)_resourceManager.Open(visaName);
            LogWriteLine("Instance created.");

            Write("*RST;");
            Write(":SYST:ERR:ENAB;");
        }

        public string Identify()
        {
            var resp = Query("*IDN?");
            return resp;
        }

        public void SetOutput(double volt, double current)
        {
            Write($":VOLT {volt:g}; :CURR {current:g};");
        }

        public void SetOutput(double volt, double current, bool onOff)
        {
            Write($":VOLT {volt:g};:CURR {current:g};:OUTP:STAT {(onOff ? "ON" : "OFF")};");
        }

        public double SetOutputGetActualVolt(double volt, double current)
        {
            var resp = Query($":VOLT {volt:g};:CURR {current:g};:MEAS:VOLT?;");
            return double.Parse(resp);
        }

        public double GetActualVolt()
        {
            var resp = Query(":MEAS:VOLT?;");
            return double.Parse(resp);
        }

        public double GetActualCurrent()
        {
            var resp = Query(":MEAS:CURR?");
            return double.Parse(resp);
        }

        public string GetErrors()
        {
            string request = ":SYST:ERR?";
            var resp = Query(request);
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
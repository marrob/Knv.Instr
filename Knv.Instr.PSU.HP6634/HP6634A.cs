
/*
 * 
 * A HP663A Does not support standard SCPI commands
 * 
 * Example: 
 * *IDN?\n response -> \n  Last error is: Addressed to talk and nothing to say
 * ID? -> HP6634A\r\n
 * 
 * VSET does not have Response! If you read you will get Error...
 * 
 * 
 * .NET Framework: 4.8.04084
 *  Visual Studio: 2022 Community (64-bit) Version 17.2.6
 *  TestStnad: TestStand Version 2017 (17.0.0.184) 32-bit
 *  
 * --- NI VISA ---
 * NationalInstruments.Visa
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

namespace Knv.Instr.PSU.HP6634A
{
    using Ivi.Visa;
    using NationalInstruments.Visa;
    using System;

    public class HP6634A : Log, IDisposable, IPowerSupply
    { 
        bool _disposed = false;
        readonly IVisaSession _session = null;
        readonly bool _simulation = false;


        public HP6634A(string visaName, bool simulation)
        {
            _simulation = simulation;
            if (!_simulation)
                _session = new ResourceManager().Open(visaName);
        }

        public string Identify()
        {
            if (_simulation)
                return "Simulated HP6634A";
            else
                return Query($"ID?");
        }

        public void SetOutput(double volt, double current)
        {
            Write($"VSET {volt};ISET {current}");
        }

        public void OnOff(bool onOff)
        {
            Write($"OUT {(onOff ? "1" : "0")}");
        }

        public void SetOutput(double volt, double current, bool onOff)
        {
            Write($"VSET {volt};ISET {current};OUT {(onOff ? "1" : "0")}");
        }

        public double SetOutputGetActualVolt(double volt, double current)
        {
            var resp = Query($"VSET {volt};ISET {current};VOUT?");
            return double.Parse(resp, System.Globalization.CultureInfo.GetCultureInfo("en-US"));
        }

        public double GetActualVolt()
        {
            var resp = Query($"VOUT?");
            return double.Parse(resp, System.Globalization.CultureInfo.GetCultureInfo("en-US"));
        }

        public double GetActualCurrent()
        {
            var resp = Query($"IOUT?");
            return double.Parse(resp, System.Globalization.CultureInfo.GetCultureInfo("en-US"));
        }

        public string GetErrors()
        {
            string request = "ERR?";
            var resp = Query(request);
            return resp;
        }

        public string Query(string request)
        {
            ((MessageBasedSession)_session).RawIO.Write($"{request}\r\n");
            LogWriteLine($"Tx:{request}");     
            var response = ((MessageBasedSession)_session).RawIO.ReadString().Trim( new char[] {'\r', '\n', ' ' });
            LogWriteLine($"Rx:{response}");
            return response;
        }

        public void Write(string request)
        {
            ((MessageBasedSession)_session).RawIO.Write($"{request}\r\n");
            LogWriteLine($"Tx:{request}");
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
                LogWriteLine("Instance disposed.");
            }
            _disposed = true;

        }
    }
}
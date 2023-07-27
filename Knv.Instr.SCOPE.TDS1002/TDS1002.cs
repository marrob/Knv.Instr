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

namespace Knv.Instr.SCOPE.TDS1002
{
    using System;
    using Ivi.Visa;
    using NationalInstruments.Visa;

    public class TDS1002 : Log, IDisposable
    {
        bool _disposed = false;
        readonly IVisaSession _session = null;

        public TDS1002(string visaName, bool simulation)
        {
            _session = new ResourceManager().Open(visaName);
        }

        public string Identify()
        {
            var resp = Query($"*IDN?");
            return resp;
        }

        public string Query(string request)
        {
            ((MessageBasedSession)_session).RawIO.Write($"{request}\n");
            LogWriteLine($"Tx:{request}");
            var response = ((MessageBasedSession)_session).RawIO.ReadString().Trim(new char[] { '\r', '\n', ' ' });
            LogWriteLine($"Rx:{response}");
            return response;
        }

        public void Write(string request)
        {
            ((MessageBasedSession)_session).RawIO.Write($"{request}\n");
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

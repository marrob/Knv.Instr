
namespace Knv.Instr.SCOPE.TDS1002
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.IO.Ports;

    /* C:\Program Files\IVI Foundation\VISA\Microsoft.NET\Framework64\v2.0.50727\VISA.NET Shared Components 5.11.0\Ivi.Visa.dll*/
    using Ivi.Visa;
    /* C:\Program Files\IVI Foundation\VISA\Microsoft.NET\Framework64\v4.0.30319\NI VISA.NET 19.0\NationalInstruments.Visa.dll*/
    using NationalInstruments.Visa;



    public class TDS1002 : Log, IDisposable
    {
        bool _disposed = false;
        readonly IVisaSession _session = null;

        public TDS1002(string visaName, bool isSim)
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

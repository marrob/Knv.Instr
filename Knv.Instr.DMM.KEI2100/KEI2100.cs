

namespace Knv.Instr.DMM.KEI2100
{

    using System;
    using NationalInstruments.Visa;
    public class KEI2100 : Log, /*IPowerSupply,*/ IDisposable
    {

        bool _disposed = false;

        readonly ResourceManager _resourceManager;
        readonly MessageBasedSession _session;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="visaName">KEI2100</param>
        /// <param name="isSim"></param>
        public KEI2100(string visaName, bool isSim)
        {
            _resourceManager = new ResourceManager();
            _session = (MessageBasedSession)_resourceManager.Open(visaName);
            LogWriteLine("Instance created.");
            Write("*RST;");
            
        }

        public void Config(string ressoultion, string function, double range)
        { 
        
        }

        public void Config(string function, double range)
        {

            switch (function)
            {

                case "DCV":
                    {

                        break;
                    }

                case "DCA":
                    {

                        break;
                    }
                case "2WR":
                    {

                        break;
                    }
                case "4WR":
                    {

                        break;

                    }
                case "CAP":
                    {

                        break;
                    }
                case "CON":
                    {

                        break;
                    }
                case "DIO":
                    {

                        break;
                    }

            }
        }


        public string Identify()
        {
            var resp = Query("*IDN?");
            return resp;
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

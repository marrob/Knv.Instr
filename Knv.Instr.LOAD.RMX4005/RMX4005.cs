
namespace Knv.Instr.LOAD.RMX4005
{
    using System;
    using NationalInstruments.Visa;
    using Ivi.Visa;

    public class RMX4005 : IElectronicsLoad
    {
        bool _disposed = false;
        readonly bool _simulation = false;
        readonly IVisaSession _session = null;

        public RMX4005(string resourceName, bool simulation)
        {
            _simulation = simulation;
            if (_simulation)
                _session = null;
            else 
                _session = new ResourceManager().Open(resourceName);
        }
        public void Config(int channel, string range, double current)
        {

            if (_simulation)
                return;

            string cmd = "";
            switch (range.Trim().ToUpper())
            {
                case "HI":
                    {
                        if (channel == 1)
                        {
                            cmd = $":CURR:STAT:HIGH:AVAL {current};:CURR:STAT:REC A;";
                        }
                        else if (channel == 2)
                        {
                            cmd = $":CURR:STAT:HIGH:AVAL {current};:CURR:STAT:REC B;";
                        }
                        break;
                    }

                case "LO":
                    {
                        if (channel == 1)
                        {
                            cmd = $":CURR:STAT:LOW:AVAL {current};:CURR:STAT:REC A;";
                        }
                        else if (channel == 2)
                        {
                            cmd = $":CURR:STAT:LOW:AVAL {current};:CURR:STAT:REC B;";
                        }
                        break;
                    }
                default: throw new ArithmeticException($" The range {range} not supported. Supported reages: LO, HI");
            }

            Write(cmd);
        }

        public void OnOff(bool enable)
        {
            if (_simulation)
                return;
            if (enable)
                Write(":GLOB:LOAD ON");
            else
                Write(":GLOB:LOAD OFF");
        }

        public string Identify()
        {
            if (_simulation)
                return "Simulated RMX4005";
            else
                return Query($"IDN?");
        }

        public string Query(string request)
        {
            ((MessageBasedSession)_session).RawIO.Write($"{request}\r\n");
            var response = ((MessageBasedSession)_session).RawIO.ReadString().Trim(new char[] { '\r', '\n', ' ' });
            return response;
        }

        public void Write(string request)
        {
            ((MessageBasedSession)_session).RawIO.Write($"{request}\r\n");
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

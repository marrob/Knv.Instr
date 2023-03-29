

namespace Knv.Instr.DMM.KEI6500
{

    using System;
    using System.Data.SqlClient;
    using NationalInstruments.Visa;
    public class KEI6500 : Log, IDigitalMultiMeter, IDisposable
    {

        bool _disposed = false;

        readonly ResourceManager _resourceManager;
        readonly MessageBasedSession _session;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="visaName">KEI2100</param>
        /// <param name="isSim"></param>
        public KEI6500(string visaName, bool isSim)
        {
            _resourceManager = new ResourceManager();
            _session = (MessageBasedSession)_resourceManager.Open(visaName);
            LogWriteLine("Instance created.");
            Write("*RST;*CLS");


        }
        /// <summary>
        /// DCV:0.1V, 1.0V, 10V, 100V, 1000V
        /// DCC:0.01A, 0.1A, 1.0A, 3.0A 
        /// </summary>
        /// <param name="ressoultion"></param>
        /// <param name="function"></param>
        /// <param name="range"></param>
        /// <exception cref="ArgumentException"></exception>
        public void Config(string function, double range, double resoultionDigits, int powerlineFreq)
        {
            //ToDo Resoultion
            //ToDo PoerLineFreq
            // 4.5 digit - 000.00
            // 5.5 digit - 000.000
            // 6.5 digit - 000.0000

            switch (function)
            {
                case "DCV": 
                    { 
                        Write($"CONF:VOLT:DC {range}");
                        Write($"SENS:VOLT:RANG:AUTO OFF");
                        if (resoultionDigits == 6.5)
                        {   
                            // 000.0000 range: 100mV ez a  6.5digit mód
                            // 099.9999  
                            // 100.0000
                            Write($"SENS:VOLT:RES 0.1uV");
                        }

                        break;
                    } 
                case "DCC":
                    {
                        Write($"CONF:CURR:DC {range}");
                        Write($"SENS:CURR:RANG:AUTO OFF");

                        if (resoultionDigits == 6.5)
                        {
                            Write($"SENS:CURR:RES 10nA");
                        }

                        break;
                    }
                case "2WR":
                    {
                        Write($"CONF:RES {range}");
                        Write($"SENS:RES:RANG:AUTO OFF");
                        break;
                    }
                case "4WR":
                    {
                        Write($"CONF:FRES {range}");
                        Write($"SENS:FRES:RANG:AUTO OFF");
                        break;
                    }
                default: throw new ArgumentException("Nem támogatott funkció: Ezek támogatottak: DCV, DCC, 2WR, 4WR");
            }   
        }

        public void Config(string function, double range)
        {

            Config(function, range, 4.5, 50);

        }

        public double Read()
        {
            var value = Query("READ?");
            return double.Parse(value);
        }

        public string Identify()
        {
            var resp = Query("*IDN?");
            return resp;
        }

        public void WriteTextToDisplay(string text)
        {
            Write($"DISP:TEXT:CLE");
            Write($"DISP:TEXT \"{text}\"");
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

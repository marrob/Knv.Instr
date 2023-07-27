﻿

namespace Knv.Instr.DMM.KEI2100
{

    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using NationalInstruments.Visa;
    public class KEI2100 : IDigitalMultiMeter, IDisposable
    {
        public Dictionary<string, string[]> Ranges = new Dictionary<string, string[]>()
        {
            { "DCV", new string[]{ "100mV", "1V", "10V", "100V", "1000V" } },
            { "DCC", new string[]{ "10mA", "100mA", "1A", "3A" } },         
            { "2WR", new string[]{ "100R", "1K00", "100K", "1M00", "10M0", "100M" } },
            { "4WR", new string[]{ "100R", "1K00", "100K", "1M00", "10M0", "100M" } },
            { "ACV", new string[]{ "100mV", "1V", "750V" } },
            { "ACC", new string[]{ "1A", "3A" } },
        };

        bool _disposed = false;
        readonly bool _simulation = false;
        readonly ResourceManager _resourceManager;
        readonly MessageBasedSession _session;

        /// <summary>
        /// KEI2100
        /// </summary>
        /// <param name="visaName">KEI2100</param>
        /// <param name="simulation"></param>
        public KEI2100(string visaName, bool simulation)
        {
            _simulation = simulation;
            if (!_simulation)
            {
                _resourceManager = new ResourceManager();
                _session = (MessageBasedSession)_resourceManager.Open(visaName);
                Write("*RST;*CLS");
            }
        }
        /// <summary>
        /// Configuration of the NI KEI2100
        /// </summary>
        public void Config(string function, string rangeName, double digits, int powerlineFreq)
        {
            string[] rangeItems;
            if (!Ranges.TryGetValue(function, out rangeItems))
                throw new ArgumentException($" The {function} is not supported. Supported functions: {string.Join(",", Ranges.Keys)}");

            if (!rangeItems.Contains(rangeName))
                throw new ArgumentException($" The {function} is not supported this range {rangeName}. Supported ranges: {string.Join(",", rangeItems)}");
            
            if (_simulation)
                return;
   
            switch (function)
            {
                case "DCV":
                    {
                        Write($"CONF:VOLT:DC {rangeName}");
                        Write($"SENS:VOLT:RANG:AUTO OFF");
                        if (digits == 6.5)
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
                        Write($"CONF:CURR:DC {rangeName}");
                        Write($"SENS:CURR:RANG:AUTO OFF");

                        if (digits == 6.5)
                        {
                            Write($"SENS:CURR:RES 10nA");
                        }

                        break;
                    }
                case "2WR":
                    {
                        Write($"CONF:RES {rangeName}");
                        Write($"SENS:RES:RANG:AUTO OFF");
                        break;
                    }
                case "4WR":
                    {
                        Write($"CONF:FRES {rangeName}");
                        Write($"SENS:FRES:RANG:AUTO OFF");
                        break;
                    }
            }
        }
           
        public void Config(string function, string rangeName)
        {
            Config(function, rangeName, 4.5, 50);
        }

        public void Config(string function, string rangeName, double digits)
        {
            Config(function, rangeName, digits, 50);
        }

        public double Read()
        {
            if (!_simulation)
                return double.Parse(Query("READ?"));
            else
                return new Random().NextDouble();
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

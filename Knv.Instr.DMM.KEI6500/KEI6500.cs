

namespace Knv.Instr.DMM.KEI6500
{
    using NationalInstruments.Visa;
    using NUnit.Framework.Internal;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class KEI6500 : Log, /*IDigitalMultiMeter,*/ IDisposable
    {
        public Dictionary<string, string[]> Ranges = new Dictionary<string, string[]>()
        {
            { "DCV", new string[]{ "100e-3", "1", "10", "100", "1000" } },
            { "2WR", new string[]{ "1", "10", "100", "1000", "10e+3", "100e+3", "1e+6", "10e+6", "100e+6" } },
            { "4WR", new string[]{ "1", "10", "100", "1000", "10e+3" } },

           /*{ "DCC", new string[]{ "10uA", "100uA", "1mA", "10mA", "100mA", "1A", "3A", "10A" } },
            { "ACV", new string[]{ "100mV", "1V", "10V", "100V", "750V" } },
            { "ACC", new string[]{ "100uA", "1mA", "10mA", "100mA", "1A", "3A", "10A" } },      
            */
        };


        bool _disposed = false;
        bool _simulation = false;
        ResourceManager _resourceManager;
        MessageBasedSession _session;

        /// <summary>
        /// TestExec kompatiblitáshoz
        /// </summary>
        public KEI6500()
        {
            
        }

        public KEI6500(string visaName, bool simulation)
        {
            _simulation = simulation;
            if (!_simulation)
            {
                _resourceManager = new ResourceManager();
                _session = (MessageBasedSession)_resourceManager.Open(visaName);
                LogWriteLine("Instance created.");
                Write("*RST;*CLS");
            }
        }

        /// <summary>
        /// Ez az Open a TesTexec miatt van.
        /// </summary>
        public void Open(string visaName, bool simulation)
        {
            _simulation = simulation;
            if (_resourceManager == null)
            {
                if (!_simulation)
                {
                    _resourceManager = new ResourceManager();
                    _session = (MessageBasedSession)_resourceManager.Open(visaName);
                    LogWriteLine("Instance created.");
                    Write("*RST;*CLS");
                }
            }
        }

        public void Config(string function, string rangeName)
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
                // --- Measuring DCV with high accuracy ---
                //*RST                      Reset the DMM6500
                //:SENS:FUNC "VOLT:DC"      Set the instrument to measure DC voltage
                //:SENS:VOLT:RANG 10        Set the measure range to 10 V
                //:SENS:VOLT:INP AUTO       Set the input impedance to auto so the instrument selects 10 Ω for the 10 V range
                //:SENS:VOLT:NPLC 10        Set the integration rate (NPLCs) to 10
                //:SENS:VOLT:AZER ON        Enable autozero
                //:SENS:VOLT:AVER:TCON REP  Set the averaging filter type to repeating
                //:SENS:VOLT:AVER:COUN 100  Set the filter count to 100
                //:SENS:VOLT:AVER ON        Enable the filter
                //:READ?                    Read the voltage value; it is a few seconds before the reading is returned
                case "DCV":
                    {
                        //Fix: 2026.02.11
                        Write($":SENS:FUNC \"VOLT:DC\"");
                        Write($":SENS:VOLT:RANG:AUTO OFF");
                        Write($":SENS:VOLT:RANG {rangeName}");
                        break;
                    }
                // --- Measuring DC Current ---
                //*RST                      Reset the DMM6500
                //:SENS:CURR ???????

                /*
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
                */

                // --- 2Wire Resistance ---
                // 1Ω range = 10 mA
                // 10 Ω range = 10 mA
                // 100 Ω range = 1 mA
                // 1 kΩ range = 1 mA
                // 10 kΩ range = 100 µA
                // 100 kΩ range = 10 µA
                // 1 MΩ range = 10 µA
                // 10 MΩ range = 0.7 nA || 10 MΩ
                // 100 MΩ range = 0.7 µA || 10 MΩ
                // Ratiometric method(10 MΩ and 100 MΩ ranges): Test current is generated by a 0.7 μA source in parallel with a 10 MΩ reference resistor

                //*RST                      Reset the DMM6500
                //:SENS:FUNC "RES"          Set the instrument to measure 2Wire Resistance 
                //:SENS:RES:RANG:AUTO OFF   Manual Range
                //:SENS:RES:RANG 10         10Ω Range

                case "2WR":
                    {
                        Write($":SENS:FUNC \"RES\"");
                        Write($":SENS:RES:RANG:AUTO OFF");
                        Write($":SENS:RES:RANG {rangeName}");
                        break;
                    }

                    //  --- Resistor grading and binning test ---
                    //*RST                          Reset the DMM6500
                    //:SENS:FUNC "FRES"             Set function to 4-wire measurement
                    //:SENS:FRES:RANG: AUTO ON      Enable auto range
                    //:SENS:FRES:OCOM ON            Enable offset compensation
                    //:SENS:FRES:AZER ON            Enable auto zero
                    //:SENS:FRES:NPLC 1             Set NPLC to 1
                    //:READ? 
                    case "4WR":
                        {
                            Write($":SENS:FUNC \"FRES\"");
                            Write($":SENS:FRES:RANG:AUTO OFF");
                            Write($":SENS:FRES:RANG {rangeName}");
                            Write($":SENS:FRES:OCOM ON");
                            Write($":SENS:FRES:AZER ON");
                            Write($":SENS:FRES:NPLC 1");
                        break;
                        }
            }
        }

        public double Read()
        {
            if (!_simulation)
            {
                string strValue = Query("READ?");
                //string strValue = "0.555";
                return double.Parse(strValue);
            }
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
            Write($"DISP:CLE");
            Write($"DISP:SCR SWIPE_USER");
            Write($"DISP:USER1:TEXT \"{text}\"");
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
                _resourceManager = null;
                _session = null;
                LogWriteLine("Instance disposed.");
            }
            _disposed = true;
        }
    }
}

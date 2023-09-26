/*
***Sample IO by NI RMX-400x Loads COnsatnt Current Mode Setup.vi ***

 *IDN\n
-> National Instruments RMX-4005,GEW161695, V2.16

*RST\n
*ESE 60;*SRE;*CLS

:SYST:ERR?\n
-> 0, "No error"

:CHAN 1;
:SYST:ERR?\n
-> 0, "No error"

:MODE CCL\n
:SYST:ERR?\n
-> 0, "No error"


:CHAN:ID?;\n
-> National Instruments RMX-4005,GEW161695, V2.16

:CURR:STAT:LOW:AVAL 0.10000;
:CURR:STAT:REC A;
:CURR:STAT:LOW:RISE 0.080000;
:CURR:STAT:LOW:FALL 0.080000;\n
:SYST:ERR?\n
-> National Instruments RMX-4005,GEW161695, V2.16

:CONF:VOLT:RANG L;\n
:SYST:ERR?\n

:LOAD:TYPE LOAD;:LOAD:DEL 0.00000;:LOAD ON;
:SYST:ERR?\n

:FETC:CURR?\n

:FETC:POW?\n
-> 0.49667

:SYST:ERR?\n
-> 0, "No error"

:FETC:VOLT?;\n
-> 4.99297\n

:SYST:ERR?\n
-> 0, "No error"


*** END ***
*/

namespace Knv.Instr.LOAD.RMX4005
{
    using System;
    using NationalInstruments.Visa;
    using Ivi.Visa;
    using System.Collections.Generic;
    using static NUnit.Framework.Constraints.Tolerance;

    public class RMX4005 : IElectronicsLoad
    {
        bool _disposed = false;
        readonly bool _simulation = false;
        readonly IVisaSession _session = null;
        public Dictionary<string, string> Modes = new Dictionary<string, string>()
        {
            { "Constant Current 7A 16V", "CCL-VL" },
            { "Constant Current 7A 80V", "CCL-VH" },
            { "Constant Current 70A 16V", "CCH-VL" },
            { "Constant Current 70A 80V", "CCH-VH" },
        };

        public RMX4005(string resourceName, bool simulation)
        {
            _simulation = simulation;
            if (_simulation)
            {
                _session = null;
            }
            else
            {
                _session = new ResourceManager().Open(resourceName);
                ((MessageBasedSession)_session).TerminationCharacter = (byte)'\n';
                ((MessageBasedSession)_session).TimeoutMilliseconds = 3000;
                ((MessageBasedSession)_session).TerminationCharacterEnabled = true;

                Write($"*RST");

                var errors = GetErrors();
                if(errors.Count > 0)
                    throw new Exception($"Error: RMX4005: { string.Join(",", errors)}");
            }
        }

        /*
         * RMX-4005:  
         *  - Ez a modul 1 csatornás (2db egy csatornás load modul van az ECUTS-ben) 
         *  - 1db Mainframe van az ECUTSB-en ehhez csatlakozik a 2db egycsatornás load modul
         *  - Low(CCL): 7A 
         *  - High(CCH):70A, 
         *  - Voltage: 0..80V -> mind később kiderült ez így egy KISBASZOT FASZSÁG
         *    CCL és CCH módban is két külön feszültség tartomány van
         *    :CONF:VOLT:RANG L 16V-ig
         *    :CONF:VOLT:RANG H 80V-ig
         *    
         *  - Az egycsatornás moduljaink "A" Value és "B" Value-t is be lehet állítani 
         *   
         *  
         *  CCL - Constant Current Low Range
         *  CCH - Constant Current High Range
         */
        public void Config(string mode = "CCL-VL", string channel = "1", double current = 2.0)
        {


            if (_simulation)
                return;

            if (channel == "0")
                throw new ArgumentException($"Error: Channel indexing start with 1!");

            mode = mode.Trim().ToUpper();

            if(!Modes.ContainsValue(mode))
                throw new ArgumentException($"Error: Not supported mode:{mode}.Supported Modes:CCL-VL, CCL-VH, CCH-VL, CCH-VH");
           
           // Write($"*RST");
            
            /*
             * Selects the channel that the channel-specific
             * commands use. This command does not change
             * the channel in the display screen.
             */

            /*
             *
             *:CHAN 1 -> Sets channel 1 as the specific channel
             *CH1..CH8 -> 1..8 
             *
             */
            Write($":CHAN {channel};");

            switch (mode.Trim().ToUpper())
            {
                case "CCL-VL":
                    {
                        //CC static mode, low range
                        Write($":MODE CCL");
                        //:CURR:STAT:LOW:AVAL 1  -> Sets low range CC static mode A Value to 1 A.
                        Write($":CURR:STAT:LOW:AVAL {current}");
                        //Sets whether A Value or B Value is the currently active value in CC static mode.
                        Write($":CURR:STAT:REC A");
                        //Sets the low range rising/falling slew rates.
                        //Sets the rising slew rate to 0.001 A / μs.
                        Write($":CURR:STAT:LOW:RISE 0.08");
                        Write($":CURR:STAT:LOW:FALL 0.08");
                        //Low range: 16V
                        //High range: 80V
                        Write($":CONF:VOLT:RANG L");
                        break;
                    }

                case "CCL-VH":
                    {
                        Write($":MODE CCL");
                        Write($":CURR:STAT:LOW:AVAL {current}");
                        Write($":CURR:STAT:REC A");
                        Write($":CURR:STAT:LOW:RISE 0.08");
                        Write($":CURR:STAT:LOW:FALL 0.08");
                        Write($":CONF:VOLT:RANG H");
                        break;
                    }

                case "CCH-VL":
                    {
                        Write($":MODE CCH");
                        Write($":CURR:STAT:HIGH:AVAL {current}");
                        Write($":CURR:STAT:REC A");
                        Write($":CURR:STAT:LOW:RISE 0.08");
                        Write($":CURR:STAT:LOW:FALL 0.08");
                        Write($":CONF:VOLT:RANG L");
                        break;
                    }
                case "CCH-VH":
                    {
                        Write($":MODE CCH");
                        Write($":CURR:STAT:HIGH:AVAL {current}");
                        Write($":CURR:STAT:REC A");
                        Write($":CURR:STAT:LOW:RISE 0.08");
                        Write($":CURR:STAT:LOW:FALL 0.08");
                        Write($":CONF:VOLT:RANG H");
                        break;
                    }
                default: throw new ArithmeticException($"Error: This {mode} not supported. Supported reages: CCL, CCH ");
            }

            var errors = GetErrors();
            if (errors.Count > 0)
                throw new Exception($"Error: RMX4005: {string.Join(",", errors)}");
        }

        /// <summary>
        /// Over voltage protection limits the amount of voltage sunk. 
        /// If the OVP trips, the RMX-400x series load stops sinking voltage
        /// OPP
        /// Default: 16.5V
        /// </summary>
        public void OverVoltageProtection(string channel = "1", double voltage = 30)
        {
            if (_simulation)
                return;

            Write($":CHAN {channel};");
            Write($":CONF:PROT:VOLT:STAT ON;");
            //:CONFigure:PROTection:VOLTage:LEVel?\n
            Write($":CONF:PROT:VOLT:LEV {voltage};");
            var errors = GetErrors();
            if (errors.Count > 0)
                throw new Exception($"Error: RMX4005: {string.Join(",", errors)}");
        }

        /// <summary>
        /// Under voltage protection turns off the load when the voltage drops below a set limit.
        /// OVP
        /// </summary>
        public void UnderVoltageProtection(string channel = "1", double voltage = 30)
        {
            if (_simulation)
                return;

            Write($":CHAN {channel};");
            Write($":CONF:PROT:UVP:LEV {voltage};");
            var errors = GetErrors();
            if (errors.Count > 0)
                throw new Exception($"Error: RMX4005: {string.Join(",", errors)}");

        }

        public double GetActualVolt()
        {
            //:FETC:VOLT?;\n
            if (_simulation)
                return new Random().NextDouble();
            else
            {
                var resp = Query(":FETC:VOLT?;");
                return double.Parse(resp, System.Globalization.CultureInfo.GetCultureInfo("en-US"));
            }
        }

        public double GetActualCurrent()
        {
            //:FETC:CURR?;\n
            if (_simulation)
                return new Random().NextDouble();
            else
            {
                var resp =  Query(":FETC:CURR?;");
                return double.Parse(resp, System.Globalization.CultureInfo.GetCultureInfo("en-US"));
            }
        }

        public void OnOff(bool enable)
        {
            if (_simulation)
                return;

            //:LOAD\sON;\n -> Current Channel
            //:GLOB:LOAD\sON;\n -> All Channel

            //Load: Load, Program, Sequence
            Write(":LOAD:TYPE LOAD;");
            //Delay - Sets or queries the load delay time for the specific channel.
            Write(":LOAD:DEL 0.00000;");
            if (enable)
                Write(":LOAD ON;");
            else
                Write(":LOAD OFF;");
        }

        public string Identify()
        {
            if (_simulation)
                return "Simulated RMX4005";
            else
                return Query($"*IDN?;");
        }

        public string Query(string request)
        {
            ((MessageBasedSession)_session).RawIO.Write($"{request}\n");
            var response = ((MessageBasedSession)_session).RawIO.ReadString().Trim(new char[] { '\r', '\n', ' ' });
            return response;
        }

        public List<string> GetErrors()
        {      
            var errors = new List<string>();
            if (_simulation)
                return errors;

            for (int i = 0; i < 10; i++)
            {    
                var resp = Query(":SYST:ERR?;");
                if (resp.ToUpper().Contains("NO ERROR"))
                    break;
               else
                    errors.Add(resp);
            }
            return errors;
        }

        public void Write(string request)
        {
            ((MessageBasedSession)_session).RawIO.Write($"{request}\n");
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

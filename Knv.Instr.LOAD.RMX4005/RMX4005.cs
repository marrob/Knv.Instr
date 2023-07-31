
namespace Knv.Instr.LOAD.RMX4005
{
    using System;
    using NationalInstruments.Visa;
    using Ivi.Visa;
    using NUnit.Framework;
    using System.Collections.Generic;

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
            {

                _session = new ResourceManager().Open(resourceName);
                ((MessageBasedSession)_session).TerminationCharacter = (byte)'\n';
                ((MessageBasedSession)_session).TerminationCharacterEnabled = true;
            }
        }

        /*
         * RMX-4005:  
         *  - Ez a modul 1 csatornás (2db egy csatornás load modul van az ECUTS-ben) 
         *  - 1db Mainframe van az ECUTSB-en ehhez csatlakozik a 2db egycsatornás load modul
         *  - Low(CCL): 7A 
         *  - High(CCH):70A, 
         *  - Voltage: 0..80V
         *  - Az egycsatornás moduljaink csak A Value és B Value-t is be lehet állítani támogatják
         *  
         *  
         *  CCL - Constant Current Low Range
         *  CCH - Constant Current High Range
         */

        /// <summary>
        /// mode: 
        ///     CCL - Constant Current Low Range
        ///     CCH - Constant Current High Range
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="channel"></param>
        /// <param name="range"></param>
        /// <param name="current"></param>
        /// <exception cref="ArithmeticException"></exception>

        public void Config(string mode = "CCL", string channel = "1", double current = 2.0)
        {

            if (_simulation)
                return;
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
                case "CCL":
                    {
                        //:CURR:STAT:LOW:AVAL 1  -> Sets low range CC static mode A Value to 1 A.
                        Write($":CURR:STAT:LOW:AVAL {current};");
                        //:CURR:STAT:REC 1 -> Sets or queries whether A Value or B Value is the currently active value in CC static mode.
                        Write($":CURR:STAT:REC A;");
                        break;
                    }

                case "CCH":
                    {
                        Write($":CURR:STAT:HIGH:AVAL {current}");
                        Write($":CURR:STAT:REC A;");
                        break;
                    }
                default: throw new ArithmeticException($" This {mode} not supported. Supported reages: CCL, CCH ");
            }
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
            //:LOAD\sON;\n -> Current Channel
            //:GLOB:LOAD\sON;\n -> All Channel

            if (_simulation)
                return;
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

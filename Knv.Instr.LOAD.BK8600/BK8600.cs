/*

- A BK6800 nem tűri a sorvégi ponos vesszőt, ilyenkor mineképpen vár egy utasításra és ha nincs akkor hiba.


*** END ***
*/

namespace Knv.Instr.LOAD.BK8600
{
    using System;
    using NationalInstruments.Visa;
    using Ivi.Visa;
    using System.Collections.Generic;

    public class BK8600: IDisposable //: IElectronicsLoad 
    {
        bool _disposed = false;
        readonly bool _simulation = false;
        readonly IVisaSession _session = null;
        public Dictionary<string, string> Modes = new Dictionary<string, string>()
        {
            { "Constant Current 3A 120V", "CCL" },
            { "Constant Current 30A 120V", "CCH" },
        };

        public BK8600(string resourceName, bool simulation)
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

                Write($"*CLS"); //Clear Status Register - Torli a hibákat is.
                Write($"*RST"); // Factory Default


                var errors = GetErrors();
                if(errors.Count > 0)
                    throw new Exception($"Error: BK8600: { string.Join(",", errors)}");
            }
        }

        /// <summary>
        /// BK8600  
        /// CCL - 3A
        /// CCH - 30A
        /// </summary>
        /// <param name="mode">CCL or CCH</param>
        /// <param name="current"></param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArithmeticException"></exception>
        /// <exception cref="Exception"></exception>
        public void Config(string mode = "CCL", double current = 0.2)
        {
            if (_simulation)
                return;

            mode = mode.Trim().ToUpper();

            if (!Modes.ContainsValue(mode))
                throw new ArgumentException($" The {mode} is not supported. Supported functions: {string.Join(",", Modes.Values)}");

            switch (mode.Trim().ToUpper())
            {
                case "CCL":// Constant Current Low Range
                    {
                        Write($"SOUR:FUNC CURR"); //CURRent, RESistance, VOLTage, POWer
                        Write($"CURR:RANGE 3");
                        Write($"SOUR:FUNC:MODE FIX"); //FIXed or LIST
                        Write($"SOUR:CURR {current}"); //Sets the current that load will regulate when operating in constant current mode
                        break;
                    }

                case "CCH":// Constant Current High Range
                    {
                        Write($"SOUR:FUNC CURR");
                        Write($"CURR:RANGE 30");
                        Write($"SOUR:FUNC:MODE FIX");
                        Write($"SOUR:CURR {current}");
                        break;
                    }
                default: throw new ArithmeticException($"Error: This {mode} not supported. Supported reages: CCL, CCH ");
            }

            var errors = GetErrors();
            if (errors.Count > 0)
                throw new Exception($"Error: BK8600: {string.Join(",", errors)}");
        }


        public void OverCurrentProtection(double current)
        {      
            if (_simulation)
                return;

            Write($"CURR:PROT:STAT ON");
            Write($"CURR:PROT:LEV {current}");

            var errors = GetErrors();
            if (errors.Count > 0)
                throw new Exception($"Error: BK8600: {string.Join(",", errors)}");
        }


        public void OverVoltageProtection(double voltage)
        {
            throw new NotSupportedException();
        }

        public void UnderVoltageProtection(string channel = "1", double voltage = 30)
        {
            throw new NotSupportedException();
        }

        public double GetActualVolt()
        {
            if (_simulation)
                return new Random().NextDouble();
            else
            {
                var resp = Query(":FETC:VOLT?");
                return double.Parse(resp, System.Globalization.CultureInfo.GetCultureInfo("en-US"));
            }
        }

        public double GetActualCurrent()
        { 
            if (_simulation)
                return new Random().NextDouble();
            else
            {
                var resp =  Query(":FETC:CURR?");
                return double.Parse(resp, System.Globalization.CultureInfo.GetCultureInfo("en-US"));
            }
        }

        public void OnOff(bool enable)
        {
            if (_simulation)
                return;

            if (enable)
                Write("SOUR:INP ON");
            else
                Write("SOUR:INP OFF");
        }

        public string Identify()
        {
            if (_simulation)
                return "Simulated BK8600";
            else
                return Query($"*IDN?");
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
                var resp = Query(":SYST:ERR?");
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

namespace Knv.Instruments.GenericNiVisa
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Ivi.Visa;
    using System.IO;
    using NationalInstruments.Visa;

    /* 
     * .NET Framework: 4.8.04084
     *  Visual Studio: 2022 Community (64-bit) Version 17.2.6
     *  TestStnad: TestStand Version 2017 (17.0.0.184) 32-bit
     *  
     * --- NI VISA ---
     * NationalInstruments.Visa
     * C:\Program Files(x86)\IVI Foundation\VISA\Microsoft.NET\Framework32\v4.0.30319\NI VISA.NET 19.0\NationalInstruments.Visa.dll
     * C:\Program Files\IVI Foundation\VISA\Microsoft.NET\Framework64\v4.0.30319\NI VISA.NET 19.0\NationalInstruments.Visa.dll

     *Ivi.Visa
     * C:\Program Files(x86)\IVI Foundation\VISA\Microsoft.NET\Framework32\v2.0.50727\VISA.NET Shared Components 5.8.0\Ivi.Visa.dll
     * C:\Program Files\IVI Foundation\VISA\Microsoft.NET\Framework64\v2.0.50727\VISA.NET Shared Components 5.11.0\Ivi.Visa.dll
     */

    public class Visa
    {
        MessageBasedSession Session;

        readonly List<string> LogLines = new List<string>();
        bool _IsSimulation;

        /// <summary>
        /// https://www.ni.com/docs/en-US/bundle/labview/page/lvinstio/visa_resource_name_generic.html
        /// </summary>
        string ResourceName;


        public Visa():this(false)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logPath"></param>
        /// <param name="logEnable"></param>
        public Visa(bool isSim)
        {
            _IsSimulation = isSim;
        }

        /// <summary>
        /// Open the device
        /// </summary>
        /// <param name="resourceName">eg:TCPIP0::192.168.100.8::inst0::INSTR </param>
        /// <exception cref="Exception"></exception>
        public void Open(string resourceName)
        {
            ResourceName = resourceName;
            if (_IsSimulation)
                return;
            try
            {
                using (var rmSession = new ResourceManager())
                {
                    Session = (MessageBasedSession)rmSession.Open(resourceName);
                }
                LogWriteLine($"OPEN:{ResourceName}");
            }
            catch (Exception ex)
            {
                string err = $"ERROR OPEN:{ResourceName} - {ex.Message}";
                LogWriteLine(err);
                throw new Exception(err);
            }
        }

        /// <summary>
        /// Write only a command.
        /// </summary>
        /// <param name="cmd"></param>
        /// <exception cref="Exception"></exception>
        public void WriteLine(string cmd)
        {
            if (_IsSimulation)
                return;
            try
            {
                LogWriteLine($"TX:{cmd}");
                Session?.RawIO.Write($"{cmd}\n");
            }
            catch (Exception ex)
            {
                string err = $"ERROR TX:{ResourceName} - {cmd} - {ex.Message}";
                LogWriteLine(err);
                throw new Exception(err);   
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"> SCPI Command, eg:*IDN? </param>
        /// <returns>SCPI Response eg:  </returns>
        /// <exception cref="Exception"></exception>
        public string WrtieReadLine(string cmd)
        {
            string resp = "";
            if (_IsSimulation)
                return resp;
            WriteLine(cmd);
            try
            {
                resp = Session?.RawIO.ReadString().Trim();
                LogWriteLine($"RX:{resp}");
            }
            catch (Exception ex)
            {
                string err = $"ERROR RX:{ResourceName} - {cmd} - {ex.Message}";
                LogWriteLine(err);
                throw new Exception(err);
            }
            return resp;
        }

        /// <summary>
        /// Close the Resource
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void Close()
        {
            try
            {
                Session?.Dispose();
                LogWriteLine($"CLOSE:{ResourceName}");
            }
            catch (Exception ex)
            {
                string err = $"ERROR CLOSE:{ResourceName} - {ex.Message}";
                LogWriteLine(err);
                throw new Exception(err);
            }
        }

        public void LogWriteLine(string line)
        {
            var dt = DateTime.Now;
            LogLines.Add($"{dt:yyyy}.{dt:MM}.{dt:dd} {dt:HH}:{dt:mm}:{dt:ss} {line}");
        }

        public void LogSave(string directory)
        {
            LogSave(directory, "");
        }

        public void LogSave(string directory, string prefix)
        {
            var dt = DateTime.Now;
            var fileName = $"{prefix}_{dt:yyyy}{dt:MM}{dt:dd}_{dt:HH}{dt:mm}{dt:ss}.log";
            using (var file = new StreamWriter($"{directory}\\{fileName}", true, Encoding.ASCII))
                LogLines.ForEach(file.WriteLine);
        }
    }

}

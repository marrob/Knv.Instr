
namespace Knv.Instruments.GenericSerial
{
    using System;
    using System.Text;
    using System.IO;
    using System.IO.Ports;

    public class Serial
    {
        public SerialPort SerialPort;
        string PortName;
        /// <summary>
        /// D:\\log.txt
        /// </summary>
        readonly string LogFilePath = "";
        readonly bool LogEnabled;

        public Serial()
        {
            LogFilePath = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments);
            LogEnabled = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logPath"></param>
        /// <param name="logEnable"></param>
        public Serial(string logPath, bool logEnable)
        {
            LogEnabled = logEnable;
            LogFilePath = logPath;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="portName">Serial Port Name eg.: COM13</param>
        /// <param name="readTimeout">eg:1000ms</param>

        public void Open(string portName, int readTimeout)
        {
            PortName = portName;

            try
            {
                SerialPort = new SerialPort(portName);
                SerialPort.ReadTimeout = readTimeout;
                SerialPort.Open();
            }
            catch (Exception ex)
            {
                string err = $"ERROR OPEN:{PortName} - {ex.Message}";
                LogWirteLine(err);
                throw new Exception(err);
            }
        }

        public void Open(string portName, int baudrate, string newLine, int readTimeout)
        {
            PortName = portName;
            try
            {
                SerialPort = new SerialPort(portName, baudrate);
                SerialPort.ReadTimeout = 1000;
                SerialPort.NewLine = newLine;
                SerialPort.Open();

                LogWirteLine($"OPEN:{PortName}");
            }
            catch (Exception ex)
            {
                string err = $"ERROR OPEN:{PortName} - {ex.Message}";
                LogWirteLine(err);
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
            try
            {
                LogWirteLine($"TX:{cmd}");
                SerialPort?.WriteLine($"{cmd}");
            }
            catch (Exception ex)
            {
                string err = $"ERROR TX:{PortName} - {cmd} - {ex.Message}";
                LogWirteLine(err);
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
            WriteLine(cmd);
            try
            {
                resp = SerialPort?.ReadLine().Trim();
                LogWirteLine($"RX:{resp}");
            }
            catch (Exception ex)
            {
                string err = $"ERROR RX:{PortName} - {cmd} - {ex.Message}";
                LogWirteLine(err);
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
                SerialPort?.Close();
                LogWirteLine($"CLOSE:{PortName}");
            }
            catch (Exception ex)
            {
                string err = $"ERROR CLOSE:{PortName} - {ex.Message}";
                LogWirteLine(err);
                throw new Exception(err);
            }
        }

        public void LogWirteLine(string line)
        {
            if (LogEnabled)
            {
                var fileWrite = new StreamWriter(LogFilePath, true, Encoding.ASCII);
                fileWrite.NewLine = "\r\n";
                fileWrite.Write($"{DateTime.Now.ToString("yyyy.MM.dd-HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture)}>{line}\r\n");
                fileWrite.Flush();
                fileWrite.Close();
            }
        }

        public double GetLogFileSizeKB()
        {
            if (File.Exists(LogFilePath))
            {
                FileInfo fi = new FileInfo(LogFilePath);
                return fi.Length / 1024;
            }
            else
            {
                return 0;
            }
        }

    }
}

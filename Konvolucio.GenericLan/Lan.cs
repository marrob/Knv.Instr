
namespace Konvolucio.PsuOverLan
{
    using System;
    using System.Net.Sockets;
    using System.Net;
    using System.Text;
    using System.IO;

    public class Lan
    {

        /// <summary>
        /// D:\\log.txt
        /// </summary>
        readonly string LogFilePath = "";
        readonly bool LogEnabled;

        IPEndPoint InstrIp;
        Socket InstrSocket;


        public Lan()
        {
            LogFilePath = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments);
            LogEnabled = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logPath"></param>
        /// <param name="logEnable"></param>
        public Lan(string logPath, bool logEnable)
        {
            LogEnabled = logEnable;
            LogFilePath = logPath;
        }

        public void Open(string instrIPAddress, int instrPortNo)
        {        
            InstrIp = new IPEndPoint(IPAddress.Parse(instrIPAddress), instrPortNo);
            InstrSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            InstrSocket.ReceiveTimeout = 1000;
            try
            {
                if (!InstrSocket.Connected)
                    InstrSocket.Connect(InstrIp);
            }
            catch (Exception ex)
            {
                string err = $"ERROR TO CONNET TO SERVER:{InstrIp.Address}:{InstrIp.Port} - {ex.Message}";
                LogWirteLine(err);
                throw new ApplicationException("Instrument at " + this.InstrIp.Address + ":" + this.InstrIp.Port + " is not connected");
            }
        }

        public void WriteLine(string cmd)
        {
            try
            {
                LogWirteLine($"TX:{cmd}");
                InstrSocket.Send(Encoding.ASCII.GetBytes(cmd + "\n"));
            }
            catch (Exception ex)
            {
                string err = $"ERROR TX:{InstrIp.Address}:{InstrIp.Port} - {cmd} - {ex.Message}";
                LogWirteLine(err);
                throw new Exception(err);
            }
        }

        public string WriteReadLine(string cmd)
        {
            string resp = "";
            WriteLine(cmd);
            try
            {
                byte[] data = new byte[1024];
                int receivedDataLength = InstrSocket.Receive(data);
                resp = Encoding.ASCII.GetString(data, 0, receivedDataLength).Trim();
                LogWirteLine($"RX:{resp}");
            }
            catch (Exception ex)
            {
                string err = $"ERROR RX:{InstrIp.Address}:{InstrIp.Port} - {cmd} - {ex.Message}";
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
                InstrSocket.Close();
                LogWirteLine($"CLOSE:{InstrIp.Address}:{InstrIp.Port}");
            }
            catch (Exception ex)
            {
                string err = $"ERROR CLOSE:{InstrIp.Address}:{InstrIp.Port} - {ex.Message}";
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

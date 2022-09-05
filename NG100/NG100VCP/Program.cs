using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace NG100VCP
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(PSP.SCPIWriteRead("COM13", "*OPC?"));
            Console.WriteLine(PSP.SCPIWriteRead("COM13", "*IDN?"));
            
            Console.ReadLine();
        }
    }
    static class PSP
    {
        public static string SCPIWriteRead(string comport, string command)
        {
            string response;
            var sp = new SerialPort(comport);
            sp.Open();
            sp.WriteLine(command);
            response = sp.ReadLine();
            sp.Close();
            return response;
        }
    }
}

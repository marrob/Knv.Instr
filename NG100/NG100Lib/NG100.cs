
namespace NG100Lib
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using System.IO.Ports;
    using System.ComponentModel;

    public class NG100
    {

        public SerialPort _sp;


        public NG100()
        {

        }


        public void Open(string comport)
        {
            _sp = new SerialPort(comport);
            _sp.ReadTimeout = 1000;
            _sp.Open();
        }



        public string SendCommand(string command)
        {
            _sp.WriteLine(command);
            return _sp.ReadLine();

        }

        public void Dispose()
        {
            _sp.Close();
            _sp.Dispose();
        }
    }
}

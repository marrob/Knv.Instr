namespace Knv.Instr.Serial
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using NUnit.Framework;

    [TestFixture]
    internal class TDK1002_SerialOverVisa
    {
        [Test]
        public void TEK1002_Indentify()
        {
            string PortName = "COM13";
            var visa = new Serial("D:\\TEK1002_Log.txt", true);
            visa.Open(PortName,9600, "\r", 1000);
            string resp =  visa.WrtieReadLine("*IDN?");
            Assert.AreEqual("TEKTRONIX,TDS 1002,0,CF:91.1CT FV:v2.06 TDS2CM:CMV:v1.04", resp);
        }
    }
}

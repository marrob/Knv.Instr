

namespace Knv.Instr.Visa
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using NUnit.Framework;

    [TestFixture]
    internal class TDK1002_SerialOverVisa_UnitTest
    {
        [Test]
        public void TEK1002_Indentify()
        {
            string resourceName = "ASRL13::INSTR";
            var visa = new Visa(false);
            visa.Open(resourceName);
            string resp =  visa.WrtieReadLine("*IDN?");
            visa.LogSave("D:\\", "TEK1002_Log");
            Assert.AreEqual("TEKTRONIX,TDS 1002,0,CF:91.1CT FV:v2.06 TDS2CM:CMV:v1.04", resp);
        }

        [Test]
        public void TEK1002_Indentify_Sim()
        {
            string resourceName = "ASRL13::INSTR";
            var visa = new Visa(true);
            visa.Open(resourceName);
            string resp = visa.WrtieReadLine("*IDN?");
            visa.LogSave("D:\\", "TEK1002_Log");
            Assert.AreEqual("TEKTRONIX,TDS 1002,0,CF:91.1CT FV:v2.06 TDS2CM:CMV:v1.04", resp);

        }
    }
}

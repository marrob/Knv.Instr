using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Diagnostics;

namespace Knv.Instruments.GenericLan
{
    [TestFixture]
    public class DMM6500_UnitTest
    {

        [Test]
        public void DMM6500_Identify()
        {
            var dmm = new Lan();
            dmm.Open("192.168.100.9", 5025);
            string resp = dmm.WriteReadLine("*IDN?");
            Assert.AreEqual("KEITHLEY INSTRUMENTS,MODEL DMM6500,04429665,1.0.04b", resp);
            dmm.Close();
        }
    }
}

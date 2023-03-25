using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Diagnostics;

namespace Knv.Instr.Eth
{
    [TestFixture]
    public class SDG2042X_UnitTest
    {
        /// <summary>
        /// RAW SOCKET
        /// SCPI-RAW (TCP/UDP)(IANA official)
        /// </summary>
        [Test]
        public void SDG2042X_Identify()
        {
            var arb = new Lan();
            arb.Open("192.168.100.8", 5025);
            string resp = arb.WriteReadLine("*IDN?");
            Assert.AreEqual("Siglent Technologies,SDG2042X,SDG2XCA4162310,2.01.01.35R3B2", resp);
            arb.Close();
        }
    }
}

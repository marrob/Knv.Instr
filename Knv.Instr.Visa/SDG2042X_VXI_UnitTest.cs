
namespace Knv.Instr.Visa
{

    using NUnit.Framework;



    using System.Collections.Generic;

    /* 
     * *** NI VISA ---
     * NationalInstruments.Visa
     * C:\Program Files(x86)\IVI Foundation\VISA\Microsoft.NET\Framework32\v4.0.30319\NI VISA.NET 19.0\NationalInstruments.Visa.dll
     * C:\Program Files\IVI Foundation\VISA\Microsoft.NET\Framework64\v4.0.30319\NI VISA.NET 19.0\NationalInstruments.Visa.dll
     */
    using NationalInstruments.Visa;

    /*
     * *** Ivi.Visa ***
     * C:\Program Files(x86)\IVI Foundation\VISA\Microsoft.NET\Framework32\v2.0.50727\VISA.NET Shared Components 5.8.0\Ivi.Visa.dll
     * C:\Program Files\IVI Foundation\VISA\Microsoft.NET\Framework64\v2.0.50727\VISA.NET Shared Components 5.11.0\Ivi.Visa.dll
     */
    using Ivi.Visa;
    using System.Runtime.Remoting.Messaging;

    [TestFixture]
    internal class SDG2042X_VXI_UnitTest
    {
        string _resName = "TCPIP0::192.168.100.8::inst0::INSTR";

        [SetUp]
        public void TestSetup(){

        }

        [Test]
        public void SDG2042X_OverUSB() 
        {
            List<string> devices = new List<string>();
            using (var rm = new ResourceManager())
            {
                IEnumerable<string> resources = rm.Find("TCPIP?*");
                foreach (string s in resources)
                {
                    ParseResult parseResult = rm.Parse(s);
                    devices.Add($"{s} - {parseResult.InterfaceType}");
                }
            }
            Assert.IsTrue(devices.Count != 0);
        }

        [Test]
        public void GetUsbResources()
        {
            ///USB0::0xF4EC::0xEE38::SDG2XCA4162310::INSTR
            List<string> devices = new List<string>();
            using (var rm = new ResourceManager())
            {
                IEnumerable<string> resources = rm.Find("USB?*");
                foreach (string s in resources)
                {
                    ParseResult parseResult = rm.Parse(s);
                    devices.Add($"{s}");
                }
            }
            Assert.IsTrue(devices.Count != 0);
            MessageBasedSession mbSession;
            using (var rmSession = new ResourceManager())
            {
                mbSession = (MessageBasedSession)rmSession.Open(devices[0]);
            }
            mbSession.RawIO.Write("*IDN?\n");
            var response = mbSession.RawIO.ReadString();
            Assert.AreEqual("Siglent Technologies,SDG2042X,SDG2XCA4162310,2.01.01.35R3B2\n", response);
            mbSession.Dispose();
        }


        [Test]
        public void DeviceIsPresent_SDG2042X() 
        {
            List<string> devices = new List<string>();
            using (var rm = new ResourceManager())
            {
                IEnumerable<string> resources = rm.Find("TCPIP?*");
                foreach (string s in resources)
                {
                    ParseResult parseResult = rm.Parse(s);
                    devices.Add($"{s}");
                }
            }
            Assert.AreEqual(devices[0], _resName);
        }


        [Test]
        public void Identify_SDG2042X()
        {
            MessageBasedSession mbSession;
            using (var rmSession = new ResourceManager())
            {
                mbSession = (MessageBasedSession)rmSession.Open(_resName);
            }
            mbSession.RawIO.Write("*IDN?\n");
            Assert.AreEqual("Siglent Technologies,SDG2042X,SDG2XCA4162310,2.01.01.22R5\n", mbSession.RawIO.ReadString());
            mbSession.Dispose();

        }
    }
}

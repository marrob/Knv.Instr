

/*
 * Assembly NationalInstruments.Common:
 * C:\Program Files (x86)\National Instruments\Measurement Studio\DotNET\v4.0\AnyCPU\NationalInstruments.Common 19.1.40
 * 
 * NationalInstruments.NI4882: 
 * C:\Program Files (x86)\National Instruments\MeasurementStudioVS2012\DotNET\Assemblies\Current\NationalInstruments.NI4882.dll
 * 
 */
namespace Knv.Instr.GPIB
{
    using NationalInstruments.NI4882;
    using NUnit.Framework;
    using System.Runtime.Remoting.Messaging;
    using System.Threading;

    [TestFixture]
    internal class GPIB_Basic_UnitTest
    {
        [Test]
        public void HP53131A_IDN()
        {
            Device dev = new Device(0, 20);
            dev.IOTimeout = TimeoutValue.T100ms;
            dev.Write("*idn?\n");
            var resp = dev.ReadString();
            Assert.AreEqual("HEWLETT-PACKARD,53131A,0,3427\n", resp);
        }
    }
}

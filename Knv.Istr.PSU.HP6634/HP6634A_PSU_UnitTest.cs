

/*
 * 
 * A HP663A Does not support standard SCPI commands
 * 
 * Example: 
 * *IDN?\n response -> \n  Last error is: Addressed to talk and nothing to say
 * ID? -> HP6634A\r\n
 * 
 * VSET does not have Response! If you read you will get Error...
 * 
 */
namespace Knv.Instr.DAQ
{
    using Knv.Instr.GPIB;
    using NationalInstruments.NI4882;
    using NUnit.Framework;
    using NUnit.Framework.Internal.Execution;
    using System.Runtime.Remoting.Messaging;
    using System.Threading;
    //using Intsr;

    [TestFixture]
    internal class HP6634A_PSU_UnitTest
    {
        const int ADDRESS = 7;

        [Test]
        public void HP6634A_ID()
        {
            var dev = new Device(0, ADDRESS);
            dev.IOTimeout = TimeoutValue.T100ms;
            dev.Write("ID?\r\n");
            var resp = dev.ReadString();
            Assert.AreEqual("HP6634A\r\n", resp);
        }


        [Test]
        public void HP6634A_SetVolt()
        {
            var dev = new Device(0, ADDRESS);
            dev.IOTimeout = TimeoutValue.T100ms;
            dev.Write("VSET 2.2\r\n");
            dev.Write("ISET 0.5\r\n");
        }

        [Test]
        public void SetVoltCurrent()
        {
            using (var psu = new HP6634A_PSU(ADDRESS, isSim: false))
            {
                var resp = psu.Identify();
                Assert.AreEqual("HP6634A", resp);
                psu.SetOutput(volt: 12, current: 01);
            }
        }

        [Test]
        public void SetVoltCurrentOnOff()
        {
            using (var psu = new HP6634A_PSU(ADDRESS, isSim: false))
            {
                var resp = psu.Identify();
                Assert.AreEqual("HP6634A", resp);
                psu.SetOutput(volt: 12, current: 01, true);
            }
        }

        [Test]
        public void GenericIdentfy()
        {
            using (var genericPsu = new GenericPowerSupply(new HP6634A_PSU(ADDRESS, false)))
            {
                var resp = genericPsu.Identify();
                Assert.AreEqual("HP6634A", resp);
                genericPsu.SetOutput(volt: 12, current: 01);

            }
        
        }

    }
}

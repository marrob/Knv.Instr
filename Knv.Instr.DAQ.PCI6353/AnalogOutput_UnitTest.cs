

namespace Knv.Instr.DAQ.PCI6353
{
    using NUnit.Framework;

    [TestFixture]
    internal class AnalogOutput_UnitTest
    {
        [Test]
        public void DeviceIsPresent()
        {
            Assert.IsTrue(Tools.DeviceIsPresent("Dev1"));  
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(-1)]
        public void SetVoltage(double value) 
        {
            AnalogOutput.SetChannel("Dev1", "ao0", value);
        }

        [Test]
        public void SignalGenerator0()
        {
            using (var sg1 = new AnalogSignalGenSoftTiming("Dev1"))
            {

                /*
                sg1.Start("ao0");
                System.Threading.Thread.Sleep(100000);
                sg1.Stop();
                */
            }
        }

        [Test]
        public void SignalGenerator()
        {
            using (var sg1 = new AnalogSignalGenSoftTiming("Dev1"))
            {
                using (var sg2 = new AnalogSignalGenSoftTiming("Dev1"))
                {
                    /*
                    sg1.Start("ao0");
                    sg2.Start("ao1");
                    System.Threading.Thread.Sleep(100000);
                    sg1.Stop();
                    sg2.Stop();
                    */
                }
            }
        }
    }
}

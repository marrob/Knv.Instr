

namespace Knv.Instr.DAQ.PCI6353
{
    using NUnit.Framework;

    [TestFixture]
    internal class AnalogInput_UnitTest
    {
        [Test]
        public void DeviceIsPresent()
        {
            Assert.IsTrue(Tools.DeviceIsPresent("Dev1"));  
        }


        [Test]
        public void GetOneVoltage()
        {
            var value = AnalogInput.GetOneChannel("Dev1", "ai0");
            Assert.IsTrue(value != 0);
        }

        [Test]
        public void SamplingTest()
        {
            var result = AnalogInput.NormalMeasureStart(visaName:"Dev1", channel: "ai0", samples: 1000, sFreq: 1000000);

            Instr.Tools.SignalToFile(data: result, "Sapling Test", "D:\\");

        }
    }
}

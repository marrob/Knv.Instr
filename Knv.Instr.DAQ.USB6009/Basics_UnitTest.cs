

namespace Knv.Instr.DAQ.USB6009
{
    using NationalInstruments.DAQmx;
    using NUnit.Framework;
    using System.Linq;

    [TestFixture]
    internal class Basics_UnitTest
    {
        [Test]
        public void DeviceIsPresent_LowLevel()
        {
            var devs = DaqSystem.Local.Devices;
            Assert.IsTrue(devs.Contains("USB6009"));
        }

        [Test]
        public void GetOneSingleEndedChannel_LowLevel()
        {
            string visaName = "USB6009";
            string channel = "ai0";
            var terminalConfig = AITerminalConfiguration.Rse;
            double result = 0;

            using (var myTask = new Task())
            {
                string physicalChannel = $"{visaName}/{channel}";
                myTask.AIChannels.CreateVoltageChannel(physicalChannel, ""/*$"AI:{channel}"*/, terminalConfig, -10, 10, AIVoltageUnits.Volts);
                AnalogMultiChannelReader reader = new AnalogMultiChannelReader(myTask.Stream);
                myTask.Control(TaskAction.Verify);
                result = reader.ReadSingleSample()[0];
            }
        }

        [Test]
        public void GetOneSingleEndedChannel()
        {
            var value = AnalogInput.GetOneSingleEndedChannel("USB6009", "ai0");
            Assert.IsTrue(value != 0);
        }

        [Test]
        public void GetOneDifferentialChannel()
        {
            var value = AnalogInput.GetOneSingleEndedChannel("USB6009", "ai0");
            Assert.IsTrue(value != 0);
        }

        [Test]
        public void SamplingTest()
        {
            var result = AnalogInput.NormalSingleEndedMeasureStart(visaName:"Dev1", channel: "ai0", samples: 1000, sFreq: 1000000);

            Tools.SignalToFile(data: result, "Sapling Test", "D:\\");

        }
    }
}

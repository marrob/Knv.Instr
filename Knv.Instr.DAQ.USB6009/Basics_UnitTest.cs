

namespace Knv.Instr.DAQ.USB6009
{
    using NationalInstruments.DAQmx;
    using NUnit.Framework;
    using System;
    using System.Linq;

    [TestFixture]
    internal class Basics_UnitTest
    {
        [Test]
        public void SineOne()
        {
            Assert.AreEqual(1, Math.Sin(Math.PI / 2));
        }

        [Test]
        public void SineDeg()
        {
            Assert.AreEqual(1.2246063538223773E-16d, Math.Sin(Math.PI));
            Assert.AreEqual(1, Math.Sin(Math.PI / 2));
            Assert.AreEqual(1, Math.Sin(Math.PI / 180 * 90));
        }
        [Test]
        public void DeviceIsPresent_LowLevel()
        {
            var devs = DaqSystem.Local.Devices;
            Assert.IsTrue(devs.Contains("USB6009"));
        }

        [Test]
        public void GetOneSingleEndedChannel_LowLevel()
        {
            string resourceName = "USB6009";
            string channel = "ai0";
            var terminalConfig = AITerminalConfiguration.Rse;
            double result = 0;

            using (var myTask = new Task())
            {
                string physicalChannel = $"{resourceName}/{channel}";
                myTask.AIChannels.CreateVoltageChannel(physicalChannel, "", terminalConfig, -5, 5, AIVoltageUnits.Volts);
                AnalogMultiChannelReader reader = new AnalogMultiChannelReader(myTask.Stream);
                myTask.Control(TaskAction.Verify);
                result = reader.ReadSingleSample()[0];
            }
        }

        /*
         * A kimenet a metódus lefutása után is kinnmarad!! 
         */
        [Test]
        public void SetAnalogOutput_LowLevel()
        {
            string resourceName = "USB6009";
            string channel = "ao0";
            double ao0Voltage = 2.2;

            using (var myTask = new Task())
            {
                string physicalChannel = $"{resourceName}/{channel}";
                myTask.AOChannels.CreateVoltageChannel(physicalChannel, "", -5, 5, AOVoltageUnits.Volts);
                var writer = new AnalogSingleChannelWriter(myTask.Stream);
                writer.WriteSingleSample(true, ao0Voltage);
            }
        }


        [Test]
        public void SignalGeneratorWithSoftTimming()
        {
            using (var sg1 = new SineGenSwTiming("USB6009"))
            {
                sg1.Start(channel: "ao0", offset: 2.5, amplitude: 2.5, freq: 1, samples: 100);
                System.Threading.Thread.Sleep(1000);
                sg1.Stop();
            }
        }

        [Test]
        public void SineGenBackgroundWorker()
        {
            using (var sg1 = new SineGenBackgroundWorker("USB6009"))
            {
                sg1.Start(channel: "ao0", offset: 2.5, amplitude: 2.5, freq: 1, samples: 100);
                System.Threading.Thread.Sleep(10000);
                sg1.Abort();
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
            var result = AnalogInput.NormalSingleEndedMeasureStart(resourceName: "USB6009", channel: "ai0", samples: 1000, sFreq: 48000);

            Tools.SignalToFile(data: result, "Sapling Test", "D:\\");

        }
    }
}

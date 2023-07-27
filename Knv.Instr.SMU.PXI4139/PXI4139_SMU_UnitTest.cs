
namespace Knv.Instr.SMU.PXI4139
{
    using NUnit.Framework;

    [TestFixture]
    internal class PXI4139_SMU_UnitTest
    {
        string RESOURCE_NAME = "J18_SMU";



        [Test]
        public void Identify()
        {
            using (var smu = new PXI4139(RESOURCE_NAME, simulation: false))
            {
                var resp = smu.Identify();
                Assert.IsTrue(resp.Contains("National Instruments"));
            }
        }


        [Test]
        public void MeasureVoltSmallestRange()
        {
            using (var smu = new PXI4139(RESOURCE_NAME, simulation: false))
            {
                var measValue = smu.GetActualVolt();
                Assert.IsTrue(-0.5 < measValue && measValue < 0.5);
            }
        }

        [Test]
        public void VoltageSource_SetMultiplieTimes()
        {
            using (var smu = new PXI4139(RESOURCE_NAME, simulation: false))
            {
                var resp = smu.Identify();
                Assert.IsTrue(resp.Contains("National Instruments"));
                smu.ConfigVoltageSource("600mV", "100uA");
                smu.SetVoltageSource(voltage:0.05, current:0.00001);
                smu.SetVoltageSource(voltage: 0.06, current: 0.00001);
                smu.OnOff(enable: true);
            }
        }

        [Test]
        public void VoltageSource_ConfigMultiplieTimes()
        {
            using (var smu = new PXI4139(RESOURCE_NAME, simulation: false))
            {
                var resp = smu.Identify();
                Assert.IsTrue(resp.Contains("National Instruments"));

                smu.ConfigVoltageSource("600mV", "100uA");
                smu.SetVoltageSource(voltage: 0.05, current: 0.00001);

                smu.ConfigVoltageSource("6V", "10mA");
                smu.SetVoltageSource(voltage: 3.0, current: 0.005);

                smu.OnOff(enable: true);
                smu.OnOff(enable: false);
            }
        }

        [Test]
        public void VoltageSource_Measure()
        {

            double volts;
            using (var smu = new PXI4139(RESOURCE_NAME, simulation: false))
            {
                var resp = smu.Identify();
                Assert.IsTrue(resp.Contains("National Instruments"));

                smu.ConfigVoltageSource("6V", "1mA");
                smu.SetVoltageSource(voltage: 3.0, current: 0.0005);
                smu.OnOff(enable: true);
                volts = smu.GetActualVolt();
                Assert.Less(volts, 3.1);
                Assert.Greater(volts, 2.9);

                smu.ConfigVoltageSource("60V", "1mA");
                smu.SetVoltageSource(voltage: 10.0, current: 0.0005);
                volts = smu.GetActualVolt();
                Assert.Less(volts, 10.1);
                Assert.Greater(volts, 9.9);


                smu.OnOff(enable: false);   
                System.Threading.Thread.Sleep(100);
                volts = smu.GetActualVolt();
                Assert.Less(volts, 0.5);
                Assert.Greater(volts, -0.5);
            }
        }

    }


}

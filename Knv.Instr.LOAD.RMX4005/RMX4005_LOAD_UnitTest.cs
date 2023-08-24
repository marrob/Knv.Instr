
namespace Knv.Instr.LOAD.RMX4005
{
    using NUnit.Framework;

    [TestFixture]
    internal class RMX4005_LOAD_UnitTest
    {
        string RESOURCE_NAME = "ELOAD";

        [Test]
        public void Identify()
        {
            /*
             * "NATIONAL INSTRUMENTS,RMX-4002,GEV200429,V2.16"
             */
            using (var load = new RMX4005(RESOURCE_NAME, simulation: false))
            {
                var resp = load.Identify().ToUpper();
                Assert.IsTrue(resp.Contains("NATIONAL INSTRUMENTS"));
            }
        }


        [Test]
        public void ConfigLoad()
        {
            using (var load = new RMX4005(RESOURCE_NAME, simulation: false))
            {
                var resp = load.Identify().ToUpper();
                Assert.IsTrue(resp.Contains("NATIONAL INSTRUMENTS"));


                load.OverVoltageProtection(channel: "1", voltage: 30.0);
                load.UnderVoltageProtection(channel: "1", voltage: 30.0);
                load.Config(mode: "CCH-VL", channel: "1", current: 1.0);
                load.OnOff(enable: true);

                var errors = load.GetErrors();
                Assert.AreEqual(0, errors.Count);
            }
        }

        [Test]
        public void ConfigLoad_LowRange_ConstantCurrentX()
        {
            using (var load = new RMX4005(RESOURCE_NAME, simulation: false))
            {
                var resp = load.Identify().ToUpper();
                Assert.IsTrue(resp.Contains("NATIONAL INSTRUMENTS"));

                load.Config(mode: "CCL-VL", channel: "1", current: 0.03);
                load.OnOff(enable: true);
            }
        }

        [Test]
        public void ConfigLoad_LowRange_ConstantCurrent()
        {
            using (var load = new RMX4005(RESOURCE_NAME, simulation: false))
            {
                var resp = load.Identify().ToUpper();
                Assert.IsTrue(resp.Contains("NATIONAL INSTRUMENTS"));

                load.Config(mode: "CCL-VL", channel: "1", current: 1.0);
                load.OnOff(enable: true);

                double current = load.GetActualCurrent();
                Assert.Less(current, 0.1);
                Assert.Greater(current, -0.1);


                double voltage = load.GetActualVolt();
                Assert.Less(voltage, 0.1);
                Assert.Greater(voltage, -0.1);

                var errors = load.GetErrors();
                Assert.AreEqual(0 , errors.Count);
            }
        }


        [Test]
        public void ConfigLoad_HighRange_ConstantCurrent()
        {
            using (var load = new RMX4005(RESOURCE_NAME, simulation: false))
            {
                var resp = load.Identify().ToUpper();
                Assert.IsTrue(resp.Contains("NATIONAL INSTRUMENTS"));

                load.Config(mode: "CCH-VL", channel: "1", current: 69.0);
                load.OnOff(enable: true);

                double current = load.GetActualCurrent();
                Assert.Less(current, 0.1);
                Assert.Greater(current, -0.1);


                double voltage = load.GetActualVolt();
                Assert.Less(voltage, 0.1);
                Assert.Greater(voltage, -0.1);

                var errors = load.GetErrors();
                Assert.AreEqual(0, errors.Count);
            }
        }
    }
}

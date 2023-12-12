
namespace Knv.Instr.LOAD.BK8600
{
    using NUnit.Framework;

    [TestFixture]
    internal class BK8600_UnitTest
    {
        string RESOURCE_NAME = "ELOAD-BK8600";

        [Test]
        public void Identify()
        {
            /*
             * "B&K PRECISION, 8600, 802197036737120008, 1.37-1.42"
             */
            using (var load = new BK8600(RESOURCE_NAME, simulation: false))
            {
                var resp = load.Identify().ToUpper();
                Assert.IsTrue(resp.Contains("B&K PRECISION"));
            }
        }


        [Test]
        public void ConfigConstantCurrentHighModeInputEnable()
        {
            using (var load = new BK8600(RESOURCE_NAME, simulation: false))
            {
                var resp = load.Identify().ToUpper();
                Assert.IsTrue(resp.Contains("B&K PRECISION"));

                load.Config(mode: "CCH", current: 1.0);
                load.OnOff(enable: true);

                var errors = load.GetErrors();
                Assert.AreEqual(0, errors.Count);
            }
        }

        [Test]
        public void ConfigLoad_LowRange_ConstantCurrentX()
        {
            using (var load = new BK8600(RESOURCE_NAME, simulation: false))
            {
                var resp = load.Identify().ToUpper();
                Assert.IsTrue(resp.Contains("B&K PRECISION"));
                load.Config(mode: "CCL", current: 0.5);
                load.OnOff(enable: true);
            }
        }

        [Test]
        public void ConfigLoad_LowRange_ConstantCurrent()
        {
            using (var load = new BK8600(RESOURCE_NAME, simulation: false))
            {
                var resp = load.Identify().ToUpper();
                Assert.IsTrue(resp.Contains("B&K PRECISION"));

                load.Config(mode: "CCL", current: 1.0);
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
    }
}

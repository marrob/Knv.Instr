
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
            using (var load = new RMX4005(RESOURCE_NAME, simulation: false))
            {
                var resp = load.Identify();
                Assert.IsTrue(resp.Contains("National Instruments"));
            }
        }

        [Test]
        public void MeasureVoltSmallestRange()
        {
            using (var load = new RMX4005(RESOURCE_NAME, simulation: false))
            {
                var resp = load.Identify();
                Assert.IsTrue(resp.Contains("National Instruments"));


            }
        }
    }
}

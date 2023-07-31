
namespace Knv.Instr.DMM.PXI4082
{
    using NUnit.Framework;

    [TestFixture]
    internal class PXI4082_DMM_UnitTest
    {
        string RESOURCE_NAME = "J3_DMM";

        [Test]
        public void Identify()
        {
            using (var dmm = new PXI4082(RESOURCE_NAME, simulation: false))
            {
                var resp = dmm.Identify();
                Assert.IsTrue(resp.Contains("National Instruments"));
            }
        }

        [Test]
        public void MeasureVoltSmallestRange()
        {
            using (var dmm = new PXI4082(RESOURCE_NAME, simulation: false))
            {
                var resp = dmm.Identify();
                Assert.IsTrue(resp.Contains("National Instruments"));

                dmm.Config("DCV", rangeName: "1V");
                var measValue = dmm.Read();
                Assert.IsTrue(-0.5 < measValue && measValue < 0.5);

            }
        }

        [Test]
        public void Measure2WireResistance()
        {
            using (var dmm = new PXI4082(RESOURCE_NAME, simulation: false))
            {
                var resp = dmm.Identify();
                Assert.IsTrue(resp.Contains("National Instruments"));
                 
                dmm.Config("2WR", rangeName: "100K");
                var measValue = dmm.Read();
                Assert.IsTrue(double.IsNaN(measValue));

            }
        }
    }
}

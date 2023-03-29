
namespace Knv.Instr.DMM.PXI4082
{
    using NUnit.Framework;

    [TestFixture]
    internal class PXI4082_DMM_UnitTest
    {
        string VISA_NAME = "J3_DMM";

        [Test]
        public void Identify()
        {
            using (var dmm = new PXI4082(VISA_NAME, isSim: false))
            {
                var resp = dmm.Identify();
                Assert.IsTrue(resp.Contains("National Instruments"));
            }
        }

        [Test]
        public void MeasureVoltSmallestRange()
        {
            using (var dmm = new PXI4082(VISA_NAME, isSim: false))
            {
                var resp = dmm.Identify();
                Assert.IsTrue(resp.Contains("National Instruments"));

                dmm.Config("DCV", range: 0.1);
                var measValue = dmm.Read();
                Assert.IsTrue(-0.5 < measValue && measValue < 0.5);

            }
        }

        [Test]
        public void Measure2WireResistance()
        {
            using (var dmm = new PXI4082(VISA_NAME, isSim: false))
            {
                var resp = dmm.Identify();
                Assert.IsTrue(resp.Contains("National Instruments"));
                 
                dmm.Config("2WR", range: 100000000);
                var measValue = dmm.Read();
                Assert.IsTrue(double.IsNaN(measValue));

            }
        }
    }
}

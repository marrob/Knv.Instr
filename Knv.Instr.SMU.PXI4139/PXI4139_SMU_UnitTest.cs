
namespace Knv.Instr.SMU.PXI4139
{
    using NUnit.Framework;

    [TestFixture]
    internal class PXI4139_SMU_UnitTest
    {
        string VISA_NAME = "SMU1";



        [Test]
        public void Identify()
        {
            using (var smu = new PXI4139(VISA_NAME, simulation: false))
            {
                var resp = smu.Identify();
                Assert.IsTrue(resp.Contains("National Instruments"));
            }
        }

#if false
        [Test]
        public void MeasureVoltSmallestRange()
        {
            using (var dmm = new PXI4139(VISA_NAME, isSim: false))
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
            using (var dmm = new PXI4139(VISA_NAME, isSim: false))
            {
                var resp = dmm.Identify();
                Assert.IsTrue(resp.Contains("National Instruments"));
                 
                dmm.Config("2WR", rangeName: "100K");
                var measValue = dmm.Read();
                Assert.IsTrue(double.IsNaN(measValue));

            }
        }
#endif
    }


}

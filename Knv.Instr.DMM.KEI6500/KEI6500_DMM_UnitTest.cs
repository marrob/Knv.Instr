
namespace Knv.Instr.DMM.KEI6500
{
    using NUnit.Framework;
    using System.Reflection;
    using System.Threading;

    [TestFixture]
    internal class KEI6500_DMM_UnitTest
    {
        string VISA_NAME = "KEI6500";

        [Test]
        public void Identify()
        {
            using (var dmm = new KEI6500(VISA_NAME, simulation: false))
            {
                try
                {
                    var resp = dmm.Identify();
                    Assert.IsTrue(resp.Contains("KEITHLEY INSTRUMENTS INC."));
                    Assert.IsTrue(resp.Contains("MODEL 6500"));
                }
                finally
                {
                    dmm.LogSave(Constants.LogRootDirecotry, $"{MethodBase.GetCurrentMethod().DeclaringType.Name}_{MethodBase.GetCurrentMethod().Name}");
                }
            
            }
        }

        [Test]
        public void MeasureVoltSmallestRange()
        {
            using (var dmm = new KEI6500(VISA_NAME, simulation: false))
            {
                var resp = dmm.Identify();
                Assert.IsTrue(resp.Contains("KEITHLEY INSTRUMENTS INC."));

                dmm.Config("DCV", rangeName: "1V");
                var measValue = dmm.Read();
                Assert.IsTrue(-0.5 < measValue && measValue < 0.5);

            }
        }

        [Test]
        public void WriteTestToDiaplay()
        {
            using (var dmm = new KEI6500(VISA_NAME, simulation: false))
            {
                try
                {
                    var resp = dmm.Identify();
                    Assert.IsTrue(resp.Contains("KEITHLEY INSTRUMENTS INC."));

                    for (int i = 0; i < 100; i++)
                    {
                        dmm.WriteTextToDisplay($"MIKI TE FASZ {i}");
                        Thread.Sleep(20);
                    }
                }
                finally
                {
                    dmm.LogSave(Constants.LogRootDirecotry, $"{MethodBase.GetCurrentMethod().DeclaringType.Name}_{MethodBase.GetCurrentMethod().Name}");
                }

            }
        }
    }
}


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
                    Assert.IsTrue(resp.Contains("KEITHLEY INSTRUMENTS"));
                    Assert.IsTrue(resp.Contains("DMM6500"));
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
                Assert.IsTrue(resp.Contains("KEITHLEY INSTRUMENTS"));

                dmm.Config("DCV", rangeName: "1V");
                var measValue = dmm.Read();
                Assert.IsTrue(-0.5 < measValue && measValue < 0.5);

            }
        }


        [Test]
        public void ConfigurationTest()
        {
            double measValue = 0;

            using (var dmm = new KEI6500(VISA_NAME, simulation: false))
            {
                
                var resp = dmm.Identify();
                Assert.IsTrue(resp.Contains("KEITHLEY INSTRUMENTS"));

                dmm.Config("DCV", rangeName: "1");
                measValue = dmm.Read();
                Assert.IsTrue(-0.5 < measValue && measValue < 0.5);


                dmm.Config("DCV", rangeName: "100e-3");
                measValue = dmm.Read();
                Assert.IsTrue(-0.5 < measValue && measValue < 0.5);


                dmm.Config("DCV", rangeName: "1000");
                measValue = dmm.Read();
                Assert.IsTrue(-0.5 < measValue && measValue < 0.5);

                dmm.LogSave("c:\\Users\\Public\\Documents\\", "KEI6500");
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
                    Assert.IsTrue(resp.Contains("KEITHLEY INSTRUMENTS"));

                    for (int i = 0; i < 100; i++)
                    {
                        dmm.WriteTextToDisplay($"MIKI TE FASZ!");
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

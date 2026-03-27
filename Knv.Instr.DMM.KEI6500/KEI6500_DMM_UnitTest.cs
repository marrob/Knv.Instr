
namespace Knv.Instr.DMM.KEI6500
{
    using NUnit.Framework;
    using System;
    using System.Reflection;
    using System.Threading;

    [TestFixture]
    internal class KEI6500_DMM_UnitTest
    {
        readonly string VISA_NAME = "KEI6500";
        readonly string LOG_PATH = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments); //c:\\Users\\Public\\Documents\\

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
        public void DCV_MeasureVoltSmallestRange_UnitTest()
        {
            using (var dmm = new KEI6500(VISA_NAME, simulation: false))
            {
                var resp = dmm.Identify();
                Assert.IsTrue(resp.Contains("KEITHLEY INSTRUMENTS"));

                dmm.Config("DCV", rangeName: "1");
                var measValue = dmm.Read();
                Assert.IsTrue(-0.5 < measValue && measValue < 0.5);
            }
        }


        [Test]
        public void DCV_Configuration_UnitTest()
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

                dmm.LogSave(LOG_PATH, "KEI6500__DCV");
            }
        }

        [Test]
        public void RES_2WR_Configuration_UnitTest()
        {
            double measValue = 0;

            using (var dmm = new KEI6500(VISA_NAME, simulation: false))
            {

                var resp = dmm.Identify();
                Assert.IsTrue(resp.Contains("KEITHLEY INSTRUMENTS"));

                dmm.Config("2WR", rangeName: "1");
                measValue = dmm.Read();
                Assert.AreEqual(measValue, 9.9E+37);

                dmm.Config("2WR", rangeName: "10");
                measValue = dmm.Read();
                Assert.AreEqual(measValue, 9.9E+37);

                dmm.Config("2WR", rangeName: "100");
                measValue = dmm.Read();
                Assert.AreEqual(measValue, 9.9E+37);

                dmm.Config("2WR", rangeName: "1000");
                measValue = dmm.Read();
                Assert.AreEqual(measValue, 9.9E+37);

                dmm.Config("2WR", rangeName: "10e+3");
                measValue = dmm.Read();
                Assert.AreEqual(measValue, 9.9E+37);

                dmm.Config("2WR", rangeName: "100e+3");
                measValue = dmm.Read();
                Assert.AreEqual(measValue, 9.9E+37);

                dmm.Config("2WR", rangeName: "1e+6");
                measValue = dmm.Read();
                Assert.AreEqual(measValue, 9.9E+37);

                dmm.Config("2WR", rangeName: "10e+6");
                measValue = dmm.Read();
                Assert.AreEqual(measValue, 9.9E+37);

                dmm.Config("2WR", rangeName: "100e+6");
                measValue = dmm.Read();
                Assert.AreEqual(measValue, 9.9E+37);

                dmm.LogSave(LOG_PATH, "KEI6500__2WR");
            }
        }

        [Test]
        public void RES_4WR_Configuration_UnitTest()
        {
            double measValue = 0;

            using (var dmm = new KEI6500(VISA_NAME, simulation: false))
            {

                var resp = dmm.Identify();
                Assert.IsTrue(resp.Contains("KEITHLEY INSTRUMENTS"));

                dmm.Config("4WR", rangeName: "1");
                measValue = dmm.Read();
                Assert.AreEqual(measValue, 9.9E+37);

                dmm.Config("4WR", rangeName: "10");
                measValue = dmm.Read();
                Assert.AreEqual(measValue, 9.9E+37);

                dmm.Config("4WR", rangeName: "100");
                measValue = dmm.Read();
                Assert.AreEqual(measValue, 9.9E+37);

                dmm.Config("4WR", rangeName: "1000");
                measValue = dmm.Read();
                Assert.AreEqual(measValue, 9.9E+37);

                dmm.Config("4WR", rangeName: "10e+3");
                measValue = dmm.Read();
                Assert.AreEqual(measValue, 9.9E+37);

                dmm.LogSave(LOG_PATH, "KEI6500__4WR");
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

                dmm.LogSave(LOG_PATH, "KEI6500__TEXT");
            }
        }
    }
}

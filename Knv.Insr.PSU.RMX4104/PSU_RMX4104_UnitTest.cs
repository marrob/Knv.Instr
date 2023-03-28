
namespace Knv.Instr.PSU.RMX4104
{
    using Ivi.Visa;
    using NUnit.Framework;
    using NUnit.Framework.Internal.Execution;
    using System.Reflection;
    using System.Runtime.Remoting.Messaging;
    using System.Threading;


    [TestFixture]
    internal class PSU_RMX4104_UnitTest
    {

        string VISA_NAME = "J24_PPS_1";
        [Test]
        public void Identify()
        {

            using (var psu = new RMX4104(VISA_NAME, isSim:false))
            { 
                var resp = psu.Identify();
                Assert.IsTrue(resp.Contains("NATIONAL INSTRUMENTS"));
                Assert.IsTrue(resp.Contains("RMX36-24-LAN"));
                psu.LogSave(Constants.LogRootDirecotry, MethodBase.GetCurrentMethod().Name);
            }
        }

        [Test]
        public void RawGetVolt()
        {
            using (var psu = new RMX4104(VISA_NAME, isSim: false))
            {
                var resp = psu.Query(":MEAS:VOLT?;");
                Assert.IsTrue(!string.IsNullOrEmpty(resp));
            }
        }

        [Test]
        public void SetVoltCurrent()
        {
            using (var psu = new RMX4104(VISA_NAME, isSim: false))
            {
                var resp = psu.Identify();
                Assert.IsTrue(resp.Contains("RMX36-24-LAN"));
                psu.SetOutput(volt: 12, current: 0.1);
                psu.LogSave(Constants.LogRootDirecotry, MethodBase.GetCurrentMethod().Name);
            }
        }

        [Test]
        public void GetActualVolts()
        {
            using (var psu = new RMX4104(VISA_NAME, isSim: false))
            {
                var resp = psu.Identify();
                psu.SetOutput(volt: 0, current: 0.1, onOff: true);
                Thread.Sleep(100);
                double value = psu.GetActualVolt();
                Assert.IsTrue(value < 1);
                psu.LogSave(Constants.LogRootDirecotry, MethodBase.GetCurrentMethod().Name);
            }
        }

        [Test]
        public void GetActualCurrent()
        {
            using (var psu = new RMX4104(VISA_NAME, isSim: false))
            {
                var resp = psu.Identify();
                double value = psu.GetActualVolt();
                Assert.IsTrue(value < 1);
                psu.LogSave(Constants.LogRootDirecotry, MethodBase.GetCurrentMethod().Name);
            }
        }


        [TestCase(-0.1, 0, 0.1, 11, 12, 13)]
        public void SetVoltCurrentOnOff(double min1, double nominal1, double max1, double min2, double nominal2, double max2)
        {
            using (var psu = new RMX4104(VISA_NAME, isSim: false))
            {
                try
                {
                    var resp = psu.Identify();
                    Assert.IsTrue(resp.Contains("RMX36-24-LAN"));
                    double volts = 0;

                    psu.SetOutput(volt: nominal1, current: 0.1, onOff: true);

                    for (int i = 0; i < 10; i++)
                    { 
                        volts = psu.GetActualVolt();
                        if (min1 < volts && volts < max1)
                            break;

                        Thread.Sleep(50);
                    }
                    Assert.IsTrue(min1 < volts && volts < max1);


                    psu.SetOutput(volt: nominal2, current: 0.1, onOff: true);
                    for (int i = 0; i < 10; i++)
                    {
                        volts = psu.GetActualVolt();
                        if (min2 < volts && volts < max2)
                            break;
                        Thread.Sleep(50);
                    }
                    Assert.IsTrue(min2 < volts && volts < max2);
                }
                finally
                {
                    psu.LogSave(Constants.LogRootDirecotry, MethodBase.GetCurrentMethod().Name);
                }
            }
        }

        [Test]
        public void GenericIdentfy()
        {
            /*
            using (var genericPsu = new GenericPowerSupply(new HP6634A_PSU(ADDRESS, false)))
            {
                var resp = genericPsu.Identify();
                Assert.AreEqual("HP6634A", resp);
                genericPsu.SetOutput(volt: 12, current: 01);

            }*/
        
        }

    }
}

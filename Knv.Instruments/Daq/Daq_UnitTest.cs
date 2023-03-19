

namespace Knv.Instruments.Daq
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using NUnit.Framework;

    [TestFixture]
    internal class Daq_UnitTest
    {
        [Test]
        public void DeviceIsPresent()
        {
            Assert.IsTrue(DaqTools.DeviceIsPresent("Dev1"));  
        }


        [Test]
        public void GetOneVoltage()
        {
            var value = DaqTools.AIgetOneVolt("Dev1", "ai0");
            Assert.IsTrue(value != 0);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(-1)]
        public void SetVoltage(double value) 
        {
            DaqTools.AOsetVoltage("Dev1", "ao0", value);
        }

        [Test]
        public void SignalGenerator0()
        {
            using (var sg1 = new SignalGenerator("Dev1"))
            {

                sg1.Start("ao0");
                System.Threading.Thread.Sleep(100000);
                sg1.Stop();

            }
        }

        [Test]
        public void SignalGenerator()
        {
            using (var sg1 = new SignalGenerator("Dev1"))
            {
                using (var sg2 = new SignalGenerator("Dev1"))
                {
                    sg1.Start("ao0");
                    sg2.Start("ao1");
                    System.Threading.Thread.Sleep(100000);
                    sg1.Stop();
                    sg2.Stop();

                }
            }
        }
    }
}

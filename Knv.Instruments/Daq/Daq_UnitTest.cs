

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


     

    }
}

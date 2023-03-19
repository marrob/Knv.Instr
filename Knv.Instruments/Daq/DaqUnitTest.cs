

namespace Knv.Instruments.Daq
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using NUnit.Framework;

    [TestFixture]
    internal class DaqUnitTest
    {

        [Test]
        public void GetOneVoltAsTool()
        {
            var value = DaqTools.GetOneVolt("Dev1", "ai0");
            Assert.IsTrue(value != 0);
        }

    }
}

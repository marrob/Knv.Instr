
namespace Knv.Instr.DAC.M9185A
{
    using NUnit.Framework;
    using System;
    using System.Diagnostics;

    [TestFixture]
    internal class KEI2100_DMM_UnitTest
    {
        const string VISA_NAME = "PXI14::13::0::INSTR";

        [Test]
        public void Identify()
        {

            using (M9185 dac = new M9185(VISA_NAME))
            {
                dac.Open();
                Console.WriteLine(dac.Description());
                dac.VoltageOutputConfig(10, 5);
                //dac.VoltageOutputConfig(2, 5);
            }
        }
    }
}

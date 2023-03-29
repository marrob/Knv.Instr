
namespace Knv.Instr.PSU.RMX4104
{
    using Knv.Instr.DMM.KEI2100;
    using NUnit.Framework;
    using System.Reflection;
    using System.Threading;

    [TestFixture]
    internal class KEI2100_DMM_UnitTest
    {
        string VISA_NAME = "KEI2100";

        [Test]
        public void Identify()
        {
            using (var psu = new KEI2100(VISA_NAME, isSim: false))
            {
                try
                {
                    var resp = psu.Identify();
                    Assert.IsTrue(resp.Contains("KEITHLEY INSTRUMENTS INC."));
                    Assert.IsTrue(resp.Contains("MODEL 2100"));
                }
                finally
                {
                    psu.LogSave(Constants.LogRootDirecotry, $"{MethodBase.GetCurrentMethod().DeclaringType.Name}_{MethodBase.GetCurrentMethod().Name}");
                }
            
            }
        }
    }
}

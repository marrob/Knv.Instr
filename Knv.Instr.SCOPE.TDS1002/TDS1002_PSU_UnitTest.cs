namespace Knv.Instr.PSU.HP6634A
{
    using NUnit.Framework;
    using System.Reflection;
    using System.Threading;
    using Knv.Instr.SCOPE.TDS1002;

    [TestFixture]
    internal class TDS1002_PSU_UnitTest
    {
        const string VISA_NAME = "TDS1002";

        [Test]
        public void Identify()
        {
            using (var psu = new TDS1002(VISA_NAME, isSim: false))
            {
                try
                {
                    var resp = psu.Identify();
                    Assert.IsTrue(resp.Contains("TDS 1002"));
                }
                finally
                {
                    psu.LogSave(Constants.LogRootDirecotry, $"{MethodBase.GetCurrentMethod().DeclaringType.Name}_{MethodBase.GetCurrentMethod().Name}");
                }
            }
        }
    }
}

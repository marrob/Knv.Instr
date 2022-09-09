using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace N6972A
{
    [TestFixture]
    public class N6972A_UnitTest
    {
        N6972ALAN _psp;

        [SetUp]
        public void TestSetup(){
            _psp = new N6972ALAN("192.168.1.101", 5025);
        }


        [Test]
        public void PowerSupplyIDN() {

            var response = _psp.WriteReadLine("*IDN?");
            Assert.AreEqual("Agilent Technologies,N6972A,MY59170474,B.02.03.1268", response);
        }

    }
}

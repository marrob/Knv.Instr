using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knv.Instr.DAQ.PCI6353
{


    [TestFixture]
    internal class Math_UnitTest
    {

        [Test]
        public void SineOne()
        { 
            Assert.AreEqual(1, Math.Sin(Math.PI/2));
        }



        [Test]
        public void SineDeg()
        {
            Assert.AreEqual(1.2246063538223773E-16d, Math.Sin(Math.PI));
            Assert.AreEqual(1, Math.Sin(Math.PI/2));
            //  Assert.AreEqual(1, Math.Sin(Math.PI/2));

           Assert.AreEqual(1, Math.Sin(Math.PI/180 * 90 ));
        }


    }
}
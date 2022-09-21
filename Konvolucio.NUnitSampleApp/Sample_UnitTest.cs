using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Threading;

namespace NUnitTeasApp
{
    [TestFixture]
    internal class Sample_UnitTest
    {
        bool _ResourceIsBusy ;

        /// <summary>
        /// Ez minden teszt előt 1x biztos lefut
        /// Valamilyen erőforrás lefoglalásához jó
        /// </summary>
        [SetUp]
        public void TestSetup()
        {
            
            _ResourceIsBusy = true;
            Console.WriteLine($"SampleTimoeoutTest.TestSetup.{_ResourceIsBusy} ");
        }

        /// <summary>
        /// 
        /// </summary>
        [TearDown]
        public void TestCleanUp()
        {
            _ResourceIsBusy = false;
            Console.WriteLine($"SampleTimoeoutTest.TestCleanUp.{_ResourceIsBusy} ");
        }

        [Test]
        public void AddTwoNumber()
        {
            Assert.AreEqual(2, 1 + 1);
        }

        [TestCase(5, 1, Description = "arg1 > arg2 ?")]
        [TestCase(6, 2, Description = "arg1 > arg2 ?")]
        public void Arguments(int arg1, int arg2)
        {
            Assert.IsTrue(arg1 > arg2);
        }

        [Test]
        public void SampleTimoeoutTest()
        {
            var wait = new AutoResetEvent(false);

            Action asyncMethod = () =>
            {
                Thread.Sleep(900);
            };

            AsyncCallback cpltCallback = (itfAr) =>
            {
                Console.WriteLine("SampleTimoeoutTest.Complete.");
                wait.Set();
            };

           var result = asyncMethod.BeginInvoke(cpltCallback, null);


            if (!wait.WaitOne(TimeSpan.FromSeconds(1)))
                Assert.Fail("Timeout");

            if (result is Exception)
                Assert.Fail((result as Exception).Message);

        }
    }
}

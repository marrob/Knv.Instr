using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Diagnostics;
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

        [Test]
        public void IncraseOutputVoltage(){

            _psp.WriteLine($"VOLT 0");
            double volts = 0;
            for (int i = 0; i < 79; i++)
            {
                _psp.WriteLine($"VOLT {volts}");
                volts += 0.5;
                System.Threading.Thread.Sleep(500);
            }
        }

        [Test]
        public void SetVoltAndEnableOutput()
        {
            _psp.WriteLine($"VOLT 10");
            _psp.WriteLine("OUTP ON");
            Assert.AreEqual(10, double.Parse(_psp.WriteReadLine("VOLT?")));
        }


        [Test]
        public void RumpUpTest()
        {
            var sw = new Stopwatch();

            _psp.WriteLine("OUTP ON");
            _psp.WriteLine($"VOLT 0");
            double volts = 0;
            do{
                volts = double.Parse(_psp.WriteReadLine("VOLT?"));
            } while (volts > 0.1);
            sw.Start();

            _psp.WriteLine($"VOLT 40");
            do{
                volts = double.Parse(_psp.WriteReadLine("VOLT?"));
            } while (volts < 39.9);
            sw.Stop();

            Console.WriteLine($"RumpUp Time {sw.ElapsedMilliseconds} ms"); 

        }


        [Test]
        public void DownProgTest()
        {
            var sw = new Stopwatch();

            _psp.WriteLine("OUTP ON");
            _psp.WriteLine($"VOLT 40");
            System.Threading.Thread.Sleep(2000);
            double volts = 0;
            do
            {
                volts = double.Parse(_psp.WriteReadLine("VOLT?"));
            } while (volts < 39.9);
            sw.Start();

            _psp.WriteLine($"VOLT 0");
            do
            {
                volts = double.Parse(_psp.WriteReadLine("VOLT?"));
            } while (volts > 0.1);
            sw.Stop();

            Console.WriteLine($"DownProg Time {sw.ElapsedMilliseconds} ms");

        }


    }
}

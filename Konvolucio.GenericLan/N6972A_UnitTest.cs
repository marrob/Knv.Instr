using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Diagnostics;

namespace Konvolucio.PsuOverLan
{
    [TestFixture]
    public class N6972A_UnitTest
    {
        Lan PSU;

        [SetUp]
        public void TestSetup()
        {
            PSU = new Lan();
            PSU.Open("192.168.1.101", 5025);
        }


        [Test]
        public void PowerSupplyIDN() {

            var response = PSU.WriteReadLine("*IDN?");
            Assert.AreEqual("Agilent Technologies,N6972A,MY59170474,B.02.03.1268", response);
        }

        [Test]
        public void IncraseOutputVoltage(){

            PSU.WriteLine($"VOLT 0");
            double volts = 0;
            for (int i = 0; i < 79; i++)
            {
                PSU.WriteLine($"VOLT {volts}");
                volts += 0.5;
                System.Threading.Thread.Sleep(500);
            }
        }

        [Test]
        public void SetVoltAndEnableOutput()
        {
            PSU.WriteLine($"VOLT 10");
            PSU.WriteLine("OUTP ON");
            Assert.AreEqual(10, double.Parse(PSU.WriteReadLine("VOLT?")));
        }


        [Test]
        public void RumpUpTest()
        {
            var sw = new Stopwatch();

            PSU.WriteLine("OUTP ON");
            PSU.WriteLine($"VOLT 0");
            double volts = 0;
            do{
                volts = double.Parse(PSU.WriteReadLine("VOLT?"));
            } while (volts > 0.1);
            sw.Start();

            PSU.WriteLine($"VOLT 40");
            do{
                volts = double.Parse(PSU.WriteReadLine("VOLT?"));
            } while (volts < 39.9);
            sw.Stop();

            Console.WriteLine($"RumpUp Time {sw.ElapsedMilliseconds} ms"); 

        }


        [Test]
        public void DownProgTest()
        {
            var sw = new Stopwatch();

            PSU.WriteLine("OUTP ON");
            PSU.WriteLine($"VOLT 40");
            System.Threading.Thread.Sleep(2000);
            double volts = 0;
            do
            {
                volts = double.Parse(PSU.WriteReadLine("VOLT?"));
            } while (volts < 39.9);
            sw.Start();

            PSU.WriteLine($"VOLT 0");
            do
            {
                volts = double.Parse(PSU.WriteReadLine("VOLT?"));
            } while (volts > 0.1);
            sw.Stop();

            Console.WriteLine($"DownProg Time {sw.ElapsedMilliseconds} ms");

        }


    }
}

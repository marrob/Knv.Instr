
/*
 * 
 * .NET Resources for NI Hardware and Software
 * https://www.ni.com/hu-hu/support/documentation/supplemental/13/national-instruments--net-support.html
 * 
 * NI-DAQmx
 * <Public Documents>\National Instruments\NI-DAQ\Documentation
 * Examples: 
 * Users\Public\Documents\National Instruments\NI-DAQ\Examples\DotNET<x.x>
 * 
 * 
 * 
 * Assembly NationalInstruments.Common:
 * C:\Program Files (x86)\National Instruments\Measurement Studio\DotNET\v4.0\AnyCPU\NationalInstruments.Common 19.1.40
 *
 * Assembly NationalInstruments.DAQmx
 * C:\Program Files (x86)\National Instruments\MeasurementStudioVS2012\DotNET\Assemblies (64-bit)\Current\NationalInstruments.DAQmx.dll
 *
 */
namespace Knv.Instr.DAQ.PCI6353
{
    using NUnit.Framework;

    [TestFixture]
    internal class DAQ_Basic_UnitTest
    {
        [Test]
        public void DeviceIsPresent()
        {
            Assert.IsTrue(Tools.DeviceIsPresent("Dev1"));  
        }


        [Test]
        public void GetOneVoltage()
        {
            var value = Tools.AIgetOneVolt("Dev1", "ai0");
            Assert.IsTrue(value != 0);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(-1)]
        public void SetVoltage(double value) 
        {
            Tools.AOsetVoltage("Dev1", "ao0", value);
        }

        [Test]
        public void SignalGenerator0()
        {
            using (var sg1 = new SignalGenerator("Dev1"))
            {

                sg1.Start("ao0");
                System.Threading.Thread.Sleep(100000);
                sg1.Stop();

            }
        }

        [Test]
        public void SignalGenerator()
        {
            using (var sg1 = new SignalGenerator("Dev1"))
            {
                using (var sg2 = new SignalGenerator("Dev1"))
                {
                    sg1.Start("ao0");
                    sg2.Start("ao1");
                    System.Threading.Thread.Sleep(100000);
                    sg1.Stop();
                    sg2.Stop();

                }
            }
        }
    }
}

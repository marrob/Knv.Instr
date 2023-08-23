
namespace Knv.Instr.GEN.PXI5413
{
    using NUnit.Framework;
    using System.Timers;
    using System;
    using System.IO;
    using System.Threading;

    [TestFixture]
    internal class PXI5413_UnitTest
    {
        string RESOURCE_NAME = "J22_FGEN_1";

        [Test]
        public void SineWaveGeneration()
        {
            var samples = Signals.SineGen(offset: 0, amplitude: 1, samples: 100);
            var mydoc = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            Signals.DoubleArrayToFile(samples, $@"{mydoc}\sivewave.csv");
        }

        [Test]
        public void PwmGeneration_50_sample10()
        {
            var samples = Signals.PwmGen(offset: 0, amplitude: 1, dutyCycle: 0.5, samples: 10);
            var mydoc = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            Signals.DoubleArrayToFile(samples, $@"{mydoc}\pwm.csv");
        }

        [Test]
        public void PwmGeneration()
        {
            var samples = Signals.PwmGen(offset: 0, amplitude: 1, 0.5, 100);
            var mydoc = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            Signals.DoubleArrayToFile(samples, $@"{mydoc}\pwm.csv");
        }


        [Test]
        public void Identify()
        {
            /*
             * "NATIONAL INSTRUMENTS, NI PXIE-5413 (2CH), 0X0209F43E"
             */
            using (var gen = new PXI5413(RESOURCE_NAME, "0", simulation: false))
            {
                var resp = gen.Identify().ToUpper();
                Assert.IsTrue(resp.Contains("NATIONAL INSTRUMENTS"));
            }
        }

        [Test]
        public void Generator_SampleConfig()
        {
            using (var gen = new PXI5413(RESOURCE_NAME, "0", simulation: false))
            {
                var resp = gen.Identify().ToUpper();
                Assert.IsTrue(resp.Contains("NATIONAL INSTRUMENTS"));
                gen.ConfigWaveform(waveformName: "Square", amplitudeVpp: 1, frequencyHz: 100, offsetVp: 0.5);
                gen.Start();
                Thread.Sleep(1000);
                gen.Stop();
            }
        }

        [Test]
        public void Generator_Change_Parameter_RunTime()
        {
            using (var gen = new PXI5413(RESOURCE_NAME, "0", simulation: false))
            {
                var resp = gen.Identify().ToUpper();
                Assert.IsTrue(resp.Contains("NATIONAL INSTRUMENTS"));
                gen.ConfigWaveform(waveformName: "Square", amplitudeVpp: 1, frequencyHz: 100, offsetVp: 0.5, dutyCycle: 50);
                gen.Start();
                System.Threading.Thread.Sleep(1000);
                gen.Stop();

            }
        }

        [Test]
        public void Generator_DutyCycle()
        {
            using (var gen = new PXI5413(RESOURCE_NAME, "0", simulation: false))
            {
                var resp = gen.Identify().ToUpper();
                Assert.IsTrue(resp.Contains("NATIONAL INSTRUMENTS"));
                gen.ConfigWaveform(waveformName: "Square", amplitudeVpp: 1, frequencyHz: 10000, offsetVp: 0.5);
                gen.Start();
                Thread.Sleep(1000);
                gen.Stop();

                Thread.Sleep(1000);
                gen.ConfigWaveform(waveformName: "Square", amplitudeVpp: 1, frequencyHz: 10000, offsetVp: 0.5);
                gen.Start();
                Thread.Sleep(1000);
                gen.Stop();
            }
        }


        [Test]
        public void MultiGenerator()
        {
            using (var gen0 = new PXI5413(RESOURCE_NAME, "0", simulation: false))
            {
                var resp = gen0.Identify().ToUpper();
                Assert.IsTrue(resp.Contains("NATIONAL INSTRUMENTS"));
                gen0.ConfigWaveform(waveformName: "Square", amplitudeVpp: 1, frequencyHz: 10000, offsetVp: 0.5);
                gen0.Start();
                System.Threading.Thread.Sleep(1000);
                gen0.Stop();

                using (var gen1 = new PXI5413(RESOURCE_NAME, "1", simulation: false))
                {
                    resp = gen1.Identify().ToUpper();
                    Assert.IsTrue(resp.Contains("NATIONAL INSTRUMENTS"));
                    gen1.ConfigWaveform(waveformName: "Square", amplitudeVpp: 1, frequencyHz: 10000, offsetVp: 0.5);
                    gen1.Start();
                    System.Threading.Thread.Sleep(1000);
                    gen1.Stop();
                }
            }
        }


        [Test]
        public void Pwm()
        { 
            using(var gen = new PXI5413(RESOURCE_NAME, "0", simulation: false))
            {
                var resp = gen.Identify().ToUpper();
                Assert.IsTrue(resp.Contains("NATIONAL INSTRUMENTS"));
                gen.ConfigPwm(vpp: 2, offset: 0, frequencyHz: 100, dutyCycle: 50);
                gen.Start();
                System.Threading.Thread.Sleep(1000);
                gen.Stop();
            }
        
        }

        [Test]
        public void PwmGenerationTTC500_6_3_15_S01()
        {
            /*
             * Apply 1kHz rectangle 50% duty cycle, 1Vpp, 2.5VDC to CPs
             */

            using (var gen = new PXI5413(RESOURCE_NAME, "0", simulation: false))
            {
                var resp = gen.Identify().ToUpper();
                Assert.IsTrue(resp.Contains("NATIONAL INSTRUMENTS"));
                gen.ConfigPwm(vpp: 1, offset: 2.5, frequencyHz: 1000, dutyCycle: 50);
                gen.Start();
                System.Threading.Thread.Sleep(5000);
                gen.Stop();
            }
        }


        [Test]
        public void PwmGenerationTTC500_6_3_15_S02()
        {
            /*
             * High 90R * 14mA = 1.26V
             * Low 90R * 7mA = 0.63V
             */

            using (var gen = new PXI5413(RESOURCE_NAME, "0", simulation: false))
            {
                var resp = gen.Identify().ToUpper();
                Assert.IsTrue(resp.Contains("NATIONAL INSTRUMENTS"));
                gen.ConfigPwm(vpp: 0.63, offset: 0.63, frequencyHz: 1000, dutyCycle: 50);
                gen.Start();
                Thread.Sleep(5000);
                gen.Stop();

                Thread.Sleep(1000);

                gen.ConfigPwm(vpp: 0.63, offset: 0.63, frequencyHz: 1000, dutyCycle: 50);
                gen.Start();
                Thread.Sleep(5000);
                gen.Stop();
            }

        }
    }
}


namespace Knv.Instr.GEN.PXI5413
{
    using NUnit.Framework;

    [TestFixture]
    internal class PXI5413_GEN_UnitTest
    {
        string RESOURCE_NAME = "J22_FGEN_1";

        [Test]
        public void Identify()
        {
            /*
             * "NATIONAL INSTRUMENTS, NI PXIE-5413 (2CH), 0X0209F43E"
             */
            using (var gen = new PXI5413(RESOURCE_NAME, simulation: false))
            {
                var resp = gen.Identify().ToUpper();
                Assert.IsTrue(resp.Contains("NATIONAL INSTRUMENTS"));
            }
        }

        [Test]
        public void Generator_SampleConfig()
        {
            using (var gen = new PXI5413(RESOURCE_NAME, simulation: false))
            {
                var resp = gen.Identify().ToUpper();
                Assert.IsTrue(resp.Contains("NATIONAL INSTRUMENTS"));
                gen.SetWaveform(channel: "0", waveformName: "Square");
                gen.SetFrequency(channel: "0", frequencyHz: 100);
                gen.SetAmplitude(channel: "0", amplitudeVpp: 1);
                gen.SetOffset(channel: "0", offsetVp: 0.5);
                gen.Start();
                System.Threading.Thread.Sleep(1000);
                gen.Stop();
            }
        }

        [Test]
        public void Generator_Change_Parameter_RunTime()
        {
            using (var gen = new PXI5413(RESOURCE_NAME, simulation: false))
            {
                var resp = gen.Identify().ToUpper();
                Assert.IsTrue(resp.Contains("NATIONAL INSTRUMENTS"));
                gen.SetWaveform(channel: "0", waveformName: "Square");
                gen.SetFrequency(channel: "0", frequencyHz: 100);
                gen.SetAmplitude(channel: "0", amplitudeVpp: 1);
                gen.SetOffset(channel: "0", offsetVp: 0.5);
                gen.Start();
                gen.SetAmplitude(channel: "0", amplitudeVpp: 5);
                gen.SetOffset(channel: "0", offsetVp: 2.5);
                gen.SetFrequency(channel: "0", frequencyHz: 200);
                gen.Start();
                gen.SetOffset(channel: "0", offsetVp: 0.25);
                gen.SetAmplitude(channel: "0", amplitudeVpp: 0.5);
                gen.Stop();
                System.Threading.Thread.Sleep(1000);
                gen.Stop();

            }
        }

        [Test]
        public void Generator_DutyCycle()
        {
            using (var gen = new PXI5413(RESOURCE_NAME, simulation: false))
            {
                var resp = gen.Identify().ToUpper();
                Assert.IsTrue(resp.Contains("NATIONAL INSTRUMENTS"));
                gen.SetWaveform(channel: "0", waveformName: "Square");
                gen.SetFrequency(channel: "0", frequencyHz: 10000);
                gen.SetAmplitude(channel: "0", amplitudeVpp: 1);
                gen.SetOffset(channel: "0", offsetVp: 0.5);
                gen.SetDutyCycle(channel: "0", dutyCycle: 0.2);
                gen.Start();
                System.Threading.Thread.Sleep(1000);
                gen.Stop();

            }
        }
    }
}

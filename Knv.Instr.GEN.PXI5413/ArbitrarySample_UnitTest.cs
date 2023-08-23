
namespace Knv.Instr.GEN.PXI5413
{
    using NUnit.Framework;
    using NationalInstruments.ModularInstruments.NIFgen;

    [TestFixture]
    internal class ArbitrarySample_UnitTest
    {
        string ResourceName = "J22_FGEN_1";
        string ChannelName = "0";

        [Test]
        public void ArbitraryShortWaveform()
        {
            double sampleRate = 200000;
            ClockMode sampleClockMode = ClockMode.Automatic;
            short[] waveform = { 32478, 32478, 0, 0 };
            const double gain = 1;

            using (NIFgen session = new NIFgen(ResourceName, ChannelName, true, ""))
            {
                session.Trigger.SetTriggerMode(ChannelName, TriggerMode.Continuous);
                session.Output.OutputMode = OutputMode.Arbitrary;
                
                session.Arbitrary.SampleRate = sampleRate;
                session.Timing.SampleClock.ClockMode = sampleClockMode;
                session.Output.SetEnabled(ChannelName, true);
                int waveformHandle = session.Arbitrary.Waveform.Allocate(ChannelName, waveform.Length);
                session.Arbitrary.Waveform.Write(ChannelName, waveformHandle, waveform);
               
                //Gain can be set on the fly after IntateGeneration.
                session.Arbitrary.SetGain(ChannelName, gain); 
                //This is the start of the generation.
                session.InitiateGeneration();
                System.Threading.Thread.Sleep(5000);
                session.AbortGeneration();
            }
        }

        [Test]
        public void Arbitrary2V5Waveform()
        {
            double sampleRate = 200000;

            ClockMode sampleClockMode = ClockMode.Automatic;
            //Normalizált értékek -1 és 1 között
            double[] waveform = { 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, };
            const double gain = 2.5;

            using (NIFgen session = new NIFgen(ResourceName, ChannelName, true, ""))
            {
                session.Trigger.SetTriggerMode(ChannelName, TriggerMode.Continuous);
                session.Output.OutputMode = OutputMode.Arbitrary;
                /*
                 * Ha generator és load impedanciája azonos 
                 * akkor az FGEN duplázza feszültséget, hogy kompenzálja a load impedanciát.
                 * session.Output.SetLoadImpedance(ChannelName, -1);
                 */
                session.Output.SetLoadImpedance(ChannelName, double.MaxValue);
                session.Arbitrary.SampleRate = sampleRate;
                session.Timing.SampleClock.ClockMode = sampleClockMode;
                session.Output.SetEnabled(ChannelName, true);
                int waveformHandle = session.Arbitrary.Waveform.Allocate(ChannelName, waveform.Length);
                session.Arbitrary.Waveform.Write(ChannelName, waveformHandle, waveform);
                //Gain can be set on the fly after IntateGeneration.
                session.Arbitrary.SetGain(ChannelName, gain);
                session.InitiateGeneration();
                System.Threading.Thread.Sleep(5000);
                session.AbortGeneration();
            }
        }

        [Test]
        public void ArbitraryDoubleWaveform()
        {
            double sampleRate = 10000;
            ClockMode sampleClockMode = ClockMode.Automatic;

           
            double[] waveform = { 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, }; 
            const double gain = 5;

            /*
             * - A kimeneti feszültséget a wavefworm tömb, a gain és az offszet határozza meg.
             * - Az offszet nem lehet nagyobb a gain felénél.
             * - A gain maximum 6V lehet nagy impedanciás kimeneten. (50 Ohm load esetén 12V) -> SetLoadImpedance(ChannelName, -1);
             *
             * pl: ha 0 és 5V közztt szeretnél PWM jelet generálni akkor a 
             * tömb legyen 
             *  - waveform = { 1, 1, 1, 0, 0, 0}
             *  - gain = 5
             *  - offset = 0
             *  - SetLoadImpedance(ChannelName, double.MaxValue);
             *  - SetTriggerMode(ChannelName, TriggerMode.Continuous);
             *  
             *  
             *  pl: ha 1Vpp + 2.5V offsetet szeretnél generálni akkor a
             *  akkor ennek megfelelő osztót kell beállítani a tömbe
             *  1/(6maxgain/1V) = 0.17
             *  - waveform = { 0.17, 0.17, 0.17, 0, 0, 0} ellenőrzés:  0.17x6 + 2.5 = 3.5V
             *  - gain = 6
             *  - offset = 2.5
             *  
             *  
             *  ha a sample rate 10 000Hz és 10db mintád van, akkor az eredő frekencia 1kHz lesz.
             *
             */

            using (NIFgen session = new NIFgen(ResourceName, ChannelName, true, ""))
            {
                session.Trigger.SetTriggerMode(ChannelName, TriggerMode.Continuous);
                session.Output.OutputMode = OutputMode.Arbitrary;
                session.Output.SetLoadImpedance(ChannelName, double.MaxValue);
                session.Arbitrary.SampleRate = sampleRate;
                session.Timing.SampleClock.ClockMode = sampleClockMode;
                session.Output.SetEnabled(ChannelName, true);
                int waveformHandle = session.Arbitrary.Waveform.Allocate(ChannelName, waveform.Length);
                session.Arbitrary.Waveform.Write(ChannelName, waveformHandle, waveform);
                session.Arbitrary.SetGain(ChannelName, gain);
                session.Arbitrary.SetOffset(ChannelName, 2.5);
                //This is the start of the generation.
                session.InitiateGeneration();
                System.Threading.Thread.Sleep(5000);
                session.AbortGeneration();
            }
        }
    }
}

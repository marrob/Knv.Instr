

namespace Knv.Instr.DAQ.USB6009
{
    using System;
    using NUnit.Framework;
    using NationalInstruments.DAQmx;
    using System.Threading;

    [TestFixture]
    internal class SignalGen
    {


        [Test]
        public void AnalogSignalGenSoftTiming()
        {
            using (var asgst = new AnalogSignalGenSoftTiming("Dev1"))
            {
                asgst.Start("ao0", amplitude: 5, freq: 1, samples: 10);
                Thread.Sleep(10000);
                asgst.Stop();
            }
        }


        [Test]
        public void SineWave_AO_PCI_6353()
        {
            var myTask = new Task();
            myTask.AOChannels.CreateVoltageChannel("Dev1/ao0", "aoChannel", -10, 10, AOVoltageUnits.Volts);
            myTask.Control(TaskAction.Verify);

            Timing timing = myTask.Timing;
            timing.SampleTimingType = SampleTimingType.SampleClock;

            double requiredFreq = 50;
            int samples = 256;
            double amp = 4;


            //Ez hozza létre a bufferben a jel alakját
            double samplingClockRate = requiredFreq * samples;
            double[] samplesBuffer = new double[samples];
            double deltaT = 1 / samplingClockRate;

            for (int i = 0; i < samples; i++)
            {
                samplesBuffer[i] = amp * Math.Sin((2.0 * Math.PI) * requiredFreq * i * deltaT);
            }

            myTask.Timing.ConfigureSampleClock(
                /*
                 * On Board Clock: "", string.Empty 
                 *                     - 100 MHz Timebase (default) NI PCIe-6353
                 * External Clock: "/Dev1/PFI0"
                 */
                signalSource: "", // On Board Clock mekkora? 


               /*
                * rate: PCIe-6353: 1CH max: 2.86 MS/s
                * 
                * 
                */
               rate: samplingClockRate,

               activeEdge: SampleClockActiveEdge.Rising,

               /*
                * FiniteSamples: 1x kiküldi a buffer tartalmát és vége (több periódus is lehet a bufferben)
                * ContinuousSamples: folyamatosan küldi a buffer tartalamát amíg Task.Stop be nem következik
                */
               sampleMode: SampleQuantityMode.FiniteSamples,

               samplesPerChannel: samples);


            /*** External Digital Trigger ****/
            /*
            myTask.Triggers.StartTrigger.ConfigureDigitalEdgeTrigger
                (
                    source: "/Dev1/PFI0",
                    edge: DigitalEdgeStartTriggerEdge.Rising
                );
            */

            var writer = new AnalogSingleChannelWriter(myTask.Stream);

            /*
             * autoStart: false -> myTask.Start()-al indul a jelgenerálás
             * autoStart: true -> ez után indul automatikusan, az isméterlt myTaskStart() hibát okoz... 
             */
            writer.WriteMultiSample(autoStart: false, samplesBuffer);


            myTask.Done += (o, e) =>
            {
               // myTask.Dispose();

              /*
               * Ezt a myTask.Stop() váltja ki 
               */

            };

            /*
             * Ha manuális a trigger, akkor minden Start után lefut a buffer tartalma, ha  SampleQuantityMode.FiniteSamples
             * A Stop-ot mindig ki kell adni Start előtt egyébként HIBA
             * 
             * IDE TEGYÉL TÖRÉSPONTOT
             */
            myTask.Start();

            System.Threading.Thread.Sleep(1000);

            /*
             *A Stop hívása váltja ki MyTask.Done esményt!
             * 
             */
            myTask.Stop();

            if (myTask != null)
                myTask.Dispose();
        }
    }
}

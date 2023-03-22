

namespace Knv.Instruments.Daq
{
    using System;
    using NUnit.Framework;
    using NationalInstruments.DAQmx;

    [TestFixture]
    internal class SignalGen_UnitTest
    {
        [Test]
        public void DeviceIsPresent()
        {
            var myTask = new Task();
            myTask.AOChannels.CreateVoltageChannel("Dev1/ao0", "aoChannel", -10, 10, AOVoltageUnits.Volts);
            myTask.Control(TaskAction.Verify);

            Timing timing = myTask.Timing;
            timing.SampleTimingType = SampleTimingType.SampleClock;

            double requiredFreq = 100000;
            int samples = 40;
            double amp = 4;

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
               sampleMode: SampleQuantityMode.ContinuousSamples,

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
                myTask.Dispose();
            };

            myTask.Start();

            myTask.Stop();

            if (myTask != null)
                myTask.Dispose();
        }
    }
}

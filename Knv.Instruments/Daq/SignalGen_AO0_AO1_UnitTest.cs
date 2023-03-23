

namespace Knv.Instruments.Daq
{
    using System;
    using NUnit.Framework;
    using NationalInstruments.DAQmx;
    using Knv.Instruments.Common;
    using System.Reflection;
    using Common;
    using System.Runtime.Remoting.Metadata.W3cXsd2001;

    [TestFixture]
    internal class SignalGen_UnitTest
    {

        Task InitPositiveHalfSine(string physicalChannel, double timeMs, double vpeak)
        {
            var myTask = new Task();
            myTask.AOChannels.CreateVoltageChannel(physicalChannel, "ao0Channel", -10, 10, AOVoltageUnits.Volts);
            myTask.Control(TaskAction.Verify);

            Timing timing = myTask.Timing;
            timing.SampleTimingType = SampleTimingType.SampleClock;

            int samples = 1024;
            double samplingClockRate = 1/timeMs * samples;
            double[] samplesBuffer = new double[samples];
            double deltaT = 1 / samplingClockRate;

            for (int i = 0; i < samples; i++)
                samplesBuffer[i] = 2 * vpeak * Math.Sin(Math.PI * 1.0/timeMs * i * deltaT);

            myTask.Timing.ConfigureSampleClock(
               signalSource: "",
               rate: samplingClockRate,
               activeEdge: SampleClockActiveEdge.Rising,
               sampleMode: SampleQuantityMode.FiniteSamples,
               samplesPerChannel: samples);

            var writer = new AnalogSingleChannelWriter(myTask.Stream);
            writer.WriteMultiSample(autoStart: false, samplesBuffer);

            return myTask;
        }

        Task InitNegativeHalfSine(string physicalChannel, double timeMs, double vpeak)
        {
            var myTask = new Task();


            myTask.AOChannels.CreateVoltageChannel(physicalChannel, "ao1Channel", -10, 10, AOVoltageUnits.Volts);
            myTask.Control(TaskAction.Verify);

            Timing timing = myTask.Timing;
            timing.SampleTimingType = SampleTimingType.SampleClock;

            int samples = 1024;
            double samplingClockRate = 1 / timeMs * samples;
            double[] samplesBuffer = new double[samples];
            double deltaT = 1 / samplingClockRate;

            for (int i = 0; i < samples; i++)
                samplesBuffer[i] = 2 * vpeak * Math.Sin(Math.PI + Math.PI * 1.0 / timeMs * i * deltaT);

            myTask.Timing.ConfigureSampleClock(
               signalSource: "",
               rate: samplingClockRate,
               activeEdge: SampleClockActiveEdge.Rising,
               sampleMode: SampleQuantityMode.FiniteSamples,
               samplesPerChannel: samples);

            var writer = new AnalogSingleChannelWriter(myTask.Stream);
            writer.WriteMultiSample(autoStart: false, samplesBuffer);

            return myTask;
        }



        /// <summary>
        /// Egyszerre csak egy AO Task létezhet, ezért amikor AO0 Task végzett, csak ezután Követekezhet az AO1
        /// 
        /// https://knowledge.ni.com/KnowledgeArticleDetails?id=kA00Z0000019KWYSA2&l=hu-HU
        /// 
        /// </summary>
        [Test]
        public void PositiveAndNegativeSineWave()
        {
            Task posSineTask = null;
            Task negSineTask = null;

            //for (int i = 0; i < 10; i++)
            {
                posSineTask = InitPositiveHalfSine("Dev1/ao0", 0.02, 4);
                posSineTask.Start();
                System.Threading.Thread.Sleep(20);
                posSineTask.Dispose();

                negSineTask = InitNegativeHalfSine("Dev1/ao1", 0.02, 4);
                negSineTask.Start();
                System.Threading.Thread.Sleep(20);
                negSineTask.Dispose();
            }
        }


        [Test]
        public void PositiveHalfeSine_UnitTest()
        {
            var task = InitPositiveHalfSine("Dev1/ao0", 0.05, 4);

            task.Start();
            System.Threading.Thread.Sleep(1000);
            task.Stop();

            if (task != null)
                task.Dispose();
        }

        [Test]
        public void NegativeHalfeSine_UnitTest()
        {
           var task = InitNegativeHalfSine("Dev1/ao1", 0.05, 4);

            task.Start();
            System.Threading.Thread.Sleep(1000);
            task.Stop();

           if (task != null)
                task.Dispose();
        }
    }
}

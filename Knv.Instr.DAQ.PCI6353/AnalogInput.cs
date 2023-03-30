
namespace Knv.Instr.DAQ.PCI6353
{
    using NationalInstruments;
    using NationalInstruments.DAQmx;
    using NUnit.Framework;
    using System;
    using System.Timers;


    public class AnalogInput
    {
        public static bool IsSimualtion { get; set; } = false;


        /// <summary>
        /// Beolvassa egy AI bemenetét
        /// A kártya pl lehet NI PCIe-6353
        /// </summary>
        /// <param name="visaName">pl:Dev1 ezt a MAX-ban találod meg </param>
        /// <param name="channel">pl: "ai0" </param>
        /// <returns></returns>
        public static double GetOneChannel(string visaName, string channel)
        {
            double result = 0;
            if (!IsSimualtion)
            {
                using (var myTask = new Task())
                {
                    string physicalChannel = $"{visaName}/{channel}";
                    myTask.AIChannels.CreateVoltageChannel(physicalChannel, "aiChannel", (AITerminalConfiguration)(-1), -10, 10, AIVoltageUnits.Volts);
                    AnalogMultiChannelReader reader = new AnalogMultiChannelReader(myTask.Stream);
                    myTask.Control(TaskAction.Verify);
                    result = reader.ReadSingleSample()[0];
                }
            }
            else
            {
                Random rnd = new Random();
                result = rnd.Next(-10, 10);
            }
            return result;
        }

        public static double[] NormalMeasureStart(string visaName, string channel, int samples, int sFreq)
        {
            using (var myTask = new Task())
            {
                string physicalChannel = $"{visaName}/{channel}";
                myTask.AIChannels.CreateVoltageChannel(physicalChannel, "", AITerminalConfiguration.Rse, -10, 10, AIVoltageUnits.Volts);
                myTask.Timing.ConfigureSampleClock("", sFreq, SampleClockActiveEdge.Rising, SampleQuantityMode.FiniteSamples, samples);
                myTask.Control(TaskAction.Verify);
                var reader = new AnalogSingleChannelReader(myTask.Stream);

                var wave = reader.ReadWaveform(samples);
                double[] result = new double[wave.Samples.Count];

                for (int i = 0; i < wave.Samples.Count; i++)
                    result[i] = wave.Samples[i].Value;
                return result;
            }
        }
    }
}

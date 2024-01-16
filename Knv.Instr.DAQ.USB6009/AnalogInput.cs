namespace Knv.Instr.DAQ.USB6009
{ 
    using NationalInstruments.DAQmx;
    using System;

    public class AnalogInput
    {
        public static bool IsSimualtion { get; set; } = false;

        /// <summary>
        /// Beolvassa egy AI bemenetét
        /// A kártya pl lehet NI PCIe-6353
        /// </summary>
        /// <param name="resourceName">pl:Dev1 ezt a MAX-ban találod meg </param>
        /// <param name="channel">pl: "ai0" </param>
        /// <returns></returns>
        static double GetOneChannel(string resourceName, string channel, AITerminalConfiguration terminalConfiguration)
        {
            double result = 0;
            if (!IsSimualtion)
            {
                using (var myTask = new Task())
                {
                    string physicalChannel = $"{resourceName}/{channel}";
                    myTask.AIChannels.CreateVoltageChannel(physicalChannel, ""/*$"AI:{channel}"*/, terminalConfiguration, -10, 10, AIVoltageUnits.Volts);
                    AnalogMultiChannelReader reader = new AnalogMultiChannelReader(myTask.Stream);
                    myTask.Control(TaskAction.Verify);
                    result = reader.ReadSingleSample()[0];
                }
            }
            else
            {
                Random rnd = new Random();
                result = rnd.Next(-5, 5);
            }
            return result;
        }

        public static double GetOneSingleEndedChannel(string resourceName, string channel)
        {
            return GetOneChannel(resourceName, channel, AITerminalConfiguration.Rse);
        }

        /// <summary>
        /// pl PCI-6353-nál AI0 (AI0+) és AI8(AI0-) 
        /// </summary>
        /// <param name="resourceName">pl:Dev1 ezt a MAX-ban találod meg.</param>
        /// <param name="channel">pl: "ai0</param>
        /// <returns></returns>
        public static double GetOneDifferentialChannel(string resourceName, string channel)
        {
            return GetOneChannel(resourceName, channel, AITerminalConfiguration.Differential);
        }

        /// <summary>
        /// Normal hasonlóan a szkópos méréseknél idítás után megál amint megteleik a memória.
        /// </summary>
        /// <param name="resourceName"></param>
        /// <param name="channel"></param>
        /// <param name="samples"></param>
        /// <param name="sFreq">USB-6009-nél max 48000Hz lehet.</param>
        /// <returns></returns>
        public static double[] NormalMeasureStart(string resourceName, string channel, int samples, int sFreq, AITerminalConfiguration terminalConfiguration)
        {
            using (var myTask = new Task())
            {
                string physicalChannel = $"{resourceName}/{channel}";
                myTask.AIChannels.CreateVoltageChannel(physicalChannel,"", AITerminalConfiguration.Rse, -5, 5, AIVoltageUnits.Volts);
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

        public static double[] NormalSingleEndedMeasureStart(string resourceName, string channel, int samples, int sFreq)
        {
            return NormalMeasureStart(resourceName, channel, samples, sFreq, AITerminalConfiguration.Rse);
        }
    }
}

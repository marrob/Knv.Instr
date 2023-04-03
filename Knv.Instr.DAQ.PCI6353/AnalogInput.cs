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
    using NationalInstruments.DAQmx;
    using System;

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
        static double GetOneChannel(string visaName, string channel, AITerminalConfiguration terminalConfiguration)
        {
            double result = 0;
            if (!IsSimualtion)
            {
                using (var myTask = new Task())
                {
                    string physicalChannel = $"{visaName}/{channel}";
                    myTask.AIChannels.CreateVoltageChannel(physicalChannel, $"AI:{channel}", terminalConfiguration, -10, 10, AIVoltageUnits.Volts);
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

        public static double GetOneSingleEndedChannel(string visaName, string channel)
        {
            return GetOneChannel(visaName, channel, AITerminalConfiguration.Rse);
        }

        /// <summary>
        /// pl PCI-6353-nál AI0 (AI0+) és AI8(AI0-) 
        /// </summary>
        /// <param name="visaName">pl:Dev1 ezt a MAX-ban találod meg.</param>
        /// <param name="channel">pl: "ai0</param>
        /// <returns></returns>
        public static double GetOneDifferentialChannel(string visaName, string channel)
        {
            return GetOneChannel(visaName, channel, AITerminalConfiguration.Differential);
        }

        /// <summary>
        /// Normal hasonlóan a szkópos méréseknél idítás után megál amint megteleik a memória.
        /// </summary>
        /// <param name="visaName"></param>
        /// <param name="channel"></param>
        /// <param name="samples"></param>
        /// <param name="sFreq"></param>
        /// <returns></returns>
        public static double[] NormalMeasureStart(string visaName, string channel, int samples, int sFreq, AITerminalConfiguration terminalConfiguration)
        {
            using (var myTask = new Task())
            {
                string physicalChannel = $"{visaName}/{channel}";
                myTask.AIChannels.CreateVoltageChannel(physicalChannel, $"AI:{channel}", AITerminalConfiguration.Rse, -10, 10, AIVoltageUnits.Volts);
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

        public static double[] NormalSingleEndedMeasureStart(string visaName, string channel, int samples, int sFreq)
        {
            return NormalMeasureStart(visaName, channel, samples, sFreq, AITerminalConfiguration.Rse);
        }
    }
}

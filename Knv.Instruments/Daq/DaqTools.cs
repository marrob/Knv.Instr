

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
 * Assembly NationalInstruments.Common
 * C:\Program Files (x86)\National Instruments\MeasurementStudioVS2012\DotNET\Assemblies\Current\NationalInstruments.Common.dll
 *
 * Assembly NationalInstruments.DAQmx
 * C:\Program Files (x86)\National Instruments\MeasurementStudioVS2012\DotNET\Assemblies (64-bit)\Current\NationalInstruments.DAQmx.dll
 *
 */
namespace Knv.Instruments.Daq
{
    using NationalInstruments.DAQmx;
    public class DaqTools
    {
        public static double GetOneVolt(string device, string channel)
        {
            double retval = 0;
            using (var myTask = new NationalInstruments.DAQmx.Task())
            {
                string physicalChannel = $"{device}/{channel}";
                myTask.AIChannels.CreateVoltageChannel(physicalChannel, "", (AITerminalConfiguration)(-1), -10, 10, AIVoltageUnits.Volts);
                AnalogMultiChannelReader reader = new AnalogMultiChannelReader(myTask.Stream);
                myTask.Control(TaskAction.Verify);
                retval = reader.ReadSingleSample()[0];
            }
            return retval;
        }
    }
}


namespace Knv.Instr.DAQ.PCI6353
{
    using NationalInstruments.DAQmx;
    using System;
    using System.Linq;

    static public class AnalogOutput
    {
        public static bool simualtion { get; set; } = false;


        /// <summary>
        /// Beállít egy feszültséget egy analóg kimeneten
        /// Kártya pl lehet NI PCIe-6353, ezen kettő db Analog Out csatorna van: ao0 és ao1
        /// A beállított feszültség a kötkező beállításig vagy újraidnitásig a kiementen van! 
        /// </summary>
        /// <param name="visaName"></param>
        /// <param name="channel"></param>
        /// <param name="voltage"></param>
        public static void SetChannel(string visaName, string channel, double voltage)
        {
            if (simualtion)
                return;

            using (var myTask = new Task())
            {
                string physicalChannel = $"{visaName}/{channel}";
                myTask.AOChannels.CreateVoltageChannel(physicalChannel, "aoChannel", -10, 10, AOVoltageUnits.Volts);
                var writer = new AnalogSingleChannelWriter(myTask.Stream);
                writer.WriteSingleSample(true, voltage);
            }
        }
    }
}

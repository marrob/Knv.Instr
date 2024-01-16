
namespace Knv.Instr.DAQ.USB6009
{
    using NationalInstruments.DAQmx;

    static public class AnalogOutput
    {
        public static bool simualtion { get; set; } = false;


        /// <summary>
        /// Beállít egy feszültséget egy analóg kimeneten
        /// Kártya pl lehet NI PCIe-6353 vagy USB6009, ezeken kettő db Analog Out csatorna van: ao0 és ao1
        /// A beállított feszültség a következő beállításig vagy újraidnitásig a kiementen van! 
        /// </summary>
        /// <param name="resourceName"></param>
        /// <param name="channel"></param>
        /// <param name="voltage"></param>
        public static void SetChannel(string resourceName, string channel, double voltage)
        {
            if (simualtion)
                return;

            using (var myTask = new Task())
            {
                string physicalChannel = $"{resourceName}/{channel}";
                myTask.AOChannels.CreateVoltageChannel(physicalChannel, "aoChannel", 0, 5, AOVoltageUnits.Volts);
                var writer = new AnalogSingleChannelWriter(myTask.Stream);
                writer.WriteSingleSample(true, voltage);
            }
        }
    }
}

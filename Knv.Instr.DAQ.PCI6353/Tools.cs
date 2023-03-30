namespace Knv.Instr.DAQ.PCI6353
{

    using NationalInstruments.DAQmx;
    using System;
    using System.Linq;

    public static class Tools
    {
        public static bool IsSimualtion { get; set; } = false;


        public static bool DeviceIsPresent(string deviceName)
        {
            bool retval = false;
            var devs = DaqSystem.Local.Devices;
            retval = devs.Contains(deviceName);
            return retval;
        }

        /// <summary>
        /// Beolvas egy analog bemenetet
        /// A kártya pl lehet NI PCIe-6353
        /// </summary>
        /// <param name="deviceName">pl:Dev1 ezt a MAX-ban találod meg </param>
        /// <param name="channel">pl: "ai0" </param>
        /// <returns></returns>
        public static double AIgetOneVolt(string deviceName, string channel)
        {
            double retval = 0;
            if (!IsSimualtion)
            {
                using (var myTask = new Task())
                {
                    string physicalChannel = $"{deviceName}/{channel}";
                    myTask.AIChannels.CreateVoltageChannel(physicalChannel, "aiChannel", (AITerminalConfiguration)(-1), -10, 10, AIVoltageUnits.Volts);
                    AnalogMultiChannelReader reader = new AnalogMultiChannelReader(myTask.Stream);
                    myTask.Control(TaskAction.Verify);
                    retval = reader.ReadSingleSample()[0];
                }
            }
            else
            {
                Random rnd = new Random();
                retval = rnd.Next(-10, 10);
            }
            return retval;
        }

        /// <summary>
        /// Beállít egy feszültséget egy analóg kimeneten
        /// Kártya pl lehet NI PCIe-6353, ezen kettő db Analog Out csatorna van: ao0 és ao1
        /// A beállított feszültség a kötkező beállításig vagy újraidnitásig a kiementen van! 
        /// </summary>
        /// <param name="deviceName"></param>
        /// <param name="channel"></param>
        /// <param name="voltage"></param>
        public static void AOsetVoltage(string deviceName, string channel, double voltage) 
        {
            if (IsSimualtion)
                return;

            using (var myTask = new Task())
            {
                string physicalChannel = $"{deviceName}/{channel}";
                myTask.AOChannels.CreateVoltageChannel(physicalChannel, "aoChannel", -10, 10, AOVoltageUnits.Volts);
                var writer = new AnalogSingleChannelWriter(myTask.Stream);
                writer.WriteSingleSample(true, voltage);
            }
        }
    }
}

namespace Knv.Instr.DAQ.PCI6353
{

    using NationalInstruments.DAQmx;
    using System;
    using System.IO;
    using System.Linq;

    public static class Tools
    {
        public static bool DeviceIsPresent(string visaName)
        {
            bool retval = false;
            var devs = DaqSystem.Local.Devices;
            retval = devs.Contains(visaName);
            return retval;
        }

        public static void SignalToFile(double[] data, string title, string directory)
        {
            var dt = DateTime.Now;
            var fileName = $"{title}_{dt:yyyy}{dt:MM}{dt:dd}_{dt:HH}{dt:mm}{dt:ss}.csv";

            if (!File.Exists(directory))
                Directory.CreateDirectory(directory);

            var path = $"{directory}\\{fileName}";
            using (var sw = new StreamWriter(path))
            {
                foreach (var value in data)
                    sw.WriteLine($"\"{value:#.000}\"");
            }
        }

        private class Waveform
        {
            public DateTime Timestamp { get; set; }
            public double DeltaX { get; set; }
            public double[] YArray { get; set; }
        }

    }
}

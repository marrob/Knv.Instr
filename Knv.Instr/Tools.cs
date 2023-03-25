

namespace Knv.Instr
{
    using System;
    using System.IO;

    public class Tools
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="path"></param>
        public static void SignalToFile(double[] data, string title)
        {
            var dt = DateTime.Now;
            var fileName = $"{title}_{dt:yyyy}{dt:MM}{dt:dd}_{dt:HH}{dt:mm}{dt:ss}.csv";
            var directory = Constants.LogRootDirecotry;

            if (!File.Exists(directory))
                Directory.CreateDirectory(directory);

            var path = $"{directory}\\{fileName}";
            using (var sw = new StreamWriter(path))
            {
                foreach (var value in data)
                    sw.WriteLine($"{value:#.000}");
            }
        }

        //public static string GetTempPath ();


        private class Waveform
        {
            public DateTime Timestamp { get; set; }
            public double DeltaX { get; set; }
            public double[] YArray { get; set; }
        }

    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knv.Instr.GEN.PXI5413
{
    internal class Signals
    {

        public static double[] SineGen(double offset = 0, double amplitude = 1, int samples = 100)
        {
            double[] data = new double[samples];
            for (int i = 0; i < samples; i++)
            {
                data[i] = offset + amplitude * Math.Sin(Math.PI / 180.0 * 1 / samples * 360 * (i % samples));
            }
            return data;
        }


        public static double[] PwmGen(double offset = 0, double amplitude = 1, double dutyCycle = 0.5, int samples = 100)
        {
            double[] data = new double[samples];
            for (int i = 0; i < samples; i++)
            {
                data[i] = offset + amplitude * (i % samples < samples * dutyCycle ? 1 : 0);
            }
            return data;
        }

        public static void DoubleArrayToFile(double[] data, string path)
        {
            var f = new FileInfo(path);
            if (f.Exists)
                f.Delete();
            using (var sw = new StreamWriter(path))
            {
                foreach (var value in data)
                    sw.WriteLine(value.ToString());
            }
        }
    }
}


using System;

namespace Knv.Instr.DAC.M9185A
{
    internal class Program
    {
        const string INSTRUMENT_NAME = "PXI14::13::0::INSTR";
        static void Main(string[] args)
        {
            try
            {
                using (M9185 dac = new M9185(INSTRUMENT_NAME))
                {
                    dac.Open();
                    Console.WriteLine(dac.Description());
                    dac.VoltageOutputConfig(0, 5);

                    dac.VoltageOutputConfig(2, 5);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {

            }

            Console.WriteLine("Done - Press Enter to Exit");
            Console.ReadLine();
        }
    }
}

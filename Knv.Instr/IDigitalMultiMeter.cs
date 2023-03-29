using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knv.Instr
{
    public interface IDigitalMultiMeter : IDisposable
    {
        string Identify();
        void Config(string function, double range);

        double Read();

    }
}

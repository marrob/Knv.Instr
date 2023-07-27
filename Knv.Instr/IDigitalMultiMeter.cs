using System;

namespace Knv.Instr
{
    public interface IDigitalMultiMeter : IDisposable
    {
        string Identify();
        void Config(string function, string rangeName);
        void Config(string function, string rangeName, double digits);
        double Read();
    }
}

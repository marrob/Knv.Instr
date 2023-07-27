using System;

namespace Knv.Instr
{
    public interface IElectronicsLoad : IDisposable
    {
        string Identify();

        void Config(int channel, string range, double current);

        void OnOff(bool onOff);
    }
}

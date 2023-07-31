using System;

namespace Knv.Instr
{
    public interface IElectronicsLoad : IDisposable
    {
        string Identify();

        void Config(string mode = "CCL", string channel = "1", double current = 2.0);

        void OnOff(bool onOff);
    }
}

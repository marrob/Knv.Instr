using System;

namespace Knv.Instr
{
    public interface IElectronicsLoad : IDisposable
    {
        string Identify();

        void Config(string mode = "CCL-VL", string channel = "1", double current = 2.0);
        double GetActualVolt();
        double GetActualCurrent();
        void OnOff(bool onOff);
    }
}

using System;

namespace Knv.Instr
{
    public interface ISourceMeasureUnits : IDisposable
    {
        string Identify();
        void ConfigVoltageSource(string voltageRangeName, string currentLimitRangeName);
        void ConfigVoltageSource(string voltageRangeName, string currentLimitRangeName, string sense = "Local");
        void SetVoltageSource(double voltage, double current);
        void OnOff(bool enable);
        double GetActualCurrent();
        double GetActualVolt();
    }
}

using System;

namespace Knv.Instr
{
    public interface ISourceMeasureUnits : IDisposable
    {
        string Identify();
        void Reset();
        void ConfigCurrentSource(string voltageLimitRangeName, string currentRangeName);
        void ConfigCurrentSource(string voltageLimitRangeName, string currentRangeName, string sense);
        void SetCurrentSource(double voltageLimit, double currentLevel);

        void ConfigVoltageSource(string voltageRangeName, string currentLimitRangeName);
        void ConfigVoltageSource(string voltageRangeName, string currentLimitRangeName, string sense);
        void SetVoltageSource(double voltageLevel, double currentLimit);
        void OnOff(bool enable);
        double GetActualCurrent();
        double GetActualVolt();
    }
}

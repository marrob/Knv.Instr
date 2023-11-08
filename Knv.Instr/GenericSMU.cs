using System;

namespace Knv.Instr
{
    public class GenericSMU : ISourceMeasureUnits
    {
        readonly ISourceMeasureUnits _smu;

        public GenericSMU(ISourceMeasureUnits smuInstance)
        {
            _smu = smuInstance;
        }

        public void Reset()
        { 
            _smu.Reset();   
        }

        public void ConfigCurrentSource(string voltageLimitRangeName = "6V", string currentRangeName = "1mA")
        { 
            _smu.ConfigCurrentSource(voltageLimitRangeName, currentRangeName);
        }
        public void ConfigCurrentSource(string voltageLimitRangeName = "6V", string currentRangeName = "1mA", string sense = "Local")
        {
            _smu.ConfigCurrentSource(voltageLimitRangeName, currentRangeName, sense);
        }

        public void SetCurrentSource(double voltageLimit, double currentLevel)
        { 
            _smu.SetCurrentSource(voltageLimit, currentLevel);
        }

        public void ConfigVoltageSource(string voltageRangeName = "6V", string currentLimitRangeName = "100mA")
        { 
            _smu.ConfigVoltageSource(voltageRangeName, currentLimitRangeName);
        } 

        public void ConfigVoltageSource(string voltageRangeName = "6V", string currentLimitRangeName = "100mA", string sense = "Local")
        {
            _smu.ConfigVoltageSource(voltageRangeName, currentLimitRangeName, sense);
        }

        public void SetVoltageSource(double voltageLevel, double currentLimit)
        { 
            _smu.SetVoltageSource(voltageLevel, currentLimit);
        }

        public void OnOff(bool enable)
        { 
            _smu.OnOff(enable);
        }

        public double GetActualVolt()
        { 
            return(_smu.GetActualVolt());
        }

        public double GetActualCurrent()
        {
            return(_smu.GetActualCurrent());
        }   

        public string Identify()
        {
            return _smu.Identify();
        }

        public void Dispose()
        {
            _smu.Dispose();
        }
    }
}

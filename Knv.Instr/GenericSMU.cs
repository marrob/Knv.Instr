namespace Knv.Instr
{
    public class GenericSMU : ISourceMeasureUnits
    {
        readonly ISourceMeasureUnits _smu;

        public GenericSMU(ISourceMeasureUnits smuInstance)
        {
            _smu = smuInstance;
        }

        public void ConfigVoltageSource(string voltageRangeName, string currentLimitRangeName)
        { 
            _smu.ConfigVoltageSource(voltageRangeName, currentLimitRangeName);
        } 

        public void SetVoltageSource(double voltage, double current)
        { 
            _smu.SetVoltageSource(voltage, current);
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

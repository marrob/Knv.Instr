namespace Knv.Instr
{
    public class GenericPSU:IPowerSupply
    {
        readonly IPowerSupply _psu;

        public GenericPSU(IPowerSupply psuInstance)
        {
            _psu = psuInstance;
        }

        public string Identify()
        {
            return _psu.Identify();
        }

        public void SetOutput(double volt, double current)
        { 
            _psu.SetOutput(volt, current);
        }

        public void SetOutput(double volt, double current, bool onOff)
        {
            _psu.SetOutput(volt, current, onOff);
        }

        public void OnOff(bool enable)
        {
            _psu.OnOff(enable);
        }   

        public double GetActualVolt()
        {
            return _psu.GetActualVolt();
        }
        
        public double GetActualCurrent()
        {
            return _psu.GetActualCurrent();
        }

        public double SetOutputGetActualVolt(double volt, double current)
        {
            return _psu.SetOutputGetActualVolt(volt, current);
        }


        public void Dispose()
        {
            _psu.Dispose();
        }
    }
}

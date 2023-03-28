using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knv.Instr
{
    public class GenericPowerSupply:IPowerSupply
    {
        IPowerSupply _psu;

        public GenericPowerSupply(IPowerSupply psu)
        {
            _psu = psu;
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

        public void LogSave(string directory, string prefix)
        {
            _psu.LogSave(directory, prefix);
        }

        public void Dispose()
        {
            _psu.Dispose();
        }
    }
}

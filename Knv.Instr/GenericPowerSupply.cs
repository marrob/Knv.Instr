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

        public void Dispose()
        {
            _psu.Dispose();
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knv.Instr
{
    public class GenericDMM: IDigitalMultiMeter
    {
        IDigitalMultiMeter _dmm;

        public GenericDMM(IDigitalMultiMeter instanceOfDMM)
        {
            _dmm = instanceOfDMM;
        }

        public string Identify()
        { 
            return _dmm.Identify(); 
        }

        public void Config(string function, double range)
        {
            _dmm.Config(function, range);
        }

        public double Read()
        { 
            return _dmm.Read();
        }

        public void Dispose()
        {
            _dmm.Dispose();
        }
    }
}

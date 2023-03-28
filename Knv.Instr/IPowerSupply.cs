

namespace Knv.Instr
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading.Tasks;



    /*
     * 
     * Minden támogatott tápegység meg kell hogy valósítsa ezeket a funkcókat
     * 
     */

    public interface IPowerSupply: IDisposable
    {
        string Identify();
        void SetOutput(double volt, double current);
        void SetOutput(double volt, double current, bool onOff);
        double SetOutputGetActualVolt(double volt, double current);
        double GetActualVolt();
        double GetActualCurrent();
        void LogSave(string directory, string prefix);
    }
}

namespace Knv.Instr
{
    using System;
    public interface IPowerSupply: IDisposable
    {
        string Identify();
        void SetOutput(double volt, double current);
        void SetOutput(double volt, double current, bool onOff);
        void OnOff(bool enable);
        double SetOutputGetActualVolt(double volt, double current);
        double GetActualVolt();
        double GetActualCurrent();
    }
}

namespace Knv.Instr
{
    using System;
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

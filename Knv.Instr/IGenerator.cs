namespace Knv.Instr
{
    using System;
    public interface IGenerator : IDisposable
    {
        string Identify();
        void ConfigWaveform(string waveformName = "Square", double amplitudeVpp = 1, double frequencyHz = 1000, double offsetVp = 0.5, double dutyCycle = 50);
        void ConfigPwm(double vpp = 1, double offset = 0, double frequencyHz = 1000, double dutyCycle = 50);
        void Start();
        void Stop();
    }
}

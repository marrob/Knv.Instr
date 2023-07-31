namespace Knv.Instr
{
    using System;
    public interface IGenerator : IDisposable
    {
        string Identify();
        void SetWaveform(string channel = "0", string waveformName = "Square");
        void SetAmplitude(string channel = "0", double amplitudeVpp = 1);
        void SetFrequency(string channel = "0", double frequencyHz = 1000);
        void SetOffset(string channel = "0", double offsetVp = 0.5);
        void SetDutyCycle(string channel, double dutyCycle);
        void Start();
        void Stop();
    }
}

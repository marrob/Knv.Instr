namespace Knv.Instr
{
    public class GenericGEN : IGenerator
    {
        readonly IGenerator _gen;

        public GenericGEN(IGenerator genInstance)
        {
            _gen = genInstance;
        }

        public string Identify()
        {
            return _gen.Identify();
        }
        public void SetWaveform(string channel = "0", string waveformName = "Square")
        {
            _gen.SetWaveform(channel, waveformName);
        }
        public void SetAmplitude(string channel = "0", double amplitudeVpp = 1)
        {
            _gen.SetAmplitude(channel, amplitudeVpp);
        }
        public void SetFrequency(string channel = "0", double frequencyHz = 1000)
        {
            _gen.SetFrequency(channel, frequencyHz);
        }
        public void SetOffset(string channel = "0", double offsetVp = 0.5)
        {
            _gen.SetOffset(channel, offsetVp);
        }
        public void SetDutyCycle(string channel, double dutyCycle)
        { 
            _gen.SetDutyCycle(channel, dutyCycle);
        }
        public void Start()
        { 
            _gen.Start();
        }
        public void Stop()
        { 
            _gen.Stop();
        }

        public void Dispose()
        {
            _gen.Dispose();
        }
    }
}

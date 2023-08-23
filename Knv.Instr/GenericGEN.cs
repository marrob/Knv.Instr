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

        public void ConfigWaveform( string waveformName = "Square", double amplitudeVpp = 1, double frequencyHz = 1000, double offsetVp = 0.5, double dutyCycle = 50)
        { 
            _gen.ConfigWaveform(waveformName, amplitudeVpp, frequencyHz, offsetVp, dutyCycle);
        }
        public void ConfigPwm(double vpp = 1, double offset = 0, double frequencyHz = 1000, double dutyCycle = 50)
        { 
            _gen.ConfigPwm(vpp, offset, frequencyHz, dutyCycle);
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

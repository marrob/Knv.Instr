namespace Knv.Instr
{
    public class GenericLoad: IElectronicsLoad
    {
        readonly IElectronicsLoad _load;

        public GenericLoad(IElectronicsLoad loadInstance)
        {
            _load = loadInstance;
        }

        public string Identify()
        {
            return _load.Identify();
        }

        public void Config(int channel, string range, double current)
        {
            _load.Config(channel, range, current);
        }

        public void OnOff(bool enable)
        { 
            _load.OnOff(enable);
        }

        public void Dispose()
        {
            _load.Dispose();
        }
    }
}

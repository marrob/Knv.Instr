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

        public void Config(string mode = "CCL-VL", string channel = "1", double current = 0.10)
        {
            _load.Config(mode, channel, current);
        }
        public double GetActualVolt()
        { 
           return _load.GetActualVolt();
        }

        public double GetActualCurrent()
        { 
            return _load.GetActualCurrent();
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

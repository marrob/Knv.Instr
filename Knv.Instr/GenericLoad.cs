﻿namespace Knv.Instr
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

        public void Config(string mode = "CCL", string channel = "1", double current = 2.0)
        {
            _load.Config(mode, channel, current);
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

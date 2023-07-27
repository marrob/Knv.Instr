namespace Knv.Instr
{
    public class GenericDMM: IDigitalMultiMeter
    {
        readonly IDigitalMultiMeter _dmm;

        public GenericDMM(IDigitalMultiMeter dmmInstance)
        {
            _dmm = dmmInstance;
        }

        public string Identify()
        { 
            return _dmm.Identify(); 
        }

        public void Config(string function, string rangeName)
        {
            _dmm.Config(function, rangeName);
        }

        public void Config(string function, string rangeName, double digits)
        {
            _dmm.Config(function, rangeName, digits);
        }

        public double Read()
        { 
            return _dmm.Read();
        }

        public void Dispose()
        {
            _dmm.Dispose();
        }
    }
}

namespace Knv.Instr
{
    public class GenericSMU: ISourceMeasureUnits
    {
        readonly ISourceMeasureUnits _smu;

        public GenericSMU(ISourceMeasureUnits smuInstance)
        {
            _smu = smuInstance;
        }

        public string Identify()
        {
            return _smu.Identify();
        }

        public void Dispose()
        {
            _smu.Dispose();
        }
    }
}

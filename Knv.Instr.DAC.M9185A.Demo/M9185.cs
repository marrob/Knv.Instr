/*
In solution explorer, right-click on References and select Add Reference. Click the COM tab. 
Select the following type libraries: 
- IviDriver 1.0 Type Library 
- IVI AgM9185 1.0 Type Library 
Click the Select button and then OK. 

Interop type 'AgM9185Class' cannot be embedded. Use the applicable interface instead.
Kapcsold ki az Embed Interop Types-ot

Visual Studio → References →  Agilent.AgM9185.Interop és Ivi.Driver.Interop
*/

using System;
using Agilent.AgM9185.Interop;

namespace Knv.Instr.DAC.M9185A.Demo
{
    public class M9185: IDisposable
    {
        const int CHANNELS = 16;
        AgM9185 _driver = null;
        AgM9185FunctionEnum[] _functionList = new AgM9185FunctionEnum[CHANNELS];
        double[] _valueList = new double[CHANNELS];
        bool[] _enableList = new bool[CHANNELS];
        readonly string _resourceName;
        bool _disposed = false;


        public M9185(string resourceName = "PXI14::13::0::INSTR")
        {
            _resourceName = resourceName;
        }

        public void Open() 
        {
            _driver = new AgM9185();
            _driver.Initialize(_resourceName, IdQuery: true, Reset: true, "Simulate=false, DriverSetup= Model=M9185A");
        }

        public string Description()
        {
            string retval = $"Res. Name:   {_resourceName}\r\n" +
                            $"Identifier:  {_driver.Identity.Identifier}\r\n" +
                            $"Revision:    {_driver.Identity.Revision}\r\n" +
                            $"Vendor:      {_driver.Identity.Vendor}\r\n" +
                            $"Description: {_driver.Identity.Description}\r\n" +
                            $"Model:       {_driver.Identity.InstrumentModel}\r\n" +
                            $"FirmwareRev: {_driver.Identity.InstrumentFirmwareRevision}\r\n" +
                            $"Simulate:    {_driver.DriverOperation.Simulate}\r\n";

            return retval ;
        }

        public void VoltageOutputConfig(int channelIndex, double value, bool enable = true)
        {
            if (channelIndex > CHANNELS - 1)
                throw new ArgumentException( $"Hibás csatorna index! érvénys tartomány: 0..{CHANNELS-1}", "channelIndex");

            _valueList[channelIndex] = value;
            _enableList[channelIndex] = enable;

            for(int i = 0; i < CHANNELS; i++)
               _functionList[i] = AgM9185FunctionEnum.AgM9185FunctionVoltage;

            _driver.Outputs.GroupConfigureFunction(
                AgM9185ChannelGroupEnum.AgM9185ChannelGroupAll,
                ref _functionList,
                ref _valueList
             );

            _driver.Outputs.GroupEnable(
                AgM9185ChannelGroupEnum.AgM9185ChannelGroupAll, 
                ref _enableList
             );
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                if (_driver != null && _driver.Initialized)
                    _driver.Close();
            }
            _disposed = true;
        }
    }
}

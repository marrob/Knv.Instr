namespace Knv.Instr.DAQ.PCI6353
{

    using NationalInstruments.DAQmx;
    using System;
    using System.Linq;

    public static class Tools
    {
        public static bool DeviceIsPresent(string visaName)
        {
            bool retval = false;
            var devs = DaqSystem.Local.Devices;
            retval = devs.Contains(visaName);
            return retval;
        }

    }
}

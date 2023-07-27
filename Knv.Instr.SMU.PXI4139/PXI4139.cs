/* 
 * 
 * Ivi.Driver.dll
 * C:\Program Files (x86)\IVI Foundation\IVI\Microsoft.NET\Framework32\v2.0.50727\IviFoundationSharedComponents 2.0.0\Ivi.Driver.dll
 * ..\lib\IviFoundationSharedComponents 2.0.0\Ivi.Driver.dll
 *  
 * Ivi.DCPwr
 * C:\Program Files (x86)\IVI Foundation\IVI\Microsoft.NET\Framework32\v2.0.50727\IviFoundationSharedComponents 2.0.0\Ivi.DCPwr.dll
 * ..\lib\IviFoundationSharedComponents 2.0.0\Ivi.DCPwr.dll
 *  
 * NationalInstruments.Common.dll
 * C:\Program Files (x86)\National Instruments\Measurement Studio\DotNET\v4.0\AnyCPU\NationalInstruments.Common 19.1.40\NationalInstruments.Common.dll
 * ..\lib\NationalInstruments.Common(x86) 19.1.40\NationalInstruments.Common.dll
 * 
 * NationalInstruments.ModularInstruments.Common.dll
 * C:\Program Files (x86)\IVI Foundation\IVI\Microsoft.NET\Framework32\v4.5.50709\NationalInstruments.ModularInstruments.Common 22.5.0\NationalInstruments.ModularInstruments.Common.dll
 * ..\lib\NationalInstruments.ModularInstruments.Common(x86) 22.5.0\NationalInstruments.ModularInstruments.Common.dll
 * 
 * NationalInstruments.ModularInstruments.ModularInstrumentsSystem.dll
 * C:\Program Files (x86)\IVI Foundation\IVI\Microsoft.NET\Framework32\v4.5.50709\NationalInstruments.ModularInstruments.ModularInstrumentsSystem 1.4.45\NationalInstruments.ModularInstruments.ModularInstrumentsSystem.dll
 * ..\lib\NationalInstruments.ModularInstruments.Common(x86) 22.5.0\NationalInstruments.ModularInstruments.Common.dll
 * 
 * NationalInstruments.ModularInstruments.NIDCPower.Fx45.dll
 * C:\Program Files (x86)\IVI Foundation\IVI\Microsoft.NET\Framework32\v4.5.50709\NationalInstruments.ModularInstruments.NIDCPower 19.0.0\NationalInstruments.ModularInstruments.NIDCPower.Fx45.dll
 * ..\lib\NationalInstruments.ModularInstruments.NIDCPower(x86) 19.0.0\
 * 
 * 
 * - Konfigurálás előtt a sessiont Abortálni kell, különben nem lehet konfigurálni.
 * - Az NI motor a konfigurációt a Commit vagy a Initiate metódussal után ellenörzi.
 * 
 * 
 */



namespace Knv.Instr.SMU.PXI4139
{
    using Ivi.DCPwr;
    using NationalInstruments.ModularInstruments.NIDCPower;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NationalInstruments;

    public class PXI4139 : ISourceMeasureUnits
    {
        const string VoltageLevelRange = "Voltage Level Ragne";
        const string CurrentLimitRange = "Current Limit Range";

        public Dictionary<string, string[]> Ranges = new Dictionary<string, string[]>()
        {
            { VoltageLevelRange, new string[]{ "60V", "6V", "600mV" } },
            { CurrentLimitRange, new string[]{ "3A", "1A", "100mV", "10mA", "1mA", "100uA", "10uA", "1uA" } },
        };

        const string SelectedChannelName = "0";
        readonly NIDCPower _session = null;
        bool _disposed = false;
        readonly bool _simulation = false;

        public PXI4139(string resourceName, bool simulation)
        {
            _simulation = simulation;
            if (_simulation)
                return;
           _session = new NIDCPower(resourceName, idQuery: true, resetDevice: true);
        }

        public string Identify()
        {
            return $"{_session.Identity.InstrumentManufacturer}, {_session.Identity.InstrumentModel}, {_session.Identity.Revision }";
        }


        public void ConfigVoltageSource(string voltageRangeName, string currentLimitRangeName)
        {
            if (_simulation)
                return;

            /*** Voltage ***/
            string[] voltageItems;
            if (!Ranges.TryGetValue(VoltageLevelRange, out voltageItems))
                throw new ArgumentException($" The {VoltageLevelRange} is not supported. Supported functions: {string.Join(",", Ranges.Keys)}");

            if (!voltageItems.Contains(voltageRangeName))
                throw new ArgumentException($" The {voltageRangeName} is not supported range. Supported ranges: {string.Join(",", voltageItems)}");

            double voltageRange = 0.0;
            switch (voltageRangeName)
            { 
                case "60V": voltageRange = 60.0; break;
                case "6V": voltageRange = 6.0; break;
                case "600mV": voltageRange = 0.6; break;
            }

            _session.Control.Abort();
            _session.Outputs[SelectedChannelName].Source.Output.Function = DCPowerSourceOutputFunction.DCVoltage;
            _session.Outputs[SelectedChannelName].Source.Voltage.VoltageLevelAutorange = DCPowerSourceVoltageLevelAutorange.Off;
            _session.Outputs[SelectedChannelName].Source.Voltage.VoltageLevelRange = voltageRange;
            

            /*** Current ***/
            string[] currentItems;
            if (!Ranges.TryGetValue(CurrentLimitRange, out currentItems))
                throw new ArgumentException($" The {CurrentLimitRange} is not supported. Supported functions: {string.Join(",", Ranges.Keys)}");

            if (!currentItems.Contains(currentLimitRangeName))
                throw new ArgumentException($" The {CurrentLimitRange} is not supported range. Supported ranges: {string.Join(",", currentItems)}");

            double currentRange = 0.0;
            switch (currentLimitRangeName)
            {
                case "3A": currentRange = 3.0; break;
                case "1A": currentRange = 1.0; break;
                case "100mA": currentRange = 0.1; break;
                case "10mA": currentRange = 0.01; break;
                case "1mA": currentRange = 0.001; break;
                case "100uA": currentRange = 0.0001; break;
                case "10uA": currentRange = 0.00001; break;
                case "1uA": currentRange = 0.000001; break;
            }
            _session.Outputs[SelectedChannelName].Source.Voltage.CurrentLimitAutorange = DCPowerSourceCurrentLimitAutorange.Off;
            _session.Outputs[SelectedChannelName].Source.Voltage.CurrentLimitRange = currentRange;
        }


      
        public void SetVoltageSource(double voltage, double current)
        {
            if (_simulation)
                return;

            _session.Control.Abort();
            _session.Outputs[SelectedChannelName].Source.Voltage.VoltageLevel = voltage;
            _session.Outputs[SelectedChannelName].Source.Voltage.CurrentLimit = current;
            _session.Control.Initiate();
            // Wait for output to settle.
            _session.Events.SourceCompleteEvent.WaitForEvent(new PrecisionTimeSpan(21.0));
        }

       

        public void OnOff(bool enable)
        {
            if (_simulation)
                return;
            _session.Outputs[SelectedChannelName].Source.Output.Enabled = enable;
        }

        public double GetActualVolt()
        {

            if (!_simulation)
                return _session.Measurement.Measure(SelectedChannelName).VoltageMeasurements[0];
            else
                return new Random().NextDouble();
        }

        public double GetActualCurrent()
        {
            if (!_simulation)
                return _session.Measurement.Measure(SelectedChannelName).CurrentMeasurements[0];
            else
                return new Random().NextDouble();
        }

        private void ChannelCheck(string channelName)
        {
            var channels = new List<string>() { };

            foreach (IIviDCPwrOutput channel in _session.Outputs)
                channels.Add(channel.Name);

            if (!channels.Contains(channelName))
                throw new ArgumentException($"The '{channelName}' not found. Supported: {string.Join(",", channels)}");
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
                _session?.Utility.Reset();
                _session?.Dispose();
            }
            _disposed = true;
        }
    }
}

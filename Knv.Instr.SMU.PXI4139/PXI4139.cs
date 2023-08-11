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
 * C:\Program Files (x86)\IVI Foundation\IVI\Microsoft.NET\Framework32\v4.5.50709\NationalInstruments.ModularInstruments.Common 20.0.0\NationalInstruments.ModularInstruments.Common.dll
 * ..\lib\NationalInstruments.ModularInstruments.Common(x86) 20.0.0\NationalInstruments.ModularInstruments.Common.dll
 * 

 * NationalInstruments.ModularInstruments.NIDCPower.Fx45.dll
 * C:\Program Files (x86)\IVI Foundation\IVI\Microsoft.NET\Framework32\v4.5.50709\NationalInstruments.ModularInstruments.NIDCPower 21.3.1\NationalInstruments.ModularInstruments.NIDCPower.Fx45.dll
 * ..\lib\NationalInstruments.ModularInstruments.NIDCPower(x86) 21.3.1\NationalInstruments.ModularInstruments.NIDCPower.Fx45.dll
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


        #region Current Source

        public void ConfigCurrentSource(string voltageLimitRangeName = "6V", string currentRangeName = "1mA")
        {
            ConfigCurrentSource(voltageLimitRangeName, currentRangeName, "Local");
        }

        public void ConfigCurrentSource(string voltageLimitRangeName = "6V", string currentRangeName = "1mA", string sense = "Local")
        {
            if (_simulation)
                return;

            _session.Control.Abort();

            _session.Outputs[SelectedChannelName].Source.Output.Function = DCPowerSourceOutputFunction.DCCurrent;

            _session.Outputs[SelectedChannelName].Source.Current.CurrentLevelAutorange = DCPowerSourceCurrentLevelAutorange.Off;
            _session.Outputs[SelectedChannelName].Source.Current.CurrentLevelRange = CurrentRangeNameToValue(currentRangeName);  

            _session.Outputs[SelectedChannelName].Source.Current.VoltageLimitAutorange = DCPowerSourceVoltageLimitAutorange.Off;
            _session.Outputs[SelectedChannelName].Source.Current.VoltageLimitRange = VoltageRangeNameToValue(voltageLimitRangeName);

            _session.Outputs[SelectedChannelName].Source.TransientResponse = DCPowerSourceTransientResponse.Normal;

            SetSense(sense);
        }

        public void SetCurrentSource(double voltageLimit, double currentLevel)
        {
            if (_simulation)
                return;

            _session.Control.Abort();
            _session.Outputs[SelectedChannelName].Source.Current.CurrentLevel = currentLevel;
            _session.Outputs[SelectedChannelName].Source.Current.VoltageLimit = voltageLimit;
            _session.Control.Initiate();
            // Wait for output to settle.
            _session.Events.SourceCompleteEvent.WaitForEvent(new PrecisionTimeSpan(21.0));
        }
        #endregion
        
        #region Voltage Source

        public void ConfigVoltageSource(string voltageRangeName = "6V", string currentLimitRangeName = "100mA")
        {
            ConfigVoltageSource(voltageRangeName, currentLimitRangeName, "Local");
        }

        public void ConfigVoltageSource(string voltageRangeName = "6V", string currentLimitRangeName = "100mA", string sense = "Local")
        {
            if (_simulation)
                return;

            _session.Control.Abort();

            _session.Outputs[SelectedChannelName].Source.Output.Function = DCPowerSourceOutputFunction.DCVoltage;

            _session.Outputs[SelectedChannelName].Source.Voltage.VoltageLevelAutorange = DCPowerSourceVoltageLevelAutorange.Off;
            _session.Outputs[SelectedChannelName].Source.Voltage.VoltageLevelRange = VoltageRangeNameToValue(voltageRangeName);
            
            _session.Outputs[SelectedChannelName].Source.Voltage.CurrentLimitAutorange = DCPowerSourceCurrentLimitAutorange.Off;
            _session.Outputs[SelectedChannelName].Source.Voltage.CurrentLimitRange = CurrentRangeNameToValue(currentLimitRangeName);

            _session.Outputs[SelectedChannelName].Source.TransientResponse = DCPowerSourceTransientResponse.Normal;


            SetSense(sense);

            //TODO: ApertureTime
            //_session.Outputs[SelectedChannelName].Measurement.ConfigureApertureTime(1, DCPowerMeasureApertureTimeUnits.PowerLineCycles);
        }


      
        public void SetVoltageSource(double voltageLevel, double currentLimit)
        {
            if (_simulation)
                return;

            _session.Control.Abort();
            _session.Outputs[SelectedChannelName].Source.Voltage.VoltageLevel = voltageLevel;
            _session.Outputs[SelectedChannelName].Source.Voltage.CurrentLimit = currentLimit;
            _session.Control.Initiate();
            // Wait for output to settle.
            _session.Events.SourceCompleteEvent.WaitForEvent(new PrecisionTimeSpan(21.0));

        }

        #endregion



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

        internal void SetSense(string sense)
        {
            if (_simulation)
                return;

            sense = sense.ToUpper().Trim();

            switch (sense)
            {
                case "LOCAL":
                    _session.Outputs[SelectedChannelName].Measurement.Sense = DCPowerMeasurementSense.Local;
                    break;
                case "REMOTE":
                    _session.Outputs[SelectedChannelName].Measurement.Sense = DCPowerMeasurementSense.Remote;
                    break;
                default:
                    throw new ArgumentException($" The {sense} is not supported. Supported functions: Local, Remote");
            }
        }

        internal double VoltageRangeNameToValue(string rangeName)
        {
            /*** Voltage ***/
            string[] voltageItems;
            if (!Ranges.TryGetValue(VoltageLevelRange, out voltageItems))
                throw new ArgumentException($" The {VoltageLevelRange} is not supported. Supported functions: {string.Join(",", Ranges.Keys)}");

            if (!voltageItems.Contains(rangeName))
                throw new ArgumentException($" The {rangeName} is not supported range. Supported ranges: {string.Join(",", voltageItems)}");
           
            double voltageRange = 0.0;
            switch (rangeName)
            {
                case "60V": voltageRange = 60.0; break;
                case "6V": voltageRange = 6.0; break;
                case "600mV": voltageRange = 0.6; break;
            }
            return voltageRange;
        }

        internal double CurrentRangeNameToValue(string rangeName)
        {
            /*** Current ***/
            string[] currentItems;
            if (!Ranges.TryGetValue(CurrentLimitRange, out currentItems))
                throw new ArgumentException($" The {CurrentLimitRange} is not supported. Supported functions: {string.Join(",", Ranges.Keys)}");

            if (!currentItems.Contains(rangeName))
                throw new ArgumentException($" The {CurrentLimitRange} is not supported range. Supported ranges: {string.Join(",", currentItems)}");

            double currentRange = 0.0;
            switch (rangeName)
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
            return currentRange;
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

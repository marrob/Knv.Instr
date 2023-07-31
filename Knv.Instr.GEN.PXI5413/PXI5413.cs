/* 
 * 
 * Ivi.Driver.dll
 * C:\Program Files (x86)\IVI Foundation\IVI\Microsoft.NET\Framework32\v2.0.50727\IviFoundationSharedComponents 2.0.0\Ivi.Driver.dll
 * ..\lib\IviFoundationSharedComponents 2.0.0\Ivi.Driver.dll
 *  
 * Ivi.DCPwr
 * C:\Program Files (x86)\IVI Foundation\IVI\Microsoft.NET\Framework32\v2.0.50727\IviFoundationSharedComponents 2.0.0\Ivi.Fgen.dll
 * ..\lib\IviFoundationSharedComponents 2.0.0\Ivi.Fgen.dll
 *  
 * NationalInstruments.Common.dll
 * C:\Program Files (x86)\National Instruments\Measurement Studio\DotNET\v4.0\AnyCPU\NationalInstruments.Common 19.1.40\NationalInstruments.Common.dll
 * ..\lib\NationalInstruments.Common(x86) 19.1.40\NationalInstruments.Common.dll
 * 
 * NationalInstruments.ModularInstruments.Common.dll
 * C:\Program Files (x86)\IVI Foundation\IVI\Microsoft.NET\Framework32\v4.5.50709\NationalInstruments.ModularInstruments.Common 22.5.0\NationalInstruments.ModularInstruments.Common.dll
 * ..\lib\NationalInstruments.ModularInstruments.Common(x86) 22.5.0\NationalInstruments.ModularInstruments.Common.dll
 * 
 * NationalInstruments.ModularInstruments.NIFgen.Fx45.dll
 * C:\Program Files (x86)\IVI Foundation\IVI\Microsoft.NET\Framework32\v4.5.50709\NationalInstruments.ModularInstruments.NIFgen 21.8.0\NationalInstruments.ModularInstruments.NIFgen.Fx45.dll
 * ..\lib\NationalInstruments.ModularInstruments.NIFgen 21.8.0\NationalInstruments.ModularInstruments.NIFgen.Fx45.dll
 * 
 * 
 */

namespace Knv.Instr.GEN.PXI5413 
{
    using NationalInstruments.ModularInstruments.NIFgen;
    using System;
    using System.Linq;

    public class PXI5413: IGenerator, IDisposable
    {

        readonly NIFgen _session = null;
        bool _disposed = false;
        readonly bool _simulation = false;

        public PXI5413(string resourceName, bool simulation)
        {
            _simulation = simulation;
            if (_simulation)
                return;
            _session = new NIFgen(resourceName, idQuery: true, reset: true);
            _session.Output.OutputMode = OutputMode.Function;
        }

        public string Identify()
        {
            return $"{_session.Identity.InstrumentManufacturer}, {_session.Identity.InstrumentModel}, {_session.Identity.SerialNumber}";
        }

        public void SetWaveform(string channel = "0", string waveformName = "Square")
        {
            if (_simulation)
                return;

            if (Enum.TryParse(waveformName, out StandardWaveform waveform))
                _session.StandardWaveform.SetWaveformFunction(channel, waveform);
            else
            {
                var supportedWaves = string.Join(",", Enum.GetNames(typeof(StandardWaveform)).ToList());
                throw new Exception($"Waveform {waveformName} not supported. Supported waveforms:{supportedWaves}");
            }
        }

        public void SetAmplitude(string channel = "0", double amplitudeVpp = 1)
        {
            if (_simulation)
                return;
            _session.StandardWaveform.SetAmplitude(channel, amplitudeVpp);
        }  

        public void SetFrequency(string channel = "0", double frequencyHz = 1000)
        {
            if (_simulation)
                return;
            _session.StandardWaveform.SetFrequency(channel, frequencyHz);
        }   

        public void SetOffset(string channel = "0", double offsetVp = 0.5)
        {
            if (_simulation)
                return;
            _session.StandardWaveform.SetDCOffset(channel, offsetVp);
        }

        public void SetDutyCycle(string channel, double dutyCycle)
        {
            if (_simulation)
                return;
            _session.StandardWaveform.SetDutyCycleHigh(channel, dutyCycle);
        }

        public void Start()
        {
            if (_simulation)
                return;
            _session.AbortGeneration();
            _session.InitiateGeneration();
        }

        public void Stop()
        {
            if (_simulation)
                return;
            _session.AbortGeneration();
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
                _session?.AbortGeneration();
                _session?.Utility.Reset();
                _session?.Dispose();
            }
            _disposed = true;
        }
    }
}

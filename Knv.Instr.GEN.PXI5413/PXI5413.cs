/* 
 * 
 * Ivi.Driver.dll
 * C:\Program Files (x86)\IVI Foundation\IVI\Microsoft.NET\Framework32\v2.0.50727\IviFoundationSharedComponents 2.0.0\Ivi.Driver.dll
 * ..\lib\IviFoundationSharedComponents 2.0.0\Ivi.Driver.dll
 *  
 * Ivi.Fgen
 * C:\Program Files (x86)\IVI Foundation\IVI\Microsoft.NET\Framework32\v2.0.50727\IviFoundationSharedComponents 2.0.0\Ivi.Fgen.dll
 * ..\lib\IviFoundationSharedComponents 2.0.0\Ivi.Fgen.dll
 *  
 * NationalInstruments.Common.dll
 * C:\Program Files (x86)\National Instruments\Measurement Studio\DotNET\v4.0\AnyCPU\NationalInstruments.Common x.x.x\NationalInstruments.Common.dll
 * ..\lib\NationalInstruments.Common(x86) x.x.x\NationalInstruments.Common.dll
 * 
 * NationalInstruments.ModularInstruments.Common.dll
 * C:\Program Files (x86)\IVI Foundation\IVI\Microsoft.NET\Framework32\v4.5.50709\NationalInstruments.ModularInstruments.Common x.x.x\NationalInstruments.ModularInstruments.Common.dll
 * ..\lib\NationalInstruments.ModularInstruments.Common(x86) x.x.x\NationalInstruments.ModularInstruments.Common.dll
 * 
 * NationalInstruments.ModularInstruments.NIFgen.Fx45.dll
 * C:\Program Files (x86)\IVI Foundation\IVI\Microsoft.NET\Framework32\v4.5.50709\NationalInstruments.ModularInstruments.NIFgen 21.3.0\NationalInstruments.ModularInstruments.NIFgen.Fx45.dll
 * ..\lib\NationalInstruments.ModularInstruments.NIFgen 21.3.0\NationalInstruments.ModularInstruments.NIFgen.Fx45.dll
 * 
 * 
 */

namespace Knv.Instr.GEN.PXI5413 
{
    using NationalInstruments.ModularInstruments.NIFgen;
    using System;
    using System.Linq;
    using System.Runtime.Remoting.Channels;

    public class PXI5413: IGenerator, IDisposable
    {

        readonly NIFgen _session = null;
        string _channel = "0";
        bool _disposed = false;
        readonly bool _simulation = false;

        public PXI5413(string resourceName = "J22_FGEN_1", string channel = "0",  bool simulation = false)
        {
            _simulation = simulation;
            if (_simulation)
                return;
            _channel = channel;
            _session = new NIFgen(resourceName, channel, true, "");
        }

        public string Identify()
        {
            return $"{_session.Identity.InstrumentManufacturer}, {_session.Identity.InstrumentModel}, {_session.Identity.SerialNumber}";
        }


        /// <summary>
        /// A Waform genertáornak van egy sulyos korlátyja
        /// Az offszet nem lehet nagyobb mint a Vpp/2. Ebben az esetben használd az Arbitrary módot és a ConfigPwm metódust
        /// </summary>
        public void ConfigWaveform(string waveformName = "Square", double amplitudeVpp = 1, double frequencyHz = 1000, double offsetVp = 0.5, double dutyCycle = 50)
        {
            if (_simulation)
                return;

            _session.Output.OutputMode = OutputMode.Function;
            if (Enum.TryParse(waveformName, out StandardWaveform waveform))
                _session.StandardWaveform.SetWaveformFunction(_channel, waveform);
            else
            {
                var supportedWaves = string.Join(",", Enum.GetNames(typeof(StandardWaveform)).ToList());
                throw new Exception($"Waveform {waveformName} not supported. Supported waveforms:{supportedWaves}");
            }

            _session.StandardWaveform.SetAmplitude(_channel, amplitudeVpp);
            _session.StandardWaveform.SetFrequency(_channel, frequencyHz);
            _session.StandardWaveform.SetDCOffset(_channel, offsetVp);
            _session.StandardWaveform.SetDutyCycleHigh(_channel, dutyCycle);
        }

        /// <summary>
        /// Megoldja azt a problémát hogy az Vpp felétől nem lehet nagyobb az offszet
        /// </summary>
        public void ConfigPwm(double vpp = 1, double offset = 0, double frequencyHz = 1000, double dutyCycle = 50 )
        {
            if (_simulation)
                return;

            dutyCycle /= 100; 
            int samples = 1000;
            int gain = 6;

            double[] waveform = new double[samples];
            for (int i = 0; i < samples; i++)
            {
                if (i % samples < samples * dutyCycle)
                    waveform[i] = 1/(gain / vpp); 
                else
                    waveform[i] = 0;
            }

            _session?.Utility.Reset();

            double sampleRate = frequencyHz * samples;
            _session.Trigger.SetTriggerMode(_channel, TriggerMode.Continuous);
            _session.Output.OutputMode = OutputMode.Arbitrary;
            _session.Output.SetLoadImpedance(_channel, double.MaxValue);
            _session.Arbitrary.SampleRate = sampleRate;
            _session.Timing.SampleClock.ClockMode = ClockMode.Automatic;
            _session.Output.SetEnabled(_channel, true);
            int waveformHandle = _session.Arbitrary.Waveform.Allocate(_channel, waveform.Length);
            _session.Arbitrary.Waveform.Write(_channel, waveformHandle, waveform);
            _session.Arbitrary.SetGain(_channel, gain);
            _session.Arbitrary.SetOffset(_channel, offset);
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
            _session.Output.SetEnabled(_channel, false);
            _session?.AbortGeneration();
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
                _session?.StandardWaveform.SetDCOffset(_channel, 0);
                _session?.Arbitrary.SetOffset(_channel, 0);
                _session?.Utility.Reset();
                _session?.Dispose();
            }
            _disposed = true;
        }
    }
}

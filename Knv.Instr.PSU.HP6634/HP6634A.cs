
/*
 * 
 * A HP663A Does not support standard SCPI commands
 * 
 * Example: 
 * *IDN?\n response -> \n  Last error is: Addressed to talk and nothing to say
 * ID? -> HP6634A\r\n
 * 
 * VSET does not have Response! If you read you will get Error...
 * 
 */
namespace Knv.Instr.PSU.HP6634A
{
    using NationalInstruments.NI4882;
    using System;

    public class HP6634A : Log, IDisposable, IPowerSupply
    { 
        bool _disposed = false;
        Device _device;


        public HP6634A(int address, bool isSim)
        {
            _device = new Device(boardNumber: 0, new Address((byte)address));
            LogWriteLine("Instance created.");
            _device.Reset(); //Törli a beállításokat
            _device.Clear(); //Törli a hibákat
        }

        public string Identify()
        {
            var resp = Query($"ID?");
            return resp;
        }

        public void SetOutput(double volt, double current)
        {
            Write($"VSET {volt};ISET {current}");
        }

        public void SetOutput(double volt, double current, bool onOff)
        {
            Write($"VSET {volt};ISET {current};OUT {(onOff ? "1" : "0")}");
        }

        public double SetOutputGetActualVolt(double volt, double current)
        {
            var resp = Query($"VSET {volt};ISET {current};VOUT?");
            return double.Parse(resp);
        }

        public double GetActualVolt()
        {
            var resp = Query($"VOUT?");
            return double.Parse(resp);
        }

        public double GetActualCurrent()
        {
            var resp = Query($"IOUT?");
            return double.Parse(resp);
        }

        public string GetErrors()
        {
            string request = "ERR?";
            var resp = Query(request);
            return resp;
        }

        public string Query(string request)
        { 
            _device.Write($"{request}\r\n");
            LogWriteLine($"Tx:{request}");
            var response = _device.ReadString().Trim( new char[] {'\r', '\n', ' ' });
            LogWriteLine($"Rx:{response}");
            return response;
        }

        public void Write(string request)
        {
            _device.Write($"{request}\r\n");
            LogWriteLine($"Tx:{request}");
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
                _device?.Dispose();
                LogWriteLine("Instance disposed.");
            }
            _disposed = true;

        }
    }
}
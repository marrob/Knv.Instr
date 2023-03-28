
using NationalInstruments.NI4882;
using System;
using System.Security.Cryptography;

namespace Knv.Instr.GPIB
{
    public class HP6634A_PSU : IPowerSupply
    { 
        bool _disposed = false;
        Device _device;


        public HP6634A_PSU(int address, bool isSim)
        {
            _device = new Device(boardNumber: 0, new Address((byte)address));
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
            Write($"VSET {volt}");
            Write($"ISET {current}");
        }

        public void SetOutput(double volt, double current, bool onOff)
        {
            Write($"VSET {volt}");
            Write($"ISET {current}");
            Write($"OUT {(onOff ? "1":"0")}");
        }



        public string Query(string send)
        { 
            _device.Write($"{send}\r\n");
            var resp = _device.ReadString().Trim( new char[] {'\r', '\n' });
            return resp;
        }

        public void Write(string send)
        {
            _device.Write($"{send}\r\n");
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
               if(_device!= null)
                    _device.Dispose();
            _disposed = true;

        }
    }
}
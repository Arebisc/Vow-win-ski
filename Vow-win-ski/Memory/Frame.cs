using System;
using System.Linq;

namespace Vow_win_ski.Memory
{
    public class Frame
    {
        private byte[] _fields;
        public int Offset;
        public bool IsValid;
        public int FrameNumber;

        public Frame(int number)
        {
            _fields = new byte[16];
            ClearFrame();
            Offset = 0;
            IsValid = false;
            FrameNumber = number;
        }
        public void WriteFrame(byte[] data)
        {
            Offset = data.Length;
            IsValid = true;
            for (int i = 0; i < data.Length; i++)
            {
                _fields[i] = data[i];
            }
        }
        public byte[] ReadFrame()
        {
            return _fields.Take(Offset).ToArray();
        }
        public void ClearFrame()
        {
            Offset = 16;
            IsValid = false;
            for (int i = 0; i < _fields.Length; i++)
            {
                _fields[i] = 0x00;
            }
        }

        public void ShowFrame()
        {
            foreach (var field in _fields)
            {
                Console.WriteLine(field);
            }
        }
    }
}

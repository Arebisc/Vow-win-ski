using System;
using System.Linq;

namespace Vow_win_ski.Memory
{
    public class Frame
    {
        private const int FrameSize = 16;
        private readonly char[] _fields;
        public int Offset;

        public Frame()
        {
            _fields = new char[FrameSize];
            ClearFrame();
            Offset = 0;
        }
        public void WriteFrame(char[] data)
        {
            Offset = data.Length;
            for (int i = 0; i < data.Length; i++)
            {
                _fields[i] = data[i];
            }
        }
        public char[] ReadFrame()
        {
            return _fields.Take(Offset).ToArray();
        }
        public void ClearFrame()
        {
            Offset = 16;
            for (int i = 0; i < _fields.Length; i++)
            {
                _fields[i] = '0';
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

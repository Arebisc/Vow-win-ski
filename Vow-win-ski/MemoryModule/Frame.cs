using System;
using System.Linq;

namespace Vow_win_ski.MemoryModule
{
    public class AllocationUnit
    {
        private int _size;
        private readonly char[] _fields;
        public int Offset;

        public AllocationUnit(int frameSize)
        {
            _size = frameSize;
            _fields = new char[_size];
            ClearAllocationUnit();
            Offset = 0;
        }
        public void WriteAllocationUnit(char[] data)
        {
            Offset = data.Length;
            for (int i = 0; i < data.Length; i++)
            {
                _fields[i] = data[i];
            }
        }
        public char[] ReadAllocationUnit()
        {
            return _fields.Take(Offset).ToArray();
        }
        public void ClearAllocationUnit()
        {
            Offset = 0;
            for (int i = 0; i < _fields.Length; i++)
            {
                _fields[i] = '0';
            }
        }

        public void ShowAllocationUnit()
        {
            for(int i=0;i<_fields.Length;i++)
            {
                if (_fields[i] == '\r')
                {
                    Console.Write("\\r ".PadRight(4));
                }
                else if (_fields[i] == '\n')
                {
                    Console.Write("\\n ".PadRight(4));
                }
                else
                {
                    Console.Write(_fields[i]+" ".PadRight(3));
                }
            }
            Console.WriteLine("");
        }

        public char GetByte(int index)
        {
            return _fields[index];
        }

        public void ChangeByte(int index, char data)
        {
            _fields[index] = data;
        }
    }
}

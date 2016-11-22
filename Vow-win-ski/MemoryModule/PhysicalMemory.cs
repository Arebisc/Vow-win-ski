using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vow_win_ski.MemoryModule
{
   public class PhysicalMemory
   {
       private const int FramesCount = 32;
       private List<Frame> _memory;

       public PhysicalMemory()
       {
            _memory=new List<Frame>();
           for (int i = 0; i < FramesCount; i++)
           {
               _memory.Add(new Frame());
           }
       }

       public Frame GetFrame(int index)
       {
           return _memory[index];
       }

       public void ShowMemory()
       {
            Console.WriteLine("Tutaj");
           foreach (var frame in _memory)
           {
               frame.ShowFrame();
           }
       }

       public void ShowFrame(int number)
       {
           _memory[number].ShowFrame();
       }

       public void SetFrame(int index, char[] data)
       {
           _memory[index].WriteFrame(data);
       }
   }
}

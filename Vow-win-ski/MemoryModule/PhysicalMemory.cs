using System;
using System.Collections.Generic;

namespace Vow_win_ski.MemoryModule
{
   public class PhysicalMemory
   {
       private int _framesCount;
       private int _framesSize;
       private readonly List<AllocationUnit> _memory;

       public PhysicalMemory(int framesCount,int framesSize)
       {
           _framesCount = framesCount;
           _framesSize = framesSize;
            _memory=new List<AllocationUnit>();
           for (int i = 0; i < _framesCount; i++)
           {
               _memory.Add(new AllocationUnit(_framesSize));
           }
       }

       public AllocationUnit GetFrame(int index)
       {
           return _memory[index];
       }

       public void ShowMemory()
       {
            Console.WriteLine("Tutaj");
           foreach (var frame in _memory)
           {
               frame.ShowAllocationUnit();
           }
       }

       public void ShowFrame(int number)
       {
           _memory[number].ShowAllocationUnit();
       }

       public void SetFrame(int index, char[] data)
       {    
           _memory[index].ClearAllocationUnit();
           _memory[index].WriteAllocationUnit(data);
       }
   }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileSystem;
using FileSystem.FileSystem;

using Vow_win_ski.MemoryModule;
using System.Threading;
using Vow_win_ski.IPC;
using Vow_win_ski.Processes;

namespace Vow_win_ski
{
    class Program
    {
        static void Main(string[] args)
        {
            Memory pamiec = new Memory();
            PCB pcb = new PCB(1);
            PCB pcb2 = new PCB(2);
            PCB pcb3 = new PCB(3);

            char[] program = {'B', 'a', 'r', 't', 'o', 's', 'z', 'e', 'k', 'T','o','C','h','u','j'};
            char[] program1 = { 'B', 'a', 'r', 't', 'o', 's', 'z', 'e', 'k', 'T', 'o', 'C', 'h', 'u', 'j' };
            char[] program2 = { 'B', 'a', 'r', 't', 'o', 's', 'z', 'e', 'k', 'T', 'o', 'C', 'h', 'u', 'j' };

            pamiec.AllocateMemory(pcb,program);
            pamiec.AllocateMemory(pcb2,program1);
            pamiec.AllocateMemory(pcb3,program2);

            ProcessPages data = (ProcessPages)pcb.MemoryBlocks;
            ProcessPages data1 = (ProcessPages) pcb2.MemoryBlocks;
            ProcessPages data2 = (ProcessPages) pcb3.MemoryBlocks;


      
            var Char = data.ReadByte(6);
            var Char1 = data1.ReadByte(2);
            var Char2 = data2.ReadByte(14);
            var Char3 = data.ReadByte(10);
            var Char4 = data.ReadByte(12);
            var Char5 = data.ReadByte(3);
            var Char6 = data.ReadByte(8);

            pamiec.RemoveFromMemory(pcb);

            var Char7 = data1.ReadByte(12);
            data1.ChangeByte(12,'z');
            var Char8 = data1.ReadByte(12);


            pamiec.DisplayFreeFrames();
            pamiec.DisplayPageContent(pcb2.PID,1);
            pamiec.DisplayPhysicalMemory();
            pamiec.DisplayPageList(pcb2.PID);



            Console.ReadKey();
        }
    }
}

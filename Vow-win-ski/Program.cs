using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileSystem;
using FileSystem.FileSystem;
using Vow_win_ski.MemoryModule;
using System.Threading;
using Vow_win_ski.CPU;
using Vow_win_ski.IPC;
using Vow_win_ski.Processes;

namespace Vow_win_ski
{
    class Program
    {
        static void Main(string[] args)
        {
            CPU.CPU.GetInstance.DisplayDebug();

            pamiec.DisplayPhysicalMemory();
            pamiec.DisplayFreeFrames();
            pamiec.DisplayPageList(1);
            Console.ReadKey();
        }
    }
}

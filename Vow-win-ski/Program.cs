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

            pamiec.TestFillMemory();

            pamiec.DisplayPhysicalMemory();
            pamiec.DisplayFreeFrames();
            pamiec.DisplayPageList(1);

            pamiec.TestCleanMemory();

            pamiec.DisplayPhysicalMemory();
            pamiec.DisplayFreeFrames();
            pamiec.DisplayPageList(1);
            Console.ReadKey();
        }
    }
}

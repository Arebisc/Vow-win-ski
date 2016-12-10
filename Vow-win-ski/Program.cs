using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vow_win_ski.MemoryModule;
using System.Threading;
using Vow_win_ski.CPU;
using Vow_win_ski.FileSystem;
using Vow_win_ski.IPC;
using Vow_win_ski.Processes;

namespace Vow_win_ski
{
    class Program
    {
        static void InitSystemResources()
        {
            Disc.InitDisc();
        }

        static void Main(string[] args)
        {
            DisplayLogo();

            InitSystemResources();

            Shell.GetShell.OpenShell();

        }

        static void DisplayLogo()
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(" _    __                      _       __ _               _____  __    _ ");
            Console.WriteLine("| |  / /____  _      __      | |     / /(_)____         / ___/ / /__ (_)");
            Console.WriteLine("| | / // __ \\| | /| / /______| | /| / // // __ \\ ______ \\__ \\ / //_// / ");
            Console.WriteLine("| |/ // /_/ /| |/ |/ //_____/| |/ |/ // // / / //_____/___/ //   / / /  ");
            Console.WriteLine("|___/ \\____/ |__/|__/        |__/|__//_//_/ /_/       /____//_/\\_\\/_/");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine();
        }
    }
}

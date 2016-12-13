using System;
using Vow_win_ski.MemoryModule;
using Vow_win_ski.CPU;
using Vow_win_ski.FileSystem;
using Vow_win_ski.IPC;
using Vow_win_ski.Processes;

namespace Vow_win_ski
{
    class Program
    {
        static void InitSystemResources(string[] args)
        {
            //wstawiać inity here

           PipeServer.InitServer();

            if (args.Length > 0)
                Disc.InitDisc(args[0]);
            else
                Disc.InitDisc();
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Uruchamianie systemu...");
            DisplayLogo();
            InitSystemResources(args);

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

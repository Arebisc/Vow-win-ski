using System;
using System.IO;
using System.Media;
using System.Threading;
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
            Thread.Sleep(500);
            LockersHolder.InitLockers();
            Thread.Sleep(500);
            PipeServer.InitServer();
            Thread.Sleep(500);
            if (args.Length > 0)
                Disc.InitDisc(args[0]);
            else
                Disc.InitDisc();
        }


        static void Main(string[] args)
        {
            Console.WriteLine("Uruchamianie systemu...");
            Directory.SetCurrentDirectory(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).ToString()).ToString());

            DisplayLogo();
            InitSystemResources(args);
            try
            { 
                Shell.GetShell.OpenShell();
            }
            catch (Exception e)
            {
                SoundPlayer sp = new SoundPlayer("spin.wav");
                sp.PlayLooping();
                PipeServer.GetServer.Exit();
                Console.BufferHeight = 25;
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + ("Coś się popsuło :(".Length / 2)) + "}", "Coś się popsuło :("));
                Console.WriteLine();
                Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (e.GetType().ToString().Length / 2)) + "}", e.GetType().ToString()));
                Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (e.Message.Length / 2)) + "}", e.Message));
                Console.Read();
                Console.ResetColor();
                Console.Clear();
            }
        }

        static void DisplayLogo()
        {
            SoundPlayer sp1 = new SoundPlayer("close.wav");
            sp1.Play();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Thread.Sleep(100);
            Console.WriteLine(" _    __                      _       __ _               _____  __    _ ");
            Thread.Sleep(100);
            Console.WriteLine("| |  / /____  _      __      | |     / /(_)____         / ___/ / /__ (_)");
            Thread.Sleep(100);
            Console.WriteLine("| | / // __ \\| | /| / /______| | /| / // // __ \\ ______ \\__ \\ / //_// / ");
            Thread.Sleep(100);
            Console.WriteLine("| |/ // /_/ /| |/ |/ //_____/| |/ |/ // // / / //_____/___/ //   / / /  ");
            Thread.Sleep(100);
            Console.WriteLine("|___/ \\____/ |__/|__/        |__/|__//_//_/ /_/       /____//_/\\_\\/_/");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine();
        }
    }
}

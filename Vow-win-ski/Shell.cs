using System;
using Vow_win_ski.FileSystem;

namespace Vow_win_ski
{
    public class Shell
    {
        private Shell()
        {}

        private static volatile Shell instance;
        private static object syncRoot = new object();
        public static Shell GetShell
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new Shell();
                        }
                    }
                }
                return instance;
            }
        }

        public void OpenShell()
        {
            bool breakLoop = false;
            while (!breakLoop)
            {
                Console.WriteLine();
                //Disc.GetDisc.CurrentFolder.PathToFolder
                Console.Write("root\\>");
                string p1 = "";
                string p2 = "";
                string cmd = "";
                string cmdline = Console.ReadLine();

                var x = 0;
                for (var i = 0; i < cmdline.Length && x != 3; i++)
                {
                    switch (x)
                    {
                        case 0:
                            if (cmdline[i] != ' ')
                                cmd += cmdline[i];
                            else
                                x++;
                            break;
                        case 1:
                            if (cmdline[i] != ' ')
                                p1 += cmdline[i];
                            else
                                x++;
                            break;
                        case 2:
                            p2 = cmdline.Substring(p1.Length + cmd.Length + 2);
                            x++;
                            break;
                    }
                }

                cmd = cmd.ToUpper();
                switch (cmd)
                {
                    case "HELP":
                        ShowHelp();
                        break;
                    case "DIR":
                    case "LS":
                        Disc.GetDisc.ShowDirectory();
                        break;
                    case "CF":
                        Disc.GetDisc.CreateFile(p1, p2);
                        break;
                    case "TYPE":
                        Console.WriteLine(Disc.GetDisc.GetFileData(p1) ?? "Error reading file");
                        break;
                    case "DF":
                        Disc.GetDisc.DeleteFile(p1);
                        break;
                        break;
                    case "APP":
                        Disc.GetDisc.AppendToFile(p1, p2);
                        break;
                    case "DDB":
                        Disc.GetDisc.DisplayDataBlocks();
                        break;
                    case "":
                        break;
                    case "EXIT":
                        breakLoop = true;
                        break;
                    default:
                        Console.WriteLine("Unknown command");
                        break;
                }
            }
        }








        private void ShowHelp()
        {
            Console.WriteLine();
            Console.WriteLine("Parameters: [optional] {obligatory}");
            Console.WriteLine();
            Console.WriteLine("Command\t\t\tDescription");
            Console.WriteLine("HELP\t\t\tShows this list");
            Console.WriteLine("DIR \t\tShows a list of files");
            Console.WriteLine("LS \t\tSame as DIR");
            Console.WriteLine("CF {name} [data]\tCreates file {name} and fills it with [data]");
            Console.WriteLine("APP {name} [data]\tAppends [data] to file {name}");
            Console.WriteLine("TYPE {name}\tShows data from file {name}");
            Console.WriteLine("DF {name}\t\tDeletes file {name}");
            Console.WriteLine("DDB \t\t\tShows raw data of all data blocks");
        }
    }
}

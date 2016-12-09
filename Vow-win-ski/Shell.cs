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
                string parameter1 = "";
                string parameters = "";
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
                                parameter1 += cmdline[i];
                            else
                                x++;
                            break;
                        case 2:
                            parameters = cmdline.Substring(parameter1.Length + cmd.Length + 2);
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
                        Disc.GetDisc.ShowDirectory(parameter1);
                        break;
                    case "MKDIR":
                    case "MD":
                        //Disc.GetDisc.CreateFolder(parameter1);
                        break;
                    case "RMDIR":
                    case "RD":
                        //Disc.GetDisc.DeleteFolder(parameter1);
                        break;
                    case "CD":
                        //Disc.GetDisc.ChangeDirectory(parameter1);
                        break;
                    case "CF":
                        Disc.GetDisc.CreateFile(parameter1, parameters);
                        break;
                    case "TYPE":
                        Console.WriteLine(Disc.GetDisc.GetFileData(parameter1) ?? "Error reading file");
                        break;
                    case "DF":
                        Disc.GetDisc.DeleteFile(parameter1);
                        break;
                    case "TREE":
                        //Disc.GetDisc.ShowTree(parameter1);
                        break;
                    case "APP":
                        Disc.GetDisc.AppendToFile(parameter1, parameters);
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
            Console.WriteLine("DIR [path]\t\tShows a list of files and subdirectories in [path]");
            Console.WriteLine("LS [path]\t\tSame as DIR");
            Console.WriteLine("MKDIR [path]{name}\tCreates new directory {name} in [path]");
            Console.WriteLine("MD [path]{name}\t\tSame as MKDIR");
            Console.WriteLine("RMDIR [path]{name}\tDeletes directory {name} in [path]");
            Console.WriteLine("RD [path]{name}\t\tSame as RMDIR");
            Console.WriteLine("CD [path]\t\tChanges to directory [path]");
            Console.WriteLine("CF [path]{name} [data]\tCreates file {name} in [path] and fills it with [data]");
            Console.WriteLine("APP [path]{name} [data]\tAppends [data] to file {name} in [path]");
            Console.WriteLine("TYPE [path]{name}\tShows data from file {name} in [path]");
            Console.WriteLine("DF [path]{name}\t\tDeletes file {name} in [path]");
            Console.WriteLine("TREE [path]\t\tShows the directory structure of [path]");
            Console.WriteLine("DDB \t\t\tShows raw data of all data blocks");
        }
    }
}

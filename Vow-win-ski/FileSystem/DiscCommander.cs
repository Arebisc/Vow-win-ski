using System;
using FileSystem.FileSystem;

namespace FileSystem
{
    class DiscCommander
    {

        public void OpenShell(Disc disc)
        {
            bool breakLoop = false;
            while (!breakLoop)
            {
                Console.WriteLine();
                Console.Write(disc.CurrentFolder.PathToFolder + ">");
                string path = "";
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
                                path += cmdline[i];
                            else
                                x++;
                            break;
                        case 2:
                            parameters = cmdline.Substring(path.Length + cmd.Length + 2);
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
                        disc.ShowDirectory(path);
                        break;
                    case "MKDIR":
                    case "MD":
                        disc.CreateFolder(path);
                        break;
                    case "RMDIR":
                    case "RD":
                        disc.DeleteFolder(path);
                        break;
                    case "CD":
                        disc.ChangeDirectory(path);
                        break;
                    case "CF":
                        disc.CreateFile(path, parameters);
                        break;
                    case "TYPE":
                        Console.WriteLine(disc.GetFileData(path) ?? "Error reading file");
                        break;
                    case "DF":
                        disc.DeleteFile(path);
                        break;
                    case "TREE":
                        disc.ShowTree(path);
                        break;
                    case "APP":
                        disc.AppendToFile(path, parameters);
                        break;
                    case "DDB":
                        disc.DisplayDataBlocks();
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

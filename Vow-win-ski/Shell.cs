using System;
using Vow_win_ski.FileSystem;

namespace Vow_win_ski
{
    public class Shell
    {
        private Shell()
        {}

        private static volatile Shell _instance;
        private static object _syncRoot = new object();
        public static Shell GetShell
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new Shell();
                        }
                    }
                }
                return _instance;
            }
        }

        public void OpenShell()
        {
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine();
                Console.Write("root\\>");
                string p1 = "";
                string p2 = "";
                string cmd = "";
                string cmdline = Console.ReadLine();

                var x = 0;
                // ReSharper disable once PossibleNullReferenceException
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
                    case "":
                        break;
                    case "HELP":
                        ShowHelp();
                        break;
                    case "EX":
                        throw new NotImplementedException();
                        break;
                    case "QUIT":
                        exit = true;
                        break;
                    //===================================================
                    case "SRP":
                        throw new NotImplementedException();
                        break;
                    case "SRG":
                        throw new NotImplementedException();
                        break;
                    //===================================================
                    case "CP":
                        Processes.UserInterface.CreateProcess(p1, p2);
                        break;
                    case "HP":
                        Processes.UserInterface.StopProcess(p1);
                        break;
                    case "SAP":
                        Processes.UserInterface.ShowAllProcesses();
                        break;
                    //case "SP":
                        //throw new NotImplementedException();
                        //break;
                    case "SPCB":
                        Processes.UserInterface.ShowPCB(p1);
                        break;
                    case "WP":
                        Processes.UserInterface.SleepProcess(p1);
                        break;
                    case "RP":
                        Processes.UserInterface.ResumeProcess(p1);
                        break;
                    //===================================================
                    case "SPL":
                        throw new NotImplementedException();
                        break;
                    case "SPC":
                        throw new NotImplementedException();
                        break;
                    case "SEP":
                        throw new NotImplementedException();
                        break;
                    case "SM":
                        throw new NotImplementedException();
                        break;
                    case "SFIFO":
                        throw new NotImplementedException();
                        break;
                    case "SLM":
                        throw new NotImplementedException();
                        break;
                    //===================================================
                    case "SAM":
                        throw new NotImplementedException();
                        break;
                    case "SMH":
                        throw new NotImplementedException();
                        break;
                    //===================================================
                    case "SW":
                        throw new NotImplementedException();
                        break;
                    //===================================================
                    case "DIR":
                    case "LS":
                        Disc.GetDisc.ShowDirectory();
                        break;
                    case "CF":
                        Disc.GetDisc.CreateFile(p1, p2);
                        break;
                    case "TYPE":
                        Console.WriteLine(Disc.GetDisc.GetFileData(p1) ?? "Błąd czytania pliku");
                        break;
                    case "DF":
                        Disc.GetDisc.DeleteFile(p1);
                        break;
                    case "APP":
                        Disc.GetDisc.AppendToFile(p1, p2);
                        break;
                    case "SDB":
                        Disc.GetDisc.ShowDataBlocks(p1);
                        break;
                    default:
                        Console.WriteLine("Nieznane polecenie\nWpisz \"help\" aby wyświetlić listę dostępnych poleceń");
                        break;
                }
            }
        }

        private void ShowHelp()
        {
            Console.WriteLine();
            Console.WriteLine("Parametry: [opcjonalny] {wymagany}");
            Console.WriteLine();
            Console.WriteLine("Polecenia\t\t   Opis");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("-------------------------------Ogólne---------------------------------");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("HELP\t\t   Wyświetla tę listę");
            Console.WriteLine("EX\t\t   Wykonuje kolejny rozkaz");
            Console.WriteLine("QUIT\t\t   Zamyka system");

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("------------------------------Procesor--------------------------------");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("SRP\t\t   Wyświetla listę procesów Ready");
            Console.WriteLine("SRG\t\t   Wyświetla rejestry procesora");

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("-------------------------------Procesy--------------------------------");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("CP {nazwa} {prog}  Tworzy proces {nazwa} z programu {prog}");
            Console.WriteLine("HP {nazwa}\t   Zatrzymuje proces {nazwa}");
            Console.WriteLine("SAP\t\t   Wyświetla listę wszystkich procesów");
            //Console.WriteLine("SP {nazwa}\t   Wyświetla proces {nazwa}");
            Console.WriteLine("SPCB {nazwa}\t   Wyświetla listę PCB procesu {nazwa}");
            Console.WriteLine("WP {nazwa}\t   Usypia uruchomiony proces {nazwa}");
            Console.WriteLine("RP {nazwa}\t   Wznawia uśpiony proces {nazwa}");

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("-------------------------------Pamięć---------------------------------");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("SPL {nazwa}\t   Wyświetla listę stron procesu {nazwa}");
            Console.WriteLine("SPC {nazwa} {nr}   Wyświetla zawartość strony {nr} procesu {nazwa}");
            Console.WriteLine("SEP\t\t   Wyświetla puste stron");
            Console.WriteLine("SM\t\t   Wyświetla całą pamięć");
            Console.WriteLine("SLM\t\t   Wyświetla ostatnią wiadomość z pamięci");
            Console.WriteLine("SFIFO\t\t   Wyświetla kolejke FIFO");

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("-----------------------------Komunikacja------------------------------");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("SAM\t\t   Wyświetla wszystkie oczekujące komunikaty");
            Console.WriteLine("SMH\t\t   Wyświetla historię komunikatów");

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("----------------------------Synchronizacja----------------------------");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("SW\t\t   Wyświetla procesy oczekujące pod zamkiem komunikatów");

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("--------------------------------Dysk----------------------------------");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("DIR/LS\t\t   Wyświetla listę plików");
            Console.WriteLine("CF {nazwa} [dane]  Tworzy plik {nazwa} i wypełnia [dane]");
            Console.WriteLine("APP {nazwa} [dane] Dołącza [dane] do pliku {nazwa}");
            Console.WriteLine("TYPE {nazwa}\t   Wyświetla dane z pliku {nazwa}");
            Console.WriteLine("DF {nazwa}\t   Usuwa plik {nazwa}");
            Console.WriteLine("SDB [nr]\t   Wyświetla dane wszystkich bloków, [nr] bloków na ekran");
        }
    }
}

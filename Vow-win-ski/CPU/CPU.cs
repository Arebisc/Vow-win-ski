using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileSystem.FileSystem;

namespace Vow_win_ski.CPU
{
    public sealed class CPU
    {
        private static volatile CPU _instance;
        private static readonly object SyncRoot = new object();


        public Register Register;

        private CPU()
        {
            this.Register = new Register();
        }

        public static CPU GetInstance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if(_instance == null)
                            _instance = new CPU();
                    }
                }

                return _instance;
            }
        }

        public void DisplayDebug()
        {
            int debugChosen = 0;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("---------------DEBUG---------------");
            Console.WriteLine("1. Wyświetl listę procesów Ready");
            Console.WriteLine("2. Wyświetl stan rejestrów");
            Console.WriteLine("3. Wyświetl listę wszystkich procesów");
            Console.WriteLine("4. Wyświetl proces: NazwaProcesu");
            Console.WriteLine("5. Wyświetl listę PCB procesu: NazwaProcesu");
            Console.WriteLine("6. Wyświetl listę stron: NazwaProcesu");
            Console.WriteLine("7. Wyświetl zawartość stron: NazwaProcesu");
            Console.WriteLine("8. Wyświetl puste strony");
            Console.WriteLine("9. Wyświetl całą pamięć");
            Console.WriteLine("10. Wyświetl wszystkie bloki na dysku");
            Console.WriteLine("11. Wyświetl zawartość folderu głównego");
            Console.WriteLine("12. Wyświetl zawartość listy wiadomości");
            Console.WriteLine("13. Wyświetl oczekujące pod zamkiem komunikatów procesy");
            Console.WriteLine("14. Przejdź do kolejnego rozkazu");
            Console.WriteLine("15. Wykonaj resztę programu");
            Console.WriteLine("---------------DEBUG---------------");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("Wpisz akcję: ");

            try
            {
                debugChosen = Convert.ToInt32(Console.ReadLine());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            ChooseAction(debugChosen);
            Console.WriteLine();
            Console.WriteLine();
        }

        private void ChooseAction(int action)
        {
            switch (action)
            {
                case 1:
                    Console.WriteLine("Nie zaimplementowano - Wyświetl listę procesów Ready!");
                    DisplayDebug();
                    break;
                case 2:
                    Console.WriteLine("Nie zaimplementowano - Wyświetl stan rejestrów procesora!");
                    DisplayDebug();
                    break;
                case 3:
                    Console.WriteLine("Nie zaimplementowano - Wyświetl listę wszystkich procesów!");
                    DisplayDebug();
                    break;
                case 4:
                    Console.WriteLine("Nie zaimplementowano - Wyświetl proces: NazwaProcesu!");
                    DisplayDebug();
                    break;
                case 5:
                    Console.WriteLine("Nie zaimplementowano - Wyświetl listę PCB procesu: NazwaProcesu!");
                    DisplayDebug();
                    break;
                case 6:
                    Console.WriteLine("Nie zaimplementowano - Wyświetl listę stron: NazwaProcesu!");
                    DisplayDebug();
                    break;
                case 7:
                    Console.WriteLine("Nie zaimplementowano - Wyświetl zawartość stron: NazwaProcesu!");
                    DisplayDebug();
                    break;
                case 8:
                    Console.WriteLine("Nie zaimplementowano - Wyświetl puste strony!");
                    DisplayDebug();
                    break;
                case 9:
                    Console.WriteLine("Nie zaimplementowano - Wyświetl całą pamięć!");
                    DisplayDebug();
                    break;
                case 10:
                    Console.WriteLine("Nie zaimplementowano - Wyświetl wszystkie bloki na dysku!");
                    DisplayDebug();
                    break;
                case 11:
                    Console.WriteLine("Nie zaimplementowano - Wyświetl zawartość folderu głównego!");
                    DisplayDebug();
                    break;
                case 12:
                    Console.WriteLine("Nie zaimplementowano - Wyświetl zawartość listy wiadomości!");
                    DisplayDebug();
                    break;
                case 13:
                    Console.WriteLine("Nie zaimplementowano - Wyświetl oczekujące pod zamkiem komunikatów procesy!");
                    DisplayDebug();
                    break;
                case 14:
                    Console.WriteLine("Nie zaimplementowano - Przejdź do kolejnego rozkazu!");
                    DisplayDebug();
                    break;
                case 15:
                    Console.WriteLine("Nie zaimplementowano - Wykonaj program do końca!");
                    DisplayDebug();
                    break;
                default:
                    Console.WriteLine("Błąd! Wpisz inną wartość!");
                    DisplayDebug();
                    break;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using Vow_win_ski.IPC;
using Vow_win_ski.MemoryModule;

namespace Vow_win_ski.Processes
{

    public enum ReasonOfProcessTerminating
    {
        Ended = 1,
        ThrownError = 2,
        UserClosed = 3,
        CriticalError = 4,
        KilledByOther = 5,
        ClosingSystem = 6
    }


    public partial class PCB
    {

        private static LinkedList<PCB> _CreatedPCBs = new LinkedList<PCB>();
        private static int _NextPID = 1;

        /// <summary>
        /// Pusty konstruktor do testow
        /// </summary>
        public PCB()
        {
            _PID = ++_NextPID;
        }

        /// <summary>
        /// RUSZ TEN KONSTRUKTOR JESZCZE RAZ TO ZAPIERDOLE!!!!
        /// </summary>
        public PCB(string name, int priority)
        {
            _PID = ++_NextPID;
            this.Name = name;
            this.CurrentPriority = priority;
        }

        /// <summary>
        /// Tworzy nowy proces bez uruchamiania go (stan procesu = New)
        /// </summary>
        /// <param name="Name_">Nazwa procesu, musi być unikalna</param>
        /// <param name="ProgramFilePath">Ścieżka do pliku z programem (z której zostanie wczytany kod programu)</param>
        /// <remarks>Utworzenie procesu - XC</remarks>
        public PCB(string Name_, int Priority, string ProgramFilePath)
        {

            //Wczytaj program
            string Program;

            try
            {
                Program = System.IO.File.ReadAllText(ProgramFilePath);
            }
            catch
            {
                Console.WriteLine("Nie udalo sie utworzyc procesu: nie znaleziono pliku o nazwie " + ProgramFilePath);
                State = ProcessState.Terminated;
                return;
            }

            //Nazwa procesu
            if (IsProcessNameUsed(Name_))
            {

                int i = 1;
                while (IsProcessNameUsed(Name_ + i.ToString())) i++;

                Name = Name_ + i.ToString();
                Console.WriteLine("Podana nazwa [" + Name_ + "] jest juz uzywana, proces otrzymal nazwe " + Name + ".");

            }
            else
            {
                Name = Name_;
                Console.WriteLine("Proces otrzymal nazwe " + Name + ".");
            }

            //Utwórz PCB
            _PID = ++_NextPID;

            if (Priority < 0 || Priority > 7)
            {
                Console.WriteLine("Priorytet musi miescic sie w zakresie 0 - 7. Proces otrzymal priorytet 7.");
                CurrentPriority = 7;
                StartPriority = 7;
            }
            else
            {
                CurrentPriority = Priority;
                StartPriority = Priority;
            }

            client = new PipeClient(Name);


            //Ilość wymaganej pamięci (pierwsza linia)
            //string CountOfMemory_str = Program.Split('\n')[0].Split(',')[1].Trim();
            //int CountOfMemory_int = Convert.ToInt32(CountOfMemory_str.Remove(CountOfMemory_str.Length - 1, 1)); //usun litere K
            //Console.WriteLine("Wczytano z pliku " + ProgramFilePath + " kod programu dla procesu " + this.ToString());

            Memory.GetInstance.AllocateMemory(this, Program);

            _CreatedPCBs.AddLast(this);
            Console.WriteLine("Utworzono proces: " + this.ToString());
        }

        /// <summary>Zwraca blok kontrolny procesu o podanym identyfikatorze</summary>
        /// <remarks>Znalezienie bloku PCB o danej nazwie - XN</remarks>
        public static PCB GetPCB(int PID)
        {
            LinkedList<PCB>.Enumerator en = _CreatedPCBs.GetEnumerator();

            while (en.MoveNext())
            {
                if (en.Current.PID == PID)
                {
                    Console.WriteLine("Znaleziono proces o podanym PID: " + en.Current.ToString());
                    return en.Current;
                }
            }

            Console.WriteLine("Nie znaleziono procesu o PID = " + PID.ToString() + ".");
            return null;
        }

        /// <summary>Zwraca blok kontrolny procesu o podanym identyfikatorze</summary>
        /// <remarks>Znalezienie bloku PCB o danej nazwie - XN</remarks>
        public static PCB GetPCB(string Name)
        {
            LinkedList<PCB>.Enumerator en = _CreatedPCBs.GetEnumerator();

            while (en.MoveNext())
            {
                if (en.Current.Name == Name)
                {
                    Console.WriteLine("Znaleziono proces o podanej nazwie: " + en.Current.ToString());
                    return en.Current;
                }
            }

            Console.WriteLine("Nie znaleziono procesu o nazwie " + Name + ".");
            return null;
        }

        public static void PrintAllPCBs()
        {
            LinkedList<PCB>.Enumerator en = _CreatedPCBs.GetEnumerator();

            Console.WriteLine("Procesy aktualnie obecne w systemie:");

            while (en.MoveNext())
            {
                Console.WriteLine(en.Current.ToString());
            }

            Console.WriteLine();

        }

        private bool IsProcessNameUsed(string Name)
        {
            LinkedList<PCB>.Enumerator en = _CreatedPCBs.GetEnumerator();

            while (en.MoveNext())
            {
                if (en.Current.Name == Name) return true;
            }

            return false;
        }

    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vow_win_ski.IPC;
using Vow_win_ski.MemoryModule;

namespace Vow_win_ski.Processes {

    public enum ReasonOfProcessTerminating{
        /// <summary>
        /// Skończył się normalnie
        /// </summary>
        Ended = 1,

        /// <summary>
        /// Skończył się w wyniku błędu (np. błędne dane dla programu)
        /// </summary>
        ThrownError = 2,

        /// <summary>
        /// Zamknięty przez użytkownika
        /// </summary>
        UserClosed = 3,

        /// <summary>
        /// Zaszalał tak, że system musi go zamknąć (np. podzielił przez zero czy próbował zapisać systemowy obszar pamięci)
        /// </summary>
        CriticalError = 4,

        /// <summary>
        /// Zakończenie procesu rodzica
        /// </summary>
        //KilledByParent = 5,

        /// <summary>
        /// Inny proces go zakończył
        /// </summary>
        KilledByOther = 6,

        /// <summary>
        /// System się zamyka
        /// </summary>
        ClosingSystem = 7
    }
    

    /// <summary>
    /// Umożliwia zarządzenie procesami na wyższym poziomie
    /// </summary>
    public partial class PCB{

        /// <summary>
        /// Procesy bez rodzica (utworzone przez użytkownika, nie przez inny proces)
        /// </summary>
        //public static LinkedList<PCB> ProcessesCreatedByUser = new LinkedList<PCB>();

        /// <summary>
        /// Wszystkie utworzone PCB
        /// </summary>
        private static LinkedList<PCB> _CreatedPCBs = new LinkedList<PCB>();
        private static int _NextPID = 1;

        /// <summary>
        /// Pusty konstruktor do testow
        /// </summary>
        public PCB() {
            _PID = ++_NextPID;
        }

        public PCB(string name, int priority) {
            this.Name = name;
            this.CurrentPriority = priority;
        }

        /// <summary>
        /// Tworzy nowy proces bez uruchamiania go (stan procesu = New)
        /// </summary>
        /// <param name="Name_">Nazwa procesu, musi być unikalna</param>
        /// <param name="ProgramFilePath">Ścieżka do pliku z programem (z której zostanie wczytany kod programu)</param>
        /// <remarks>Utworzenie procesu - XC</remarks>
        public PCB(string Name_, int Priority, string ProgramFilePath){
            throw new NotImplementedException();

            //Wczytaj program
            string Program;

            try {
                Program = System.IO.File.ReadAllText(ProgramFilePath);
            } catch (Exception ex) {
                Console.WriteLine("Nie udalo sie utworzyc procesu: nie znaleziono pliku o nazwie " + ProgramFilePath);
                State = ProcessState.Terminated;
                return;
            }

            //Nazwa procesu
            if (IsProcessNameUsed(Name_)) {

                int i = 1;
                while(IsProcessNameUsed(Name_ + i.ToString())) {
                    i++;
                }

                Name = Name_ + i.ToString();
                Console.WriteLine("Podana nazwa [" + Name_ + "] jest juz uzywana, proces otrzymal nazwe " + Name);

            } else {
                Name = Name_;
                Console.WriteLine("Proces otrzymal nazwe " + Name + ".");
            }

            //Utwórz PCB
            _PID = ++_NextPID;
            CurrentPriority = Priority;
            StartPriority = Priority;
            client = new PipeClient(Name);

            //if(Parent == null) {
            //    ProcessesCreatedByUser.AddLast(this);
            //} else {
            //    Parent.Children.AddLast(this);
            //}


            //Ilość wymaganej pamięci (pierwsza linia)
            string CountOfMemory_str = Program.Split('\n')[0].Split(',')[1].Trim();
            int CountOfMemory_int = Convert.ToInt32(CountOfMemory_str.Remove(CountOfMemory_str.Length - 1, 1)); //usun litere K

            Console.WriteLine("Wczytano z pliku " + ProgramFilePath + " kod programu dla procesu " + this.ToString());

            //Zaalokuj pamięć
            //MemoryBlocks = new MemoryModule.ProcessPages()
            

            Console.WriteLine("Utworzono proces: " + this.ToString());

            _CreatedPCBs.AddLast(this);

        }

        /// <summary>Zwraca blok kontrolny procesu o podanym identyfikatorze</summary>
        /// <remarks>Znalezienie bloku PCB o danej nazwie - XN</remarks>
        public static PCB GetPCB(int PID){
            LinkedList<PCB>.Enumerator en = _CreatedPCBs.GetEnumerator();

            while (en.MoveNext()){
                if (en.Current.PID == PID) {
                    Console.WriteLine("Znaleziono proces o podanym PID: " + en.Current.ToString());
                    return en.Current;
                }
            }

            Console.WriteLine("Nie znaleziono proces o PID=" + PID.ToString() + ".");
            return null;
        }

        public static PCB GetPCB(string Name) {
            LinkedList<PCB>.Enumerator en = _CreatedPCBs.GetEnumerator();

            while (en.MoveNext()) {
                if (en.Current.Name == Name) {
                    Console.WriteLine("Znaleziono proces o podanej nazwie: " + en.Current.ToString());
                    return en.Current;
                }
            }

            Console.WriteLine("Nie znaleziono procesu o nazwie " + Name + ".");
            return null;
        }

        private bool IsProcessNameUsed(string Name) {
            LinkedList<PCB>.Enumerator en = _CreatedPCBs.GetEnumerator();

            while (en.MoveNext()) {
                if (en.Current.Name == Name) return true;
            }

            return false;
        }

    }
}
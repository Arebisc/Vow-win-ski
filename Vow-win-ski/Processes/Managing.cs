using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vow_win_ski.IPC;

namespace Vow_win_ski.Processes
{

    public enum ReasonOfProcessTerminating
    {
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
        KilledByParent = 5,

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
    public partial class PCB
    {

        /// <summary>
        /// Procesy bez rodzica (utworzone przez użytkownika, nie przez inny proces)
        /// </summary>
        public static LinkedList<PCB> ProcessesCreatedByUser = new LinkedList<PCB>();

        /// <summary>
        /// Wszystkie utworzone PCB
        /// </summary>
        private static LinkedList<PCB> _CreatedPCBs = new LinkedList<PCB>();
        private static int _NextPID = 1;

        /// <summary>
        /// Tworzy nowy proces bez uruchamiania go (stan procesu = New)
        /// </summary>
        /// <param name="Name_">Nazwa procesu, nie musi być unikalna</param>
        /// <param name="ProgramFilePath">Ścieżka do pliku z programem (z której zostanie wczytany kod programu)</param>
        /// <param name="Parent">Proces rodzic; null jeśli proces tworzony przez system</param>
        /// <remarks>Utworzenie procesu - XC</remarks>
        public PCB(string Name_, int Priority, string ProgramFilePath, PCB Parent)
        {
            //throw new NotImplementedException();

            //Utwórz PCB
            _PID = ++_NextPID;
            client = new PipeClient(Name);
            CurrentPriority = Priority;
            StartPriority = Priority;
            Name = Name_;

            if(Parent == null)
            {
                ProcessesCreatedByUser.AddLast(this);
            }
            else 
                Parent.Children.AddLast(this);

            //Wczytaj program
            string Program = ""; //GetFileData(ProgramFilePath);
            //Ilość wymaganej pamięci (pierwsza linia)
            string CountOfMemory_str = Program.Split('\n')[0].Split(',')[1].Trim();
            int CountOfMemory_int = Convert.ToInt32(CountOfMemory_str.Remove(CountOfMemory_str.Length - 1, 1)); //usun litere K

            Console.WriteLine("Wczytano z pliku " + ProgramFilePath + " kod programu dla procesu " + this.ToString());

            //Zaalokuj pamięć


            Console.WriteLine("Utworzono proces: " + this.ToString());

            _CreatedPCBs.AddLast(this);

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

            Console.WriteLine("Nie znaleziono proces o PID=" + PID.ToString() + ".");
            return null;
        }

    }
}
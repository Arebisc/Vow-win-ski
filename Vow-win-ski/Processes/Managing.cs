using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Vow_win_ski.CPU;

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
    public static class Managing{

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
        /// <param name="Name">Nazwa procesu, nie musi być unikalna</param>
        /// <param name="ProgramFilePath">Ścieżka do pliku z programem (z której zostanie wczytany kod programu)</param>
        /// <remarks>Utworzenie procesu - XC</remarks>
        public static PCB CreateProcess(string Name, int Priority, string ProgramFilePath){
            throw new NotImplementedException();

            //Utwórz PCB
            PCB Process = new PCB(++_NextPID);
            Process.CurrentPriority = Priority;
            Process.StartPriority = Priority;
            Process.Name = Name;

            //Zapisz referencję na rodzica

            //Dodaj do tablicy dzieci

            //Wczytaj program
            string Program = ""; //GetFileData(ProgramFilePath);
            //Ilość wymaganej pamięci (pierwsza linia)
            string CountOfMemory_str = Program.Split('\n')[0].Split(',')[1].Trim();
            int CountOfMemory_int = Convert.ToInt32(CountOfMemory_str.Remove(CountOfMemory_str.Length - 1, 1)); //usun litere K

            Console.WriteLine("Wczytano z pliku " + ProgramFilePath + " kod programu dla procesu " + Process.ToString());

            //Zaalokuj pamięć


            Console.WriteLine("Utworzono proces: " + Process.ToString());

            _CreatedPCBs.AddLast(Process);
            return Process;
        }
        
        /// <summary>
        /// Zamyka aktualnie wykonywany lub wskazany przez identyfikator proces
        /// (przejście na Terminated, przygotowanie do całkowitego usunięcia procesu)
        /// i usuwa go z listy planisty
        /// Jeśli proces zatrzymywany nie jest w stanie Running, procez zatrzymujący blokuje się pod semaforem StopperSemaphore
        /// </summary>
        /// <param name="Reason">Przyczyna zamknięcia procesu</param>
        /// <param name="ExitCode">Opcjonalny; kod wyjścia zwracany przez proces</param>
        /// <param name="Process">Opcjonalny; jeśli podano, zamyka dany proces</param>
        /// <returns>
        /// Kod odpowiedzi:
        /// 0 - zamknięto proces
        /// </returns>
        /// <remarks>Zatrzymanie procesu i zawiadomienie nadzorcy - XH, nienormowalne zatrzymanie realizacji zlecenia - XQUE</remarks>
        public static int TerminateProcess(ReasonOfProcessTerminating Reason, int ExitCode = 0, PCB Process = null){
            throw new NotImplementedException();
        }

        /// <summary>
        /// Uruchamia proces po utworzeniu go metodą CreateProcess (przejście New -> Ready) i dodaje go do listy planisty
        /// </summary>
        /// <remarks>Uruchomienie procesu - XY</remarks>  
        /// <returns>
        /// Kod odpowiedzi:
        /// 0 - uruchomiono proces
        /// 1 - nie znaleziono procesu
        /// 2 - błąd: proces ma stan inny niż New
        /// </returns>
        public static int Run(PCB Process){

            if (Process == null) {
                Console.WriteLine("Blad uruchamiania procesu: Argument jest pusta referencja.");
                return 1;
            }

            if (Process.State != ProcessState.New) {
                Console.WriteLine("Blad uruchamiania procesu: Proces musi miec stan New. " + Process.ToString());
                return 2;
            }

            Process.State = ProcessState.Ready;
            Console.WriteLine("Uruchomiono proces " + Process.ToString());

            //AddProcessToScheduler(Process);
            return 0;
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

        /// <summary>
        /// Usuwa proces o podanym identyfikatorze oraz jego wszystkie dzieci. Proces musi najpierw zostać zatrzymany przez TerminateProcess
        /// (zwalnia pamięć, coś jak destruktor)
        /// </summary>
        /// <returns>
        /// Kod odpowiedzi:
        /// 0 - usunięto proces
        /// 1 - nie znaleziono procesu
        /// 2 - błąd: przed usunięciem procesu należy go zatrzymać
        /// </returns>
        /// <remarks>Usunięcie procesu - XD</remarks>
        public static int RemoveProcess(PCB Process){
            throw new NotImplementedException();

            if (Process == null){
                Console.WriteLine("Blad usuwania procesu: Argument jest pusta referencja.");
                return 1;
            }

            if (Process.State == ProcessState.Terminated) {
                //Usuń dzieci
                int ret;
                LinkedList<PCB>.Enumerator en = Process.Children.GetEnumerator();

                while (en.MoveNext()){
                    ret = RemoveProcess(en.Current);
                    if(ret != 0) return ret;
                }

                //Zwolnij pamiec



                _CreatedPCBs.Remove(Process);
                Console.WriteLine("Usunieto proces " + Process.ToString());
                return 0;

            } else {
                Console.WriteLine("Blad usuwania procesu: Proces nie zostal zatrzymany przed usunieciem. " + Process.ToString());
                return 2;
            }
            
        }


    }
}
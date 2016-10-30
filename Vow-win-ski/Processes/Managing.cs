using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vow_win_ski.Processes
{

    public enum ReasonOfProcessTerminating
    {
        /// <summary>
        /// Skończył się normalnie
        /// </summary>
        Ended = 1,

        /// <summary>
        /// Skończył się w wyniku błedu (np. błędne dane dla programu)
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
    public static class Managing
    {
        private static LinkedList<PCB> _CreatedPCBs = new LinkedList<PCB>();
        private static int _NextPID = 1;

        /// <summary>
        /// Tworzy nowy proces bez uruchamiania go (stan procesu = New)
        /// </summary>
        /// <param name="Name">Nazwa procesu, nie musi być unikalna</param>
        /// <param name="ProgramFilePath">Ścieżka do pliku z programem (z której zostanie wczytany kod programu)</param>
        /// <remarks>Utworzenie procesu - XC</remarks>
        public static PCB CreateProcess(string Name, string ProgramFilePath)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Zamyka aktualnie wykonywany lub wskazany przez identyfikator proces (przejście na Terminated, przygotowanie do całkowitego usunięcia procesu) i usuwa go z listy planisty
        /// </summary>
        /// <param name="Reason">Przyczyna zamknięcia procesu</param>
        /// <param name="ExitCode">Opcjonalny; kod wyjścia zwracany przez proces</param>
        /// <param name="PID">Opcjonalny; jeśli podano, zamyka proces o podanym identyfikatorze</param>
        /// <returns>
        /// Kod odpowiedzi:
        /// 0 - zamknięto proces
        /// </returns>
        /// <remarks>Zatrzymanie procesu i zawiadomienie nadzorcy - XH, nienormowalne zatrzymanie realizacji zlecenia - XQUE</remarks>
        public static int TerminateProcess(ReasonOfProcessTerminating Reason, int ExitCode = 0, int PID = 0)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Uruchamia proces po utworzeniu go metodą CreateProcess (przejście New -> Ready) i dodaje go do listy planisty
        /// </summary>
        /// <remarks>Uruchomienie procesu - XY</remarks>        
        public static void Run(PCB Process)
        {
            throw new NotImplementedException();
        }

        /// <summary>Zwraca blok kontrolny procesu o podanym identyfikatorze</summary>
        /// <remarks>Znalezienie bloku PCB o danej nazwie - XN</remarks>
        public static PCB GetPCB(int PID)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Wprowadza aktualnie wykonywany proces do sekcji SMC (System Must Complete), w której nie można zatrzymać procesu.<br />
        /// Każde wywołanie metody zwiększa poziom zagnieżdżenia o 1
        /// </summary>
        /// <returns>Poziom zagnieżdżenia w sekcji SMC</returns>
        /// <remarks>Wejście do sekcji SMC - XEXC</remarks>
        public static int EnterSMC() {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Wyprowadza obecnie uruchomiony proces z sekcji SMC.
        /// Każde uruchomienie metody zmniejsza poziom zagnieżdżenia o 1
        /// </summary>
        /// <returns>Poziom zagnieżdżenia w sekcji SMC</returns>
        /// <remarks>Wyjście z sekcji SMC - XCOM</remarks>
        public static int LeaveSMC() {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Usuwa proces o podanym identyfikatorze. Proces musi najpierw zostać zatrzymany przez TerminateProcess
        /// (zwalnia pamięć, zamyka pliki, coś jak destruktor)
        /// </summary>
        /// <returns>
        /// Kod odpowiedzi:
        /// 0 - usunięto proces
        /// 1 - nie znaleziono procesu
        /// 2 - błąd: przed usunięciem procesu należy go zatrzymać
        /// </returns>
        /// <remarks>Usunięcie procesu - XD</remarks>
        public static int RemoveProcess(PCB Process)
        {
            throw new NotImplementedException();
        }

        /// <remarks>Umieszczenie bloku PCB w łańcuchu - XI</remarks>
        private static void InsertPCBIntoList(PCB Process)
        {
            _CreatedPCBs.AddLast(Process);
        }

        /// <remarks>Usunięcie bloku PCB z łańcucha - XJ</remarks>
        private static void RemovePCBFromList(PCB Process)
        {
            _CreatedPCBs.Remove(Process);
        }

    }
}
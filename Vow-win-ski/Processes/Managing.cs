using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vow_win_ski.Processes
{

    /// <summary>
    /// Powód zamknięcia procesu
    /// </summary>
    public enum ReasonOfTerminating
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
        //ClosingSystem = 7
    }



    /// <summary>
    /// Umożliwia zarządzenie procesami na wyższym poziomie
    /// </summary>
    public static class Managing
    {

        /// <summary>
        /// Początek listy procesów
        /// </summary>
        //private static PCB _ListOfPCB = null;

        /// <summary>
        /// Lista wszystkich utworzonych procesów
        /// </summary>
        private static LinkedList<PCB> _CreatedPCBs = new LinkedList<PCB>();

        /// <summary>
        /// Identyfikator, który otrzyma następny utworzony proces
        /// </summary>
        private static int _NextPID = 1;

        /// <summary>
        /// Tworzy nowy proces bez uruchamiania go (stan procesu = New)
        /// </summary>
        /// <param name="Name">Nazwa procesu, nie musi być unikalna</param>
        /// <param name="ProgramFilePath">Ścieżka do pliku z programem (z której zostanie wczytany kod programu)</param>
        /// <returns>Identyfikator procesu</returns>
        /// <remarks>Utworzenie procesu - XC</remarks>
        public static int CreateProcess(string Name, string ProgramFilePath)
        {
            return 0;
        }

        /// <summary>
        /// Zamyka aktualnie wykonywany lub wskazany przez identyfikator proces (przejście na Terminated, przygotowanie do całkowitego usunięcia procesu)
        /// </summary>
        /// <param name="Reason">Przyczyna zamknięcia procesu</param>
        /// <param name="ExitCode">Opcjonalny; kod wyjścia zwracany przez proces</param>
        /// <param name="PID">Opcjonalny; jeśli podano, zamyka proces o podanym identyfikatorze</param>
        /// <returns>
        /// Kod odpowiedzi:
        /// 0 - zamknięto proces
        /// </returns>
        /// <remarks>Zatrzymanie procesu i zawiadomienie nadzorcy - XH, nienormowalne zatrzymanie realizacji zlecenia - XQUE</remarks>
        public static int TerminateProcess(ReasonOfTerminating Reason, int ExitCode = 0, int PID = 0)
        {
            return 0;
        }

        /// <summary>
        /// Uruchamia proces po utworzeniu go metodą CreateProcess (przejście New -> Ready)
        /// </summary>
        /// <param name="PID">Identyfikator procesu</param>
        /// <remarks>Uruchomienie procesu - XY</remarks>        
        public static void Run(int PID)
        {

        }

        /// <summary>
        /// Zawiesza aktualny proces, który będzie mogł być wykonywany dopiero po wywołaniu metody Resume
        /// </summary>
        /// <remarks>Zatrzymanie procesu - XZ</remarks>
        //public static void Suspend()
        //{

        //}

        /// <summary>
        /// Przywraca proces, wyprowadzając go ze stanu zawieszenia
        /// </summary>
        /// <param name="PID">Identyfikator procesu do wznowienia</param>
        /// <returns>
        /// Kod odpowiedzi:
        /// 0 - wznowiono proces
        /// </returns>
        /// <remarks>Uruchomienie procesu - XY</remarks>
        //public static int Resume(int PID)
        //{
        //   return 0;
        //}

        /// <summary>
        /// Zwraca blok kontrolny procesu
        /// </summary>
        /// <param name="PID">Identyfikator procesu</param>
        /// <returns>Blok kontrolny wybranego procesu lub null, jeśli proces nie został znaleziony</returns>
        /// <remarks>Znalezienie bloku PCB o danej nazwie - XN</remarks>
        public static PCB GetPCB(int PID)
        {
            return null;
        }

        /// <summary>
        /// Wprowadza aktualnie wykonywany proces do sekcji SMC (System Must Complete), w której nie można zatrzymać procesu.<br />
        /// Każde wywołanie metody zwiększa poziom zagnieżdżenia o 1
        /// </summary>
        /// <returns>Poziom zagnieżdżenia w sekcji SMC</returns>
        /// <remarks>Wejście do sekcji SMC - XEXC</remarks>
        public static int EnterSMC() {
            return 0;
        }

        /// <summary>
        /// Wyprowadza obecnie uruchomiony proces z sekcji SMC.
        /// Każde uruchomienie metody zmniejsza poziom zagnieżdżenia o 1
        /// </summary>
        /// <returns>Poziom zagnieżdżenia w sekcji SMC</returns>
        /// <remarks>Wyjście z sekcji SMC - XCOM</remarks>
        public static int LeaveSMC() {
            return 0;
        }

        /// <summary>
        /// Usuwa proces o podanym identyfikatorze. Proces musi najpierw zostać zatrzymany przez TerminateProcess
        /// (zwalnia pamięć, zamyka pliki, coś jak destruktor)
        /// </summary>
        /// <param name="PID">Identyfikator procesu</param>
        /// <returns>
        /// Kod odpowiedzi:
        /// 0 - usunięto proces
        /// 1 - nie znaleziono procesu
        /// 2 - błąd: przed usunięciem procesu należy go zatrzymać
        /// </returns>
        /// <remarks>Usunięcie procesu - XD</remarks>
        public static int RemoveProcess(int PID)
        {   
            return 0;
        }

        /// <summary>
        /// Umieszcza proces na liście procesów
        /// </summary>
        /// <param name="Process">Proces do dodania</param>
        /// <remarks>Umieszczenie bloku PCB w łańcuchu - XI</remarks>
        public static void InsertPCBIntoQueue(PCB Process)
        {
            
        }

        /// <summary>
        /// Usuwa proces z listy procesów
        /// </summary>
        /// <param name="Process">Identyfikator procesu do usunięcia</param>
        /// <remarks>Usunięcie bloku PCB z łańcucha - XJ</remarks>
        public static void RemovePCBFromQueue(PCB Process)
        {
            
        }

    }
}
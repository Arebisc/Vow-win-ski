using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vow_win_ski.CPU;

namespace Vow_win_ski.Processes
{

    public enum ProcessState
    {
        /// <summary>
        /// Nowy proces, niedodany do kolejki do wykonywania
        /// </summary>
        New,

        /// <summary>
        /// Proces gotowy, oczekujący w kolejce na uruchomienie
        /// </summary>
        Ready,

        /// <summary>
        /// Proces jest obecnie wykonywany
        /// </summary>
        Running,

        /// <summary>
        /// Proces jest wstrzymany i oczekuje na coś, do czasu wznowienia nie będzie wykonywany
        /// </summary>
        Waiting,

        /// <summary>
        /// Proces zakończony, usunięty z kolejki, oczekuje na usunięcie z pamięci
        /// </summary>
        Terminated
    }


    public class PCB
    {
        /// <remarks>0 - najwyższy priorytet, 7 - najniższy</remarks>
        public int CurrentPriority = 7;

        /// <summary>
        /// Początkowy priorytet procesu
        /// </summary>
        public int StartPriority = 7;

        /// <summary>
        /// Czas procesora, podczas którego proces miał dany priorytet. Jeśli wartość zwiększy się zbyt mocno, proces otrzymuje większy priorytet
        /// (np. przy każdym wyborze procesu planista wszystkim procesom zwiększa tę wartość; jeśli osiągnie ona np. 10, proces otrzymuje większy priorytet. Po wykonaniu procesu priorytet wraca do stanu początkowego)
        /// </summary>
        public int PriorityTime = 0;


        //Sibera: w IP najlepiej zapisz indeks obecnie przetwarzanego znaku z kodu programu, jak odczytasz intrukcję mającą np. 7 znaków to zwiększasz rejestr o 7 (a przynajmniej tak jest w prawdziwych procesorach), kod pobierasz z adresu BaseMemoryAddress + IP
        public Register Registers;

        /// <summary>
        /// Nazwa procesu, nie musi być unikalna
        /// </summary>
        public string Name = "";

        /// <summary>
        /// Identyfikator procesu, musi być unikalny
        /// </summary>
        public byte PID = 0;

        public ProcessState State = ProcessState.New;

        public int InstructionCounter = 0;

        public ArrayList ProcessChildren = new ArrayList();

        /// <summary>
        /// Poziom zagnieżdżenia w sekcji SMC (System Must Complete) - procesu przebywającego w tej sekcji nie można zatrzymać
        /// </summary>
        public int SMC = 0;

        /// <summary>
        /// Czy podczas przebywania w sekcji SMC wpłynęło żądanie zatrzymania procesu
        /// </summary>
        public bool WaitingForStopping = false;

        /// <summary>
        /// Pamięć zaalokowana przez proces dla danych
        /// Jak jest z pamięcią? Jeśli proces będzie mógł dynamicznie alokować pamięć lub zajmować więcej niż jeden blok pamięci, odkomentuję
        /// </summary>        
        //public ArrayList MemoryBlocks = new ArrayList();
                
        /// <summary>
        /// Adres w pamięci, gdzie rozpoczyna się kod wykonywanego programu
        /// </summary>
        public int BaseMemoryAddress = 0;

        /// <summary>
        /// Limit przydzielonej pamięci
        /// </summary>
        public int LimitOfMemory = 0;

        //Pliki
        public ArrayList FileHandles = new ArrayList();

        /// <summary>
        /// Semafor oczekiwania na komunikat od innego procesu
        /// Jest opisane w książce z Moodle gdzieś na początku, w opisie pól PCB
        /// </summary>
        public object ReceiverMessageSemaphore = null;
        public object StopperSemaphore = null;
        public object StoppeeSemaphore = null;

        public override string ToString()
        {
            throw new NotImplementedException();
        }
    }

}

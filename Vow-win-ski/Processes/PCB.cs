using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vow_win_ski.CPU;

namespace Vow_win_ski.Processes{

    public enum ProcessState    {
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


    public class PCB{
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

        public Register Registers = new Register();

        /// <summary>
        /// Nazwa procesu, nie musi być unikalna
        /// </summary>
        public string Name = "";

        /// <summary>
        /// Identyfikator procesu, musi być unikalny
        /// </summary>
        private int _PID;

        public byte PID{
            get{
                return (byte)_PID;
            }
        }

        public ProcessState State = ProcessState.New;

        public int InstructionCounter = 0;

        public PCB Parent = null;

        public LinkedList<PCB> Children = new LinkedList<PCB>();

        /// <summary>
        /// Pamięć zaalokowana przez proces dla kodu i danych
        /// </summary>        
        public object MemoryBlocks = null;

        /// <summary>
        /// Semafor oczekiwania na komunikat od innego procesu
        /// Jest opisane w książce z Moodle gdzieś na początku, w opisie pól PCB
        /// </summary>
        public object ReceiverMessageSemaphore = null;

        /// <summary>
        /// Semafor oczekiwania na zatrzymanie - jeśli zatrzymywany proces
        /// ma stan inny niż Running, proces zatrzymujący blokuje się
        /// pod tym semaforem i odblokowuje dopiero po zamknięciu procesu
        /// </summary>
        public object StopperSemaphore = null;

        public PCB(int PID){
            _PID = PID;
        }

        public override string ToString(){
            return "[" + PID.ToString() + "] " + Name + ", stan=" + State.ToString() + ", priorytet=" + CurrentPriority.ToString();
        }
    }

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vow_win_ski.Processes
{
    class Lockers
    {
        private byte open = 0;
        Queue<Vow_win_ski.Processes.PCB> waiting;
        private int id;
        PCB proces;



        public void Lock(PCB Proces)
        {
            if (Check())
            {
                proces = Proces;
                this.id = proces.PID;
                open = 1;
                proces.State = Vow_win_ski.Processes.ProcessState.Waiting;
            }
            else
            {
                waiting.Enqueue(Proces);
                proces.State = Vow_win_ski.Processes.ProcessState.Waiting;
            }
        }

        public void Unlock(PCB Proces)
        {
            if (!Check())
            {
                if (waiting.Count() > 1)
                {
                    if (Check(Proces))
                    {
                        proces.State = Vow_win_ski.Processes.ProcessState.Ready;
                        Proces.ReceiverMessageSemaphore = 1;
                        proces = waiting.Dequeue();
                        this.id = proces.PID;
                    }
                }
                else if (waiting.Count() == 0)
                {
                    proces.State = Vow_win_ski.Processes.ProcessState.Ready;
                    Proces.ReceiverMessageSemaphore = 1;
                    open = 0;
                }
            }
        }

        public void Lock(PCB Proces, int z)
        {
            if (Check())
            {
                proces = Proces;
                this.id = proces.PID;
                open = 1;
                Proces.MemoryBlocks = 1;
            }
            else
            {
                waiting.Enqueue(Proces);
                proces.State = Vow_win_ski.Processes.ProcessState.Waiting;
            }
        }

        public void Unlock(PCB Proces, int z)
        {
            if (!Check())
            {
                if (waiting.Count() > 1)
                {
                    if (Check(Proces))
                    {
                        Proces.MemoryBlocks = 0;
                        proces = waiting.Dequeue();
                        proces.State = Vow_win_ski.Processes.ProcessState.Ready;
                        proces.MemoryBlocks = 1;
                        this.id = proces.PID;
                    }
                }
                else if (waiting.Count() == 0)
                {
                    Proces.MemoryBlocks = 0;
                    open = 0;
                }
            }
        }

        public bool Check()
        {
            if (open == 0)
                return true;
            else
                return false;
        }

        public bool Check(PCB Proces)
        {
            if (this.id == Proces.PID)
                return true;
            else
                return false;
        }
    }
}

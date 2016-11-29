using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vow_win_ski.Processes
{
    class Lockers
    {
        byte open = 0;
        Queue<Vow_win_ski.Processes.PCB> waiting;
        int id;
        PCB proces;

        Lockers()
        {
        }

        public void Lock(PCB Proces)
        {
            if (open == 0)
            {
                proces = Proces;
                this.id = proces.PID;
                open = 1;
            }
            else
            {
                waiting.Enqueue(proces);
                proces.State = Vow_win_ski.Processes.ProcessState.Waiting;
            }
        }

        public void Unlock(PCB Proces)
        {
            if (this.id == Proces.PID)
            {
                proces = waiting.Dequeue();
                proces.State = Vow_win_ski.Processes.ProcessState.Ready;
            }
        }

        public void Check()
        {

        }
    }
}

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

        public void Lock()
        {
            if (open == 0)
            {
                this.id = proces.PID;
                open = 1;
            }
            else
            {
                waiting.Enqueue(proces);
                proces.State = Vow_win_ski.Processes.ProcessState.Waiting;
            }
        }

        public void Unlock()
        {
            if (waiting.Count == 0)
                open = 0;
            else
            {
                proces = waiting.Dequeue();
                proces.State = Vow_win_ski.Processes.ProcessState.Ready;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vow_win_ski.Processes;

namespace Vow_win_ski.CPU
{
    public sealed class Scheduler
    {
        private static volatile Scheduler _instance;
        private static readonly object SyncRoot = new Object();
        private static List<PCB> WaitingForProcessorAllocation;

        private Scheduler()
        {
            WaitingForProcessorAllocation = new List<PCB>();
        }

        public static Scheduler GetInstance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                            _instance = new Scheduler();
                    }
                }

                return _instance;
            }
        }
        
        public void AddProcess(PCB process)
        {
            WaitingForProcessorAllocation.Add(process);
        }
        //TODO
        public void RemoveProcess(PCB process)
        {
            throw new NotImplementedException();
        }
        //TODO
        public void SearchForProcessInList()
        {
            throw new NotImplementedException();
        }
    }
}

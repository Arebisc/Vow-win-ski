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
        private static object syncRoot = new Object();
        private static List<PCB> WaitingProcesses;

        private Scheduler()
        {
            WaitingProcesses = new List<PCB>();
        }

        public static Scheduler GetInstance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                            _instance = new Scheduler();
                    }
                }

                return _instance;
            }
        }

        public void AddProcessToList(PCB process)
        {
            WaitingProcesses.Add(process);
        }
    }
}

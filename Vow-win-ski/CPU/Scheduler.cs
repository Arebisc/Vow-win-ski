using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Vow_win_ski.Processes;

namespace Vow_win_ski.CPU
{
    public sealed class Scheduler
    {
        private static volatile Scheduler _instance;
        private static readonly object SyncRoot = new object();
        private static List<PCB> WaitingForProcessor;

        private Scheduler()
        {

            WaitingForProcessor = new List<PCB>();
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
            WaitingForProcessor.Add(process);
        }

        public void RemoveProcess(PCB process)
        {
            WaitingForProcessor.RemoveAll(element => element.PID == process.PID);
        }

        public PCB SearchForProcessInList(PCB process)
        {
            return WaitingForProcessor.Find(element => element.PID == process.PID);
        }

        public void PrintList()
        {
            Console.WriteLine(ToString());
        }

        public override string ToString()
        {
            string result = string.Empty;

            foreach (var elem in WaitingForProcessor)
            {
                result += (elem.PID + " " + elem.Name + "\n");
            }
            return result;
        }

        public PCB PriorityAlgorithm()
        {
            return WaitingForProcessor
                .Aggregate((elem1, elem2) => 
                    (elem1.CurrentPriority < elem2.CurrentPriority ? elem1 : elem2));
        }

        public PCB GetRunningPCB()
        {
            return WaitingForProcessor
                .SingleOrDefault(x => x.State == ProcessState.Running);
        }

        public void AgingWaitingProcesses()
        {
            if (WaitingForProcessor.Count != 0)
            {
                foreach (var pcb in WaitingForProcessor)
                {
                    if (pcb.CurrentPriority > 0 && pcb.State != ProcessState.Running)
                        pcb.CurrentPriority--;
                }
            }
        }

        public void RejuvenationCurrentProcess()
        {
            if (WaitingForProcessor.Count != 0)
            {
                var runningPcb = GetRunningPCB();
                if (runningPcb.CurrentPriority < runningPcb.StartPriority)
                    runningPcb.CurrentPriority++;
            }
        }

        public bool CheckIfOtherProcessShouldGetCPU()
        {
            if (GetRunningPCB() == PriorityAlgorithm())
                return false;
            return true;
        }

        public void RevriteRegistersFromCPU()
        {
            GetRunningPCB().Registers.A = CPU.GetInstance.Register.A;
            GetRunningPCB().Registers.B = CPU.GetInstance.Register.B;
            GetRunningPCB().Registers.C = CPU.GetInstance.Register.C;
            GetRunningPCB().Registers.D = CPU.GetInstance.Register.D;
        }

        public void RevriteRegistersToCPU()
        {
            CPU.GetInstance.Register.A = GetRunningPCB().Registers.A;
            CPU.GetInstance.Register.B = GetRunningPCB().Registers.B;
            CPU.GetInstance.Register.C = GetRunningPCB().Registers.C;
            CPU.GetInstance.Register.D = GetRunningPCB().Registers.D;
        }
    }
}

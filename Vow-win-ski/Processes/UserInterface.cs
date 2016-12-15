using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vow_win_ski.Processes
{

    static class UserInterface
    {

        public static void CreateProcess(string Name, string Path)
        {
            new PCB(Name, new Random().Next(0, 7), Path, SourceOfCode.WindowsDisc).RunNewProcess();
        }

        public static void CreateProcessFromDisc(string Name, string Path)
        {
            new PCB(Name, new Random().Next(0, 7), Path, SourceOfCode.SystemDisc).RunNewProcess();
        }

        public static void StopProcess(string Name)
        {
            PCB pcb = PCB.GetPCB(Name);
            if(pcb != null) pcb.TerminateProcess(ReasonOfProcessTerminating.UserClosed);
        }

        public static void ShowAllProcesses()
        {
            PCB.PrintAllPCBs();
        }

        public static void ShowPCB(string Name)
        {
            PCB pcb = PCB.GetPCB(Name);
            if (pcb != null) pcb.PrintAllFields();
        }

        public static void SleepProcess(string Name)
        {
            PCB pcb = PCB.GetPCB(Name);
            if (pcb != null) pcb.WaitForSomething();
        }

        public static void ResumeProcess(string Name)
        {
            PCB pcb = PCB.GetPCB(Name);
            if (pcb != null) pcb.StopWaiting();
        }

    }
}
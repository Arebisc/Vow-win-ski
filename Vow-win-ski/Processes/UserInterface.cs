using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vow_win_ski.Processes {

    static class UserInterface {

        public static void CreateProcess(string Name, string Path) {
            new PCB(Name, new Random().Next(0, 7), Path).RunNewProcess();
        }

        public static void StopProcess(string Name) {
            PCB.GetPCB(Name).TerminateProcess(ReasonOfProcessTerminating.UserClosed);
        }

        public static void ShowAllProcesses() {
            PCB.PrintAllPCBs();
        }

        public static void ShowPCB(string Name) {
            PCB.GetPCB(Name).PrintAllFields();
        }

        public static void SleepProcess(string Name) {
            PCB.GetPCB(Name).WaitForSomething();
        }

        public static void ResumeProcess(string Name) {
            PCB.GetPCB(Name).StopWaiting();
        }

    }
}
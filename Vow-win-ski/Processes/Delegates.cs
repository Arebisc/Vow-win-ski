using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vow_win_ski.Processes{
    public delegate PCB CreateProcess(string Name_, int Priority, string ProgramFilePath, PCB Parent);
    //public delegate int TerminateProcess(ReasonOfProcessTerminating Reason, int ExitCode = 0, PCB Process = null);
    //public delegate int Run(PCB Process);
    public delegate PCB GetPCB(int PID);
    //public delegate int RemoveProcess(PCB Process);
    //public delegate void ShowMenu();   //wyświetla menu dla użytkownika, taki menedżer zadań
}
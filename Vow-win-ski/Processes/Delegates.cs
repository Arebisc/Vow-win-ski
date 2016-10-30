using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vow_win_ski.Processes
{
    delegate PCB CreateProcess(string Name, string ProgramFilePath);
    delegate int TerminateProcess(ReasonOfProcessTerminating Reason, int ExitCode = 0, int PID = 0);
    delegate void Run(PCB Process);
    delegate PCB GetPCB(int PID);
    delegate int EnterSMC();
    delegate int LeaveSMC();
    delegate int RemoveProcess(PCB Process);
}
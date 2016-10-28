using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vow_win_ski.Processes;

namespace Vow_win_ski.CPU
{
    public delegate void RemoveProcessFromScheduler(PCB process);

    public delegate void AddProcessToScheduler(PCB process);
}

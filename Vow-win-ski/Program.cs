using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vow_win_ski.CPU;
using Vow_win_ski.Processes;

namespace Vow_win_ski
{
    class Program
    {
        static void Main(string[] args)
        {
            Scheduler.GetInstance.AddProcess(new PCB() { PID = 1 });
            Scheduler.GetInstance.AddProcess(new PCB() { PID = 2 });
            Scheduler.GetInstance.AddProcess(new PCB() { PID = 3 });
            Scheduler.GetInstance.AddProcess(new PCB() { PID = 4 });



            Console.ReadKey();
        }
    }
}

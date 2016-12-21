using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Vow_win_ski.CPU;
using Vow_win_ski.Processes;

namespace Vow_win_ski.Tests.CPU
{
    [TestFixture]
    public class SchedulerTests
    {
        [Test]
        public void Check_PriorityAlgoritm()
        {
            var scheduler = Scheduler.GetInstance;
            var pcb1 = new PCB("first_pa", 4);
            var pcb2 = new PCB("second_pa", 3);
            var pcb3 = new PCB("third_pa", 7);
            var pcb4 = new PCB("last_pa", 1);
            var pcb5 = new PCB("latest_pa", 5);

            scheduler.AddProcess(pcb1);
            scheduler.AddProcess(pcb2);
            scheduler.AddProcess(pcb3);
            scheduler.AddProcess(pcb4);
            scheduler.AddProcess(pcb5);

            Assert.AreEqual(pcb4, scheduler.PriorityAlgorithm());

            scheduler.RemoveProcess(pcb1);
            scheduler.RemoveProcess(pcb2);
            scheduler.RemoveProcess(pcb3);
            scheduler.RemoveProcess(pcb4);
            scheduler.RemoveProcess(pcb5);
        }

        [Test]
        public void Check_GetRunningPCB()
        {
            var scheduler = Scheduler.GetInstance;
            var pcb1 = new PCB("first_grpcb", 4);
            var pcb2 = new PCB("second_grpcb", 3);
            var pcb3 = new PCB("third_grpcb", 7);
            var pcb4 = new PCB("last_grpcb", 1)
            {
                State = ProcessState.Running
            };
            var pcb5 = new PCB("latest_grpcb", 5);

            scheduler.AddProcess(pcb1);
            scheduler.AddProcess(pcb2);
            scheduler.AddProcess(pcb3);
            scheduler.AddProcess(pcb4);
            scheduler.AddProcess(pcb5);

            Assert.AreEqual(scheduler.GetRunningPCB(), pcb4);

            scheduler.RemoveProcess(pcb1);
            scheduler.RemoveProcess(pcb2);
            scheduler.RemoveProcess(pcb3);
            scheduler.RemoveProcess(pcb4);
            scheduler.RemoveProcess(pcb5);
        }
    }
}

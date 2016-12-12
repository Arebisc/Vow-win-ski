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
            var pcb1 = new PCB("first", 4);
            var pcb2 = new PCB("second", 3);
            var pcb3 = new PCB("third", 7);
            var pcb4 = new PCB("last", 1);
            var pcb5 = new PCB("latest", 5);

            scheduler.AddProcess(pcb1);
            scheduler.AddProcess(pcb2);
            scheduler.AddProcess(pcb3);
            scheduler.AddProcess(pcb4);
            scheduler.AddProcess(pcb5);
            var prior = scheduler.PriorityAlgorithm();
            scheduler.AddProcess(pcb5);

            Assert.AreEqual(pcb4, scheduler.PriorityAlgorithm());
        }
    }
}

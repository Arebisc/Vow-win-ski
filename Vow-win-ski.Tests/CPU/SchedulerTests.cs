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

            scheduler.AddProcess(new PCB("first", 4));
            scheduler.AddProcess(new PCB("second", 3));
            scheduler.AddProcess(new PCB("third", 7));
            scheduler.AddProcess(new PCB("last", 1));
            scheduler.AddProcess(new PCB("latest", 5));

            Assert.AreEqual(new PCB("last", 1), scheduler.PriorityAlgorithm());
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Vow_win_ski.Processes;

namespace Vow_win_ski.Tests.Processes
{
    [TestFixture]
    public class PCBTests
    {
        [Test]
        [TestCase("first", 1, "first", 2)]
        [TestCase("second", 2, "second", 2)]
        [TestCase("second", 3, "second", 2)]
        public void PCB_CheckEquatable(string firstName, int firstPriority, string secondName, int secondPriority)
        {
            var firstPcb = new PCB(firstName, firstPriority);
            var secondPcb = new PCB(secondName, secondPriority);
            Assert.IsTrue(firstPcb.Equals(secondPcb));
        }

    }
}

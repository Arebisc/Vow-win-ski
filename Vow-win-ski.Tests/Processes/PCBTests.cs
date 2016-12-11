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
        public void PCB_CheckEquatable()
        {
            var firstPcb = new PCB("first", 4);
            var secondPcb = new PCB("first", 5);
            Assert.IsTrue(firstPcb.Equals(secondPcb));
        }

    }
}

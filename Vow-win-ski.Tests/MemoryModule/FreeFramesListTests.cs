using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Vow_win_ski.MemoryModule;

namespace Vow_win_ski.Tests.MemoryModule
{
    [TestFixture]
    class FreeFramesListTests
    {
        [Test]
        public void Can_Remove_From_List()
        {
            //prepare
            var freeFramesList = new FreeFramesList(30);

            //action
            freeFramesList.RemoveFromList();
            freeFramesList.RemoveFromList();
            freeFramesList.RemoveFromList();

            //assertion
            Assert.AreEqual(freeFramesList.FreeFramesCount,27);
        }

        [Test]
        public void Can_Add_To_List()
        {
            //prepare
            var freeFramesList = new FreeFramesList(20);

            //action
            freeFramesList.AddToList(3);
            freeFramesList.AddToList(6);
            freeFramesList.AddToList(2);

            //assertion
            Assert.AreEqual(freeFramesList.FreeFramesCount,23);
        }
    }
}

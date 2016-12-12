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
    class QueueTests
    {
        [Test]
        public void Can_Add_And_Remove_From_Queue()
        {
            //prepare
            var queue = new FifoQueue();
            var test1 = new FrameData() {FrameNumber = 1, Id = 1};
            var test2 = new FrameData() {FrameNumber = 2, Id = 2};
            var test3 = new FrameData() {FrameNumber = 3, Id = 3};
            var test4 = new FrameData() {FrameNumber = 4, Id = 4};
            queue.AddFrame(test1);
            queue.AddFrame(test2);
            queue.AddFrame(test3);
            queue.AddFrame(test4);

            //action
            var data1 = queue.RemoveFrame();
            var data2 = queue.RemoveFrame();
            var data3 = queue.RemoveFrame();
            var data4 = queue.RemoveFrame();
            queue.AddFrame(test4);
            var data5 = queue.RemoveFrame();

            //assertion
            Assert.AreEqual(data1.Id,test1.Id);
            Assert.AreEqual(data2.Id,test2.Id);
            Assert.AreEqual(data3.Id,test3.Id);
            Assert.AreEqual(data4.Id,test4.Id);
            Assert.AreEqual(data5.Id,test4.Id);
        }

        [Test]
        public void Can_Remove_Choosen_Project()
        {
            //prepare
            var queue = new FifoQueue();
            var test1 = new FrameData() { FrameNumber = 1, Id = 1 };
            var test2 = new FrameData() { FrameNumber = 2, Id = 1 };
            var test3 = new FrameData() { FrameNumber = 3, Id = 3 };
            var test4 = new FrameData() { FrameNumber = 4, Id = 4 };
            queue.AddFrame(test1);
            queue.AddFrame(test2);
            queue.AddFrame(test3);
            queue.AddFrame(test4);

            //action
            queue.RemoveChoosenProcess(1);

            //assertion
            Assert.AreEqual(queue.Size,2);


        }
    }
}

using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Vow_win_ski.Memory;

namespace Vow_win_ski.Test
{
    [TestClass]
    public class FrameTest
    {
        [TestMethod]
        public void CanWriteandReadData()
        {
            //przygotowanie
            var frame1 = new Frame(1);
            char[] data = {'2', '3', '6', '7'};

            //dzialnie
            frame1.WriteFrame(data);

            //Assercje
            Assert.AreEqual(frame1.ReadFrame().Length,4);
            Assert.AreEqual(frame1.ReadFrame()[2],'6');
        }

        [TestMethod]
        public void CanClearData()
        {
            var frame1 = new Frame(4);
            char[] data = {'1', '2','a', '4'};

            frame1.WriteFrame(data);
            frame1.ClearFrame();

            Assert.AreEqual(frame1.ReadFrame()[10],'0');
        }
    }
}

using System;
using NUnit.Framework;
using Vow_win_ski.CPU;
using Vow_win_ski.Processes;
using Vow_win_ski.MemoryModule;

namespace Vow_win_ski.Tests.CPU
{
    [TestFixture]
    class InterpreterTests
    {
        [Test]
        [TestCase("HLT", "HLT")]
        [TestCase("XC ProcessName,FileName", "XC")]
        [TestCase("XD ProcessName", "XD")]
        [TestCase("XR", "XR")]
        [TestCase("XS ProcessName,Communicate", "XS")]
        [TestCase("XY Name", "XY")]
        [TestCase("XZ Name", "XZ")]
        [TestCase("AD A,B", "AD")]
        [TestCase("SB B,C", "SB")]
        [TestCase("MU C,D", "MU")]
        [TestCase("JM Etykieta", "JM")]
        [TestCase("Etykieta:", "Etykieta:")]
        [TestCase("MV A,B", "MV")]
        [TestCase("MN A,4", "MN")]
        [TestCase("MF FileName", "MF")]
        [TestCase("WF FileName,Content", "WF")]
        [TestCase("WR FileName,A", "WR")]
        [TestCase("DF FileName", "DF")]
        [TestCase("PO A", "PO")]
        public void Get_GetOrderName_Splitted(string order, string result)
        {
            var interpreter = Interpreter.GetInstance;
            string orderName = interpreter.GetOrderName(order);
            Assert.AreEqual(orderName, result);
        }

        [Test]
        [TestCase("HLT", "")]
        [TestCase("XC ProcessName,FileName", "ProcessName")]
        [TestCase("XD ProcessName", "ProcessName")]
        [TestCase("XR", "")]
        [TestCase("XS ProcessName,Communicate", "ProcessName")]
        [TestCase("XY Name", "Name")]
        [TestCase("XZ Name", "Name")]
        [TestCase("AD A,B", "A")]
        [TestCase("SB B,C", "B")]
        [TestCase("MU C,D", "C")]
        [TestCase("JM Etykieta", "Etykieta")]
        [TestCase("Etykieta:", "")]
        [TestCase("MV A,B", "A")]
        [TestCase("MN A,4", "A")]
        [TestCase("MF FileName", "FileName")]
        [TestCase("WF FileName,Content", "FileName")]
        [TestCase("WR FileName,A", "FileName")]
        [TestCase("DF FileName", "FileName")]
        [TestCase("PO A", "A")]
        public void Get_GetFirstOrderArgument_Splitted(string order, string result)
        {
            var interpreter = Interpreter.GetInstance;
            string orderFirstArgument = interpreter.GetOrderFirstArgument(order);
            Assert.AreEqual(orderFirstArgument, result);
        }

        [Test]
        [TestCase("HLT", "")]
        [TestCase("XC ProcessName,FileName", "FileName")]
        [TestCase("XD ProcessName", "")]
        [TestCase("XR", "")]
        [TestCase("XS ProcessName,Communicate", "Communicate")]
        [TestCase("XY Name", "")]
        [TestCase("XZ Name", "")]
        [TestCase("AD A,B", "B")]
        [TestCase("SB B,C", "C")]
        [TestCase("MU C,D", "D")]
        [TestCase("JM Etykieta", "")]
        [TestCase("Etykieta:", "")]
        [TestCase("MV A,B", "B")]
        [TestCase("MN A,4", "4")]
        [TestCase("MF FileName", "")]
        [TestCase("WF FileName,Content", "Content")]
        [TestCase("WR FileName,A", "A")]
        [TestCase("DF FileName", "")]
        [TestCase("PO A", "")]
        public void Get_GetOrderSecondArgument_Splitted(string order, string result)
        {
            var interpreter = Interpreter.GetInstance;
            string orderSecondArgument = interpreter.GetOrderSecondArgument(order);
            Assert.AreEqual(orderSecondArgument, result);
        }

        [Test]
        public void Get_GetOrderFromMemory()
        {
            string memoryContent = "AD A,4\n" +
                                   "MV A,B\n" +
                                   "MF NowyPlik\n" +
                                   "WR NowyPlik,A\n";
            string resultOrder = String.Empty;

            var pcb = new PCB("testowy", 5);
            Memory.GetInstance.AllocateMemory(pcb, memoryContent);

            resultOrder = Interpreter.GetInstance.GetOrderFromMemory(pcb);
            Assert.IsTrue(resultOrder == "AD A,4");

            resultOrder = Interpreter.GetInstance.GetOrderFromMemory(pcb);
            Assert.IsTrue(resultOrder == "MV A,B");

            resultOrder = Interpreter.GetInstance.GetOrderFromMemory(pcb);
            Assert.IsTrue(resultOrder == "MF NowyPlik");

            resultOrder = Interpreter.GetInstance.GetOrderFromMemory(pcb);
            Assert.IsTrue(resultOrder == "WR NowyPlik,A");

            resultOrder = Interpreter.GetInstance.GetOrderFromMemory(pcb);
            Assert.IsTrue(resultOrder == "");
        }
    }
}

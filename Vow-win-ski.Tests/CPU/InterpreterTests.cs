using System;
using System.Runtime.InteropServices;
using NUnit.Framework;
using Vow_win_ski.CPU;
using Vow_win_ski.Processes;
using Vow_win_ski.MemoryModule;
using CentralProcessingUnit = Vow_win_ski.CPU.CPU;

namespace Vow_win_ski.Tests.CPU
{
    [TestFixture]
    class InterpreterTests
    {
        private Register testingRegister = new Register(1, 2, 3, 4);

        private void SetTestingRegister()
        {
            testingRegister = new Register(1, 2, 3, 4);
            CentralProcessingUnit.GetInstance.Register = testingRegister;
        }

        private void ResetCPU()
        {
            CentralProcessingUnit.GetInstance.Register = new Register();
        }

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
                                   "WR NowyPlik,A\n" +
                                   "Etykieta:\n" +
                                   "HLT";
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
            Assert.IsTrue(resultOrder == "Etykieta:");

            resultOrder = Interpreter.GetInstance.GetOrderFromMemory(pcb);
            Assert.IsTrue(resultOrder == "HLT");

            resultOrder = Interpreter.GetInstance.GetOrderFromMemory(pcb);
            Assert.IsTrue(resultOrder == "");
        }

        [Test]
        public void Check_JumpInstruction()
        {
            int ordersCount = 6;
            string memoryContent = "AD C,1\n" +
                                   "AD A,4\n" +
                                   "Etykieta:\n" +
                                   "MU A,2\n" +
                                   "JM Etykieta\n" +
                                   "HLT\n";

            var pcb = new PCB("testowy", 5)
            {
                State = ProcessState.Running
            };
            Scheduler.GetInstance.AddProcess(pcb);
            Memory.GetInstance.AllocateMemory(pcb, memoryContent);

            for (int i = 0; i < ordersCount; i++)
            {
                Interpreter.GetInstance.InterpretOrder();
            }

            Assert.AreEqual(CentralProcessingUnit.GetInstance.Register.A, 8);
            Scheduler.GetInstance.RemoveProcess(pcb);
        }

        [Test]
        [TestCase("10", true)]
        [TestCase("10a", false)]
        [TestCase("a", false)]
        [TestCase("1", true)]
        public void Check_IsNumeric(string text, bool result)
        {
            Assert.AreEqual(Interpreter.GetInstance.IsNumeric(text), result);
        }

        [Test]
        [TestCase("A", "B")]
        [TestCase("A", "C")]
        [TestCase("A", "D")]
        [TestCase("B", "A")]
        [TestCase("B", "C")]
        [TestCase("B", "D")]
        [TestCase("C", "A")]
        [TestCase("C", "B")]
        [TestCase("C", "D")]
        [TestCase("D", "A")]
        [TestCase("D", "B")]
        [TestCase("D", "C")]
        public void Check_MVOrder(string register1, string register2)
        {
            SetTestingRegister();
            Interpreter.GetInstance.MVOrder(register1, register2);

            Assert.AreEqual(
                CentralProcessingUnit.GetInstance.Register.GetRegisterValueByName(register1),
                CentralProcessingUnit.GetInstance.Register.GetRegisterValueByName(register2)
                );
            ResetCPU();
        }

        [Test]
        [TestCase("A", 5)]
        [TestCase("B", 5)]
        [TestCase("C", 5)]
        [TestCase("D", 5)]
        public void Check_MNOrder(string register, int number)
        {
            SetTestingRegister();
            Interpreter.GetInstance.MNOrder(register, number);

            Assert.AreEqual(
                CentralProcessingUnit.GetInstance.Register.GetRegisterValueByName(register), number
                );
            ResetCPU();
        }

        [Test]
        [TestCase("A", "B")]
        [TestCase("A", "C")]
        [TestCase("A", "D")]
        [TestCase("B", "A")]
        [TestCase("B", "C")]
        [TestCase("B", "D")]
        [TestCase("C", "A")]
        [TestCase("C", "B")]
        [TestCase("C", "D")]
        [TestCase("D", "A")]
        [TestCase("D", "B")]
        [TestCase("D", "C")]
        public void CheckAddingRegisters(string register1, string register2)
        {
            SetTestingRegister();
            var reg1Value = CentralProcessingUnit.GetInstance.Register.GetRegisterValueByName(register1);
            Interpreter.GetInstance.ADOrder(register1, register2);
            
            Assert.AreEqual(CentralProcessingUnit.GetInstance.Register.GetRegisterValueByName(register1),
                CentralProcessingUnit.GetInstance.Register.GetRegisterValueByName(register2) + reg1Value);
            ResetCPU();
        }

        [Test]
        [TestCase("A", "B")]
        [TestCase("A", "C")]
        [TestCase("A", "D")]
        [TestCase("B", "A")]
        [TestCase("B", "C")]
        [TestCase("B", "D")]
        [TestCase("C", "A")]
        [TestCase("C", "B")]
        [TestCase("C", "D")]
        [TestCase("D", "A")]
        [TestCase("D", "B")]
        [TestCase("D", "C")]
        public void CheckSubsitutingRegisters(string register1, string register2)
        {
            SetTestingRegister();
            var reg1Value = CentralProcessingUnit.GetInstance.Register.GetRegisterValueByName(register1);
            Interpreter.GetInstance.SBOrder(register1, register2);

            Assert.AreEqual(CentralProcessingUnit.GetInstance.Register.GetRegisterValueByName(register1),
                CentralProcessingUnit.GetInstance.Register.GetRegisterValueByName(register2) - reg1Value);
            ResetCPU();
        }

        [Test]
        [TestCase("A", "B")]
        [TestCase("A", "C")]
        [TestCase("A", "D")]
        [TestCase("B", "A")]
        [TestCase("B", "C")]
        [TestCase("B", "D")]
        [TestCase("C", "A")]
        [TestCase("C", "B")]
        [TestCase("C", "D")]
        [TestCase("D", "A")]
        [TestCase("D", "B")]
        [TestCase("D", "C")]
        public void CheckMuliplyingRegisters(string register1, string register2)
        {
            SetTestingRegister();
            var reg1Value = CentralProcessingUnit.GetInstance.Register.GetRegisterValueByName(register1);
            Interpreter.GetInstance.MUOrder(register1, register2);

            Assert.AreEqual(CentralProcessingUnit.GetInstance.Register.GetRegisterValueByName(register1),
                CentralProcessingUnit.GetInstance.Register.GetRegisterValueByName(register2) * reg1Value);
            ResetCPU();
        }

        [Test]
        [TestCase("A", "4", 5)]
        [TestCase("B", "5", 7)]
        [TestCase("C", "6", 9)]
        [TestCase("D", "7", 11)]
        public void Check_AddingNumberToRegister(string register, string number, int possibleResult)
        {
            SetTestingRegister();
            Interpreter.GetInstance.ADOrder(register, number);
            Assert.AreEqual(CentralProcessingUnit.GetInstance.Register.GetRegisterValueByName(register), possibleResult);
            ResetCPU();
        }

        [Test]
        [TestCase("A", "2", -1)]
        [TestCase("B", "4", -2)]
        [TestCase("C", "1", 2)]
        [TestCase("D", "3", 1)]
        public void Check_SubstitutingNumberFromRegister(string register, string number, int possibleResult)
        {
            SetTestingRegister();
            Interpreter.GetInstance.SBOrder(register, number);
            Assert.AreEqual(CentralProcessingUnit.GetInstance.Register.GetRegisterValueByName(register), possibleResult);
            ResetCPU();
        }

        [Test]
        [TestCase("A", "2", 2)]
        [TestCase("B", "4", 8)]
        [TestCase("C", "1", 3)]
        [TestCase("D", "3", 12)]
        public void Check_MultiplyingNumberWithRegister(string register, string number, int possibleResult)
        {
            SetTestingRegister();
            Interpreter.GetInstance.MUOrder(register, number);
            Assert.AreEqual(CentralProcessingUnit.GetInstance.Register.GetRegisterValueByName(register), possibleResult);
            ResetCPU();
        }
    }
}

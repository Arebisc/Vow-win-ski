using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Vow_win_ski.MemoryModule;
using Vow_win_ski.Processes;

namespace Vow_win_ski.Tests.MemoryModule
{
    [TestFixture]
    class MemoryTests
    {
        [Test]
        [TestCase("abcdefghijklmnoprstuvwxyz",0,'a')]
        [TestCase("abcdefghijklmnoprstuvwxyz",1,'b')]
        [TestCase("abcdefghijklmnoprstuvwxyz",10,'k')]
        [TestCase("abcdefghijklmnoprstuvwxyz",3,'d')]
        [TestCase("abcdefghijklmnoprstuvwxyz",24,'z')]
        public void Can_Add_Data_To_Empty_Memory(string input,int index,char output)
        {
            //prepare
            var pcb = new PCB();

            //action
            Memory.GetInstance.AllocateMemory(pcb,input);
            var data = pcb.MemoryBlocks.ReadByte(index);

            //assetrion
            Assert.AreEqual(data,output);
        }

        [Test]
        [TestCase("abcdefghijklmnoprstuvwxyzabcdefghijklmnoprst",0,'a')]
        [TestCase("abcdefghijklmnoprstuvwxyzabcdefghijklmnoprst",18,'t')]
        [TestCase("abcdefghijklmnoprstuvwxyzabcdefghijklmnoprst",24,'z')]
        [TestCase("abcdefghijklmnoprstuvwxyzabcdefghijklmnoprst",3,'d')]
        [TestCase("abcdefghijklmnoprstuvwxyzabcdefghijklmnoprst",30,'f')]
        public void Can_Add_Data_To_Full_Memory(string input, int index, char output)
        {
            //prepare
            var pcbfiller = new PCB();
            Memory.GetInstance.TestFillMemory(pcbfiller);
            var pcb = new PCB();

            //action
            Memory.GetInstance.AllocateMemory(pcb,input);
            var data = pcb.MemoryBlocks.ReadByte(index);

            //assertion
            Assert.AreEqual(data,output);
        }

        [Test]
        [TestCase(15,'p')]
        [TestCase(16,'r')]
        [TestCase(80,'f')]
        [TestCase(160,'k')]
        public void Can_Read_Data_From_Memory(int index,char output)
        {
            //prepare
            var pcb = new PCB();
            Memory.GetInstance.TestFillMemory(pcb);

            //action
            var data = pcb.MemoryBlocks.ReadByte(index);

            //assertion
            Assert.AreEqual(data,output);

        }

        [Test]
        [TestCase("a\naaaaaaaaaaaaaaaaaaaaaaaaa",1,'\n')]
        [TestCase("aaaaaaaaaaaaaaaaaaaaaaaaaaa",16,'z')]
        [TestCase("aaaaaaaaaaaaaaaaaaaaaaaaaaa",20,'z')]
        [TestCase("aaaaaaaaaaaaaaaaaaaaaaaaaaa",15,'z')]
        [TestCase("aaaaaaaaaaaaaaaaaaaaaaaaaaa",25,'z')]
        public void Can_Change_Data(string input, int index, char change)
        {
            //prepare
            var pcbfiller = new PCB();
            var pcb = new PCB();
            Memory.GetInstance.AllocateMemory(pcb,input);

            //aciton
            pcb.MemoryBlocks.ChangeByte(index,change);
            Memory.GetInstance.TestFillMemory(pcbfiller);
            var data = pcb.MemoryBlocks.ReadByte(index);

            //assertion
            Assert.AreEqual(data,change);
        }

        [Test]
        [TestCase("Witam")]
        [TestCase("Taken process from memory")]
        public void Can_Write_Message(string message)
        {

            //prepare
            Memory.GetInstance.PlaceMessage(message);

            //action
            var response = Memory.GetInstance.ReadMessage();

            //assertion
            Assert.AreEqual(message,response,message = "Wartosc to: "+message+" powinno byc: "+response);
        }

    }
}

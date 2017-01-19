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
    class FrameTests
    {
        [Test]
        [TestCase(new char[] {'a','b','c','d','e'})]
        [TestCase(new char[] {'a','b','c','d','e', 'a', 'b', 'c', 'd', 'e'})]
        [TestCase(new char[] {'h','a','b','c','d','e'})]

        public void Can_Write_Read_From_Frame(char[] input)
        {
            //prepare
            AllocationUnit frame = new AllocationUnit(16);

            //action
            frame.WriteAllocationUnit(input);
            var output = frame.ReadAllocationUnit();

            Assert.AreEqual(input,output);
        }

        [Test]
        public void Can_Clear_Frame()
        {
            //prepare
            var frame = new AllocationUnit(16);
            frame.WriteAllocationUnit(new char[]{'a','b','c'});

            //action
            frame.ClearAllocationUnit();

            //assertion
            Assert.AreEqual(frame.Offset,0);
        }

        [Test]
        [TestCase(new char[] {'i','b','c','d','e','o'},1)]
        [TestCase(new char[] {'d','b','y','d','e','p'},2)]
        [TestCase(new char[] {'a','a','c','n','p','f'},3)]
        [TestCase(new char[] {'h','y','c',',','e','h'},4)]
        public void Can_Get_Byte(char[] input, int index)
        {
            //prepare
            var frame = new AllocationUnit(16);
            frame.WriteAllocationUnit(input);

            //action
            var output = frame.GetByte(index);

            //assertion
            Assert.AreEqual(input[index],output);
        }

        [Test]
        [TestCase(new char[] {'i', 'b', 'c', 'd', 'e', 'o'}, 1, 'a')]
        [TestCase(new char[] {'d', 'b', 'y', 'd', 'e', 'p'}, 2, 'b')]
        [TestCase(new char[] {'a', 'a', 'c', 'n', 'p', 'f'}, 3, 'c')]
        [TestCase(new char[] {'h', 'y', 'c', ',', 'e', 'h'}, 4, 'd')]
        public void Can_Change_Byte(char[] input, int index, char data)
        {
            //prepare
            var frame = new AllocationUnit(16);
            frame.WriteAllocationUnit(input);

            //action
            frame.ChangeByte(index,data);
            var output = frame.GetByte(index);

            //assertion
            Assert.AreEqual(output,data);

        }

    }
}

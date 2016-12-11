using NUnit.Framework;
using Vow_win_ski.CPU;
using Vow_win_ski.Processes;

namespace Vow_win_ski.Tests.CPU
{
    [TestFixture]
    class InterpreterTests
    {
        [Test]
        [TestCase("HLT", "HLT")]
        [TestCase("XC ProcessName,fileName", "XC")]
        [TestCase("XR", "XR")]
        [TestCase("Etykieta:", "Etykieta:")]
        [TestCase("MF Name", "MF")]
        [TestCase("XS Name,To jest komunikat", "XS")]
        public void Get_GetOrderName_Splitted(string order, string result)
        {
            var interpreter = Interpreter.GetInstance;
            string orderName = interpreter.GetOrderName(order);
            Assert.AreEqual(orderName, result);
        }

        [Test]
        [TestCase("XC ProcessName,fileName", "ProcessName")]
        [TestCase("MF Name", "Name")]
        [TestCase("XS Name,To jest komunikat", "Name")]
        public void Get_GetFirstOrderArgument_Splitted(string order, string result)
        {
            var interpreter = Interpreter.GetInstance;
            string orderFirstArgument = interpreter.GetOrderFirstArgument(order);
            Assert.AreEqual(orderFirstArgument, result);
        }

        [Test]
        [TestCase("XC ProcessName,fileName", "fileName")]
        [TestCase("XS Name,To jest komunikat", "To jest komunikat")]
        public void Get_GetOrderSecondArgument_Splitted(string order, string result)
        {
            var interpreter = Interpreter.GetInstance;
            string orderSecondArgument = interpreter.GetOrderSecondArgument(order);
            Assert.AreEqual(orderSecondArgument, result);
        }
    }
}

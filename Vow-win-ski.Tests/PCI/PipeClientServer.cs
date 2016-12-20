using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Vow_win_ski.IPC;
using Vow_win_ski.MemoryModule;

namespace Vow_win_ski.Tests.PCI
{
    [TestFixture]
    class PipeClientServer
    {
        [Test]
        [TestCase("Wiadomosc")]
        [TestCase("tekst")]
        [TestCase("cos tam innego")]
        public void Can_Read_Write_Message(string message)
        {
            PipeServer.InitServer();
            var client1 = new PipeClient("Nadawca");
            var client2 = new PipeClient("Odbiorca");

            client1.Connect();
            client1._send("Odbiorca",message);
            client1.Disconnect();

            client2.Connect();
            var receive = client2._receive();
            client2.Disconnect();

            PipeServer.GetServer.Exit();

            Assert.AreEqual(receive , true);
            Assert.AreEqual(message,Memory.GetInstance.ReadMessage());
        }
    }
}

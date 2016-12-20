using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Vow_win_ski.IPC;

namespace Vow_win_ski.Tests.PCI
{
    [TestFixture]
    class MessageTests
    {
        [Test]
        [TestCase("wiadomosc","Dostarczyciel","Odbiorca")]
        [TestCase("innytest","Odbiorca1","Nadawca1")]
        [TestCase("dfjdgegeg","odbiorca2","nadawca2")]
        public void Can_Read_Message(string message,string receiverId,string senderId)
        {
            var tempMessage = new Message(message, receiverId, senderId);

            Assert.AreEqual(message,tempMessage.GetMessage());
            Assert.AreEqual(receiverId,tempMessage.GetReceiverId());
            Assert.AreEqual(senderId,tempMessage.GetSenderId());
        }
    }
}

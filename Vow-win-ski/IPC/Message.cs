using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vow_win_ski.IPC
{
    public class Message
    {
        private string senderId;
        private string receiverId;
        private string message;

        //===================================================================================================================================
        public Message(string message, string receiverId, string senderId)
        {
            this.receiverId = receiverId;
            this.message = message;
            this.senderId = senderId;
        }
        //===================================================================================================================================
        public string GetReceiverId()
        {
            return receiverId;
        }
        public string GetMessage()
        {
            return message;
        }
        public string GetSenderId()
        {
            return senderId;
        }
        //===================================================================================================================================
    }
}

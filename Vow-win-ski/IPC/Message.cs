using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vow_win_ski.IPC
{
    class Message
    {      
        private byte senderId;
        private byte receiverId;
        private byte message;

        //===================================================================================================================================
        public Message(byte receiverId, byte message, byte senderId)
        {
            this.receiverId = receiverId;
            this.message = message;
            this.senderId = senderId;
        }
        //===================================================================================================================================
        public byte GetReceiverId()
        {
            return receiverId;
        }

        public int getMessage()
        {
            return message;
        }

        public byte GetSenderId()
        {
            return senderId;
        }
        //===================================================================================================================================
    }
}

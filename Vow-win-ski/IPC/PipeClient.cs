using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.IO.Pipes;
using Vow_win_ski.MemoryModule;

namespace Vow_win_ski.IPC
{
    class PipeClient
    {
        private NamedPipeClientStream client;
        private StreamString strString;
        private string[] message;
        private string clientId;

        //===================================================================================================================================
        private const string sender = "0";
        private const string receiver = "1";
        private const string disconnecter = "2";
        //===================================================================================================================================
        public PipeClient(string clientId)
        {
            client = new NamedPipeClientStream(".", "SERWER", PipeDirection.InOut);
            this.clientId = clientId;
            strString = new StreamString(client);
        }
        //===================================================================================================================================
        public void Connect()
        {
            if (client.IsConnected) return;
            client.Connect();
        }
        //===================================================================================================================================
        public void _send(string receiverId, string message)
        {
            strString.WriteString(sender + ";" + message + ";" + receiverId + ";" + clientId);
            Console.WriteLine(clientId+ " sent to "+receiverId+": "+message);
        }
        //===================================================================================================================================
        public void Call()
        {
            strString.WriteString(receiver + ";" + " " + ";" + " " + ";" + clientId);
        }
        //===================================================================================================================================
        public bool _receive()
        {
            Call();
            if (client.ReadByte() != 0)
            {
                var readLine = strString.ReadString();
                if (readLine != null) message = readLine.Split(';');
                Console.WriteLine(clientId + " received from " + message[1] + ": "+ message[0]);
                Memory.GetInstance.PlaceMessage(message[0]);
                return true;
            }
            else
            {
                return false;
            }
        }
        //===================================================================================================================================
        public void Disconnect()
        {
            if (!client.IsConnected) return;
            strString.WriteString(disconnecter);
        }
        //===================================================================================================================================
    }
}

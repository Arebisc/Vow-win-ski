﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO.Pipes;
using System.IO;



namespace Vow_win_ski.IPC
{
    class PipeServer
    {
        private NamedPipeServerStream Server;
        private List<Message> Messages;
        private StreamString strString;
        private Thread thread;
        private string[] message;
 
        //===================================================================================================================================
        private const string receiver = "0";
        private const string sender = "1";
        private const string disconnecter = "2";
        //===================================================================================================================================
        public PipeServer()
        {
            Server = new NamedPipeServerStream("SERWER", PipeDirection.InOut);
            Console.WriteLine("Tworzenie Serwera IPC");
            Start();
        }
        //===================================================================================================================================
        public void Build()
        {
            Server.WaitForConnection();
            Messages = new List<Message>();
            strString = new StreamString(Server);
        }
        //===================================================================================================================================
        public void ServerReceiver()
        {
            string receive = strString.ReadString();
         
            if (receive != null)
            {
                message = receive.Split(';');               
            }
        }
        //===================================================================================================================================
        public void StoreMessage()
        {
            Messages.Add(new Message(message[1], message[2], message[3]));
        }
        //===================================================================================================================================
        public void Switch()
        {
            switch (message[0])
            {
                case receiver:
                    StoreMessage();
                    break;
                case sender:
                    ServerWriter(message[3]);
                    break;
                case disconnecter:
                    Server.Disconnect();
                    Console.WriteLine("Client rozlaczony z serwerem");
                    break;
            }
        }
        //===================================================================================================================================

        public void ServerWriter(string receiverId)
        {
            if (Messages.All(x => x.GetReceiverId() != receiverId))
            {
                Server.WriteByte(0);
            }
            else
            {
                Server.WriteByte(1);
                for (int i = 0; i < Messages.Count; i++)
                {
                    if (Messages[i].GetReceiverId() != receiverId) continue;
                    Console.WriteLine("Serwer wyslal dane");
                    strString.WriteString(Messages[i].GetMessage() + ";" + Messages[i].GetSenderId());
                    Messages.RemoveAt(i);
                    break;
                }
            }
        }
        //===================================================================================================================================
        public void ServerInit()
        {
            Build();
            while (true)
            {
                if (Server.IsConnected)
                {
                    ServerReceiver();
                    Switch();
                }
                else
                {
                    Console.WriteLine("Serwer oczekuje na polaczenie");
                    Server.WaitForConnection();
                }
            }
        }
        //===================================================================================================================================
        public void Start()
        {
            thread = new Thread(ServerInit);
            thread.Start();
        }
        //===================================================================================================================================

        public void Show()
        {
            foreach (var x in Messages)
            {
                Console.WriteLine(x.GetSenderId()+ " to " +x.GetReceiverId() + " " +x.GetMessage());
            }
        }
    }
}

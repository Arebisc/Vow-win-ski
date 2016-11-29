using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO.Pipes;

namespace Vow_win_ski.IPC
{
    class PipeClient
    {
        private NamedPipeClientStream client;
        private byte[] data = new byte[4];
        private byte clientId;

        //===================================================================================================================================
        private const byte sender = 0;
        private const byte receiver = 1;
        private const byte disconnecter = 2;



        //===================================================================================================================================
        public PipeClient(byte clientId)
        {
            client = new NamedPipeClientStream(".", "SERWER", PipeDirection.InOut);
            Console.WriteLine("Utworzono Clienta IPC o ID:" + clientId);
            this.clientId = clientId;
        }
        //===================================================================================================================================
        public void Connect()
        {

            if (client.IsConnected)
            {
                Console.WriteLine("Client polaczony z serwerem");
            }
            else
            {
                client.Connect();
            }
        }
        //===================================================================================================================================
        public void _send(byte receiverId, byte message)
        {
            data[0] = sender;
            data[1] = receiverId;
            data[2] = message;
            data[3] = clientId;
            client.Write(data, 0, data.Length);



        }
        //===================================================================================================================================
        public void Call()
        {
            data[0] = receiver;
            data[1] = clientId;

            client.Write(data, 0, data.Length);
        }
        //===================================================================================================================================
        public bool _receive()
        {

            byte[] temp = new byte[4];
            Call();
            client.Read(temp, 0, 4);

            if (temp[0] != 0)
            {
                Console.WriteLine("Odebrano wiadomosc: " + temp[2] + " Od procesu o ID: " + temp[3]);
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

            if (client.IsConnected)
            {
                data[0] = disconnecter;
                client.Write(data, 0, data.Length);
            }
          
        }
        //===================================================================================================================================
    }
}

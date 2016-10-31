using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO.Pipes;



namespace Vow_win_ski.IPC
{
    class PipeServer
    {
        private NamedPipeServerStream Server;
        private List<Message> Messages; // Kolejka wiadomosci
        private Thread thread;
        private byte[] data; //Przechowkuje w [0] - Jeśli value=0 serwer czyta, jeśli value=1 serwer pisze | [1] - Idodbiorcy | [2] - komunikat | [3] - Idnadawcy
        private byte[] data; //Przechowkuje w [0] - Jeśli value=0 serwer czyta, jeśli value=1 serwer pisze | [1] - Id odbiorcy | [2] - komunikat | [3] - Id nadawcy

        //===================================================================================================================================
        private const byte sender = 1;
        private const byte receiver = 0;
        private const byte disconnecter = 2;
        /// <summary>
        /// Flagi do ustawienia stanu Serwera
        /// </summary>
        //===================================================================================================================================
        public PipeServer()
        {
            Server = new NamedPipeServerStream("SERWER", PipeDirection.InOut);
            Console.WriteLine("Utworzono Serwer IPC");
            Start();
            /// <summary>
            /// MICHAŁ: Konstruktor wywołany wraz ze startem systemu i to tyle jeśli chodzi o SERWER.
            /// </summary>
        }
        //===================================================================================================================================
        public void Build()
        {
            Server.WaitForConnection();
            Console.WriteLine("Serwer oczekuje na polaczenie");
            Messages = new List<Message>();
            data = new byte[4];
        }
        //===================================================================================================================================
        public byte[] ServerReceiver()
        {
            Console.WriteLine("Serwer otrzymal dane");
            Server.Read(data, 0, 4);
            return data;
        }
        //===================================================================================================================================
        public void StoreMessage()
        {
            Messages.Add(new Message(data[1], data[2], data[3])); // Zapisywanie do listy 
        }
        //===================================================================================================================================
        public void Switch()
        {
            if (data[0] == receiver) // Stan odczytu wiadomosci
            {
                StoreMessage();
            }
            else if (data[0] == sender) // Stan zapisu wiadomosci
            {
                ServerWriter(data[1], data[3]);
            }
            else if (data[0] == disconnecter) // Stan rozlaczenia 
            {
                Server.Disconnect();
                Console.WriteLine("Client rozlaczony z serwerem");
            }
        }
        //===================================================================================================================================

        public void ServerWriter(byte receiverId, byte senderId)
        {
            for (int i = 0; i < Messages.Count; i++)
            {
                if (Messages[i].GetReceiverId() == receiverId && Messages[i].GetSenderId() == senderId) // Czy zgadza się Id odbiorcy i nadawcy
                {
                    Console.WriteLine("Serwer wyslal dane");
                    Server.WriteByte((byte)Messages[i].getMessage());
                    Messages.RemoveAt(i);
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
                    data = ServerReceiver();
                    Switch();
                }
                else
                {
                    Console.WriteLine("Serwer oczekuje na polaczenie");
                    Server.WaitForConnection();
                }
            }
        }
        /// <summary>
        /// Inicjalizacja i Główna pętla serwera
        /// </summary>
        //===================================================================================================================================
        public void Start()
        {
            thread = new Thread(ServerInit);
            thread.Start();
        }
        //===================================================================================================================================
    }
}

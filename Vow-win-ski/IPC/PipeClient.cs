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
        /// <summary>
        /// Flagi do ustawienia stanu serwera
        /// </summary>
        //===================================================================================================================================
        public PipeClient(byte clientId)
        {
            client = new NamedPipeClientStream(".", "SERWER", PipeDirection.InOut);
            Console.WriteLine("Utworzono Clienta IPC o ID:"+clientId);
            this.clientId = clientId;
            Connect();
            /// <summary>
            /// MARCIN: Konstruktor wywolany gdy PROCES wejdzie w stan RUNNING. Podaj ID procesu w którym będzie siedział klient jako argument.
            /// </summary>
        }
        //===================================================================================================================================
        public void Connect()
        {
            client.Connect();
            if (client.IsConnected)
            {
                Console.WriteLine("Client polaczony z serwerem");
            }
        }
        //===================================================================================================================================
        public void Send(byte receiverId, byte message)
        {
            data[0] = sender;
            data[1] = receiverId;
            data[2] = message;
            data[3] = clientId;
            client.Write(data, 0, data.Length);
            /// <summary>
            /// MICHAŁ: Metoda do wysyłania komunikatu. Pierwszy argument to ID odbiorcy, drugi to wiadomosc.
            /// </summary>
        }
        //===================================================================================================================================
        public void Call(byte senderId)
        {
            data[0] = receiver;
            data[1] = clientId;
            data[3] = senderId;
            client.Write(data, 0, data.Length);
        }
        //===================================================================================================================================
        public void Receive(byte senderId)
        {
            Call(senderId);
            Console.WriteLine("Odebrano wiadomosc: "+client.ReadByte());
            /// <summary>
            /// MICHAŁ: Metoda do odebrania komunikatu. Argumentem jest ID procesu od którego chcemy odebrać komunikat.
            /// Na razie wiadomosc wypisuje tylko do konsoli, bo nie mam pojecia czy bedzie jakos wykorzystywana
            /// </summary>
        }
        //===================================================================================================================================
        public void Disconnect()
        {
            data[0] = disconnecter;
            client.Write(data, 0, data.Length);
            /// <summary>
            /// MARCIN: Gdy proces skończy być running wywołaj tą metodę, aby rozłączyć clienta z serwerem
            /// </summary>
        }
        //===================================================================================================================================
    }
}

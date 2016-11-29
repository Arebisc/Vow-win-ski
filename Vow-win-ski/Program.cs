using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vow_win_ski.IPC;

namespace Vow_win_ski
{
    class Program
    {
        static void Main(string[] args)
        {

            PipeServer server = new PipeServer();

            PipeClient client = new PipeClient(1);

            client.Connect();

            client._send(2, 10);

            client.Disconnect();

            PipeClient client2 = new PipeClient(2);

            client2.Connect();

            client2._receive();

            client2.Disconnect();




        }
    }
}

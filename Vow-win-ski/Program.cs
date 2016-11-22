using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileSystem;
using FileSystem.FileSystem;

namespace Vow_win_ski
{
    class Program
    {
        static void Main(string[] args)
        {
            Disc disc = args.Length > 0 ? new Disc(int.Parse(args[0])) : new Disc();
            DiscCommander discCommander = new DiscCommander();
            discCommander.OpenShell(disc);
            Console.ReadKey();
        }
    }
}

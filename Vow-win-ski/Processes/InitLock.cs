using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Vow_win_ski.Processes;

namespace Vow_win_ski.CPU
{
    public sealed class InitLock
    {
        private static volatile InitLock _instance;
        private static readonly object SyncRoot = new object();
        private static Lockers MemoLock;
        private static Lockers ProcLock;

        private InitLock()
        {
            Lockers MemoLock = new Lockers();
            Lockers ProcLock = new Lockers();
        }
    }
}

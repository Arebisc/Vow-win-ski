using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Vow_win_ski.Processes;

namespace Vow_win_ski.CPU
{
    public sealed class LockersHolder
    {
        private static volatile LockersHolder _instance;
        private static readonly object SyncRoot = new object();
        public static Lockers MemoLock;
        public static Lockers ProcLock;

        public static void InitLockers()
        {
            _instance = new LockersHolder();
        }

        private LockersHolder()
        {
            Lockers MemoLock = new Lockers();
            Lockers ProcLock = new Lockers();
        }

        public static LockersHolder GetInstance => _instance;
    }
}

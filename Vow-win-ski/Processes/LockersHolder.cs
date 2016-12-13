using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vow_win_ski.Processes
{
    public sealed class LockersHolder
    {
        private static volatile LockersHolder _instance;
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

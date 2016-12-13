using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vow_win_ski.Processes;

namespace Vow_win_ski.CPU
{
    public sealed class CPU
    {
        private static volatile CPU _instance;
        private static readonly object SyncRoot = new object();
        public Register Register;

        private CPU()
        {
            this.Register = new Register();
        }

        public static CPU GetInstance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if(_instance == null)
                            _instance = new CPU();
                    }
                }

                return _instance;
            }
        }
    }
}

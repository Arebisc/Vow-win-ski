using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vow_win_ski.CPU
{
    public sealed class CPU
    {
        private static volatile CPU _instance;
        private static object syncRoot = new Object();

        private CPU()
        {
            register = new Register();
        }

        public Register register;

        public static CPU GetInstance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
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

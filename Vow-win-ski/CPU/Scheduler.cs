using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vow_win_ski.CPU
{
    public class Scheduler
    {
        private static volatile Scheduler _instance;
        private static object syncRoot = new Object();

        private Scheduler()
        {
            
        }


        public static Scheduler Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                            _instance = new Scheduler();
                    }
                }

                return _instance;
            }
        }
    }
}

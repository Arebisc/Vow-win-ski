using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vow_win_ski.CPU
{
    public sealed class Interpreter
    {
        private static readonly object SyncRoot = new object();
        private static volatile Interpreter _instance;

        private Interpreter() { }

        public static Interpreter GetInstance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if(_instance == null)
                            _instance = new Interpreter();
                    }
                }

                return _instance;
            }
        }
        //TODO
        private void InterpretOrder(string order)
        {
            throw new NotImplementedException();
        }

    }
}

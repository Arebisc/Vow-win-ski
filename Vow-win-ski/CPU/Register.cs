using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vow_win_ski.CPU
{
    public class Register
    {
        private int _A;
        private int _B;
        private int _C;
        private int _D;

        public int A
        {
            get { return _A; }
            set { _A = value; }
        }

        public int B
        {
            get { return _B; }
            set { _B = value; }
        }

        public int C
        {
            get { return _C; }
            set { _C = value; }
        }

        public int D
        {
            get { return _D; }
            set { _D = value; }
        }

        public Register() { }

        public Register(int a, int b, int c, int d)
            : this()
        {
            A = a;
            B = b;
            C = c;
            D = d;
        }
    }
}

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
        private double _C;
        private double _D;
        private int _instructionPointer;

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

        public double C
        {
            get { return _C; }
            set { _C = value; }
        }

        public double D
        {
            get { return _D; }
            set { _D = value; }
        }

        public int InstructionPointer
        {
            get { return _instructionPointer; }
            set { _instructionPointer = value; }
        }

        public Register() { }

        public Register(int a, int b, double c, double d, int instructionPointer)
            : this()
        {
            A = a;
            B = b;
            C = c;
            D = d;
            InstructionPointer = instructionPointer;
        }

        public Register(Register register)
            : this()
        {
            A = register.A;
            B = register.B;
            C = register.C;
            D = register.D;
            InstructionPointer = register.InstructionPointer;
        }
    }
}

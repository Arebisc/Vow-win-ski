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

        public int InstructionPointer
        {
            get { return _instructionPointer; }
            set { _instructionPointer = value; }
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

        public Register(int a, int b, int c, int d, int instructionPointer)
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

        public override string ToString()
        {
            String result = String.Format("A: {0}; B: {1}; C: {2}; D: {3}", A, B, C, D);

            return result;
        }

        public void PrintRegisters()
        {
            Console.WriteLine(this.ToString());
        }
    }
}

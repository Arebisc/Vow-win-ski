using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Vow_win_ski.Processes;

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

        public void InterpretOrder(string order)
        {
            if (order.EndsWith(":"))
            {
                order.Trim(':');
                Console.WriteLine("Etykieta o nazwie: " + order);
            }
            else
            {
                switch (GetOrderName(order))
                {
                    case "HLT":
                        HLTOrder();
                        break;
                    case "XC":
                        XCOrder(GetOrderFirstArgument(order), GetOrderSecondArgument(order));
                        break;
                    case "XD":
                        XDOrder(GetOrderFirstArgument(order));
                        break;
                    case "XR":
                        XROrder();
                        break;
                    case "XS":
                        XSOrder(GetOrderFirstArgument(order), GetOrderSecondArgument(order));
                        break;
                    case "XN":
                        XNOrder(GetOrderFirstArgument(order));
                        break;
                    case "XY":
                        XYOrder(GetOrderFirstArgument(order));
                        break;
                    case "XZ":
                        XZOrder(GetOrderFirstArgument(order));
                        break;
                    case "AD":
                        ADOrder(GetOrderFirstArgument(order), GetOrderSecondArgument(order));
                        break;
                    case "SB":
                        SBOrder(GetOrderFirstArgument(order), GetOrderSecondArgument(order));
                        break;
                    case "MU":
                        MUOrder(GetOrderFirstArgument(order), GetOrderSecondArgument(order));
                        break;
                    case "JM":
                        JMOrder(GetOrderFirstArgument(order));
                        break;
                    case "MV":
                        MVOrder(GetOrderFirstArgument(order), GetOrderSecondArgument(order));
                        break;
                    case "MN":
                        MNOrder(GetOrderFirstArgument(order), Int32.Parse(GetOrderSecondArgument(order)));
                        break;
                    case "MF":
                        MFOrder(GetOrderFirstArgument(order));
                        break;
                    case "WF":
                        WFOrder(GetOrderFirstArgument(order), GetOrderSecondArgument(order));
                        break;
                    case "WR":
                        WROrder(GetOrderFirstArgument(order), GetOrderSecondArgument(order));
                        break;
                    case "DF":
                        DFOrder(GetOrderFirstArgument(order));
                        break;
                    case "PO":
                        POOrder(GetOrderFirstArgument(order));
                        break;
                }
            }
        }

        public string GetOrderFromMemory(PCB runningPCB)
        {
            var order = String.Empty;
            int iterator = 0;

            for (int i = 0; i < runningPCB.InstructionCounter; i++)
            {
                while (runningPCB.MemoryBlocks.ReadByte(iterator) != '\n' && iterator <= runningPCB.MaxMemory)
                {
                    iterator++;
                }
                iterator++;
            }

            while (runningPCB.MemoryBlocks.ReadByte(iterator) != '\n' &&  iterator <= runningPCB.MaxMemory)
            {
                order += runningPCB.MemoryBlocks.ReadByte(iterator);
                iterator++;
            }
            iterator++;
            runningPCB.InstructionCounter++;

            return order;
        }

        public string GetOrderName(string order)
        {
            string orderName = String.Empty;

            for(int i = 0; i < order.Length; i++)
            {
                if (order[i] != ' ')
                    orderName += order[i];
                else break;
            }

            return orderName;
        }

        public string GetOrderFirstArgument(string order)
        {
            var orderName = GetOrderName(order);

            var trimmedOrderName = order.TrimStart(orderName.ToCharArray());
            var trimmedWhiteSpaces = trimmedOrderName.Trim();

            if (!trimmedWhiteSpaces.Contains(','))
                return trimmedWhiteSpaces;

            var result = String.Empty;
            for (int i = 0; i < trimmedWhiteSpaces.Length; i++)
            {
                if (trimmedWhiteSpaces[i] != ',')
                    result += trimmedWhiteSpaces[i];
                else break;
            }

            return result;
        }

        public string GetOrderSecondArgument(string order)
        {
            var trimmedOrderName = order.TrimStart(GetOrderName(order).ToCharArray());
            var trimmedWhiteSpaces = trimmedOrderName.Trim();

            var trimmedFirstOrder = trimmedWhiteSpaces.TrimStart(GetOrderFirstArgument(order).ToCharArray());
            var result = trimmedFirstOrder.TrimStart(',');

            return result;
        }

        private void POOrder(string register)
        {
            Console.WriteLine("Rozkaz PO z parametrem " + register);
            //throw new NotImplementedException();
        }

        private void DFOrder(string fileName)
        {
            Console.WriteLine("Rozkaz DF z parametrem " + " " + fileName);
            //throw new NotImplementedException();
        }

        private void WROrder(string fileName, string register)
        {
            Console.WriteLine("Rozkaz WR z parametrem " + fileName + " " + register);
            //throw new NotImplementedException();
        }

        private void WFOrder(string fileName, string content)
        {
            Console.WriteLine("Rozkaz WF z parametrem " + fileName + " " + content);
            //throw new NotImplementedException();
        }

        private void MFOrder(string fileName)
        {
            Console.WriteLine("Rozkaz MF z parametrem " + " " + fileName);
            //throw new NotImplementedException();
        }

        private void MNOrder(string register, int number)
        {
            Console.WriteLine("Rozkaz MN z parametrem " + register + " " + number);
            //throw new NotImplementedException();
        }

        private void MVOrder(string register1, string register2)
        {
            Console.WriteLine("Rozkaz MV z parametrem " + register1 + " " + register2);
            //throw new NotImplementedException();
        }

        private void JMOrder(string tag)
        {
            Console.WriteLine("Rozkaz JM z parametrem " + tag);
            //throw new NotImplementedException();
        }

        private void MUOrder(string register1, string register2)
        {
            Console.WriteLine("Rozkaz MV z parametrem " + register1 + " " + register2);
            //throw new NotImplementedException();
        }

        private void SBOrder(string register1, string register2)
        {
            Console.WriteLine("Rozkaz SB z parametrem " + register1 + " " + register2);
            //throw new NotImplementedException();
        }

        private void ADOrder(string register1, string register2)
        {
            Console.WriteLine("Rozkaz AD z parametrem " + register1 + " " + register2);
            //throw new NotImplementedException();
        }

        private void XZOrder(string processName)
        {
            Console.WriteLine("Rozkaz XZ z parametrem " + processName);
            //throw new NotImplementedException();
        }

        private void XYOrder(string processName)
        {
            Console.WriteLine("Rozkaz XY z parametrem " + processName);
            //throw new NotImplementedException();
        }

        private void XNOrder(string processName)
        {
            Console.WriteLine("Rozkaz XN z parametrem " + processName);
            //throw new NotImplementedException();
        }

        private void XSOrder(string processName, string communicate)
        {
            Console.WriteLine("Rozkaz XS z parametrem " + processName + " " + communicate);
            Scheduler.GetInstance.GetRunningPCB().Send(processName, communicate);
        }

        private void XROrder()
        {
            Console.WriteLine("Rozkaz XR");
            Scheduler.GetInstance.GetRunningPCB().Receive();
        }

        private void XDOrder(string processName)
        {
            Console.WriteLine("Rozkaz XD z parametrem " + processName);
            //throw new NotImplementedException();
        }

        private void XCOrder(string processName, string fileName)
        {
            Console.WriteLine("Rozkaz XC z parametrem " + processName + " " + fileName);
            //throw new NotImplementedException();
        }

        private void HLTOrder()
        {
            Console.WriteLine("Rozkaz HLT");
            //throw new NotImplementedException();
        }
    }
}

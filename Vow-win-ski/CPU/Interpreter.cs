using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Vow_win_ski.FileSystem;
using Vow_win_ski.Processes;

namespace Vow_win_ski.CPU
{
    public sealed class Interpreter
    {
        private static readonly object SyncRoot = new object();
        private static volatile Interpreter _instance;

        private Interpreter() { }

        public int orderCounter { get; private set; }

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

        public void InterpretOrder()
        {
            if (Scheduler.GetInstance.CheckIfOtherProcessShouldGetCPU())
            {
                if (Scheduler.GetInstance.GetRunningPCB() != null)
                {
                    Scheduler.GetInstance.RevriteRegistersFromCPU();
                    Scheduler.GetInstance.GetRunningPCB().State = ProcessState.Ready;
                }
                Scheduler.GetInstance.PriorityAlgorithm().State = ProcessState.Running;
                Scheduler.GetInstance.RevriteRegistersToCPU();
                Console.WriteLine("Przełączono CPU na process: " + Scheduler.GetInstance.GetRunningPCB().Name);
                return;
            }

            orderCounter++;
            if (orderCounter % 3 == 0)
            {
                Scheduler.GetInstance.AgingWaitingProcesses();
                Scheduler.GetInstance.RejuvenationCurrentProcess();
            }

            var order = GetOrderFromMemory(Scheduler.GetInstance.GetRunningPCB());

            if (order.EndsWith(":"))
            {
                order.Trim(':');
                Console.WriteLine("Etykieta o nazwie: " + order);
                Scheduler.GetInstance.GetRunningPCB().InstructionCounter++;
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

        public void POOrder(string register)
        {
            Console.WriteLine("Rozkaz PO z parametrem " + register);
            
            Console.WriteLine(register);
        }

        public void DFOrder(string fileName)
        {
            Console.WriteLine("Rozkaz DF z parametrem " + " " + fileName);

            Disc.GetDisc.DeleteFile(fileName);
        }

        public void WROrder(string fileName, string register)
        {
            Console.WriteLine("Rozkaz WR z parametrem " + fileName + " " + register);

            Disc.GetDisc.AppendToFile(fileName, register);
        }

        public void WFOrder(string fileName, string content)
        {
            Console.WriteLine("Rozkaz WF z parametrem " + fileName + " " + content);

            Disc.GetDisc.AppendToFile(fileName, content);
        }

        public void MFOrder(string fileName)
        {
            Console.WriteLine("Rozkaz MF z parametrem " + " " + fileName);

            Disc.GetDisc.CreateFile(fileName, String.Empty);
        }

        public void MNOrder(string register, int number)
        {
            Console.WriteLine("Rozkaz MN z parametrem " + register + " " + number);
            CPU.GetInstance.Register.SetRegisterValueByName(register, number);
        }

        public void MVOrder(string register1, string register2)
        {
            Console.WriteLine("Rozkaz MV z parametrem " + register1 + " " + register2);

            CPU.GetInstance.Register.SetRegisterValueByName(register1, 
                CPU.GetInstance.Register.GetRegisterValueByName(register2));
        }

        public void JMOrder(string tag)
        {
            Console.WriteLine("Rozkaz JM z parametrem " + tag);

            if (CPU.GetInstance.Register.C != 0)
            {
                var runningPCB = Scheduler.GetInstance.GetRunningPCB();
                runningPCB.InstructionCounter = 0;

                var order = String.Empty;
                int iterator = 0;
                bool foundFlag = false;

                while (foundFlag != true)
                {
                    while (runningPCB.MemoryBlocks.ReadByte(iterator) != '\n' && iterator <= runningPCB.MaxMemory)
                    {
                        order += runningPCB.MemoryBlocks.ReadByte(iterator);
                        iterator++;
                    }
                    iterator++;
                    runningPCB.InstructionCounter++;

                    if (order.TrimEnd(':') == tag)
                        foundFlag = true;
                    order = String.Empty;
                }
            }
            CPU.GetInstance.Register.C--;
        }

        public void MUOrder(string register1, string register2)
        {
            Console.WriteLine("Rozkaz MV z parametrem " + register1 + " " + register2);
            if (!IsNumeric(register2))
            {
                var oldReg1Value = CPU.GetInstance.Register.GetRegisterValueByName(register1);
                CPU.GetInstance.Register.SetRegisterValueByName(
                    register1,
                     CPU.GetInstance.Register.GetRegisterValueByName(register2) * oldReg1Value);
            }
            else
            {
                CPU.GetInstance.Register.SetRegisterValueByName(
                    register1,
                    CPU.GetInstance.Register.GetRegisterValueByName(register1) * Int32.Parse(register2));
            }
        }

        public void SBOrder(string register1, string register2)
        {
            Console.WriteLine("Rozkaz SB z parametrem " + register1 + " " + register2);
            if (!IsNumeric(register2))
            {
                var oldReg1Value = CPU.GetInstance.Register.GetRegisterValueByName(register1);
                CPU.GetInstance.Register.SetRegisterValueByName(
                    register1,
                     CPU.GetInstance.Register.GetRegisterValueByName(register2) - oldReg1Value);
            }
            else
            {
                CPU.GetInstance.Register.SetRegisterValueByName(
                    register1,
                    CPU.GetInstance.Register.GetRegisterValueByName(register1) - Int32.Parse(register2));
            }
        }

        public void ADOrder(string register1, string register2)
        {
            Console.WriteLine("Rozkaz AD z parametrem " + register1 + " " + register2);

            if (!IsNumeric(register2))
            {
                var oldReg1Value = CPU.GetInstance.Register.GetRegisterValueByName(register1);
                CPU.GetInstance.Register.SetRegisterValueByName(
                    register1,
                     CPU.GetInstance.Register.GetRegisterValueByName(register2) + oldReg1Value);
            }
            else
            {
                CPU.GetInstance.Register.SetRegisterValueByName(
                    register1,
                    CPU.GetInstance.Register.GetRegisterValueByName(register1) + Int32.Parse(register2));
            }
            
        }

        public bool IsNumeric(string text)
        {
            int n;
            if(int.TryParse(text, out n))
                return true;
            return false;
        }

        public void XZOrder(string processName)
        {
            Console.WriteLine("Rozkaz XZ z parametrem " + processName);

            PCB.GetPCB(processName).TerminateProcess(ReasonOfProcessTerminating.Ended);
        }

        public void XYOrder(string processName)
        {
            Console.WriteLine("Rozkaz XY z parametrem " + processName);

            PCB.GetPCB(processName).RunNewProcess();
        }

        public void XNOrder(string processName)
        {
            Console.WriteLine("Rozkaz XN z parametrem " + processName);

            var pcbID = PCB.GetPCB(processName).PID;
            CPU.GetInstance.Register.SetRegisterValueByName("A", pcbID);
        }

        public void XSOrder(string processName, string communicate)
        {
            Console.WriteLine("Rozkaz XS z parametrem " + processName + " " + communicate);
            Scheduler.GetInstance.GetRunningPCB().Send(processName, communicate);
        }

        public void XROrder()
        {
            Console.WriteLine("Rozkaz XR");
            Scheduler.GetInstance.GetRunningPCB().Receive();
        }

        public void XDOrder(string processName)
        {
            Console.WriteLine("Rozkaz XD z parametrem " + processName);

            var flag = PCB.GetPCB(processName).TerminateProcess(ReasonOfProcessTerminating.KilledByOther, 
                Scheduler.GetInstance.GetRunningPCB());

            if(flag == 0)
                PCB.GetPCB(processName).RemoveProcess();
        }

        public void XCOrder(string processName, string fileName)
        {
            Console.WriteLine("Rozkaz XC z parametrem " + processName + " " + fileName);
            
            UserInterface.CreateProcessFromDisc(processName, fileName);
        }

        public void HLTOrder()
        {
            Console.WriteLine("Rozkaz HLT");
            Scheduler.GetInstance.RemoveProcess(Scheduler.GetInstance.GetRunningPCB());

            if (Scheduler.GetInstance.PriorityAlgorithm() != null)
            {
                Scheduler.GetInstance.PriorityAlgorithm().State = ProcessState.Running;
                Scheduler.GetInstance.RevriteRegistersToCPU();
            }
                
            else Console.WriteLine("Brak procesów do wykonywania!");
        }
    }
}

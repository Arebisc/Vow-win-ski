using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vow_win_ski.Memory
{
    public class Memory
    {
        private ExchangeFile _exchangeFile;
        private FifoQueue _fifoQueue;
        private PhysicalMemory _physicalMemory;
        private FreeFramesList _freeFramesList;
        private ProcessPages _processPages;

        public Memory()
        {
        }
    }
}

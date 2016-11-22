using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vow_win_ski.MemoryModule
{
    public class Memory
    {
        private ExchangeFile _exchangeFile;
        private FifoQueue _fifoQueue;
        private PhysicalMemory _physicalMemory;
        private FreeFramesList _freeFramesList;
        public List<ProcessPages> ProcessPages;

        public Memory()
        {
            _exchangeFile = new ExchangeFile();
            _fifoQueue=new FifoQueue();
            _physicalMemory = new PhysicalMemory();
            _freeFramesList=new FreeFramesList(32);
            ProcessPages = new List<ProcessPages>();
        }

        private void fillpage(int id, int number)
        {
            _freeFramesList.AddToList(3);
            _freeFramesList.RemoveFromList(1);
            _physicalMemory.ShowMemory();
        }
    }
}

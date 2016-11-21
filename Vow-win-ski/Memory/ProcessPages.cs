using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vow_win_ski.Memory
{
    public class ProcessPages
    {
        public int Id;
        private List<Page> TakenPages;
        private Memory _memory;

        public ProcessPages(ref Memory memory)
        {
            _memory = memory;
        }
        public void AddPage(Page page)
        {
            TakenPages.Add(page);
        }

        public void ChangeByte(int index, char data)
        {
            double page = Math.Ceiling((double)index/16);
            TakenPages[(int) page].GetFrameNumber();
        }
    }
}

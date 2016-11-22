using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vow_win_ski.MemoryModule
{
    public class ProcessPages
    {
        public int Id;
        private List<Page> TakenPages;

        public delegate void EmptyPageDelegate(int id,int number);
        public EmptyPageDelegate EmptyPage;

        public ProcessPages()
        {
            TakenPages=new List<Page>();
            AddPage(new Page());
        }

        public void AddPage(Page page)
        {
            TakenPages.Add(page);
        }

        public char ReadByte(int index)
        {
            return '0';
        }

        public void ChangeByte(int index, char data)
        {
            int page = (int)Math.Ceiling((double)index/16);
            if (!TakenPages[page].VaildInVaild)
            {
                EmptyPage(Id,page);
            }
        }
    }
}

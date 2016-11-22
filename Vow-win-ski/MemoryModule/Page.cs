using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vow_win_ski.MemoryModule
{
    public class Page
    {
        private int _frameIndex;
        public bool VaildInVaild;
        public bool IsModified;

        public void SetNumber(int number)
        {
            _frameIndex = number;
            VaildInVaild = false;
        }

        public int GetFrameNumber()
        {
            return _frameIndex;
        }
    }
}

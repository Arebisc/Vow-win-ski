﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vow_win_ski.Memory
{
    public class Page
    {
        private int _frameIndex;
        public bool VaildInVaild;
        public bool IsModified;

        public void SetNumber(int number)
        {
            _frameIndex = number;
            VaildInVaild = true;
        }

        public int GetFrameNumber()
        {
            return _frameIndex;
        }
    }
}

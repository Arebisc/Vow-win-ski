﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vow_win_ski.Memory
{
    public class FrameData
    {
        public int Id;
        public int FrameNumber;
    }
   public class FifoQueue
   {
        private List<FrameData> _queue;

       public void AddFrame(FrameData data)
       {
           _queue.Add(data);
       }

       public FrameData RemoveFrame()
       {
           FrameData removeData = _queue[0];
           _queue.RemoveAt(0);
           return removeData;
       }

       public void RemoveChoosenProcess(int id)
       {
           _queue = _queue
                .Select(x => x)
                .Where(x => x.Id != id)
                .ToList();
       }
   }
}

﻿using System.Collections.Generic;
using System.Linq;

namespace Vow_win_ski.MemoryModule
{
    public class FrameData
    {
        public int Id;
        public int FrameNumber;
    }
   public class FifoQueue
   {
        private List<FrameData> _queue;
       public int Size;


       public FifoQueue()
       {
           _queue = new List<FrameData>();
           Size = 0;
       }
       public void AddFrame(FrameData data)
       {
           _queue.Add(data);
           Size = _queue.Count;
       }

       public FrameData RemoveFrame()
       {
           FrameData removeData = _queue[0];
           _queue.RemoveAt(0);
           Size = _queue.Count;
           return removeData;
       }

       public void RemoveChoosenProcess(int id)
       {
           _queue = _queue
                .Select(x => x)
                .Where(x => x.Id != id)
                .ToList();
           Size = _queue.Count;
       }
   }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vow_win_ski.Memory
{
   public class FreeFramesList
   {
       private int _freeFramesCount;
       private List<int> _freeFrames;

       public FreeFramesList(int FramesCount)
       {
           _freeFramesCount = FramesCount;
       }
       public void RemoveFromList(int pageCount)
       {
           if (_freeFramesCount >= pageCount)
           {
               _freeFramesCount -= pageCount;
               _freeFrames = _freeFrames
                   .Select(x => x)
                   .Skip(pageCount)
                   .ToList();
           }
       }

       public void AddToList(int frameNumber)
       {
           _freeFrames.Add(frameNumber);
       }
   }
}

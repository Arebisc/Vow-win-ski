using System.Collections.Generic;
using System.Linq;

namespace Vow_win_ski.MemoryModule
{
   public class FreeFramesList
   {
       public int FreeFramesCount;
       private List<int> _freeFrames;

       public FreeFramesList(int framesCount)
       {
           FreeFramesCount = framesCount;
            for(int i=0;i<framesCount;i++)
                _freeFrames.Add(i);
       }
       public int RemoveFromList()
       {
               int number = _freeFrames[0];
               FreeFramesCount -= 1;
               _freeFrames = _freeFrames
                   .Select(x => x)
                   .Skip(1)
                   .ToList();
               return number;
       }

       public void AddToList(int frameNumber)
       {
           _freeFrames.Add(frameNumber);
       }
   }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace Vow_win_ski.MemoryModule
{
    public class ProcessPages
    {
        public int Id;
        public int PagesCount;
        private List<Page> TakenPages;

        public delegate char GetCharDelegate(int id,int number);
        public GetCharDelegate GetChar;

        public delegate void ChangeByteDelegate(int id, int number, char data);
        public ChangeByteDelegate ChangeByteDel;

        public delegate void UploadChangesDelegate(int id, int frameNumber, int pageNumber);
        public UploadChangesDelegate UploadChangesDel;


        public ProcessPages(int id,int framesCount)
        {
            Id = id;
            PagesCount = framesCount;
            TakenPages=new List<Page>();
            for(int i=0;i<framesCount;i++)
            TakenPages.Add(new Page());
        }

        public void AddFrame(int pageNumber,int frameNumber)
        {
            TakenPages[pageNumber].SetNumber(frameNumber);
            TakenPages[pageNumber].VaildInVaild = true;
            TakenPages[pageNumber].IsModified = false;
        }

        public void RemoveFrame(int frameNumber)
        {
            foreach (Page page in TakenPages)
            {
                if (page.GetFrameNumber() == frameNumber)
                {
                    page.VaildInVaild = false;
                    if (page.IsModified)
                    {
                        UploadChangesDel(Id, TakenPages.IndexOf(page), frameNumber);
                    }
                }
            }
        }

        public bool IsPageInMemory(int pageNumber)
        {
            return TakenPages[pageNumber].VaildInVaild;
        }

        public int[] ReadFrameNumbers()
        {
            var frames = TakenPages.Where(x => x.VaildInVaild).Select(x => x.GetFrameNumber()).ToArray();
            return frames;
        }

        public int ReadFrameNumber(int pageNumber)
        {
            int frameNumber = TakenPages[pageNumber].GetFrameNumber();
            return frameNumber;
        }

        public char ReadByte(int index)
        {
            char Byte = GetChar(Id, index);
            return Byte;
        }

        public void ChangeByte(int index, char data)
        {
            ChangeByteDel(Id, index, data);
        }
        
        public void SetModified(int pageNumber)
        {
            TakenPages[pageNumber].IsModified = true;
        }
    }
}

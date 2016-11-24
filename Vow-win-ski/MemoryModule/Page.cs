namespace Vow_win_ski.MemoryModule
{
    public class Page
    {
        private int _frameIndex;
        public bool VaildInVaild;
        public bool IsModified;


        public Page()
        {
            VaildInVaild = false;
            IsModified = false;
        }
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

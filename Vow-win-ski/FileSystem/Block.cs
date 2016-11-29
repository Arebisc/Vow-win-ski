using System;

namespace FileSystem.FileSystem
{
    class Block
    {
        private readonly int _blockSize;
        public byte[] BlockData { get; set; }

        public Block()
        {
            _blockSize = 32;
            BlockData = new byte[_blockSize];
            SetBlank();
        }

        /// <summary>
        /// Fills block with maximum value (255)
        /// </summary>
        public void SetBlank()
        {
            for (int i = 0; i < BlockData.Length; i++)
            {
                BlockData[i] = 255;
            }
        }

    }
}

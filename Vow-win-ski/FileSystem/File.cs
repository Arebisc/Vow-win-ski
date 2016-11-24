using System;

namespace FileSystem.FileSystem
{
    class File
    {
        public string FileName { get; private set; }
        public int FileSize { get; private set; }
        public DateTime CreationDateTime { get; private set; }
        public int DataBlockPointer { get; private set; }

   
        public File(string name, int size, int dataBlockPointer)
        {
            FileName = name;
            CreationDateTime = DateTime.Now;
            FileSize = size;
            DataBlockPointer = dataBlockPointer;
        }

        public void Append(int size)
        {
            FileSize += size;
        }

    }
}

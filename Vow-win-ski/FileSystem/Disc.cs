using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*
 path = root\xd\k1
 BeginIn = root\xd\
 foldertodelete = k1
 */

namespace Vow_win_ski.FileSystem
{
    class Disc
    {
        private static volatile Disc _instance;

        private readonly int _blockSize = 32;
        private static int _numberOfBlocks;
        private Block[] _blocks;
        private BitArray _occupiedBlocksArray;
        private Folder _rootFolder;

        /// <summary>
        /// Use this if no parameter is provided
        /// </summary>
        public static void InitDisc()
        {
            _numberOfBlocks = 32;
            _instance = new Disc();
        }

        /// <summary>
        /// Use this if a parameter is provided
        /// </summary>
        /// <param name="numberOfBlocks"></param>
        public static void InitDisc(int numberOfBlocks)
        {
            _numberOfBlocks = numberOfBlocks;
            _instance = new Disc(_numberOfBlocks);
        }

        public static Disc GetDisc => _instance;

        private Disc()
        {
            Console.WriteLine("Creating disc with the default blocks number (32).");
            _numberOfBlocks = 32;
            _blocks = new Block[_numberOfBlocks].Select(h => new Block()).ToArray(); //initialize elements in array
            _occupiedBlocksArray = new BitArray(_numberOfBlocks);
            _rootFolder = new Folder();
        }

        private Disc(int numberOfblocks)
        {
            if (numberOfblocks < 2 || numberOfblocks >= 255)
            {
                _numberOfBlocks = 32;
                Console.WriteLine("Disc parameter was invalid. Creating disc with the default blocks number (32).");
            }
            else
            {
                Console.WriteLine("Creating disc with custom block numer: " + numberOfblocks);
                _numberOfBlocks = numberOfblocks;
            }
            _blocks = new Block[_numberOfBlocks].Select(h => new Block()).ToArray(); //initialize elements in array
            _occupiedBlocksArray = new BitArray(_numberOfBlocks);
            _rootFolder = new Folder();
        }

        //=================================================================================================================================
      
        public void ShowDirectory()
        {
            Console.WriteLine("Directory of root\\\n");
            if (_rootFolder.FilesInDirectory.Count == 0)
            {
                Console.WriteLine("This directory is empty.");
                return;
            }
            foreach (var file in _rootFolder.FilesInDirectory)
            {
                Console.WriteLine(file.FileName.PadRight(17) + file.FileSize + " B\t" + file.CreationDateTime);
            }
        }

        //=================================================================================================================================

        public void DisplayDataBlocks()
        {
            Console.WriteLine("\nNumber of blocks: " + _numberOfBlocks + "\tBlock size: " + _blockSize + " B");
            Console.WriteLine("Total Space: " + _numberOfBlocks*_blockSize + " B");
            Console.WriteLine("Free space: " + (from bool bit in _occupiedBlocksArray where !bit select bit).Count()*32 + " B\tOccupied space: " + (from bool bit in _occupiedBlocksArray where bit select bit).Count()*32 + " B");
            Console.WriteLine("Free blocks: " + (from bool bit in _occupiedBlocksArray where !bit select bit).Count() + "\t\tOccupied Blocks: " + (from bool bit in _occupiedBlocksArray where bit select bit).Count() + "\n");

            for (int i = 0; i < _numberOfBlocks; i++)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("[Block nr "+i+"]: ");
                Console.Write(_occupiedBlocksArray[i] ? "Occupied\n" : "Free\n");
                Console.ForegroundColor = ConsoleColor.Gray;

                foreach (byte t in _blocks[i].BlockData)
                {
                    Console.Write(t.ToString().PadRight(5));
                }
                if (i%32 == 0 && i != 0)
                {
                    Console.Write("Press any key...");
                    Console.ReadKey();
                    Console.WriteLine();
                }
            }
        }
            
        //=================================================================================================================================
        public void CreateFile(string path, string data)
        {
            string nameForNewFile = path;

            if (data.Any(d => d > 255))
            {
                Console.WriteLine("Error: Data contains illegal character");
                return;
            }

            int newFileSize = data.Length;

            if(_rootFolder.FilesInDirectory.Any(file => file.FileName == nameForNewFile))
            { 
                Console.WriteLine("Error: File \"" + path + "\" already exists");
                return;
            }

            if (nameForNewFile.Length == 0 || nameForNewFile.Length > 15 || nameForNewFile.Contains("\\") || nameForNewFile.Contains("/") || nameForNewFile.Contains("\t") || nameForNewFile == "root")
            {
                Console.WriteLine("Error: Invalid File name");
                Console.WriteLine("File name must be between 1 and 15 characters long and cannot contain:");
                Console.WriteLine("'\t' '/' '\\' 'root'");
                return;
            }

            int blocksneeded = newFileSize / 32 + 1;
            if (newFileSize%32 > 0) blocksneeded++;
            if (blocksneeded > 33)
            {
                Console.WriteLine("Error: Exceeded maximum file size");
                Console.WriteLine("Required size: " + newFileSize +" B\tMaximum size allowed: "+ _blockSize*32 + " B");
                return;
            }

            int freeBlocks = (from bool bit in _occupiedBlocksArray where !bit select bit).Count();
            if (blocksneeded > freeBlocks)
            {
                Console.WriteLine("Not enough to free blocks to save new file");
                Console.WriteLine("Required: " + blocksneeded + "\tAvailable: " + freeBlocks);
                return;
            }

            int[] blocksToBeOccupied = new int[blocksneeded];
            for (int i = 0; i < blocksneeded; i++)
            {
                for (int j = 0; j < _numberOfBlocks; j++)
                {
                    if (_occupiedBlocksArray[j] == false)
                    {
                        blocksToBeOccupied[i] = j;
                        _occupiedBlocksArray[j] = true;
                        break;
                    }
                }
            }

            //fills index block
            _blocks[blocksToBeOccupied[0]].SetBlank();
            for (int i = 0, j = 1; j < blocksneeded; i++, j++)
            {
                _blocks[blocksToBeOccupied[0]].BlockData[i] = Convert.ToByte(blocksToBeOccupied[j]);
            }

            //fills data blocks
            int inBlockPointer = 0, blockPointer = 1;
            while (data != "")
            {
                if (inBlockPointer == 32)
                {
                    inBlockPointer = 0;
                    blockPointer++;
                }
                _blocks[blocksToBeOccupied[blockPointer]].BlockData[inBlockPointer] = Convert.ToByte(data[0]);
                inBlockPointer++;

                data = data.Substring(1);
            }

            _rootFolder.FilesInDirectory.Add(new File(nameForNewFile, newFileSize, blocksToBeOccupied[0]));
            Console.WriteLine("New File \"" + nameForNewFile + "\" has been created in root\\");
        }
        //=================================================================================================================================

        public string GetFileData(string path)
        {
            string fileToOpen = path;

            int indexBlock = _rootFolder.FilesInDirectory.Single(x => x.FileName == fileToOpen).DataBlockPointer;
            int length = _rootFolder.FilesInDirectory.Single(x => x.FileName == fileToOpen).FileSize;
            List<byte> dataBlocksToRead = new List<byte>();

            /*for (var i = 0; _blocks[indexBlock].BlockData[i] != 255; i++)
            {
                dataBlocksToRead.Add(_blocks[indexBlock].BlockData[i]);
            }*/
            dataBlocksToRead.AddRange(_blocks[indexBlock].BlockData);

            List<byte> dataList = new List<byte>();
            int inBlockPointer = 0, blockPointer = 0;
            while (length != 0)
            {
                if (inBlockPointer == 32)
                {
                    inBlockPointer = 0;
                    blockPointer++;
                }
                dataList.Add(_blocks[dataBlocksToRead[blockPointer]].BlockData[inBlockPointer]);
                inBlockPointer++;

                length--;
            }
            return Encoding.ASCII.GetString(dataList.ToArray());
        }
        //=================================================================================================================================

        public void DeleteFile(string path)
        {            
            string filenameToDelete = path;
            if (_rootFolder.FilesInDirectory.All(x => x.FileName != filenameToDelete))
            {
                Console.WriteLine("Error: Specified file \"" + filenameToDelete + "\" does not exist in root\\");
                return;
            }
            File fileToDelete = _rootFolder.FilesInDirectory.Single(x => x.FileName == filenameToDelete);

            _occupiedBlocksArray[fileToDelete.DataBlockPointer] = false;
            foreach (var datablocknr in _blocks[fileToDelete.DataBlockPointer].BlockData)
            {
                if (datablocknr == 255) break;
                _occupiedBlocksArray[datablocknr] = false;
            }
            _rootFolder.FilesInDirectory.RemoveAll(x => x.FileName == filenameToDelete);

            Console.WriteLine("File \"" + filenameToDelete + "\" has been deleted");
        }
        //=================================================================================================================================

        public void AppendToFile(string path, string data)
        {
            string filenameToAppend = path;

            if (_rootFolder.FilesInDirectory.All(x => x.FileName != filenameToAppend))
            {
                Console.WriteLine("Error: Specified file \"" + filenameToAppend + "\" does not exist in root\\");
                return;
            }
            File fileToAppend = _rootFolder.FilesInDirectory.Single(x => x.FileName == filenameToAppend);

            if (data.Any(d => d > 255))
            {
                Console.WriteLine("Error: Data contains illegal character");
                return;
            }

            int allBytesToAppend = data.Length;
            //                              space available in the last block
            int bytesNeeded = data.Length - (32 - fileToAppend.FileSize%32);
            if (bytesNeeded < 0) bytesNeeded = 0;
            int blocksNeeded = bytesNeeded/32;
            if (bytesNeeded%32 > 0) blocksNeeded++;
            int freeBlocks = (from bool bit in _occupiedBlocksArray where !bit select bit).Count();
            if (blocksNeeded > freeBlocks)
            {
                Console.WriteLine("Not enough to free blocks to save new data file");
                Console.WriteLine("Required: " + blocksNeeded + "\tAvailable: " + freeBlocks);
                return;
            }

            int freePointers = 32;
            for (var i = 0; _blocks[fileToAppend.DataBlockPointer].BlockData[i] != 255; i++)
            {
                freePointers--;
            }

            if (blocksNeeded > freePointers)
            {
                Console.WriteLine("Error: Exceeded maximum file size");
                Console.WriteLine("Required size: " + fileToAppend.FileSize + data.Length + " B\tMaximum size allowed: " + _blockSize * 32 + " B");
                return;
            }

            int[] blocksToBeOccupied = new int[blocksNeeded];
            for (int i = 0; i < blocksNeeded; i++)
            {
                for (int j = 0; j < _numberOfBlocks; j++)
                {
                    if (_occupiedBlocksArray[j] == false)
                    {
                        blocksToBeOccupied[i] = j;
                        _occupiedBlocksArray[j] = true;
                        break;
                    }
                }
            }

            //fills index block
            int inBlockPointer = 32 - freePointers;
            foreach (var b in blocksToBeOccupied)
            {
                _blocks[fileToAppend.DataBlockPointer].BlockData[inBlockPointer] = Convert.ToByte(b);
            }

            //fills last data block to the end
            if (fileToAppend.FileSize%32 != 0)
            {
                inBlockPointer = fileToAppend.FileSize%32;
                while (inBlockPointer < 32 && data.Length > 0)
                {
                    _blocks[_blocks[fileToAppend.DataBlockPointer].BlockData[31 - freePointers]].BlockData[inBlockPointer]
                        = Convert.ToByte(data[0]);
                    data = data.Substring(1);
                    inBlockPointer++;
                }
            }
            //fills new data blocks
            inBlockPointer = 0;
            int blockPointer = 0;
            while (data != "")
            {
                if (inBlockPointer == 32)
                {
                    inBlockPointer = 0;
                    blockPointer++;
                }
                _blocks[blocksToBeOccupied[blockPointer]].BlockData[inBlockPointer] = Convert.ToByte(data[0]);
                inBlockPointer++;

                data = data.Substring(1);
            }
            fileToAppend.Append(allBytesToAppend);
            Console.WriteLine(allBytesToAppend + " B have been appended to file \"" + filenameToAppend + "\"");
        }

        //=================================================================================================================================
    }
}
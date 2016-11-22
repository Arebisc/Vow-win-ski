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

namespace FileSystem.FileSystem
{
    class Disc
    {
        private readonly int _blockSize = 32;
        private readonly int _numberOfBlocks;
        private Block[] _blocks;
        private BitArray _occupiedBlocksArray;
        private Folder _rootFolder;
        public Folder CurrentFolder { get; private set; }

        public Disc()
        {
            Console.WriteLine("Creating disc with the default blocks number (32).");
            _numberOfBlocks = 32;
            _blocks = new Block[_numberOfBlocks].Select(h => new Block()).ToArray(); //initialize elements in array
            _occupiedBlocksArray = new BitArray(_numberOfBlocks);
            _rootFolder = new Folder();
            CurrentFolder = _rootFolder;
        }

        public Disc(int numberOfblocks)
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
            CurrentFolder = _rootFolder;
        }

        //=================================================================================================================================

        private Folder GetDirectory(string path)
        {
            Folder tempFolder;
            string[] pathfolders = path.Split('\\');
            pathfolders = pathfolders.Where(x => !string.IsNullOrEmpty(x)).ToArray();
            if (pathfolders.Length == 0)
            {
                //no path provided
                return CurrentFolder;
            }
            if (pathfolders[0] == "root")
            {
                //path is absolute
                pathfolders = pathfolders.Skip(1).ToArray();
                tempFolder = _rootFolder;
            }
            else
            {
                //path is relative
                tempFolder = CurrentFolder;
            }

            foreach (var folderName in pathfolders)
            {
                bool exists = false;
                foreach (var folderInDirectory in tempFolder.FoldersInDirectory)
                {
                    if (folderInDirectory.FolderName == folderName)
                    {
                        tempFolder = folderInDirectory;
                        exists = true;
                    }
                }
                if (!exists)
                {
                    Console.WriteLine("Error: Specified path: \"" + path + "\" does not exist");
                    return null;
                }
            }
            return tempFolder;
        }
        //=================================================================================================================================

        public void ChangeDirectory(string path)
        {
            if (GetDirectory(path) == null) return;
            CurrentFolder = GetDirectory(path);
        }
        //=================================================================================================================================

        public void ShowDirectory(string path)
        {
            if (GetDirectory(path) == null) return;
            Console.WriteLine("Directory of " + GetDirectory(path).PathToFolder + "\n");
            if (GetDirectory(path).FoldersInDirectory.Count + GetDirectory(path).FilesInDirectory.Count == 0)
            {
                Console.WriteLine("This directory is empty.");
                return;
            }
            foreach (var folder in GetDirectory(path).FoldersInDirectory)
            {
                Console.WriteLine(folder.FolderName.PadRight(17) + "<DIR>");
            }
            foreach (var file in GetDirectory(path).FilesInDirectory)
            {
                Console.WriteLine(file.FileName.PadRight(17) + file.FileSize + " B\t" + file.CreationDateTime);
            }
        }
        //=================================================================================================================================

        public void CreateFolder(string path)
        {
            if (path.EndsWith("\\"))
                path = path.Reverse().Skip(1).Reverse().ToString();
            // ReSharper disable once StringLastIndexOfIsCultureSpecific.1
            string nameForNewFolder = path.Substring(path.LastIndexOf("\\") + 1);
            string beginIn = path.Substring(0, path.Length - nameForNewFolder.Length);
            if (GetDirectory(beginIn) == null) return;

            foreach (var folder in GetDirectory(beginIn).FoldersInDirectory)
            {
                if (folder.FolderName == nameForNewFolder)
                {
                    Console.WriteLine("Error: Directory \"" + path + "\" already exists");
                    return;
                }
            }
            if(nameForNewFolder.Length == 0 || nameForNewFolder.Length > 15 || nameForNewFolder.Contains("\\") || nameForNewFolder.Contains("/") || nameForNewFolder.Contains(" ") || nameForNewFolder == "root")
            {
                Console.WriteLine("Error: Invalid Folder name");
                Console.WriteLine("Folder name must be between 1 and 15 characters long and cannot contain:");
                Console.WriteLine("'\t' '/' '\\' 'root'");
                return;
            }
            GetDirectory(beginIn).FoldersInDirectory.Add(new Folder(GetDirectory(beginIn).PathToFolder, nameForNewFolder));
            Console.WriteLine("Directory \"" + nameForNewFolder + "\" has been created in " + GetDirectory(beginIn).PathToFolder);
        }
        //=================================================================================================================================

        public void DeleteFolder(string path)
        {
            path = GetDirectory(path).PathToFolder;
            path = path.Remove(path.Length - 1);
            // ReSharper disable once StringLastIndexOfIsCultureSpecific.1
            string folderToDelete = path.Substring(path.LastIndexOf("\\") + 1);
            string beginIn = path.Substring(0, path.Length - folderToDelete.Length);
            if (GetDirectory(path) == _rootFolder)
            {
                Console.WriteLine("Error: root directory cannot be deleted");
                return;
            }
            if (GetDirectory(path) == null) return;

            if (CurrentFolder.PathToFolder.Length >= path.Length)
            {
                if (GetDirectory(path).PathToFolder ==
                    CurrentFolder.PathToFolder.Substring(0, GetDirectory(path).PathToFolder.Length))
                {
                    ChangeDirectory(beginIn);
                }
            }

            while (GetDirectory(path).FoldersInDirectory.Count != 0)
            {
                DeleteFolder(GetDirectory(path).FoldersInDirectory[0].PathToFolder);
            }

            while (GetDirectory(path).FilesInDirectory.Count != 0)
            {
                DeleteFile(GetDirectory(path).PathToFolder + GetDirectory(path).FilesInDirectory[0].FileName);
            }

            for (int i = 0; i < GetDirectory(beginIn).FoldersInDirectory.Count; i++)
            {
                if (GetDirectory(beginIn).FoldersInDirectory[i].FolderName == folderToDelete)
                {
                    GetDirectory(beginIn).FoldersInDirectory.RemoveAt(i);
                    break;
                }
            }
            Console.WriteLine("Directory \"" + GetDirectory(beginIn).PathToFolder + folderToDelete + "\" has been deleted");
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
            if (path.EndsWith("\\"))
                path = path.Reverse().Skip(1).Reverse().ToString();
            // ReSharper disable once StringLastIndexOfIsCultureSpecific.1
            string nameForNewFile = path.Substring(path.LastIndexOf("\\") + 1);
            string beginIn = path.Substring(0, path.Length - nameForNewFile.Length);
            if (GetDirectory(beginIn) == null) return;

            foreach (var d in data)
            {
                if (d > 255)
                {
                    Console.WriteLine("Error: Data contains illegal character");
                    return;
                }
            }

            int newFileSize = data.Length;
            foreach (var file in GetDirectory(beginIn).FilesInDirectory)
            {
                if (file.FileName == nameForNewFile)
                {
                    Console.WriteLine("Error: File \"" + path + "\" already exists");
                    return;
                }
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

            GetDirectory(beginIn).FilesInDirectory.Add(new File(nameForNewFile, newFileSize, blocksToBeOccupied[0]));
            Console.WriteLine("New File \"" + nameForNewFile + "\" has been created in " + GetDirectory(beginIn).PathToFolder);
        }
        //=================================================================================================================================

        public string GetFileData(string path)
        {
            // ReSharper disable once StringLastIndexOfIsCultureSpecific.1
            string fileToOpen = path.Substring(path.LastIndexOf("\\") + 1);
            string beginIn = path.Substring(0, path.Length - fileToOpen.Length);
            if (GetDirectory(beginIn) == null) return null;

            int indexBlock = GetDirectory(beginIn).FilesInDirectory.Single(x => x.FileName == fileToOpen).DataBlockPointer;
            int length = GetDirectory(beginIn).FilesInDirectory.Single(x => x.FileName == fileToOpen).FileSize;
            List<int> dataBlocksToRead = new List<int>();

            for (var i = 0; _blocks[indexBlock].BlockData[i] != 255; i++)
            {
                dataBlocksToRead.Add(_blocks[indexBlock].BlockData[i]);
            }

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
            // ReSharper disable once StringLastIndexOfIsCultureSpecific.1
            string filenameToDelete = path.Substring(path.LastIndexOf("\\") + 1);
            string beginIn = path.Substring(0, path.Length - filenameToDelete.Length);

            if (GetDirectory(beginIn) == null) return;

            File fileToDelete = null;
            foreach (File file in GetDirectory(beginIn).FilesInDirectory)
            {
                if (file.FileName == filenameToDelete)
                {
                    fileToDelete = file;
                    break;
                }
            }
            if (fileToDelete == null)
            {
                Console.WriteLine("Error: Specified file " + filenameToDelete + " does not exit in " + beginIn);
                return;
            }

            _occupiedBlocksArray[fileToDelete.DataBlockPointer] = false;
            foreach (var datablocknr in _blocks[fileToDelete.DataBlockPointer].BlockData)
            {
                if (datablocknr == 255) break;
                _occupiedBlocksArray[datablocknr] = false;
            }

            for (int i = 0; i < GetDirectory(beginIn).FilesInDirectory.Count; i++)
            {
                if (GetDirectory(beginIn).FilesInDirectory[i].FileName == filenameToDelete)
                {
                    GetDirectory(beginIn).FilesInDirectory.RemoveAt(i);
                    break;
                }
            }
            Console.WriteLine("File \"" + GetDirectory(beginIn).PathToFolder + filenameToDelete + "\" has been deleted");
        }
        //=================================================================================================================================
        private void ShowFiles(File file, int level)
        {
            for (int i = 0; i < level; i++)
            {
                Console.Write(i + 1 == level ? "└───" : "    ");
            }
            Console.WriteLine(file.FileName);
        }

        private void ShowFolders(Folder folder, int level)
        {
            for (int i = 0; i < level; i++)
            {
                Console.Write(i + 1 == level ? "└───" : "    ");
            }

            Console.WriteLine(folder.FolderName + "\\");
            foreach (var fol in folder.FoldersInDirectory)
            {
                ShowFolders(fol, level + 1);
            }
            foreach (var fil in folder.FilesInDirectory)
            {
                ShowFiles(fil, level + 1);
            }
        }

        public void ShowTree(string path)
        {
            if (GetDirectory(path) == null) return;

            int level = 0;
            Console.WriteLine(GetDirectory(path).FolderName + "\\");
            foreach (var folder in GetDirectory(path).FoldersInDirectory)
            {
                ShowFolders(folder, level + 1);
            }
            foreach (var file in GetDirectory(path).FilesInDirectory)
            {
                ShowFiles(file, level + 1);
            }
        }
        //=================================================================================================================================

        public void AppendToFile(string path, string data)
        {
            // ReSharper disable once StringLastIndexOfIsCultureSpecific.1
            string filenameToAppend = path.Substring(path.LastIndexOf("\\") + 1);
            string beginIn = path.Substring(0, path.Length - filenameToAppend.Length);

            if (GetDirectory(beginIn) == null) return;

            File fileToAppend = null;
            foreach (File file in GetDirectory(beginIn).FilesInDirectory)
            {
                if (file.FileName == filenameToAppend)
                {
                    fileToAppend = file;
                    break;
                }
            }
            if (fileToAppend == null)
            {
                Console.WriteLine("Error: Specified file " + filenameToAppend + " does not exit in " + beginIn);
                return;
            }

            foreach (var d in data)
            {
                if (d > 255)
                {
                    Console.WriteLine("Error: Data contains illegal character");
                    return;
                }
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
            Console.WriteLine(allBytesToAppend + " B have been appended to file \"" + GetDirectory(beginIn).PathToFolder + filenameToAppend + "\"");
        }

        //=================================================================================================================================
    }
}
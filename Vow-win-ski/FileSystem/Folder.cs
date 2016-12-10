using System.Collections.Generic;

namespace Vow_win_ski.FileSystem
{
    class Folder
    {
        public string FolderName { get; private set; }
        //public List<Folder> FoldersInDirectory { get; private set; }
        public List<File> FilesInDirectory { get; private set; }
        //public string PathToFolder { get; }

        /// <summary>
        /// Creates root folder
        /// </summary>
        public Folder()
        {
            FolderName = "root";
            //PathToFolder = "root\\";
            //FoldersInDirectory = new List<Folder>();
            FilesInDirectory = new List<File>();
        }

        /*
        public Folder(string path, string name)
        {
            FolderName = name;
            PathToFolder = path + name + "\\";
            FoldersInDirectory = new List<Folder>();
            FilesInDirectory = new List<File>();
        }*/

    }
}

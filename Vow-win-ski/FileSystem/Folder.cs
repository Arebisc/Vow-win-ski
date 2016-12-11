using System.Collections.Generic;

namespace Vow_win_ski.FileSystem
{
    class Folder
    {
        public string FolderName { get; private set; }
        public List<File> FilesInDirectory { get; private set; }

        /// <summary>
        /// Creates root folder
        /// </summary>
        public Folder()
        {
            FolderName = "root";
            FilesInDirectory = new List<File>();
        }
    }
}

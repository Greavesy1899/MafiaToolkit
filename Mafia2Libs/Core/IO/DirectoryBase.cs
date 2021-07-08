using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Core.IO
{
    public class DirectoryBase
    {
        protected DirectoryInfo directory;
        private List<FileBase> loadedFiles;

        public DirectoryBase(DirectoryInfo info)
        {
            directory = info;
            loadedFiles = new List<FileBase>();
        }

        public DirectoryInfo GetDirectoryInfo()
        {
            return directory;
        }

        public void AddLoadedFile(FileBase file)
        {
            loadedFiles.Add(file);
            file.SetParent(this);
        }
     
        public List<T> GetFilesFromDirectory<T>()
        {
            List<T> collectedFiles = new List<T>();

            foreach (var file in loadedFiles)
            {
                if(file is T)
                {
                    T Data = (T)Convert.ChangeType(file, typeof(T));
                    collectedFiles.Add(Data);
                }
            }

            return collectedFiles;
        }

        public void Delete()
        {
            if(directory != null)
            {
                if(directory.Exists)
                {
                    directory.Delete(true);
                }
            }
        }
    }
}

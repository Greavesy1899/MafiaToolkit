using System;
using System.Diagnostics;
using System.IO;
using Utils.Extensions;

namespace Core.IO
{
    public class FileBase
    {
        protected FileInfo file;
        protected DirectoryBase parentDirectory;

        private string extension;

        public FileBase(FileInfo info)
        {
            file = info;
            extension = info.Extension.Replace(".", "").ToUpper();
        }

        public string GetFileSizeAsString()
        {
            return file.CalculateFileSize();
        }

        public string GetLastTimeWrite()
        {
            return file.LastWriteTime.ToString();
        }

        public string GetExtensionLower()
        {
            return GetExtensionUpper().ToLower();
        }

        public FileInfo GetUnderlyingFileInfo()
        {
            return file;
        }

        public string GetName()
        {
            return file.Name;
        }

        public string GetNameWithoutExtension()
        {
            var size = extension.Length + 1;
            return file.Name.Remove(file.Name.Length - size, size);
        }

        public virtual string GetExtensionUpper()
        {
            return extension;
        }

        public void SetParent(DirectoryBase info)
        {
            parentDirectory = info;
        }

        public virtual bool Open()
        {
            return (Process.Start(file.FullName) != null ? true : false);
        }

        public virtual void Save()
        {
            throw new NotImplementedException("Override the base class for this method to function");
        }

        public virtual void Delete()
        {
            if(file != null)
            {
                if(file.Exists)
                {
                    file.Delete();
                }
            }
        }
    }
}

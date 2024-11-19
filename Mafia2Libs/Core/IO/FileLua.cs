using System;
using System.IO;
using Utils.Lua;

namespace Core.IO
{
    public class FileLua : FileBase
    {
        private string extension = string.Empty;

        public FileLua(FileInfo info) : base(info)
        {
            extension = info.Extension;
        }

        private bool IsBytecode()
        {
            using (BinaryReader reader = new BinaryReader(File.Open(file.FullName, FileMode.Open)))
            {
                var magic = reader.ReadInt32();
                if (magic == 1635077147)
                {
                    return true;
                }
            }

            return false;
        }

        public void TryDecompileBytecode()
        {
            if(IsBytecode())
            {
                LuaHelper.ReadFile(file);
            }
        }

        public override bool Open()
        {
            if(IsBytecode())
            {
                LuaHelper.ReadFile(file);
            }
            else
            {
                // base class *should* open as process
                return base.Open();
            }

            return true;
        }

        public override void Save()
        {
            throw new NotImplementedException();
        }

        public override string GetExtensionUpper()
        {
            return extension;
        }
    }
}

using System;
using System.IO;
using System.Windows;

namespace Core.IO
{
    public class FileXlaybin : FileBase
    {
        public FileXlaybin(FileInfo info) : base(info)
        {

        }

        public override string GetExtensionUpper()
        {
            return "XLAYBIN";
        }

        public override void Save()
        {
            throw new NotImplementedException("This functionality is not supported for this filetype.");
        }

        public override bool Open()
        {
            MessageBox.Show("No support for XLAYBIN. Did you mean to press XBin?", "Toolkit", MessageBoxButton.OK);
            return false;
        }
    }
}

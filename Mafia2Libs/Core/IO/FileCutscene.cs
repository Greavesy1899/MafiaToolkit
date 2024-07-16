using Gibbed.Illusion.FileFormats.Hashing;
using Mafia2Tool.Forms;
using ResourceTypes.Cutscene;
using System;
using System.IO;

namespace Core.IO
{
    public class FileCutscene : FileBase
    {
        private CutsceneLoader Loader;

        public FileCutscene(FileInfo info) : base(info)
        {
            
        }

        public override bool Open()
        {
            //foreach (string f in Directory.EnumerateFiles(@"G:\Program Files\Mafia II\Cutscenes", "*.cut"))
            //{
            //    using (MemoryStream ms = new())
            //    {
            //        Loader = new CutsceneLoader(f);
            //        Loader.WriteToStream(ms);
            //
            //        var originalData = File.ReadAllBytes(f);
            //        var newData = ms.ToArray();
            //        var ogHash = FNV32.Hash(originalData, 0, originalData.Length);
            //        var newHash = FNV32.Hash(newData, 0, newData.Length);
            //
            //        if (ogHash != newHash)
            //        {
            //            throw new Exception("Hash mismatch.");
            //        }
            //    }
            //}

            Loader = new CutsceneLoader(file);
            CutsceneEditor editor = new CutsceneEditor(this);
            editor.Show();
            return true;
        }

        public override void Save()
        {
            throw new NotImplementedException();
        }

        public override string GetExtensionUpper()
        {
            return "CUT";
        }
        public CutsceneLoader GetCutsceneLoader()
        {
            return Loader;
        }
    }
}

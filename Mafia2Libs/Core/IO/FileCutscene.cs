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

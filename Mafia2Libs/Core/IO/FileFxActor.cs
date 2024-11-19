using System.IO;
using Toolkit.Forms;

namespace Core.IO
{
    public class FileFxActor : FileBase
    {
        public FileFxActor(FileInfo info) : base(info) { }

        public override string GetExtensionUpper()
        {
            return "FXA";
        }

        public override bool Open()
        {
            FxActorEditor ActorEditor = new FxActorEditor(GetUnderlyingFileInfo());
            return true;
        }
    }
}

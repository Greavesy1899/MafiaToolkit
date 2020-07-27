using Mafia2Tool;
using System.IO;

namespace Core.IO
{
    class FileActor : FileBase
    {
        public FileActor(FileInfo info) : base(info)
        {
        }

        public override string GetExtensionUpper()
        {
            return "ACT";
        }

        public override bool Open()
        {
            ActorEditor editor = new ActorEditor(file);
            return true;
        }
    }
}

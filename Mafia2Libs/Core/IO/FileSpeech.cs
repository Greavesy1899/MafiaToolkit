using Mafia2Tool;
using System.IO;

namespace Core.IO
{
    class FileSpeech : FileBase
    {
        public FileSpeech(FileInfo info) : base(info)
        {
        }

        public override string GetExtensionUpper()
        {
            return "SPE";
        }

        public override bool Open()
        {
            SpeechEditor editor = new SpeechEditor(file);
            return true;
        }
    }
}

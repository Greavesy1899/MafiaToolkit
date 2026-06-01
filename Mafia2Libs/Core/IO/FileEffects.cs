using Toolkit.Forms;
using System.IO;

namespace Core.IO
{
    /// <summary>
    /// File handler for ".eff" — the payload of an SDS "Effects" resource (Mafia II).
    /// Opens the chunk-tree editor.
    /// </summary>
    public class FileEffects : FileBase
    {
        public FileEffects(FileInfo info) : base(info)
        {
        }

        public override string GetExtensionUpper()
        {
            return "EFF";
        }

        public override bool Open()
        {
            EffectsEditor editor = new EffectsEditor(file);
            return true;
        }
    }
}

using System.IO;

namespace ApexSDK
{
    public class Modifier : IModifier
    {
        private ModifierType type;

        public ModifierType Type {
            get { return type; }
            set { type = value; }
        }

        public virtual void ReadFromFile(BinaryReader reader) { }
        public virtual void WriteToFile(BinaryWriter writer) { }
    }
}

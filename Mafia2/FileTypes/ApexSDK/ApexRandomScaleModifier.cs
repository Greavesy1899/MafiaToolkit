using System.IO;

namespace ApexSDK
{
    public class ApexRandomScaleModifier : IModifier
    {
        private ModifierType type;

        public ModifierType Type {
            get { return type; }
            set { type = value; }
        }

        public void ReadFromFile(BinaryReader reader)
        {

        }

        public void WriteToFile(BinaryWriter writer)
        {

        }
    }
}

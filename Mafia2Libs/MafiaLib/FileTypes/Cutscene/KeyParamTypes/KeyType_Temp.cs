using System.IO;
using Utils.Extensions;

namespace ResourceTypes.Cutscene.KeyParams
{
    public class KeyType_Temp : IKeyType
    {
        public byte[] KeyData;

        public override void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            base.ReadFromFile(stream, isBigEndian);
            KeyData = stream.ReadBytes(Size - 12);
        }

        public override string ToString()
        {
            return string.Format("NOT_REVERSED::KeyType: {0}", KeyType);
        }
    }
}

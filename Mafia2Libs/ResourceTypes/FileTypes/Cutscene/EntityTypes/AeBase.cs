using ResourceTypes.Cutscene.KeyParams;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using Utils.Extensions;

namespace ResourceTypes.Cutscene.AnimEntities
{
    public class AeBaseData
    {
        [Browsable(false)]
        public int DataType { get; set; } // We might need an enumator for this
        [Browsable(false)]
        public int Size { get; set; } // Total Size of the data. includes Size and DataType.
        [Browsable(false)]
        public int KeyDataSize { get; set; } // Size of all the keyframes? Also count and the Unk01?
        public int Unk00 { get; set; }
        public int Unk01 { get; set; }
        public int NumKeyFrames { get; set; } // Number of keyframes. Start with 0xE803 or 1000
        public IKeyType[] KeyFrames { get; set; }

        public virtual void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            DataType = stream.ReadInt32(isBigEndian);
            Size = stream.ReadInt32(isBigEndian);
            Unk00 = stream.ReadInt32(isBigEndian);
            KeyDataSize = stream.ReadInt32(isBigEndian);
            Unk01 = stream.ReadInt32(isBigEndian);
            NumKeyFrames = stream.ReadInt32(isBigEndian);

            KeyFrames = new IKeyType[NumKeyFrames];

            for (int i = 0; i < NumKeyFrames; i++)
            {
                Debug.Assert(stream.Position != stream.Length, "Reached the end to early?");

                int Header = stream.ReadInt32(isBigEndian);
                Debug.Assert(Header == 1000, "Keyframe magic did not equal 1000");
                int Size = stream.ReadInt32(isBigEndian);
                int KeyType = stream.ReadInt32(isBigEndian);
                AnimKeyParamTypes KeyParamType = (AnimKeyParamTypes)KeyType;

                IKeyType KeyParam = CutsceneKeyParamFactory.ReadAnimEntityFromFile(KeyParamType, Size, stream);
                KeyFrames[i] = KeyParam;
            }
        }

        public virtual void WriteToFile(MemoryStream stream, bool isBigEndian)
        {
            stream.Write(DataType, isBigEndian);
            stream.Write(Size, isBigEndian);
            stream.Write(Unk00, isBigEndian);
            stream.Write(KeyDataSize, isBigEndian);
            stream.Write(Unk01, isBigEndian);
            stream.Write(NumKeyFrames, isBigEndian);

            for(int i = 0; i < NumKeyFrames; i++)
            {
                using (MemoryStream KeyParamStream = new MemoryStream())
                {
                    // Get KeyParam
                    IKeyType KeyParam = KeyFrames[i];
                    KeyParamStream.Write(1000, isBigEndian); // Write the header
                    KeyParamStream.Write(KeyParam.Size, isBigEndian);
                    KeyParamStream.Write(KeyParam.KeyType, isBigEndian);
                    KeyParam.WriteToFile(KeyParamStream, isBigEndian);

                    KeyParamStream.Seek(4, SeekOrigin.Begin);
                    KeyParamStream.Write((uint)KeyParamStream.Length, isBigEndian);
                    KeyParamStream.Seek(0, SeekOrigin.End);
                    stream.Write(KeyParamStream.ToArray());
                }
            }
        }

        public override string ToString()
        {
            return string.Format("{0}", DataType);
        }
    }
}

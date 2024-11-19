using System.ComponentModel;
using System.IO;
using ResourceTypes.Cutscene.CurveParams;
using Utils.Extensions;
using Utils.Logging;

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
        public int Unk01 { get; set; }
        public ICurveParam[] KeyFrames { get; set; }
        [Browsable(false)]
        public string CutsceneName { get; set; }

        public virtual void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            DataType = stream.ReadInt32(isBigEndian);
            Size = stream.ReadInt32(isBigEndian);
            int KeyDataHeader = stream.ReadInt32(isBigEndian);

            ToolkitAssert.Ensure(KeyDataHeader == 101, "Keyheader magic did not equal 101");

            KeyDataSize = stream.ReadInt32(isBigEndian);
            Unk01 = stream.ReadInt32(isBigEndian);
            int NumKeyFrames = stream.ReadInt32(isBigEndian);

            KeyFrames = new ICurveParam[NumKeyFrames];

            for (int i = 0; i < NumKeyFrames; i++)
            {
                ToolkitAssert.Ensure(stream.Position != stream.Length, "Reached the end to early?");

                int Header = stream.ReadInt32(isBigEndian);
                ToolkitAssert.Ensure(Header == 1000, "Keyframe magic did not equal 1000");

                using (BinaryReader br = new(new MemoryStream(stream.ReadBytes(stream.ReadInt32(isBigEndian) - 8))))
                {
                    KeyFrames[i] = CutsceneCurveParamFactory.ReadFromFile(br, CutsceneName, i);
                }
            }
        }

        public virtual void WriteToFile(MemoryStream stream, bool isBigEndian)
        {
            stream.Write(101, isBigEndian);

            using (MemoryStream ms = new())
            {
                using (BinaryWriter bw = new(ms))
                {
                    bw.Write(Unk01);
                    bw.Write(KeyFrames.Length);

                    for (int i = 0; i < KeyFrames.Length; i++)
                    {
                        ICurveParam CurveParam = KeyFrames[i];
                        bw.Write(1000); // Write the header

                        byte[] keyData;

                        using (MemoryStream keyMs = new())
                        {
                            using (BinaryWriter keyBw = new(keyMs))
                            {
                                CurveParam.Write(keyBw);
                            }

                            keyData = keyMs.ToArray();
                        }

                        bw.Write(keyData.Length + 8);
                        bw.Write(keyData);
                    }
                }

                byte[] data = ms.ToArray();

                stream.Write(data.Length + 8, isBigEndian);
                stream.Write(data);
            }   
        }

        public override string ToString()
        {
            return string.Format("Type: {0}", DataType);
        }
    }
}

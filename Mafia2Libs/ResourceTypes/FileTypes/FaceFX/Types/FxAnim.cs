using System;
using System.ComponentModel;
using System.IO;

namespace ResourceTypes.OC3.FaceFX
{
    // A key used for an FxAnimCurve.
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class FxAnimKey
    {
        public float TimeIn { get; set; }
        public float Value { get; set; }
        public float SlopeIn { get; set; }
        public float SlopeOut { get; set; }

        public void ReadFromFile(BinaryReader reader)
        {
            TimeIn = reader.ReadSingle();
            Value = reader.ReadSingle();
            SlopeIn = reader.ReadSingle();
            SlopeOut = reader.ReadSingle();
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(TimeIn);
            writer.Write(Value);
            writer.Write(SlopeIn);
            writer.Write(SlopeOut);
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", TimeIn, Value);
        }
    }
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class FxAnim : FxNamedObject
    {
        public FxAnimCurve[] Curves { get; set; }
        public float Unk0 {get;set;}
        public float Unk1 { get; set; }
        public uint Unk2 { get; set; }
        public uint Unk3 { get; set; }
        public uint Unk4 { get; set; }
        public uint Unk5 { get; set; }

        public override void Deserialize(FxArchive Owner, BinaryReader Reader)
        {
            base.Deserialize(Owner, Reader);

            uint NumCurves = Reader.ReadUInt32();

            // Read animation curves
            Curves = new FxAnimCurve[NumCurves];
            for (int i = 0; i < NumCurves; i++)
            {
                FxAnimCurve AnimCurve = new FxAnimCurve();
                AnimCurve.Deserialize(Owner, Reader);
                Curves[i] = AnimCurve;
            }

            // FxAnimKeys Storage. This contains all curve AnimKeys.
            uint NumAnimKeys = Reader.ReadUInt32();
            FxAnimKey[] AnimKeyStorage = new FxAnimKey[NumAnimKeys];
            for (int i = 0; i < NumAnimKeys; i++)
            {
                FxAnimKey NewKey = new FxAnimKey();
                NewKey.ReadFromFile(Reader);
                AnimKeyStorage[i] = NewKey;
            }

            // FxAnimCurves - Find out how many Keys for each curve
            uint NumCurves2 = Reader.ReadUInt32();
            uint CurrentUsedKeys = 0;
            for (int i = 0; i < NumCurves2; i++)
            {
                uint KeysPerCurve = Reader.ReadUInt32();
                Curves[i].AnimKeys = new FxAnimKey[KeysPerCurve];
                Array.Copy(AnimKeyStorage, CurrentUsedKeys, Curves[i].AnimKeys, 0, KeysPerCurve);
                CurrentUsedKeys += KeysPerCurve;
            }

            // Ending data - not sure what all this is..
            Unk0 = Reader.ReadSingle(); // usually 0.16
            Unk1 = Reader.ReadSingle(); // usually 0.22 
            Unk2 = Reader.ReadUInt32(); // Next three are zero
            Unk3 = Reader.ReadUInt32();
            Unk4 = Reader.ReadUInt32();
            Unk5 = Reader.ReadUInt32(); // Bottom is usually -1
        }

        public override void Serialize(FxArchive Owner, BinaryWriter Writer)
        {
            base.Serialize(Owner, Writer);

            // Cache off number of animation keys
            int NumAnimKeys = 0;

            // Write AnimCurves
            Writer.Write(Curves.Length);
            foreach (FxAnimCurve Curve in Curves)
            {
                Curve.Serialize(Owner, Writer);

                NumAnimKeys += Curve.AnimKeys.Length;
            }

            // Write FxAnimKeys storage
            Writer.Write(NumAnimKeys);
            foreach (FxAnimCurve Curve in Curves)
            {
                foreach (FxAnimKey Key in Curve.AnimKeys)
                {
                    Key.WriteToFile(Writer);
                }
            }

            // Write Curves number of animations
            Writer.Write(Curves.Length);
            foreach (FxAnimCurve Curve in Curves)
            {
                Writer.Write(Curve.AnimKeys.Length);
            }

            // Write footer
            Writer.Write(Unk0);
            Writer.Write(Unk1);
            Writer.Write(Unk2);
            Writer.Write(Unk3);
            Writer.Write(Unk4);
            Writer.Write(Unk5);
        }

        public override void PopulateStringTable(FxArchive Owner)
        {
            base.PopulateStringTable(Owner);

            foreach(FxAnimCurve Curve in Curves)
            {
                Curve.PopulateStringTable(Owner);
            }
        }
    }
}

using System;
using System.IO;

namespace ResourceTypes.OC3.FaceFX
{
    // A key used for an FxAnimCurve.
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

    /* A structure to allow manipulation of FxFaceGroupNode throughout 
    * an animation. Uses AnimKeys to pinpoint where in the animation 
    * the manipulation should take place and how strong it should be.
    */
    public class FxAnimCurve
    {
        public uint Name { get; set; }
        public uint Unk0 { get; set; }
        public FxAnimKey[] AnimKeys { get; set; }
    }

    public class FxAnim
    {
        public uint AnimName { get; set; }
        public FxAnimCurve[] Curves { get; set; }
        public float Unk0 {get;set;}
        public float Unk1 { get; set; }
        public uint Unk2 { get; set; }
        public uint Unk3 { get; set; }
        public uint Unk4 { get; set; }
        public uint Unk5 { get; set; }

        public void ReadFromFile(BinaryReader reader)
        {
            AnimName = reader.ReadUInt32();
            uint NumCurves = reader.ReadUInt32();

            // Read animation curves
            Curves = new FxAnimCurve[NumCurves];
            for (int i = 0; i < NumCurves; i++)
            {
                FxAnimCurve AnimCurve = new FxAnimCurve();
                AnimCurve.Name = reader.ReadUInt32();
                AnimCurve.Unk0 = reader.ReadUInt32(); // I wonder if this is evaluator?
                Curves[i] = AnimCurve;
            }

            // FxAnimKeys Storage. This contains all curve AnimKeys.
            uint NumAnimKeys = reader.ReadUInt32();
            FxAnimKey[] AnimKeyStorage = new FxAnimKey[NumAnimKeys];
            for (int i = 0; i < NumAnimKeys; i++)
            {
                FxAnimKey NewKey = new FxAnimKey();
                NewKey.ReadFromFile(reader);
                AnimKeyStorage[i] = NewKey;
            }

            // FxAnimCurves - Find out how many Keys for each curve
            uint NumCurves2 = reader.ReadUInt32();
            uint CurrentUsedKeys = 0;
            for (int i = 0; i < NumCurves2; i++)
            {
                uint KeysPerCurve = reader.ReadUInt32();
                Curves[i].AnimKeys = new FxAnimKey[KeysPerCurve];
                Array.Copy(AnimKeyStorage, CurrentUsedKeys, Curves[i].AnimKeys, 0, KeysPerCurve);
                CurrentUsedKeys += KeysPerCurve;
            }

            // Ending data - not sure what all this is..
            Unk0 = reader.ReadSingle(); // usually 0.16
            Unk1 = reader.ReadSingle(); // usually 0.22 
            Unk2 = reader.ReadUInt32(); // Next three are zero
            Unk3 = reader.ReadUInt32();
            Unk4 = reader.ReadUInt32();
            Unk5 = reader.ReadUInt32(); // Bottom is usually -1
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(AnimName);

            // Cache off number of animation keys
            int NumAnimKeys = 0;

            // Write AnimCurves
            writer.Write(Curves.Length);
            foreach(FxAnimCurve Curve in Curves)
            {
                writer.Write(Curve.Name);
                writer.Write(Curve.Unk0);

                NumAnimKeys += Curve.AnimKeys.Length;
            }

            // Write FxAnimKeys storage
            writer.Write(NumAnimKeys);
            foreach(FxAnimCurve Curve in Curves)
            {
                foreach(FxAnimKey Key in Curve.AnimKeys)
                {
                    Key.WriteToFile(writer);
                }
            }

            // Write Curves number of animations
            writer.Write(Curves.Length);
            foreach (FxAnimCurve Curve in Curves)
            {
                writer.Write(Curve.AnimKeys.Length);
            }

            // Write footer
            writer.Write(Unk0);
            writer.Write(Unk1);
            writer.Write(Unk2);
            writer.Write(Unk3);
            writer.Write(Unk4);
            writer.Write(Unk5);
        }
    }
}

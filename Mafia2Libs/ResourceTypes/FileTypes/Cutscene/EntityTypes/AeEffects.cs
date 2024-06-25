using System.IO;
using Utils.Extensions;
using Utils.Logging;

namespace ResourceTypes.Cutscene.AnimEntities
{
    public class AeEffectsWrapper : AnimEntityWrapper
    {
        public AeEffectsWrapper() : base()
        {
            AnimEntityData = new AeEffectsBase();
        }

        public override AnimEntityTypes GetEntityType()
        {
            return AnimEntityTypes.AeEffects;
        }
    }

    public class AeEffectsBase : AeBaseData
    {
        public class Effect
        {
            public string Name { get; set; }
            public int Unk0 { get; set; }

            public override string ToString()
            {
                return string.Format("{0} {1}", Name, Unk0);
            }
        }

        public int Unk02 { get; set; }
        public int Unk03 { get; set; }
        public int Unk04 { get; set; }
        public byte[] Unk05 { get; set; }
        /*
        public Effect[] Unk04_Effects { get; set; }
        public int Unk05 { get; set; }
        public int Unk06 { get; set; }
        public int Unk07 { get; set; }
        public Effect[] Unk07_Effects { get; set; }
        public int Unk08 { get; set; }
        public int Unk09 { get; set; }
        public int Unk10 { get; set; }
        public Effect[] Unk10_Effects { get; set; }
        */
        public override void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            base.ReadFromFile(stream, isBigEndian);
            ToolkitAssert.Ensure(stream.Position != stream.Length, "I've read the parent class data, although i've hit the eof!");

            Unk02 = stream.ReadInt32(isBigEndian);
            Unk03 = stream.ReadInt32(isBigEndian);
            Unk04 = stream.ReadInt32(isBigEndian);
            Unk05 = stream.ReadBytes(112);
            /*
            ToolkitAssert.Ensure(stream.Position != stream.Length, "I've read the parent class data, although i've hit the eof!");

            Unk04_Effects = new Effect[Unk04];
            for(int i = 0; i < Unk04; i++)
            {
                Effect NewEffect = new Effect();
                NewEffect.Name = stream.ReadStringBuffer(4);
                NewEffect.Unk0 = stream.ReadInt32(isBigEndian);
                Unk04_Effects[i] = NewEffect;
            }

            ToolkitAssert.Ensure(stream.Position != stream.Length, "I've read the parent class data, although i've hit the eof!");

            Unk05 = stream.ReadInt32(isBigEndian);
            Unk06 = stream.ReadInt32(isBigEndian);
            Unk07 = stream.ReadInt32(isBigEndian);

            ToolkitAssert.Ensure(stream.Position != stream.Length, "I've read the parent class data, although i've hit the eof!");

            Unk07_Effects = new Effect[Unk07];
            for (int i = 0; i < Unk07; i++)
            {
                Effect NewEffect = new Effect();
                NewEffect.Name = stream.ReadStringBuffer(4);
                NewEffect.Unk0 = stream.ReadInt32(isBigEndian);
                Unk07_Effects[i] = NewEffect;
            }

            ToolkitAssert.Ensure(stream.Position != stream.Length, "I've read the parent class data, although i've hit the eof!");

            Unk08 = stream.ReadInt32(isBigEndian);
            Unk09 = stream.ReadInt32(isBigEndian);
            Unk10 = stream.ReadInt32(isBigEndian);

            ToolkitAssert.Ensure(stream.Position != stream.Length, "I've read the parent class data, although i've hit the eof!");

            Unk10_Effects = new Effect[Unk07];
            for (int i = 0; i < Unk07; i++)
            {
                Effect NewEffect = new Effect();
                NewEffect.Name = stream.ReadStringBuffer(4);
                NewEffect.Unk0 = stream.ReadInt32(isBigEndian);
                Unk10_Effects[i] = NewEffect;
            }

            ToolkitAssert.Ensure(stream.Position == stream.Length, "We expect to have hit the eof by now.");
            */
        }

        public override void WriteToFile(MemoryStream stream, bool isBigEndian)
        {
            base.WriteToFile(stream, isBigEndian);
            stream.Write(Unk02, isBigEndian);
            stream.Write(Unk03, isBigEndian);
            stream.Write(Unk04, isBigEndian);
            stream.Write(Unk05);
        }
    }
}

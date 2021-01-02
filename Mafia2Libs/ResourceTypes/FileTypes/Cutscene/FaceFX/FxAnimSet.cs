using System.IO;

namespace ResourceTypes.OC3.FaceFX
{
    public class FxAnimGroup
    {
        public uint GroupName { get; set; }
        public FxAnim[] Animations { get; set; }

        public void ReadFromFile(BinaryReader reader)
        {
            GroupName = reader.ReadUInt32();
            uint NumAnims = reader.ReadUInt32();

            Animations = new FxAnim[NumAnims];
            for (int x = 0; x < NumAnims; x++)
            {
                FxAnim AnimObject = new FxAnim();
                AnimObject.ReadFromFile(reader);
                Animations[x] = AnimObject;
            }
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(GroupName);
            writer.Write(Animations.Length);

            foreach(FxAnim Animation in Animations)
            {
                Animation.WriteToFile(writer);
            }
        }
    }

    public class FxAnimSet : FxArchive
    {
        private uint Num;
        private uint Size;
        private uint AnimSetName;
        private uint Unk04;
        private FxAnimGroup[] AnimGroups;

        public void ReadFromFile(string filename)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(filename, FileMode.Open)))
            {
                ReadFromFile(reader);
            }
        }

        public override void ReadFromFile(BinaryReader reader)
        {
            Num = reader.ReadUInt32();
            Size = reader.ReadUInt32();

            base.ReadFromFile(reader);

            // AnimSet ID
            AnimSetName = reader.ReadUInt32();
            Unk04 = reader.ReadUInt32(); // Unknown; could be data about published or not.

            // Read groups.
            uint NumAnimGroups = reader.ReadUInt32();
            AnimGroups = new FxAnimGroup[NumAnimGroups];
            for(int i = 0; i < NumAnimGroups; i++)
            {
                FxAnimGroup GroupObject = new FxAnimGroup();
                GroupObject.ReadFromFile(reader);
                AnimGroups[i] = GroupObject;
            }
        }

        public void WriteToFile(string filename)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(filename, FileMode.Create)))
            {
                WriteToFile(writer);
            }
        }

        public override void WriteToFile(BinaryWriter writer)
        {
            writer.Write(Num);
            writer.Write(Size);

            base.WriteToFile(writer);

            // Write AnimSet ID
            writer.Write(AnimSetName);
            writer.Write(Unk04);

            writer.Write(AnimGroups.Length);
            foreach(FxAnimGroup Group in AnimGroups)
            {
                Group.WriteToFile(writer);
            }
        }
    }
}

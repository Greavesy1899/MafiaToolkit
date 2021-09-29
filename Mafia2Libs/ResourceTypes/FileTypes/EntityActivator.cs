using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Utils.Helpers.Reflection;
using System.Xml.Linq;

namespace ResourceTypes.EntityActivator
{
    public class EntityActivator
    {
        public class EntitySet0
        { 
            [PropertyForceAsAttribute]
            public ulong EntityHash { get; set; }
            [PropertyForceAsAttribute]
            public uint Unk01 { get; set; }
            public EntitySet1[] EntitySets { get; set; }

            public void WriteToFile(BinaryWriter writer)
            {
                writer.Write(EntityHash);
                writer.Write(Unk01);
                writer.Write(EntitySets.Length);

                foreach (EntitySet1 Set in EntitySets)
                {
                    Set.WriteToFile(writer);
                }
            }
        }

        public class EntitySet1
        {
            [PropertyForceAsAttribute]
            public ulong EntityHash { get; set; }
            public ulong[] OtherHashes { get; set; }

            public void WriteToFile(BinaryWriter writer)
            {
                writer.Write(EntityHash);
                writer.Write(OtherHashes.Length);

                foreach (ulong Hash in OtherHashes)
                {
                    writer.Write(Hash);
                }
            }
        }

        private byte[] magic = { (byte)'a', (byte)'t', (byte)'n', (byte)'e' };
        public EntitySet0[] Sets { get; set; }
        private int Unk1 { get; set; }

        public void ReadFromFile(FileInfo Info)
        {
            string FullName = Info.FullName;
            using(BinaryReader reader = new BinaryReader(File.Open(FullName, FileMode.Open)))
            {
                reader.ReadInt32(); // Magic inverted - enta
                Unk1 = reader.ReadInt32();
                int Count = reader.ReadInt32();

                Sets = new EntitySet0[Count];
                for (int y = 0; y < Count; y++)
                {
                    EntitySet0 NewSet = new EntitySet0();
                    NewSet.EntityHash = reader.ReadUInt64();
                    NewSet.Unk01 = reader.ReadUInt32();

                    uint Count00 = reader.ReadUInt32();
                    NewSet.EntitySets = new EntitySet1[Count00];
                    for (int i = 0; i < Count00; i++)
                    {
                        EntitySet1 NextSet = new EntitySet1();
                        NextSet.EntityHash = reader.ReadUInt64();
                        uint Count0 = reader.ReadUInt32();

                        NextSet.OtherHashes = new ulong[Count0];
                        for (int x = 0; x < NextSet.OtherHashes.Length; x++)
                        {
                            NextSet.OtherHashes[x] = reader.ReadUInt64();
                        }

                        NewSet.EntitySets[i] = NextSet;
                    }

                    Sets[y] = NewSet;
                }

                XElement Root = ReflectionHelpers.ConvertPropertyToXML<EntityActivator>(this);
                Root.Save("EntityActivator.xml");
            }
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(magic);
            writer.Write(Unk1);
            writer.Write(Sets.Length);

            foreach (EntitySet0 Set in Sets)
            {
                Set.WriteToFile(writer);
            }
        }
    }
}

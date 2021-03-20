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
            public ulong Unk01 { get; set; }
            public EntitySet1[] EntitySets { get; set; }
        }

        public class EntitySet1
        {
            [PropertyForceAsAttribute]
            public ulong EntityHash { get; set; }
            public ulong[] OtherHashes { get; set; }
        }

        public EntitySet0[] Sets { get; set; }

        public void ReadFromFile(FileInfo Info)
        {
            string FullName = Info.FullName;
            using(BinaryReader reader = new BinaryReader(File.Open(FullName, FileMode.Open)))
            {
                reader.BaseStream.Seek(0xC, SeekOrigin.Begin);

                Sets = new EntitySet0[17];
                for (int y = 0; y < 17; y++)
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
    }
}

using BitStreams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceTypes.Prefab.CrashObject
{
    public class S_AddDeformPartsInitData : S_GlobalInitData
    {
        public S_InitDeformPart[] Parts { get; set; }

        public S_AddDeformPartsInitData() : base()
        {
            Parts = new S_InitDeformPart[0];
        }

        public override void Load(BitStream MemStream)
        {
            base.Load(MemStream);

            uint NumInitDeformParts = MemStream.ReadUInt32();
            Parts = new S_InitDeformPart[NumInitDeformParts];
            for (uint i = 0; i < NumInitDeformParts; i++)
            {
                S_InitDeformPart DeformPart = new S_InitDeformPart();
                DeformPart.Load(MemStream);
                Parts[i] = DeformPart;
            }
        }

        public override void Save(BitStream MemStream)
        {
            base.Save(MemStream);

            MemStream.WriteUInt32((uint)Parts.Length);
            foreach(S_InitDeformPart DeformPart in Parts)
            {
                DeformPart.Save(MemStream);
            }
        }
    }
}

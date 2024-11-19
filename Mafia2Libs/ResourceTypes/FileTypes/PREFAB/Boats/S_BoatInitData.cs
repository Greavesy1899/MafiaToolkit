using BitStreams;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using Utils.Helpers.Reflection;

namespace ResourceTypes.Prefab.CrashObject
{
    public class S_BoatInitData : S_ActorDeformInitData
    {
        [Category("Boat")]
        public ulong CentreOfMassFrameHash { get; set; }
        [Category("Boat")]
        public ulong[] EmittersFrameHashes { get; set; }

        public S_BoatInitData() : base()
        {
            EmittersFrameHashes = new ulong[0];
        }

        public override void Load(BitStream MemStream)
        {
            base.Load(MemStream);

            CentreOfMassFrameHash = MemStream.ReadUInt64();

            // Load emitters
            uint Unk1 = MemStream.ReadUInt32();
            EmittersFrameHashes = new ulong[Unk1];
            for(int i = 0; i < Unk1; i++)
            {
                EmittersFrameHashes[i] = MemStream.ReadUInt64();
            }
        }

        public override void Save(BitStream MemStream)
        {
            base.Save(MemStream);

            MemStream.WriteUInt64(CentreOfMassFrameHash);

            // Write emitters
            MemStream.WriteInt32(EmittersFrameHashes.Length);
            foreach(ulong Value in EmittersFrameHashes)
            {
                MemStream.WriteUInt64(Value);
            }
        }
    }
}

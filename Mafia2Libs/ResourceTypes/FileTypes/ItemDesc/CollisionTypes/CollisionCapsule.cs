// SPDX-License-Identifier: LicenseRef-PolyForm-Strict-1.0.0
// Copyright (c) 2018-2026 Greavesy

using System.IO;

namespace ResourceTypes.ItemDesc
{
    public class CollisionCapsule
    {
        float[] floats = new float[2];

        public CollisionCapsule(BinaryReader reader)
        {
            ReadFromFile(reader);
        }

        public void ReadFromFile(BinaryReader reader)
        {
            floats[0] = reader.ReadSingle();
            floats[1] = reader.ReadSingle();
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(floats[0]);
            writer.Write(floats[1]);
        }
    }
}

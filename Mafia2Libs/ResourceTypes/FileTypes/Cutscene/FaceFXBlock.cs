// SPDX-License-Identifier: LicenseRef-PolyForm-Strict-1.0.0
// Copyright (c) 2018-2026 Greavesy

using System.ComponentModel;
using System.IO;
using Utils.StringHelpers;

namespace ResourceTypes.Cutscene
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class FaceFXBlock
    {
        public string Name { get; set; }
        public FaceFXBlock(BinaryReader br)
        {
            Read(br);
        }

        public void Read(BinaryReader br)
        {
            Name = br.ReadString16();
        }

        public void Write(BinaryWriter bw)
        {
            bw.WriteString16(Name);
        }
    }
}

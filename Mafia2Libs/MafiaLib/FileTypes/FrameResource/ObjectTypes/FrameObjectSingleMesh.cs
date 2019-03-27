using System.ComponentModel;
using System.IO;
using SharpDX;
using Mafia2;
using Utils.SharpDXExtensions;
using Utils.Extensions;
using Utils.Types;

namespace ResourceTypes.FrameResource
{
    public class FrameObjectSingleMesh : FrameObjectJoint
    {
        SingleMeshFlags flags;
        BoundingBox bounds;
        byte unk_14_byte;
        int meshIndex;
        int materialIndex;
        Hash unk_17_textureHash;
        byte unk_18_byte1;
        byte unk_18_byte2;
        byte unk_18_byte3;

        [Editor(typeof(FlagEnumUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public SingleMeshFlags Flags {
            get { return flags; }
            set { flags = value; }
        }
        public BoundingBox Boundings {
            get { return bounds; }
            set { bounds = value; }
        }
        public byte Unk_14 {
            get { return unk_14_byte; }
            set { unk_14_byte = value; }
        }
        public int MeshIndex {
            get { return meshIndex; }
            set { meshIndex = value; }
        }
        public int MaterialIndex {
            get { return materialIndex; }
            set { materialIndex = value; }
        }
        public Hash Unk17 {
            get { return unk_17_textureHash; }
            set { unk_17_textureHash = value; }
        }
        public byte Unk_18_1 {
            get { return unk_18_byte1; }
            set { unk_18_byte1 = value; }
        }
        public byte Unk_18_2 {
            get { return unk_18_byte2; }
            set { unk_18_byte2 = value; }
        }
        public byte Unk_18_3 {
            get { return unk_18_byte3; }
            set { unk_18_byte3 = value; }
        }

        /// <summary>
        /// Read single mesh data from reader.
        /// </summary>
        /// <param name="reader"></param>
        public FrameObjectSingleMesh(BinaryReader reader) : base()
        {
            ReadFromFile(reader);
        }

        /// <summary>
        /// Build basic singleMesh data.
        /// </summary>
        public FrameObjectSingleMesh() : base()
        {
            flags = SingleMeshFlags.Unk14_Flag | SingleMeshFlags.flag_32 | SingleMeshFlags.flag_67108864;
            bounds = new BoundingBox();
            unk_14_byte = 255;
            meshIndex = 0;
            materialIndex = 0;
            transformMatrix = new TransformMatrix();
            unk_17_textureHash = new Hash();
            unk_18_byte1 = 0;
            unk_18_byte2 = 0;
            unk_18_byte3 = 0;
        }

        public override void ReadFromFile(BinaryReader reader)
        {
            base.ReadFromFile(reader);
            flags = (SingleMeshFlags)reader.ReadInt32();
            bounds = BoundingBoxExtenders.ReadFromFile(reader);
            unk_14_byte = reader.ReadByte();
            meshIndex = reader.ReadInt32();
            materialIndex = reader.ReadInt32();
            unk_17_textureHash = new Hash(reader);
            unk_18_byte1 = reader.ReadByte();
            unk_18_byte2 = reader.ReadByte();
            unk_18_byte3 = reader.ReadByte();
        }

        public override void WriteToFile(BinaryWriter writer)
        {
            base.WriteToFile(writer);
            writer.Write((int)flags);
            bounds.WriteToFile(writer);
            writer.Write(unk_14_byte);
            writer.Write(meshIndex);
            writer.Write(materialIndex);
            unk_17_textureHash.WriteToFile(writer);
            writer.Write(unk_18_byte1);
            writer.Write(unk_18_byte2);
            writer.Write(unk_18_byte3);
        }

        public override string ToString()
        {
            return string.Format("{0}", Name);
        }
    }
}

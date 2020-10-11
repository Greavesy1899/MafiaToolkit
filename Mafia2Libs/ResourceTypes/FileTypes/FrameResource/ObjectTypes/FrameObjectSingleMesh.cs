using System.ComponentModel;
using System.IO;
using SharpDX;
using Utils.SharpDXExtensions;
using Utils.Extensions;
using Utils.Types;

namespace ResourceTypes.FrameResource
{
    public class FrameObjectSingleMesh : FrameObjectJoint
    {
        SingleMeshFlags flags;
        BoundingBox bounds;
        byte unk14;
        int meshIndex;
        int materialIndex;
        Hash omTextureHash;
        byte unk18_1;
        byte unk18_2;
        byte unk18_3;

        private FrameMaterial material;
        private FrameGeometry geometry;

        [Editor(typeof(FlagEnumUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public SingleMeshFlags SingleMeshFlags {
            get { return flags; }
            set { flags = value; }
        }
        public BoundingBox Boundings {
            get { return bounds; }
            set { bounds = value; }
        }
        public byte Unk_14 {
            get { return unk14; }
            set { unk14 = value; }
        }
        [ReadOnly(true)]
        public int MeshIndex {
            get { return meshIndex; }
            set { meshIndex = value; }
        }
        [ReadOnly(true)]
        public int MaterialIndex {
            get { return materialIndex; }
            set { materialIndex = value; }
        }
        public Hash OMTextureHash {
            get { return omTextureHash; }
            set { omTextureHash = value; }
        }
        public byte Unk_18_1 {
            get { return unk18_1; }
            set { unk18_1 = value; }
        }
        public byte Unk_18_2 {
            get { return unk18_2; }
            set { unk18_2 = value; }
        }
        public byte Unk_18_3 {
            get { return unk18_3; }
            set { unk18_3 = value; }
        }

        [TypeConverter(typeof(ExpandableObjectConverter)), Category("Linked Blocks"), Description("Avoid editing!")]
        public FrameGeometry Geometry {
            get { return geometry; }
            set { geometry = value; }
        }

        [TypeConverter(typeof(ExpandableObjectConverter)), Category("Linked Blocks"), Description("Avoid editing!")]
        public FrameMaterial Material {
            get { return material; }
            set { material = value; }
        }

        public FrameObjectSingleMesh(MemoryStream reader, bool isBigEndian) : base()
        {
            ReadFromFile(reader, isBigEndian);
        }

        public FrameObjectSingleMesh(FrameObjectSingleMesh other) : base(other)
        {
            flags = other.flags;
            bounds = other.bounds;
            unk14 = other.unk14;
            meshIndex = other.meshIndex;
            materialIndex = other.materialIndex;
            omTextureHash = new Hash(other.omTextureHash.String);
            unk18_1 = other.unk18_1;
            unk18_2 = other.unk18_2;
            unk18_3 = other.unk18_3;
            material = other.material;
            geometry = other.geometry;
        }

        public FrameObjectSingleMesh() : base()
        {
            flags = SingleMeshFlags.Unk14_Flag | SingleMeshFlags.flag_32 | SingleMeshFlags.flag_67108864;
            bounds = new BoundingBox();
            unk14 = 255;
            meshIndex = 0;
            materialIndex = 0;
            localTransform = new Matrix();
            omTextureHash = new Hash();
            unk18_1 = 0;
            unk18_2 = 0;
            unk18_3 = 0;
        }

        public override void ReadFromFile(MemoryStream reader, bool isBigEndian)
        {
            base.ReadFromFile(reader, isBigEndian);
            flags = (SingleMeshFlags)reader.ReadInt32(isBigEndian);
            bounds = BoundingBoxExtenders.ReadFromFile(reader, isBigEndian);
            unk14 = reader.ReadByte8();
            meshIndex = reader.ReadInt32(isBigEndian);
            materialIndex = reader.ReadInt32(isBigEndian);
            omTextureHash = new Hash(reader, isBigEndian);
            unk18_1 = reader.ReadByte8();
            unk18_2 = reader.ReadByte8();
            unk18_3 = reader.ReadByte8();
        }

        public override void WriteToFile(BinaryWriter writer)
        {
            base.WriteToFile(writer);
            writer.Write((int)flags);
            bounds.WriteToFile(writer);
            writer.Write(unk14);
            writer.Write(meshIndex);
            writer.Write(materialIndex);
            omTextureHash.WriteToFile(writer);
            writer.Write(unk18_1);
            writer.Write(unk18_2);
            writer.Write(unk18_3);
        }

        public override string ToString()
        {
            return string.Format("{0}", Name);
        }
    }
}

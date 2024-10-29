using Rendering.Factories;
using Rendering.Graphics;
using System.ComponentModel;
using System.IO;
using System.Numerics;
using ResourceTypes.BufferPools;
using Utils.Extensions;
using Utils.Models;
using Utils.Types;
using Utils.VorticeUtils;
using Vortice.Mathematics;

namespace ResourceTypes.FrameResource
{
    public class FrameObjectSingleMesh : FrameObjectJoint
    {
        SingleMeshFlags flags;
        BoundingBox bounds;
        int meshIndex;
        int materialIndex;
        HashName omTextureHash;
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
        public byte DeformPartIndex { get; set; }
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
        public HashName OMTextureHash {
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
            get { return GetGeometry(); }
            set { geometry = value; }
        }

        [TypeConverter(typeof(ExpandableObjectConverter)), Category("Linked Blocks"), Description("Avoid editing!")]
        public FrameMaterial Material {
            get { return GetMaterial(); }
            set { material = value; }
        }

        public FrameObjectSingleMesh(FrameObjectSingleMesh other) : base(other)
        {
            flags = other.flags;
            bounds = other.bounds;
            DeformPartIndex = other.DeformPartIndex;
            meshIndex = other.meshIndex;
            materialIndex = other.materialIndex;
            omTextureHash = new HashName(other.omTextureHash.String);
            unk18_1 = other.unk18_1;
            unk18_2 = other.unk18_2;
            unk18_3 = other.unk18_3;
            material = other.material;
            geometry = other.geometry;
        }

        public FrameObjectSingleMesh(FrameResource OwningResource) : base(OwningResource)
        {
            flags = SingleMeshFlags.Unk14_Flag | SingleMeshFlags.flag_32 | SingleMeshFlags.flag_67108864;
            bounds = new BoundingBox();
            DeformPartIndex = 255;
            meshIndex = 0;
            materialIndex = 0;
            omTextureHash = new HashName();
            unk18_1 = 0;
            unk18_2 = 0;
            unk18_3 = 0;
        }

        public override void ReadFromFile(MemoryStream reader, bool isBigEndian)
        {
            base.ReadFromFile(reader, isBigEndian);
            flags = (SingleMeshFlags)reader.ReadInt32(isBigEndian);
            bounds = BoundingBoxExtenders.ReadFromFile(reader, isBigEndian);
            DeformPartIndex = reader.ReadByte8();
            meshIndex = reader.ReadInt32(isBigEndian);
            materialIndex = reader.ReadInt32(isBigEndian);
            omTextureHash = new HashName(reader, isBigEndian);
            unk18_1 = reader.ReadByte8();
            unk18_2 = reader.ReadByte8();
            unk18_3 = reader.ReadByte8();
        }

        public override void WriteToFile(BinaryWriter writer)
        {
            base.WriteToFile(writer);
            writer.Write((int)flags);
            bounds.WriteToFile(writer);
            writer.Write(DeformPartIndex);
            writer.Write(meshIndex);
            writer.Write(materialIndex);
            omTextureHash.WriteToFile(writer);
            writer.Write(unk18_1);
            writer.Write(unk18_2);
            writer.Write(unk18_3);
        }

        protected override void SanitizeOnSave()
        {
            base.SanitizeOnSave();

            /* Start check regarding OM Flag */

            // Check if OM Texture is valid
            bool bIsOMTextureValid = (omTextureHash.Hash != 0);

            // Cache-Off flag
            bool bHasOMFlag = flags.HasFlag(SingleMeshFlags.OM_Flag);

            // If we have the flag but we it isn't valid
            if (bHasOMFlag && !bIsOMTextureValid)
            {
                // Remove flag
                flags &= ~SingleMeshFlags.OM_Flag;
            }

            // If we have a valid hash but don't have the flag.
            if(bIsOMTextureValid && !bHasOMFlag)
            {
                // Add flag
                flags |= SingleMeshFlags.OM_Flag;
            }
            /* End check regarding OM Flag */
        }

        protected FrameMaterial ConstructMaterialObject()
        {
            Material = OwningResource.ConstructFrameAssetOfType<FrameMaterial>();
            AddRef(FrameEntryRefTypes.Material, Material.RefID);
            return Material;
        }

        protected FrameGeometry ConstructGeometryObject()
        {
            geometry = OwningResource.ConstructFrameAssetOfType<FrameGeometry>();
            AddRef(FrameEntryRefTypes.Geometry, geometry.RefID);
            return geometry;
        }

        public FrameMaterial GetMaterial()
        {
            if(material == null)
            {
                return ConstructMaterialObject();
            }

            return material;
        }

        public FrameGeometry GetGeometry()
        {
            if(geometry == null)
            {
                return ConstructGeometryObject();
            }

            return geometry;
        }

        public virtual void CreateMeshFromRawModel(ModelWrapper NewModel)
        {
            ConstructMaterialObject();
            ConstructGeometryObject();

            NewModel.SetFrameMesh(this);
            NewModel.CreateObjectsFromModel();
        }

        public override void ConstructRenderable()
        {
            RenderModel Renderable = RenderableFactory.BuildRenderModelFromFrame(this);
            RenderAdapter = new Rendering.Core.RenderableAdapter();
            RenderAdapter.InitAdaptor(Renderable, this);
        }

        public override string ToString()
        {
            return string.Format("{0}", Name);
        }

        public IndexBuffer GetIndexBuffer(int lod)
        {
            return OwningResource.SceneData.IndexBufferPool.GetBuffer(Geometry.LOD[lod]
                .IndexBufferRef.Hash);
        }
        
        public VertexBuffer GetVertexBuffer(int lod)
        {
            return OwningResource.SceneData.VertexBufferPool.GetBuffer(Geometry.LOD[lod]
                .VertexBufferRef.Hash);
        }
        
        
    }
}

using System.Numerics;
using Gibbed.Illusion.FileFormats.Hashing;
using ResourceTypes.Collisions;
using ResourceTypes.Materials;
using Utils.Models;

namespace ResourceTypes.ModelHelpers.ModelExporter
{
    public interface IImportHelper
    {
        void Setup();
        void Store();
    }

    public class MT_CollisionHelper : IImportHelper
    {
        public struct CollisionGroup
        {
            public string FaceGroupName { get; set; }
            public CollisionMaterials CollisionMaterial { get; set; }
        }

        private MT_Collision OwningObject; 

        public CollisionGroup[] Materials { get; set; }

        public MT_CollisionHelper(MT_Collision CollisionObject)
        {
            OwningObject = CollisionObject;
        }

        public void Setup()
        {
            if(OwningObject != null)
            {
                Materials = new CollisionGroup[OwningObject.FaceGroups.Length];
                for(int i = 0; i < OwningObject.FaceGroups.Length; i++)
                {
                    string MaterialName = OwningObject.FaceGroups[i].Material.Name;
                    Materials[i].FaceGroupName = MaterialName;
                    Materials[i].CollisionMaterial = CollisionEnumUtils.MaterialNameToEnumValue(MaterialName);
                }
            }
        }

        public void Store()
        {
            if (OwningObject != null)
            {
                for(int i = 0; i < OwningObject.FaceGroups.Length; i++)
                {
                    string NewCollisionName = Materials[i].CollisionMaterial.ToString();
                    OwningObject.FaceGroups[i].Material.Name = NewCollisionName;
                    Materials[i].FaceGroupName = NewCollisionName;
                }
            }
        }
    }

    public class MT_ObjectHelper : IImportHelper
    {
        private MT_Object OwningObject;

        public string ObjectName { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector3 Scale { get; set; }
        public MT_ObjectType ObjectType { get; set; }

        public MT_ObjectHelper(MT_Object ModelObject)
        {
            OwningObject = ModelObject;
        }

        public void Setup()
        {
            if(OwningObject != null)
            {
                ObjectName = OwningObject.ObjectName;
                Position = OwningObject.Position;
                Rotation = OwningObject.Rotation;
                Scale = OwningObject.Scale;
                ObjectType = OwningObject.ObjectType;
            }
        }

        public void Store()
        {
            if(OwningObject != null)
            {
                OwningObject.ObjectName = ObjectName;
                OwningObject.Position = Position;
                OwningObject.Rotation = Rotation;
                OwningObject.Scale = Scale;
                OwningObject.ObjectType = ObjectType;
            }
        }
    }

    public class MT_LodHelper : IImportHelper
    {
        
        private MT_Lod OwningObject;

        public string[] FaceGroupMaterials { get; set; }
        public bool ImportNormals { get; set; }
        public bool ImportTangents { get; set; }
        public bool ImportDiffuseUV { get; set; }
        public bool ImportUV0 { get; set; }
        public bool ImportUV1 { get; set; }
        public bool ImportOMUV { get; set; }
        public bool ImportColour0 { get; set; }
        public bool ImportColour1 { get; set; }

        // Flip utilities
        public bool FlipDiffuseUV { get; set; }
        public bool FlipUV0 { get; set; }
        public bool FlipUV1 { get; set; }
        public bool FlipOMUV { get; set; }

        public MT_LodHelper(MT_Lod LodObject)
        {
            OwningObject = LodObject;
        }

        public void Setup()
        {
            if (OwningObject != null)
            {
                FaceGroupMaterials = new string[OwningObject.FaceGroups.Length];
                for (int i = 0; i < OwningObject.FaceGroups.Length; i++)
                {
                    FaceGroupMaterials[i] = OwningObject.FaceGroups[i].Material.Name;
                }

                ImportNormals = OwningObject.VertexDeclaration.HasFlag(VertexFlags.Normals);
                ImportTangents = OwningObject.VertexDeclaration.HasFlag(VertexFlags.Tangent);
                ImportDiffuseUV = OwningObject.VertexDeclaration.HasFlag(VertexFlags.TexCoords0);
                ImportUV0 = OwningObject.VertexDeclaration.HasFlag(VertexFlags.TexCoords1);
                ImportUV1 = OwningObject.VertexDeclaration.HasFlag(VertexFlags.TexCoords2);
                ImportOMUV = OwningObject.VertexDeclaration.HasFlag(VertexFlags.ShadowTexture);
                ImportColour0 = OwningObject.VertexDeclaration.HasFlag(VertexFlags.Color);
                ImportColour1 = OwningObject.VertexDeclaration.HasFlag(VertexFlags.Color1);
            }
        }

        public void Store()
        {
            if (OwningObject != null)
            {
                for (int i = 0; i < OwningObject.FaceGroups.Length; i++)
                {
                    OwningObject.FaceGroups[i].Material.Name = FaceGroupMaterials[i];
                }

                // Update the declaration
                VertexFlags NewDeclaration = 0;
                NewDeclaration = VertexFlags.Position;
                NewDeclaration |= (ImportNormals ? VertexFlags.Normals : 0);
                NewDeclaration |= (ImportTangents ? VertexFlags.Tangent : 0);
                NewDeclaration |= (ImportDiffuseUV ? VertexFlags.TexCoords0 : 0);
                NewDeclaration |= (ImportUV0 ? VertexFlags.TexCoords1 : 0);
                NewDeclaration |= (ImportUV1 ? VertexFlags.TexCoords2 : 0);
                NewDeclaration |= (ImportOMUV ? VertexFlags.ShadowTexture : 0);
                NewDeclaration |= (ImportColour0 ? VertexFlags.Color : 0);
                NewDeclaration |= (ImportColour1 ? VertexFlags.Color1 : 0);
                OwningObject.VertexDeclaration = NewDeclaration;

                // Update lods
                if(FlipDiffuseUV)
                {
                    FlipChannel(0);
                }
                if (FlipUV0)
                {
                    FlipChannel(1);
                }
                if (FlipUV1)
                {
                    FlipChannel(2);
                }
                if (FlipOMUV)
                {
                    FlipChannel(3);
                }
            }
        }

        private void FlipChannel(int TexIndex)
        {
            foreach(Vertex CurVertex in OwningObject.Vertices)
            {
               if(OwningObject.VertexDeclaration.HasFlag(VertexFlags.TexCoords0))
                {
                    CurVertex.UVs[0] = new Vector2((float)CurVertex.UVs[0].X, (1f - (float)CurVertex.UVs[0].Y));
                }

                if (OwningObject.VertexDeclaration.HasFlag(VertexFlags.TexCoords1))
                {
                    CurVertex.UVs[1] = new Vector2((float)CurVertex.UVs[1].X, (1f - (float)CurVertex.UVs[1].Y));
                }

                if (OwningObject.VertexDeclaration.HasFlag(VertexFlags.TexCoords2))
                {
                    CurVertex.UVs[2] = new Vector2((float)CurVertex.UVs[2].X, (1f - (float)CurVertex.UVs[2].Y));
                }

                if (OwningObject.VertexDeclaration.HasFlag(VertexFlags.ShadowTexture))
                {
                    CurVertex.UVs[3] = new Vector2((float)CurVertex.UVs[3].X, (1f - (float)CurVertex.UVs[3].Y));
                }
            }
        }
    }

    public class MT_MaterialHelper : IImportHelper
    {
        public ulong Hash { get { return FNV64.Hash(Instance.Name); } }
        public MT_MaterialInstance Instance { get; private set; }
        public MaterialPreset Preset { get; private set; }
        public int LibraryIndex { get; set; }
        public IMaterial Material { get; private set; }

        public MT_MaterialHelper(MT_MaterialInstance InInstance)
        {
            Instance = InInstance;

            Preset = MaterialPreset.Default;
            LibraryIndex = 0;
        }

        public void Setup()
        {
            // We've been selected, so make sure we have a material to represent
            if (Material == null)
            {
                // Not really changed but does the same thing
                OnMaterialPresetChanged();
            }
        }

        public void Store()
        {

        }

        // NB: Not using properties here as I've had the problem of not functions not being called.
        // Just safer with a good old fashioned approach.
        public void SetPreset(MaterialPreset NewPreset)
        {
            if(Preset != NewPreset)
            {
                OnMaterialPresetChanged();
                Preset = NewPreset;
            }
        }

        private void OnMaterialPresetChanged()
        {
            Material = MaterialsManager.CreateMaterialNoStore(Hash, Instance.Name, Preset);

            // Only check if we have a diffuse.
            if (Instance.MaterialFlags.HasFlag(MT_MaterialInstanceFlags.HasDiffuse))
            {
                Material.SetTextureFor("S000", Instance.DiffuseTexture);
            }
        }
    }
}

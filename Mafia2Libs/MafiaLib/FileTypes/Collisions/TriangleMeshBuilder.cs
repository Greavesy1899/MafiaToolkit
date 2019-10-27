using System;
using System.Collections.Generic;
using System.IO;
using ResourceTypes.Collisions.Opcode;
using SharpDX;
using Utils;
using static Utils.Models.M2TStructure;

namespace ResourceTypes.Collisions
{
    public interface ITriangleMeshBuilder
    {
        TriangleMesh Build();
    }

    public class TriangleMeshBuilderFromM2TStructure : TriangleMesh, ITriangleMeshBuilder
    {
        private readonly Lod sourceModel;

        public TriangleMeshBuilderFromM2TStructure(Lod sourceModel)
        {
            this.sourceModel = sourceModel;
        }

        public TriangleMesh Build()
        {
            IList<Vector3> vertexList = new List<Vector3>();
            foreach (var vertex in sourceModel.Vertices)
            {
                vertexList.Add(vertex.Position);
            }

            IList<Triangle> triangleList = new List<Triangle>();
            for (int i = 0; i < sourceModel.Indices.Length; i += 3)
            {
                triangleList.Add(new Triangle(sourceModel.Indices[i], sourceModel.Indices[i + 1], sourceModel.Indices[i + 2]));
            }

            IList<ushort> materialsList = new List<ushort>();
            foreach (ModelPart modelPart in sourceModel.Parts)
            {
                CollisionMaterials partMaterial;
                if (!Enum.TryParse(modelPart.Material, true, out partMaterial))
                {
                    partMaterial = CollisionMaterials.Concrete; // fallback material
                };

                for (int i = (int) modelPart.StartIndex; i < modelPart.StartIndex + modelPart.NumFaces; i++)
                {
                    materialsList.Add((ushort)partMaterial);
                }
            }

            Init(vertexList, triangleList, materialsList);

            using (BinaryWriter writer = new BinaryWriter(File.Open("mesh.bin", FileMode.Create)))
            {
                Save(writer);
            }

            FBXHelper.CookTriangleCollision("mesh.bin", "cook.bin");

            TriangleMesh cookedTriangleMesh = new TriangleMesh();
            using (BinaryReader reader = new BinaryReader(File.Open("cook.bin", FileMode.Open)))
            {
                cookedTriangleMesh.Load(reader);
            }


            if (File.Exists("mesh.bin")) File.Delete("mesh.bin");
            if (File.Exists("cook.bin")) File.Delete("cook.bin");

            cookedTriangleMesh.Force32BitIndices();
            return cookedTriangleMesh;
        }

        private void Init(IList<Vector3> vertexList, IList<Triangle> triangleList, IList<ushort> materialsList)
        {
            SerialFlags = MeshSerialFlags.MSF_MATERIALS;
            Vertices = vertexList;
            Triangles = triangleList;
            MaterialIndices = materialsList;
            ExtraTriangleData = new List<byte>(new byte[Triangles.Count]);
        }
    }
}
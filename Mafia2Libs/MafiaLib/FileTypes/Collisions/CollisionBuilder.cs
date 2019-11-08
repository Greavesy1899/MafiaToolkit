using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Gibbed.Illusion.FileFormats.Hashing;
using ResourceTypes.Collisions.Opcode;
using SharpDX;
using Utils;
using Utils.Models;
using static Utils.Models.M2TStructure;

namespace ResourceTypes.Collisions
{
    public class CollisionModelBuilder
    {
        public Collision.CollisionModel BuildFromM2TStructure(M2TStructure sourceModel)
        {
            Collision.CollisionModel collisionModel = new Collision.CollisionModel();
            collisionModel.Hash = FNV64.Hash(sourceModel.Name);

            Lod modelLod = sourceModel.Lods[0];

            IList<Vector3> vertexList = modelLod.Vertices.Select(v => v.Position).ToList();

            // Material sections should be unique and sorted by material

            var sortedParts = new SortedDictionary<ushort, List<ModelPart>>(
                modelLod.Parts
                    .GroupBy(p => MaterialToIndex(p.Material))
                    .ToDictionary(p => p.Key, p => p.ToList())
            );
            collisionModel.Sections = new List<Collision.Section>(sortedParts.Count);

            IList<TriangleMesh.Triangle> orderedTriangles = new List<TriangleMesh.Triangle>(modelLod.Indices.Length / 3);
            IList<ushort> materials = new List<ushort>();

            foreach (var part in sortedParts)
            {
                var sameMaterialParts = part.Value;
                collisionModel.Sections.Add(new Collision.Section
                {
                    Material = part.Key - 2,
                    Start = orderedTriangles.Count * 3,
                    NumEdges = (int) sameMaterialParts.Sum(p => p.NumFaces) * 3
                });

                foreach (var sameMaterialPart in sameMaterialParts)
                {
                    var start = (int) sameMaterialPart.StartIndex;
                    var end = start + sameMaterialPart.NumFaces * 3;
                    for (int ci = start; ci < end; ci += 3)
                    {
                        orderedTriangles.Add(new TriangleMesh.Triangle(
                            modelLod.Indices[ci],
                            modelLod.Indices[ci + 1],
                            modelLod.Indices[ci + 2]
                        ));
                        materials.Add(part.Key);
                    }
                }
            }

            collisionModel.Mesh = new TriangleMeshBuilder().Build(vertexList, orderedTriangles, materials);
            return collisionModel;
        }

        private ushort MaterialToIndex(string material)
        {
            CollisionMaterials parsedMaterial;
            if (!Enum.TryParse(material, true, out parsedMaterial))
                parsedMaterial = CollisionMaterials.Concrete; // fallback material
            return (ushort) parsedMaterial;
        }
    }

    public class TriangleMeshBuilder : TriangleMesh
    {
        public TriangleMesh Build(IList<Vector3> vertexList, IList<Triangle> triangleList, IList<ushort> materialsList)
        {
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
            ExtraTriangleData = new List<ExtraTrigDataFlag>(new ExtraTrigDataFlag[Triangles.Count]);
        }
    }
}
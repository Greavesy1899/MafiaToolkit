using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using Gibbed.Illusion.FileFormats.Hashing;
using ResourceTypes.ModelHelpers.ModelExporter;
using Utils.Helpers;
using Utils.VorticeUtils;

namespace ResourceTypes.Collisions
{
    public class CollisionModelBuilder
    {
        public Collision.CollisionModel BuildFromMTCollision(string Name, MT_Collision CollisionObject)
        {
            Collision.CollisionModel collisionModel = new Collision.CollisionModel();
            collisionModel.Hash = FNV64.Hash(Name);

            IList<Vector3> vertexList = CollisionObject.Vertices.ToList();

            // Material sections should be unique and sorted by material
            var sortedParts = new SortedDictionary<ushort, List<MT_FaceGroup>>(
                CollisionObject.FaceGroups
                    .GroupBy(p => CollisionEnumUtils.MaterialNameToIndex(p.Material.Name))
                    .ToDictionary(p => p.Key, p => p.ToList())
            );
            collisionModel.Sections = new List<Collision.Section>(sortedParts.Count);

            IList<TriangleMesh.Triangle> orderedTriangles = new List<TriangleMesh.Triangle>(CollisionObject.Indices.Length / 3);
            IList<ushort> materials = new List<ushort>();

            foreach (var part in sortedParts)
            {
                var sameMaterialParts = part.Value;
                collisionModel.Sections.Add(new Collision.Section
                {
                    Material = part.Key - 2,
                    Start = orderedTriangles.Count * 3,
                    NumEdges = (int)sameMaterialParts.Sum(p => p.NumFaces) * 3
                });

                foreach (var sameMaterialPart in sameMaterialParts)
                {
                    var start = (int)sameMaterialPart.StartIndex;
                    var end = start + sameMaterialPart.NumFaces * 3;
                    for (int ci = start; ci < end; ci += 3)
                    {
                        orderedTriangles.Add(new TriangleMesh.Triangle(
                            CollisionObject.Indices[ci],
                            CollisionObject.Indices[ci + 1],
                            CollisionObject.Indices[ci + 2]
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

    public class TriangleCooking : TriangleMesh
    {
        public TriangleMesh Cook(TriangleMesh mesh)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open("Mesh.bin", FileMode.Create)))
            {
                WriteRawFormatToFile(writer, mesh);
            }

            PhysXHelper.CookTriangleCollision("Mesh.bin", "Cook.bin");

            TriangleMesh cookedTriangleMesh = new TriangleMesh();
            using (BinaryReader reader = new BinaryReader(File.Open("Cook.bin", FileMode.Open)))
            {
                cookedTriangleMesh.Load(reader);
            }

            if (File.Exists("Mesh.bin"))
            {
                File.Delete("Mesh.bin");
            }

            if (File.Exists("Cook.bin"))
            {
                File.Delete("Cook.bin");
            }

            cookedTriangleMesh.Force32BitIndices();
            return cookedTriangleMesh;
        }

        public void WriteRawFormatToFile(BinaryWriter writer, TriangleMesh mesh)
        {
            writer.Write(mesh.NumVertices);
            foreach (var Entry in mesh.Vertices)
            {
                Vector3Utils.WriteToFile(Entry, writer);
            }

            writer.Write(mesh.NumTriangles * 3); // Write Number of Indices, not triangles.
            foreach (var Entry in mesh.Triangles)
            {
                writer.Write(Entry.v0);
                writer.Write(Entry.v1);
                writer.Write(Entry.v2);
            }

            writer.Write(mesh.MaterialIndices.Count);
            foreach (var Entry in mesh.MaterialIndices)
            {
                writer.Write(Entry);
            }
        }

    }
    public class TriangleMeshBuilder : TriangleMesh
    {
        public TriangleMesh Build(IList<Vector3> vertexList, IList<Triangle> triangleList, IList<ushort> materialsList)
        {
            Init(vertexList, triangleList, materialsList);
            TriangleCooking cooked = new TriangleCooking();
            return cooked.Cook(this);
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
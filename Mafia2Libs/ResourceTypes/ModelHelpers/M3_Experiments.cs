using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Utils.Models;
using ResourceTypes.FrameResource;
using static ResourceTypes.FrameResource.FrameLOD;
using System.Windows;
using SharpDX;

namespace Mafia2Tool.MafiaLib.ModelHelpers
{
    public class M3_ExperimentalTests
    {
        byte[] VertexBuffer = null;
        byte[] IndexBuffer = null;
        int VertexCount = 17751;
        Vector3 Offset;
        float Scale;
        VertexFlags VertexDecleration = 0;

        private static int GetVertexComponentLength(VertexFlags flags)
        {
            switch (flags)
            {
                case VertexFlags.Position:
                case VertexFlags.Skin:
                    return 8;
                case VertexFlags.Normals:
                case VertexFlags.Color:
                case VertexFlags.TexCoords0:
                case VertexFlags.TexCoords1:
                case VertexFlags.TexCoords2:
                case VertexFlags.Unk05:
                case VertexFlags.ShadowTexture:
                case VertexFlags.Color1:
                case VertexFlags.DamageGroup:
                    return 4;
                case VertexFlags.Tangent:
                    return 0;
                case VertexFlags.BBCoeffs:
                    return 12;
                default:
                    return -1;
            }
        }

        public Dictionary<VertexFlags, VertexOffset> GetVertexOffsets(out int stride)
        {
            Dictionary<VertexFlags, VertexOffset> dictionary = new Dictionary<VertexFlags, VertexOffset>();
            int num = 0;
            foreach (VertexFlags vertexFlags in VertexFlagOrder)
            {
                if (VertexDecleration.HasFlag(vertexFlags))
                {
                    int vertexComponentLength = GetVertexComponentLength(vertexFlags);
                    if (vertexComponentLength > 0)
                    {
                        VertexOffset vertexOffset = new VertexOffset()
                        {
                            Offset = num,
                            Length = vertexComponentLength
                        };
                        dictionary.Add(vertexFlags, vertexOffset);
                        num += vertexComponentLength;
                    }
                }
            }
            stride = num;
            return dictionary;
        }

        public void ReadPrerequisites(string name)
        {
            using(BinaryReader reader = new BinaryReader(File.Open(name, FileMode.Open)))
            {
                ushort MeshFlags = reader.ReadUInt16();
                VertexDecleration = (VertexFlags)reader.ReadUInt32();

                int IndexSize = reader.ReadInt32();
                IndexBuffer = reader.ReadBytes(IndexSize);

                VertexCount = reader.ReadInt32();
                int VertexSize = reader.ReadInt32();
                VertexBuffer = reader.ReadBytes(VertexSize);
            }

            //VertexDecleration = (VertexFlags)134933;
            //Offset = new Vector3(-233.86592f, -642.7829f, -1.5614158f);
            //Scale = 0.013024263f;

            int vertexSize;
            Dictionary<VertexFlags, FrameLOD.VertexOffset> vertexOffsets = GetVertexOffsets(out vertexSize);

            int tempVertexSize = VertexBuffer.Length / VertexCount;
            Vertex[] Vertices = new Vertex[VertexCount];

            for (int x = 0; x != VertexCount; x++)
            {
                //declare data required and send to decompresser
                byte[] data = new byte[tempVertexSize];
                Array.Copy(VertexBuffer, (x * tempVertexSize), data, 0, tempVertexSize);
                Vertex decompressed = VertexTranslator.DecompressVertex(data, VertexDecleration, Vector3.Zero, 1.525879E-05f, vertexOffsets);
                Vertices[x] = decompressed;
            }

            Int3[] Triangles = new Int3[IndexBuffer.Length / 3];
            int index = 0;
            for (int y = 0; y != IndexBuffer.Length; y+=6)
            {
                Int3 triangle = new Int3();
                triangle.X = BitConverter.ToUInt16(IndexBuffer, y + 0);
                triangle.Y = BitConverter.ToUInt16(IndexBuffer, y + 2);
                triangle.Z = BitConverter.ToUInt16(IndexBuffer, y + 4);
                Triangles[index] = triangle;
                index++;
            }

            File.WriteAllLines(name+".obj", BuildMesh(Vertices, Triangles));
        }

        private string[] BuildMesh(Vertex[] Vertices, Int3[] Triangles)
        {
            List<string> file = new List<string>();

            foreach (var vert in Vertices)
            {
                file.Add(string.Format("v {0} {1} {2}", vert.Position.X, vert.Position.Y, vert.Position.Z));
            }

            file.Add("");
            file.Add("g mesh");
            file.Add("");

            foreach (var tri in Triangles)
            {
                file.Add(string.Format("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}", tri.X + 1, tri.Y + 1, tri.Z + 1));
            }

            return file.ToArray();
        }
    }
}

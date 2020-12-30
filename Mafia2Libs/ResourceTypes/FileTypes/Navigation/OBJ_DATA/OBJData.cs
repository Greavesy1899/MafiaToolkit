using SharpDX;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.SharpDXExtensions;
using Utils.StringHelpers;

namespace ResourceTypes.Navigation
{
    public class OBJData
    {
        public struct ConnectionStruct
        {
            uint flags;
            uint nodeID;
            uint connectedNodeID;

            public uint Flags {
                get { return flags; }
                set { flags = value; }
            }
            public uint NodeID {
                get { return nodeID; }
                set { nodeID = value; }
            }
            public uint ConnectedNodeID {
                get { return connectedNodeID; }
                set { connectedNodeID = value; }
            }
        }
        public struct VertexStruct
        {
            uint unk7;
            Vector3 position;
            float unk0;
            float unk1;
            int unk2;
            short unk3;
            short unk4;
            int unk5;
            int unk6;

            public uint Unk7 {
                get { return unk7; }
                set { unk7 = value; }
            }
            public Vector3 Position {
                get { return position; }
                set { position = value; }
            }
            public float Unk0 {
                get { return unk0; }
                set { unk0 = value; }
            }
            public float Unk1 {
                get { return unk1; }
                set { unk1 = value; }
            }
            public int Unk2 {
                get { return unk2; }
                set { unk2 = value; }
            }
            public short Unk3 {
                get { return unk3; }
                set { unk3 = value; }
            }
            public short Unk4 {
                get { return unk4; }
                set { unk4 = value; }
            }
            public int Unk5 {
                get { return unk5; }
                set { unk5 = value; }
            }
            public int Unk6 {
                get { return unk6; }
                set { unk6 = value; }
            }


            public override string ToString()
            {
                return string.Format("{0} {1} {2} {3} {4} {5} {6}", unk7, unk0, unk2, unk3, unk4, unk5, unk6);
            }
        }

        int unk0; // Usually 2
        int fileIDHPD;
        int unk3HPD; // Usually 100412
        int bitFlagsHPD;
        int vertSize;
        int triSize;
        public VertexStruct[] vertices;
        public ConnectionStruct[] connections;
        public KynogonRuntimeMesh runtimeMesh;

        public OBJData(BinaryReader reader)
        {
            ReadFromFile(reader);
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(unk0);
            writer.Write(fileIDHPD);
            writer.Write(unk3HPD);
            writer.Write(bitFlagsHPD);
            writer.Write(vertSize);
            writer.Write(triSize);

            for (int i = 0; i < vertices.Length; i++)
            {
                var vertex = vertices[i];
                writer.Write(vertex.Unk7);

                Vector3 pos = vertex.Position;
                float z = pos.Z;
                pos.Z = -pos.Y;
                pos.Y = z;
                vertex.Position = pos;
                Vector3Extenders.WriteToFile(vertex.Position, writer);
                writer.Write(vertex.Unk0);
                writer.Write(vertex.Unk1);
                writer.Write(vertex.Unk2);
                writer.Write(vertex.Unk3);
                writer.Write(vertex.Unk4);
                writer.Write(vertex.Unk5);
                writer.Write(vertex.Unk6);
            }

            for (int i = 0; i < connections.Length; i++)
            {
                var connection = connections[i];
                writer.Write(connection.Flags);
                writer.Write(connection.NodeID);
                writer.Write(connection.ConnectedNodeID);
            }

            runtimeMesh.WriteToFile(writer);
        }

        public void ReadFromFile(BinaryReader reader)
        {
            unk0 = reader.ReadInt32();
            fileIDHPD = reader.ReadInt32();
            unk3HPD = reader.ReadInt32();
            bitFlagsHPD = reader.ReadInt32();
            
            vertSize = reader.ReadInt32();
            triSize = reader.ReadInt32();
            //writer.WriteLine(string.Format("{0}, )

            StreamWriter writer = File.CreateText("NAV_AI_OBJ_DATA_" + fileIDHPD + ".txt");
            writer.WriteLine(string.Format("{0} {1} {2} {3}", unk0, fileIDHPD, unk3HPD, bitFlagsHPD));

            //List<string> data = new List<string>();
            List<Vector3> Points = new List<Vector3>();

            vertices = new VertexStruct[vertSize];
            for (int i = 0; i < vertSize; i++)
            {
                VertexStruct vertex = new VertexStruct();
                vertex.Unk7 = reader.ReadUInt32() & 0x7FFFFFFF;
                vertex.Position = Vector3Extenders.ReadFromFile(reader);
                Vector3 pos = vertex.Position;
                float y = pos.Y;
                pos.Y = -pos.Z;
                pos.Z = y;
                vertex.Position = pos;
                //writer.WriteLine(vertex.Position);
                vertex.Unk0 = reader.ReadSingle();
                vertex.Unk1 = reader.ReadSingle();
                vertex.Unk2 = reader.ReadInt32();
                vertex.Unk3 = reader.ReadInt16();
                vertex.Unk4 = reader.ReadInt16();
                vertex.Unk5 = reader.ReadInt32();
                vertex.Unk6 = reader.ReadInt32();
                vertices[i] = vertex;

                Points.Add(vertex.Position);
            }

            writer.WriteLine("");
            connections = new ConnectionStruct[triSize];
            for (int i = 0; i < triSize; i++)
            {
                ConnectionStruct connection = new ConnectionStruct();
                connection.Flags = reader.ReadUInt32() & 0x7FFFFFFF;
                connection.NodeID = reader.ReadUInt32() & 0x7FFFFFFF;
                connection.ConnectedNodeID = reader.ReadUInt32() & 0x7FFFFFFF;
                connections[i] = connection;
                //writer.WriteLine(string.Format("{0} {1} {2}", connection.Flags, connection.NodeID, connection.ConnectedNodeID));
            }

            //Read KynogonRuntimeMesh
            runtimeMesh = new KynogonRuntimeMesh();
            runtimeMesh.ReadFromFile(reader, writer);
        }
    }
}

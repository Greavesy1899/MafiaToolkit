using System.Collections.Generic;
using System.IO;
using System.Numerics;
using Utils.VorticeUtils;

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
        public class VertexStruct
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

            public List<VertexStruct> IncomingConnections { get; set; }
            public List<VertexStruct> OutgoingConnections { get; set; }

            public VertexStruct()
            {
                IncomingConnections = new List<VertexStruct>();
                OutgoingConnections = new List<VertexStruct>();
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
        byte[] Footer;

        public OBJData(BinaryReader reader)
        {
            ReadFromFile(reader);
        }

        public void ReadFromFile(BinaryReader reader)
        {
            unk0 = reader.ReadInt32();
            fileIDHPD = reader.ReadInt32();
            unk3HPD = reader.ReadInt32();
            bitFlagsHPD = reader.ReadInt32();
            
            vertSize = reader.ReadInt32();
            triSize = reader.ReadInt32();

            List<Vector3> Points = new List<Vector3>();

            vertices = new VertexStruct[vertSize];
            for (int i = 0; i < vertSize; i++)
            {
                VertexStruct vertex = new VertexStruct();
                vertex.Unk7 = reader.ReadUInt32(); // ^ 0x80000000
                vertex.Position = Vector3Extenders.ReadFromFile(reader);  // TODO: Construct KynogonUtils to accomodate this
                Vector3 pos = vertex.Position;
                float y = pos.Y;
                pos.Y = -pos.Z;
                pos.Z = y;
                vertex.Position = pos;
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

            connections = new ConnectionStruct[triSize];
            for (int i = 0; i < triSize; i++)
            {
                ConnectionStruct connection = new ConnectionStruct();
                connection.Flags = reader.ReadUInt32() ^ 0x80000000;
                connection.NodeID = reader.ReadUInt32();
                connection.ConnectedNodeID = reader.ReadUInt32();
                connections[i] = connection;
            }

            //Read KynogonRuntimeMesh
            runtimeMesh = new KynogonRuntimeMesh();
            runtimeMesh.ReadFromFile(reader);

            // read footer
            /*if (!runtimeMesh.bDEBUG_HASEXTRADATA)
            {
                Name = StringHelpers.ReadString(reader);
                uint SizeofName = reader.ReadUInt32();
                uint Header = reader.ReadUInt32();
            }*/

            Footer = reader.ReadBytes((int)(reader.BaseStream.Length - reader.BaseStream.Position));

            GenerateConnections();
            DumpToASCII("NAV_OBJ_DATA_"+fileIDHPD+".txt");
        }

        public void WriteToFile(NavigationWriter writer)
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
                writer.Write(vertex.Unk7); // | 0x80000000

                Vector3 pos = vertex.Position;
                float z = pos.Z; // TODO: Construct KynogonUtils to accomodate this
                pos.Z = -pos.Y;
                pos.Y = z;
                Vector3Extenders.WriteToFile(pos, writer); // NB: DO NOT SET vertex.Position as pos!!
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
                writer.Write(connection.Flags | 0x80000000);
                writer.Write(connection.NodeID);
                writer.Write(connection.ConnectedNodeID);
            }

            runtimeMesh.WriteToFile(writer);

            writer.Write(Footer);

            // write footer
            /* if (!runtimeMesh.bDEBUG_HASEXTRADATA)
             {
                 writer.Write(Padding);
                 StringHelpers.WriteString(writer, Name);
                 writer.Write(Name.Length + 1); // extra 1 is the padding
                 writer.Write(303296513);
             }*/
        }

        public void GenerateConnections()
        {
            for (int i = 0; i < vertSize; i++)
            {
                VertexStruct vertex = vertices[i];

                int ConnectionOffset = vertex.Unk2 - 1;
                if (ConnectionOffset != -1)
                {
                    bool bEndOfArray = false;
                    ConnectionStruct CurConnection = connections[ConnectionOffset];
                    while(CurConnection.NodeID == i && !bEndOfArray)
                    {
                        VertexStruct ConnectedVertex = vertices[CurConnection.ConnectedNodeID];
                        vertex.OutgoingConnections.Add(ConnectedVertex);
                        ConnectedVertex.IncomingConnections.Add(vertex);

                        ConnectionOffset++;
                        if (ConnectionOffset >= connections.Length)
                        {
                            bEndOfArray = true;
                        }
                        else
                        {
                            CurConnection = connections[ConnectionOffset];
                        }
                    }
                }
            }
        }

        public void DumpToASCII(string name)
        {
            using (StreamWriter writer = new StreamWriter(name + ".txt"))
            {
                writer.WriteLine("HEADER -------------");
                writer.WriteLine("Unk0: {0}", unk0);
                writer.WriteLine("FileID_HPD: {0}", fileIDHPD);
                writer.WriteLine("Unk3_HPD: {0}", unk3HPD);
                writer.WriteLine("BitFlags_HPD: {0}", bitFlagsHPD);
                writer.WriteLine("Vertex Size: {0}", vertSize);
                writer.WriteLine("Connection Size: {0}", triSize);
                writer.WriteLine("");

                writer.WriteLine("NODES---------------");
                int Index = 0;
                foreach (var vertex in vertices)
                {
                    writer.WriteLine("Idx: {9} - \tUnk0: {0} \tUnk1: {1} \tUnk2: {2} \tUnk3: {3} \tUnk4: {4} \tUnk5: {5} \tUnk6: {6} \tUnk7: {7} \tPosition: {8}",
                        vertex.Unk0, vertex.Unk1, vertex.Unk2, vertex.Unk3, vertex.Unk4, vertex.Unk5, vertex.Unk6, vertex.Unk7, vertex.Position, Index);

                    Index++;
                }

                Index = 0;
                writer.WriteLine("CONNECTIONS------------------");
                foreach (var Connection in connections)
                {
                    writer.WriteLine("Idx: {3} - \tFlags: \t{0} NodeID: \t{1} ConnectedNodeID: \t{2}",
                        Connection.Flags, Connection.NodeID, Connection.ConnectedNodeID, Index);

                    Index++;
                }

                runtimeMesh.DumpToASCII(writer);

                writer.Close();
            }
        }
    }
}

using SharpDX;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ResourceTypes.Collisions.Opcode.SerializationUtils;

namespace ResourceTypes.Collisions.Opcode
{
    public class TriangleMesh
    {
        [Flags]
        enum MeshSerialFlags
        {
            MSF_MATERIALS = (1 << 0),
            MSF_FACE_REMAP = (1 << 1),
            MSF_HARDWARE_MESH = (1 << 2),
            MSF_8BIT_INDICES = (1 << 3),
            MSF_16BIT_INDICES = (1 << 4),
        }

        struct Triangle
        {
            uint v0;
            uint v1;
            uint v2;

            public Triangle(uint v0, uint v1, uint v2)
            {
                this.v0 = v0;
                this.v1 = v1;
                this.v2 = v2;
            }
        }

        private const uint SUPPORTED_MESH_VERSION = 1;

        private bool littleEndian = true;

        private MeshSerialFlags serialFlags;
        private float convexEdgeThreshold;
        private uint heightFieldVerticalAxis; // TODO: make enum
        private float heightFieldVerticalExtent;
        private uint numVertices; // TODO: remove and use vertices.Count ?
        private uint numTriangles; // TODO: remove and use triangles.Count ?
        private IList<Vector3> vertices = new List<Vector3>();
        private IList<Triangle> triangles = new List<Triangle>(); 
        private IList<ushort> materialIndices = new List<ushort>();
        private IList<uint> remapIndices = new List<uint>();
        private uint numConvexParts;
        private uint numFlatParts;
        private IList<ushort> convexParts = new List<ushort>();
        private IList<ushort> flatParts = new List<ushort>();
        private HybridModel hybridModel = new HybridModel();
        private float geomEpsilon;
        private BoundingBox boundingBox = new BoundingBox(Vector3.Zero, Vector3.Zero);
        private BoundingSphere boundingSphere = new BoundingSphere(Vector3.Zero, 0.0f);
        private float mass;
        private Matrix3x3 inertiaTensor = Matrix3x3.Zero;
        private Vector3 centerOfMass = Vector3.Zero;
        private IList<byte> extraTriangleData = new List<byte>(); // TODO: flags enum

        public void Load(BinaryReader reader)
        {
            char h0, h1, h2, h3;
            uint version;
            
            if (!ReadHeader('N', 'S', 'X', out h0, out h1, out h2, out h3, out version, out littleEndian, reader))
            {
                throw new OpcodeException("Invalid 'NSX' header"); 
            }
            if ((h0 != 'M') && (h1 != 'E') && (h2 != 'S') && (h3 != 'H'))
            {
                throw new OpcodeException("Invalid 'MESH' header");
            }
            if (version != SUPPORTED_MESH_VERSION)
            {
                throw new OpcodeException($"Unsupported mesh version {version}");
            }

            bool platformMismatch = !littleEndian;

            serialFlags = (MeshSerialFlags)ReadDword(reader, platformMismatch);
            convexEdgeThreshold = ReadFloat(reader, platformMismatch);
            heightFieldVerticalAxis = ReadDword(reader, platformMismatch);
            numVertices = ReadDword(reader, platformMismatch);
            numTriangles = ReadDword(reader, platformMismatch);

            ReadVertices(numVertices, reader, platformMismatch);
            ReadTriangles(numTriangles, serialFlags, reader, platformMismatch);
            ReadMaterialIndices(numTriangles, serialFlags, reader, platformMismatch);
            ReadRemapIndices(numTriangles, serialFlags, reader, platformMismatch);

            numConvexParts = ReadDword(reader, platformMismatch);
            numFlatParts = ReadDword(reader, platformMismatch);
            ReadConvexParts(numConvexParts, numTriangles, reader, platformMismatch);
            ReadFlatParts(numFlatParts, numTriangles, reader, platformMismatch);

            // TODO: hybridModel.Load(reader, platformMismatch)

            geomEpsilon = ReadFloat(reader, platformMismatch);
            ReadBounds(reader, platformMismatch);
            ReadPhysProperties(reader, platformMismatch);
            ReadExtraTriangleData(numTriangles, reader, platformMismatch);
        }

        private void ReadVertices(uint numVertices, BinaryReader reader, bool platformMismatch)
        {
            vertices = new List<Vector3>((int)numVertices);
            for (int i = 0; i < numVertices; i++)
            {
                float x = ReadFloat(reader, platformMismatch);
                float y = ReadFloat(reader, platformMismatch);
                float z = ReadFloat(reader, platformMismatch);
                vertices.Add(new Vector3(x, y, z));
            }
        }

        private void ReadTriangles(uint numTriangles, MeshSerialFlags serialFlags, BinaryReader reader, bool platformMismatch)
        {
            if (serialFlags.HasFlag(MeshSerialFlags.MSF_8BIT_INDICES) && serialFlags.HasFlag(MeshSerialFlags.MSF_16BIT_INDICES))
            {
                throw new OpcodeException($"Invalid serial flags {serialFlags}");
            }
            
            triangles = new List<Triangle>((int)numTriangles);

            if (serialFlags.HasFlag(MeshSerialFlags.MSF_8BIT_INDICES))
            {
                for (int i = 0; i < numTriangles; i++)
                {
                    uint v0 = reader.ReadByte();
                    uint v1 = reader.ReadByte();
                    uint v2 = reader.ReadByte();
                    triangles.Add(new Triangle(v0, v1, v2));
                }
            }
            else if (serialFlags.HasFlag(MeshSerialFlags.MSF_16BIT_INDICES))
            {
                for (int i = 0; i < numTriangles; i++)
                {
                    uint v0 = ReadWord(reader, platformMismatch);
                    uint v1 = ReadWord(reader, platformMismatch);
                    uint v2 = ReadWord(reader, platformMismatch);
                    triangles.Add(new Triangle(v0, v1, v2));
                }
            }
            else
            {
                for (int i = 0; i < numTriangles; i++)
                {
                    uint v0 = ReadDword(reader, platformMismatch);
                    uint v1 = ReadDword(reader, platformMismatch);
                    uint v2 = ReadDword(reader, platformMismatch);
                    triangles.Add(new Triangle(v0, v1, v2));
                }
            }
        }

        private void ReadMaterialIndices(uint numTriangles, MeshSerialFlags serialFlags, BinaryReader reader, bool platformMismatch)
        {
            if (serialFlags.HasFlag(MeshSerialFlags.MSF_MATERIALS))
            {
                materialIndices = new List<ushort>((int)numTriangles);
                for (int i = 0; i < numTriangles; i++)
                {
                    ushort mi = ReadWord(reader, platformMismatch);
                    materialIndices.Add(mi);
                }
            }
            else
            {
                materialIndices.Clear();
            }
        }

        private void ReadRemapIndices(uint numTriangles, MeshSerialFlags serialFlags, BinaryReader reader, bool platformMismatch)
        {
            if (serialFlags.HasFlag(MeshSerialFlags.MSF_FACE_REMAP))
            {
                uint maxIndex = ReadDword(reader, platformMismatch);
                remapIndices = ReadIndices(maxIndex, numTriangles, reader, platformMismatch);
            }
            else
            {
                remapIndices.Clear();
            }
        }

        private void ReadConvexParts(uint numConvexParts, uint numTriangles, BinaryReader reader, bool platformMismatch)
        {
            if (numConvexParts > 0)
            {
                convexParts = new List<ushort>((int)numTriangles);
                for (int i = 0; i < numTriangles; i++)
                {
                    ushort part = ReadWord(reader, platformMismatch);
                    convexParts.Add(part);
                }
            }
            else
            {
                convexParts.Clear();
            }
        }

        private void ReadFlatParts(uint numFlatParts, uint numTriangles, BinaryReader reader, bool platformMismatch)
        {
            if (numFlatParts > 0)
            {
                flatParts = new List<ushort>((int)numTriangles);
                if (numFlatParts < 256)
                {
                    for (int i = 0; i < numTriangles; i++)
                    {
                        ushort part = reader.ReadByte();
                        flatParts.Add(part);
                    }
                }
                else
                {
                    for (int i = 0; i < numTriangles; i++)
                    {
                        ushort part = ReadWord(reader, platformMismatch);
                        flatParts.Add(part);
                    }
                }
            }
            else
            {
                flatParts.Clear();
            }
        }

        private void ReadBounds(BinaryReader reader, bool platformMismatch)
        {
            boundingSphere.Center.X = ReadFloat(reader, platformMismatch);
            boundingSphere.Center.Y = ReadFloat(reader, platformMismatch);
            boundingSphere.Center.Z = ReadFloat(reader, platformMismatch);
            boundingSphere.Radius = ReadFloat(reader, platformMismatch);
            boundingBox.Minimum.X = ReadFloat(reader, platformMismatch);
            boundingBox.Minimum.Y = ReadFloat(reader, platformMismatch);
            boundingBox.Minimum.Z = ReadFloat(reader, platformMismatch);
            boundingBox.Maximum.X = ReadFloat(reader, platformMismatch);
            boundingBox.Maximum.Y = ReadFloat(reader, platformMismatch);
            boundingBox.Maximum.Z = ReadFloat(reader, platformMismatch);
        }

        private void ReadPhysProperties(BinaryReader reader, bool platformMismatch)
        {
            mass = ReadFloat(reader, platformMismatch);
            inertiaTensor.M11 = ReadFloat(reader, platformMismatch);
            inertiaTensor.M12 = ReadFloat(reader, platformMismatch);
            inertiaTensor.M13 = ReadFloat(reader, platformMismatch);
            inertiaTensor.M21 = ReadFloat(reader, platformMismatch);
            inertiaTensor.M22 = ReadFloat(reader, platformMismatch);
            inertiaTensor.M23 = ReadFloat(reader, platformMismatch);
            inertiaTensor.M31 = ReadFloat(reader, platformMismatch);
            inertiaTensor.M32 = ReadFloat(reader, platformMismatch);
            inertiaTensor.M33 = ReadFloat(reader, platformMismatch);
            centerOfMass.X = ReadFloat(reader, platformMismatch);
            centerOfMass.Y = ReadFloat(reader, platformMismatch);
            centerOfMass.Z = ReadFloat(reader, platformMismatch);
    }

        private void ReadExtraTriangleData(uint numTriangles, BinaryReader reader, bool platformMismatch)
        {
            uint numExtraTriangleData = ReadDword(reader, platformMismatch);
            if (numExtraTriangleData != numTriangles)
            {
                throw new OpcodeException($"Number of ExtraTriangleData {numExtraTriangleData} does not match the number of triangles {numTriangles}");
            }

            List<byte> data = new List<byte>((int)numExtraTriangleData);
            byte[] bytes = reader.ReadBytes((int)numExtraTriangleData);
            data.AddRange(bytes);
            extraTriangleData = data;
        }


        public void Save(BinaryWriter writer, bool littleEndian = true)
        {

        }
    }
}

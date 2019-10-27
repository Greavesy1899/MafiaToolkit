using SharpDX;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using static ResourceTypes.Collisions.Opcode.SerializationUtils;

namespace ResourceTypes.Collisions.Opcode
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class TriangleMesh : IOpcodeSerializable
    {
        [Flags]
        public enum MeshSerialFlags
        {
            // ReSharper disable InconsistentNaming
            MSF_MATERIALS = (1 << 0),
            MSF_FACE_REMAP = (1 << 1),
            MSF_HARDWARE_MESH = (1 << 2),
            MSF_8BIT_INDICES = (1 << 3),
            MSF_16BIT_INDICES = (1 << 4),
            // ReSharper restore InconsistentNaming
        }

        /// <summary>Specifies which axis is the "up" direction for a heightfield.</summary>
        enum NxHeightFieldAxis : uint
        {
            // ReSharper disable InconsistentNaming
            NX_X = 0, //!< X Axis
            NX_Y = 1, //!< Y Axis
            NX_Z = 2, //!< Z Axis
            NX_NOT_HEIGHTFIELD = 0xff //!< Not a heightfield
            // ReSharper restore InconsistentNaming
        };

        /// <summary>Structure used to store indices for a triangles points.</summary>
        public struct Triangle
        {
            public uint v0;
            public uint v1;
            public uint v2;

            public Triangle(uint v0, uint v1, uint v2)
            {
                this.v0 = v0;
                this.v1 = v1;
                this.v2 = v2;
            }
        }

        private const uint SUPPORTED_MESH_VERSION = 1;

        public MeshSerialFlags SerialFlags { get; protected set; }
        private float convexEdgeThreshold = 0.001f;
        private NxHeightFieldAxis heightFieldVerticalAxis = NxHeightFieldAxis.NX_NOT_HEIGHTFIELD;
        private float heightFieldVerticalExtent;
        public uint NumVertices => (uint)Vertices.Count;
        public uint NumTriangles => (uint)Triangles.Count;
        public IList<Vector3> Vertices { get; protected set; } = new List<Vector3>();
        public IList<Triangle> Triangles { get; protected set; } = new List<Triangle>(); 
        public IList<ushort> MaterialIndices { get; protected set; } = new List<ushort>();
        private IList<uint> remapIndices = new List<uint>();
        private uint numConvexParts;
        private uint numFlatParts;
        private IList<ushort> convexParts = new List<ushort>();
        private IList<ushort> flatParts = new List<ushort>();
        private HybridModel hybridModel = new HybridModel();
        private float geomEpsilon;
        public BoundingBox BoundingBox { get; private set; } = new BoundingBox(Vector3.Zero, Vector3.Zero);
        private BoundingSphere boundingSphere = new BoundingSphere(Vector3.Zero, 0.0f);
        private float mass;
        private Matrix3x3 inertiaTensor = Matrix3x3.Zero;
        private Vector3 centerOfMass = Vector3.Zero;
        public IList<byte> ExtraTriangleData { get; protected set; } = new List<byte>(); // TODO: flags enum


        #region Load 
        public void Load(BinaryReader reader)
        {
            char h0, h1, h2, h3;
            uint version;
            bool littleEndian;

            if (!ReadHeader('N', 'X', 'S', out h0, out h1, out h2, out h3, out version, out littleEndian, reader))
            {
                throw new OpcodeException("Invalid 'NXS' header");
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

            SerialFlags = (MeshSerialFlags)ReadDword(reader, platformMismatch);
            convexEdgeThreshold = ReadFloat(reader, platformMismatch);
            heightFieldVerticalAxis = (NxHeightFieldAxis) ReadDword(reader, platformMismatch);
            heightFieldVerticalExtent = ReadFloat(reader, platformMismatch);
            uint numVertices = ReadDword(reader, platformMismatch);
            uint numTriangles = ReadDword(reader, platformMismatch);

            ReadVertices(numVertices, reader, platformMismatch);
            ReadTriangles(numTriangles, SerialFlags, reader, platformMismatch);
            ReadMaterialIndices(numTriangles, SerialFlags, reader, platformMismatch);
            ReadRemapIndices(numTriangles, SerialFlags, reader, platformMismatch);

            numConvexParts = ReadDword(reader, platformMismatch);
            numFlatParts = ReadDword(reader, platformMismatch);
            ReadConvexParts(numConvexParts, numTriangles, reader, platformMismatch);
            ReadFlatParts(numFlatParts, numTriangles, reader, platformMismatch);

            uint modelSize = ReadDword(reader, platformMismatch); // TODO: do we need to keep it as field?
            hybridModel.Load(reader);

            geomEpsilon = ReadFloat(reader, platformMismatch);
            ReadBounds(reader, platformMismatch);
            ReadPhysProperties(reader, platformMismatch);
            ReadExtraTriangleData(numTriangles, reader, platformMismatch);
        }

        private void ReadVertices(uint numVertices, BinaryReader reader, bool platformMismatch)
        {
            Vertices = new List<Vector3>((int)numVertices);
            for (int i = 0; i < numVertices; i++)
            {
                float x = ReadFloat(reader, platformMismatch);
                float y = ReadFloat(reader, platformMismatch);
                float z = ReadFloat(reader, platformMismatch);
                Vertices.Add(new Vector3(x, y, z));
            }
        }

        private void ReadTriangles(uint numTriangles, MeshSerialFlags serialFlags, BinaryReader reader, bool platformMismatch)
        {
            if (serialFlags.HasFlag(MeshSerialFlags.MSF_8BIT_INDICES) && serialFlags.HasFlag(MeshSerialFlags.MSF_16BIT_INDICES))
            {
                throw new OpcodeException($"Invalid serial flags {serialFlags}");
            }

            Triangles = new List<Triangle>((int)numTriangles);

            if (serialFlags.HasFlag(MeshSerialFlags.MSF_8BIT_INDICES))
            {
                for (int i = 0; i < numTriangles; i++)
                {
                    uint v0 = reader.ReadByte();
                    uint v1 = reader.ReadByte();
                    uint v2 = reader.ReadByte();
                    Triangles.Add(new Triangle(v0, v1, v2));
                }
            }
            else if (serialFlags.HasFlag(MeshSerialFlags.MSF_16BIT_INDICES))
            {
                for (int i = 0; i < numTriangles; i++)
                {
                    uint v0 = ReadWord(reader, platformMismatch);
                    uint v1 = ReadWord(reader, platformMismatch);
                    uint v2 = ReadWord(reader, platformMismatch);
                    Triangles.Add(new Triangle(v0, v1, v2));
                }
            }
            else
            {
                for (int i = 0; i < numTriangles; i++)
                {
                    uint v0 = ReadDword(reader, platformMismatch);
                    uint v1 = ReadDword(reader, platformMismatch);
                    uint v2 = ReadDword(reader, platformMismatch);
                    Triangles.Add(new Triangle(v0, v1, v2));
                }
            }
        }

        private void ReadMaterialIndices(uint numTriangles, MeshSerialFlags serialFlags, BinaryReader reader, bool platformMismatch)
        {
            if (serialFlags.HasFlag(MeshSerialFlags.MSF_MATERIALS))
            {
                MaterialIndices = new List<ushort>((int)numTriangles);
                for (int i = 0; i < numTriangles; i++)
                {
                    ushort mi = ReadWord(reader, platformMismatch);
                    MaterialIndices.Add(mi);
                }
            }
            else
            {
                MaterialIndices.Clear();
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

            this.BoundingBox = new BoundingBox
            {
                Minimum =
                {
                    X = ReadFloat(reader, platformMismatch),
                    Y = ReadFloat(reader, platformMismatch),
                    Z = ReadFloat(reader, platformMismatch)
                },
                Maximum =
                {
                    X = ReadFloat(reader, platformMismatch),
                    Y = ReadFloat(reader, platformMismatch),
                    Z = ReadFloat(reader, platformMismatch)
                }
            };
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
            ExtraTriangleData = data;
        } 
        #endregion

        private void ValidateTriangleMesh()
        {
            if (SerialFlags.HasFlag(MeshSerialFlags.MSF_8BIT_INDICES) && SerialFlags.HasFlag(MeshSerialFlags.MSF_16BIT_INDICES))
            {
                throw new OpcodeException($"Invalid serial flags {SerialFlags}");
            }
            if (SerialFlags.HasFlag(MeshSerialFlags.MSF_MATERIALS) && MaterialIndices.Count != NumTriangles)
            {
                throw new OpcodeException($"Number of material indices ({MaterialIndices.Count}) does not match the number of triangles ({Triangles.Count})");
            }
            if (SerialFlags.HasFlag(MeshSerialFlags.MSF_FACE_REMAP) && remapIndices.Count != NumTriangles)
            {
                throw new OpcodeException($"Number of remap indices ({remapIndices.Count}) does not match the number of triangles ({Triangles.Count})");
            }
            if (numConvexParts > 0 && convexParts.Count != NumTriangles)
            {
                throw new OpcodeException($"Number of convex parts elements ({convexParts.Count}) does not match the number of triangles ({Triangles.Count})");
            }
            if (numFlatParts > 0 && flatParts.Count != NumTriangles)
            {
                throw new OpcodeException($"Number of flat parts elements ({flatParts.Count}) does not match the number of triangles ({Triangles.Count})");
            }
            if (ExtraTriangleData.Count != NumTriangles)
            {
                throw new OpcodeException($"Number of extra triangle data({ExtraTriangleData.Count}) does not match the number of triangles ({Triangles.Count})");
            }
        }

        public void Force32BitIndices()
        {
            SerialFlags &= ~MeshSerialFlags.MSF_8BIT_INDICES;
            SerialFlags &= ~MeshSerialFlags.MSF_16BIT_INDICES;
        }

        #region Save
        public void Save(BinaryWriter writer, Endian endian = Endian.LITTLE)
        {
            ValidateTriangleMesh();

            bool isLittleEndian = endian == Endian.LITTLE;
            bool platformMismatch = endian == Endian.BIG;

            WriteHeader('M', 'E', 'S', 'H', SUPPORTED_MESH_VERSION, isLittleEndian, writer);

            WriteDword((uint)SerialFlags, writer, platformMismatch);
            WriteFloat(convexEdgeThreshold, writer, platformMismatch);
            WriteDword((uint) heightFieldVerticalAxis, writer, platformMismatch);
            WriteFloat(heightFieldVerticalExtent, writer, platformMismatch);

            WriteDword(NumVertices, writer, platformMismatch);
            WriteDword(NumTriangles, writer, platformMismatch);

            WriteVertices(writer, platformMismatch);
            WriteTriangles(writer, platformMismatch);

            WriteMaterialIndices(writer, platformMismatch);
            WriteRemapIndices(writer, platformMismatch);

            WriteDword(numConvexParts, writer, platformMismatch);
            WriteDword(numFlatParts, writer, platformMismatch);
            WriteConvexParts(writer, platformMismatch);
            WriteFlatParts(writer, platformMismatch);

            uint modelSize = hybridModel.GetUsedBytes();
            WriteDword(modelSize, writer, platformMismatch);
            hybridModel.Save(writer, endian);

            WriteFloat(geomEpsilon, writer, platformMismatch);
            WriteBounds(writer, platformMismatch);
            // TODO: do extra check - original code checks the TriangleMeshBuilder::computeMassInfo result 
            // and in case of null just writes single -1.0f float (just mass??, without tensor matrix and COM) to stream
            WritePhysProperties(writer, platformMismatch);

            WriteExtraTriangleData(writer, platformMismatch);
        }

        private void WriteVertices(BinaryWriter writer, bool platformMismatch)
        {
            foreach (Vector3 vertex in Vertices)
            {
                WriteFloat(vertex.X, writer, platformMismatch);
                WriteFloat(vertex.Y, writer, platformMismatch);
                WriteFloat(vertex.Z, writer, platformMismatch);
            }
        }

        private void WriteTriangles(BinaryWriter writer, bool platformMismatch)
        {
            if (SerialFlags.HasFlag(MeshSerialFlags.MSF_8BIT_INDICES) && SerialFlags.HasFlag(MeshSerialFlags.MSF_16BIT_INDICES))
            {
                throw new OpcodeException($"Invalid serial flags {SerialFlags}");
            }

            if (SerialFlags.HasFlag(MeshSerialFlags.MSF_8BIT_INDICES))
            {
                foreach (Triangle triangle in Triangles)
                {
                    writer.Write((byte)triangle.v0);
                    writer.Write((byte)triangle.v1);
                    writer.Write((byte)triangle.v2);
                }
            }
            else if (SerialFlags.HasFlag(MeshSerialFlags.MSF_16BIT_INDICES))
            {
                foreach (Triangle triangle in Triangles)
                {
                    WriteWord((ushort)triangle.v0, writer, platformMismatch);
                    WriteWord((ushort)triangle.v1, writer, platformMismatch);
                    WriteWord((ushort)triangle.v2, writer, platformMismatch);
                }
            }
            else
            {
                foreach (Triangle triangle in Triangles)
                {
                    WriteDword(triangle.v0, writer, platformMismatch);
                    WriteDword(triangle.v1, writer, platformMismatch);
                    WriteDword(triangle.v2, writer, platformMismatch);
                }
            }
        }

        private void WriteMaterialIndices(BinaryWriter writer, bool platformMismatch)
        {
            if (SerialFlags.HasFlag(MeshSerialFlags.MSF_MATERIALS))
            {
                foreach (ushort materialIndex in MaterialIndices)
                {
                    WriteWord(materialIndex, writer, platformMismatch);
                }
            }
        }

        private void WriteRemapIndices(BinaryWriter writer, bool platformMismatch)
        {
            if (SerialFlags.HasFlag(MeshSerialFlags.MSF_FACE_REMAP))
            {
                WriteIndices(remapIndices, writer, platformMismatch);
            }
        }

        private void WriteConvexParts(BinaryWriter writer, bool platformMismatch)
        {
            if (numConvexParts > 0)
            {
                foreach (ushort partId in convexParts)
                {
                    WriteWord(partId, writer, platformMismatch);
                }
            }
        }

        private void WriteFlatParts(BinaryWriter writer, bool platformMismatch)
        {
            if (numFlatParts > 0)
            {
                if (numFlatParts < 256)
                {
                    writer.Write(flatParts.Select(partId => (byte)partId).ToArray());
                }
                else
                {
                    foreach (ushort partId in flatParts)
                    {
                        WriteWord(partId, writer, platformMismatch);
                    }
                }
            }
        }

        private void WriteBounds(BinaryWriter writer, bool platformMismatch)
        {
            WriteFloat(boundingSphere.Center.X, writer, platformMismatch);
            WriteFloat(boundingSphere.Center.Y, writer, platformMismatch);
            WriteFloat(boundingSphere.Center.Z, writer, platformMismatch);
            WriteFloat(boundingSphere.Radius, writer, platformMismatch);

            WriteFloat(BoundingBox.Minimum.X, writer, platformMismatch);
            WriteFloat(BoundingBox.Minimum.Y, writer, platformMismatch);
            WriteFloat(BoundingBox.Minimum.Z, writer, platformMismatch);
            WriteFloat(BoundingBox.Maximum.X, writer, platformMismatch);
            WriteFloat(BoundingBox.Maximum.Y, writer, platformMismatch);
            WriteFloat(BoundingBox.Maximum.Z, writer, platformMismatch);
        }

        private void WritePhysProperties(BinaryWriter writer, bool platformMismatch)
        {
            WriteFloat(mass, writer, platformMismatch);
            WriteFloat(inertiaTensor.M11, writer, platformMismatch);
            WriteFloat(inertiaTensor.M12, writer, platformMismatch);
            WriteFloat(inertiaTensor.M13, writer, platformMismatch);
            WriteFloat(inertiaTensor.M21, writer, platformMismatch);
            WriteFloat(inertiaTensor.M22, writer, platformMismatch);
            WriteFloat(inertiaTensor.M23, writer, platformMismatch);
            WriteFloat(inertiaTensor.M31, writer, platformMismatch);
            WriteFloat(inertiaTensor.M32, writer, platformMismatch);
            WriteFloat(inertiaTensor.M33, writer, platformMismatch);
            WriteFloat(centerOfMass.X, writer, platformMismatch);
            WriteFloat(centerOfMass.Y, writer, platformMismatch);
            WriteFloat(centerOfMass.Z, writer, platformMismatch);
        }

        private void WriteExtraTriangleData(BinaryWriter writer, bool platformMismatch)
        {
            WriteDword((uint)ExtraTriangleData.Count, writer, platformMismatch);
            writer.Write(ExtraTriangleData.ToArray());
        } 
        #endregion

        public uint GetUsedBytes()
        {
            uint triangleStride;
            if (SerialFlags.HasFlag(MeshSerialFlags.MSF_8BIT_INDICES))
            {
                triangleStride = 3;
            }
            else if (SerialFlags.HasFlag(MeshSerialFlags.MSF_16BIT_INDICES))
            {
                triangleStride = 3 * 2;
            }
            else
            {
                triangleStride = 3 * 4;
            }

            return 12 // header (2 x magic + version)
                + 4 // serial flags
                + 4 // convexEdgeThreshold
                + 4 // heightFieldVerticalAxis
                + 4 // heightFieldVerticalExtent
                + 4 // numVertices
                + 4 // numTriangles
                + NumVertices * 12 // vertices
                + NumTriangles * triangleStride // triangles
                + (SerialFlags.HasFlag(MeshSerialFlags.MSF_MATERIALS) ? NumTriangles * 2 : 0) // material indices
                + (SerialFlags.HasFlag(MeshSerialFlags.MSF_FACE_REMAP) ? GetUsedBytesByIndices(remapIndices) : 0) // remap indices
                + 4 // numConvexParts
                + 4 // numFlatParts
                + (numConvexParts > 0 ? ((uint)convexParts.Count) * 2 : 0) // convexParts
                + (numFlatParts > 0 ? (numFlatParts < 256 ? ((uint)flatParts.Count) * 1 : ((uint)flatParts.Count) * 2) : 0) // flatParts
                + 4 // modelSize
                + hybridModel.GetUsedBytes() // hybrid model
                + 4 // geomEpsilon
                + 4 * 10 // bounds
                + 4 * 13 // mass + inertia tensor + center of mass
                + 4 // numExtraTriangleData
                + ((uint)ExtraTriangleData.Count) * 1 // extraTriangleData
                ;
        }
    }
}

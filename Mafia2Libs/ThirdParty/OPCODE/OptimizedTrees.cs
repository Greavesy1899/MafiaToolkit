using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Utils.Extensions;
using Utils.VorticeUtils;
using static ResourceTypes.Collisions.PhysX.SerializationUtils;

namespace ThirdParty.OPCODE
{
    public interface AABBOptimizedTree
    {
        /* Load the tree in the format that is required for Collision files. */
        void Load_Collision(BinaryReader reader, bool endianMismatch = false);

        /* Save the tree in the format that is required for Collision files. */
        void Save_Collision(BinaryWriter writer, bool endianMismatch = false);

        void Load_FrameRes(MemoryStream stream, Endian endian = Endian.Little);
        void Save_FrameRes(MemoryStream stream, Endian endian = Endian.Little);
        uint GetUsedBytes();
    }

    public class DummyTree : AABBOptimizedTree
    {
        public void Load_Collision(BinaryReader reader, bool endianMismatch = false)
        {
            // do nothing
        }

        public void Save_Collision(BinaryWriter writer, bool endianMismatch = false)
        {
            // do nothing
        }

        public void Load_FrameRes(MemoryStream stream, Endian endian = Endian.Little)
        {
            // do nothing
        }

        public void Save_FrameRes(MemoryStream stream, Endian endian = Endian.Little)
        {
            // do nothing
        }

        public uint GetUsedBytes()
        {
            return 0;
        }
    }

    struct QuantizedAABB
    {
        public short centerX; //!< Quantized center
        public short centerY; //!< Quantized center
        public short centerZ; //!< Quantized center
        public ushort extentsX; //!< Quantized extents
        public ushort extentsY; //!< Quantized extents
        public ushort extentsZ; //!< Quantized extents
    }

    struct AABBQuantizedNoLeafNode
    {
        public QuantizedAABB aabb;
        public uint posData;
        public uint negData;

        public AABBQuantizedNoLeafNode(QuantizedAABB aabb, uint posData, uint negData)
        {
            this.aabb = aabb;
            this.posData = posData;
            this.negData = negData;
        }
    }

    /// <summary>
    /// A quantized no-leaf AABB tree.
    /// </summary>
    /// <remarks>
    /// See also original sources (<c>OPC_OptimizedTree.cpp</c>)
    /// </remarks>
    public class AABBQuantizedNoLeafTree : AABBOptimizedTree
    {
        private IList<AABBQuantizedNoLeafNode> nodes = new List<AABBQuantizedNoLeafNode>();
        private Vector3 centerCoeff = Vector3.One;
        private Vector3 extentsCoeff = Vector3.One;

        public void Load_Collision(BinaryReader reader, bool endianMismatch = false)
        {
            uint numNodes = ReadDword(reader, endianMismatch);
            nodes = new List<AABBQuantizedNoLeafNode>((int)numNodes);
            for (int i = 0; i < numNodes; i++)
            {
                QuantizedAABB aabb = new QuantizedAABB();
                aabb.centerX = ReadShort(reader, endianMismatch);
                aabb.centerY = ReadShort(reader, endianMismatch);
                aabb.centerZ = ReadShort(reader, endianMismatch);
                aabb.extentsX = ReadWord(reader, endianMismatch);
                aabb.extentsY = ReadWord(reader, endianMismatch);
                aabb.extentsZ = ReadWord(reader, endianMismatch);

                uint posData = ReadDword(reader, endianMismatch);
                uint negData = ReadDword(reader, endianMismatch);

                nodes.Add(new AABBQuantizedNoLeafNode(aabb, posData, negData));
            }

            centerCoeff.X = ReadFloat(reader, endianMismatch);
            centerCoeff.Y = ReadFloat(reader, endianMismatch);
            centerCoeff.Z = ReadFloat(reader, endianMismatch);
            extentsCoeff.X = ReadFloat(reader, endianMismatch);
            extentsCoeff.Y = ReadFloat(reader, endianMismatch);
            extentsCoeff.Z = ReadFloat(reader, endianMismatch);
        }

        public void Save_Collision(BinaryWriter writer, bool endianMismatch = false)
        {
            WriteDword((uint)nodes.Count, writer, endianMismatch);

            foreach (AABBQuantizedNoLeafNode node in nodes)
            {
                WriteShort(node.aabb.centerX, writer, endianMismatch);
                WriteShort(node.aabb.centerY, writer, endianMismatch);
                WriteShort(node.aabb.centerZ, writer, endianMismatch);
                WriteWord(node.aabb.extentsX, writer, endianMismatch);
                WriteWord(node.aabb.extentsY, writer, endianMismatch);
                WriteWord(node.aabb.extentsZ, writer, endianMismatch);
                WriteDword(node.posData, writer, endianMismatch);
                WriteDword(node.negData, writer, endianMismatch);
            }

            WriteFloat(centerCoeff.X, writer, endianMismatch);
            WriteFloat(centerCoeff.Y, writer, endianMismatch);
            WriteFloat(centerCoeff.Z, writer, endianMismatch);
            WriteFloat(extentsCoeff.X, writer, endianMismatch);
            WriteFloat(extentsCoeff.Y, writer, endianMismatch);
            WriteFloat(extentsCoeff.Z, writer, endianMismatch);
        }

        public void Load_FrameRes(MemoryStream stream, Endian endian = Endian.Little)
        {
            bool bIsBigEndian = (endian == Endian.Big);
            centerCoeff = Vector3Utils.ReadFromFile(stream, bIsBigEndian);
            extentsCoeff = Vector3Utils.ReadFromFile(stream, bIsBigEndian);

            // Read QuantizedNoLeaf nodes.
            uint NumNodes = stream.ReadUInt32(bIsBigEndian);
            nodes = new List<AABBQuantizedNoLeafNode>((int)NumNodes);
            for(int i = 0; i < NumNodes; i++)
            {
                uint PosData = stream.ReadUInt32(bIsBigEndian);
                uint NegData = stream.ReadUInt32(bIsBigEndian);

                QuantizedAABB AABB = new QuantizedAABB();
                AABB.centerX = stream.ReadInt16(bIsBigEndian);
                AABB.centerY = stream.ReadInt16(bIsBigEndian);
                AABB.centerZ = stream.ReadInt16(bIsBigEndian);
                AABB.extentsX = stream.ReadUInt16(bIsBigEndian);
                AABB.extentsY = stream.ReadUInt16(bIsBigEndian);
                AABB.extentsZ = stream.ReadUInt16(bIsBigEndian);
                nodes.Add(new AABBQuantizedNoLeafNode(AABB, PosData, NegData));
            }
        }

        public void Save_FrameRes(MemoryStream stream, Endian endian = Endian.Little)
        {
            bool bIsBigEndian = (endian == Endian.Big);
            Vector3Utils.WriteToFile(centerCoeff, stream, bIsBigEndian);
            Vector3Utils.WriteToFile(extentsCoeff, stream, bIsBigEndian);

            // Write the number of quantized nodes
            // Then continue with the nodes
            stream.Write(nodes.Count, bIsBigEndian);
            foreach(AABBQuantizedNoLeafNode Node in nodes)
            {
                stream.Write(Node.posData, bIsBigEndian);
                stream.Write(Node.negData, bIsBigEndian);
                stream.Write(Node.aabb.centerX, bIsBigEndian);
                stream.Write(Node.aabb.centerY, bIsBigEndian);
                stream.Write(Node.aabb.centerZ, bIsBigEndian);
                stream.Write(Node.aabb.extentsX, bIsBigEndian);
                stream.Write(Node.aabb.extentsY, bIsBigEndian);
                stream.Write(Node.aabb.extentsZ, bIsBigEndian);
            }
        }

        public uint GetUsedBytes()
        {
            return 4 // numNodes
                + (uint)nodes.Count * 20 // nodes
                + 12  // centerCoeff
                + 12; // extentsCoeff
        }
    }
}

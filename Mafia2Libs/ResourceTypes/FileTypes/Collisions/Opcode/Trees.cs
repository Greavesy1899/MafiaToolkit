using SharpDX;
using System.Collections.Generic;
using System.IO;
using static ResourceTypes.Collisions.Opcode.SerializationUtils;

namespace ResourceTypes.Collisions.Opcode
{
    interface AABBOptimizedTree
    {
        void Load(BinaryReader reader, bool endianMismatch = false);
        void Save(BinaryWriter writer, bool endianMismatch = false);
        uint GetUsedBytes();
    }

    class DummyTree : AABBOptimizedTree
    {
        public void Load(BinaryReader reader, bool endianMismatch = false)
        {
            // do nothing
        }

        public void Save(BinaryWriter writer, bool endianMismatch = false)
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
    class AABBQuantizedNoLeafTree : AABBOptimizedTree
    {
        private IList<AABBQuantizedNoLeafNode> nodes = new List<AABBQuantizedNoLeafNode>();
        private Vector3 centerCoeff = Vector3.One;
        private Vector3 extentsCoeff = Vector3.One;

        public void Load(BinaryReader reader, bool endianMismatch = false)
        {
            uint numNodes = ReadDword(reader, endianMismatch);
            nodes = new List<AABBQuantizedNoLeafNode>((int) numNodes);
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

        public void Save(BinaryWriter writer, bool endianMismatch = false)
        {
            WriteDword((uint) nodes.Count, writer, endianMismatch);

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

        public uint GetUsedBytes()
        {
            return 4 // numNodes
                + (uint)nodes.Count * 20 // nodes
                + 12  // centerCoeff
                + 12; // extentsCoeff
        }

    };
}

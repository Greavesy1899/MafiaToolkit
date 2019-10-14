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
    interface AABBOptimizedTree
    {
        void Load(BinaryReader reader, bool endianMismatch = false);
    }

    class DummyTree : AABBOptimizedTree
    {
        public void Load(BinaryReader reader, bool endianMismatch = false)
        {
            // do nothing
        }
    }

    struct QuantizedAABB
    {
        public short centerX; //!< Quantized center
        public short centerY; //!< Quantized center
        public short centerZ; //!< Quantized center
        public ushort extentsX;  //!< Quantized extents
        public ushort extentsY;  //!< Quantized extents
        public ushort extentsZ;  //!< Quantized extents
    }

    struct AABBQuantizedNoLeafNode
    {
        QuantizedAABB aabb;
        uint posData;
        uint negData;

        public AABBQuantizedNoLeafNode(QuantizedAABB aabb, uint posData, uint negData)
        {
            this.aabb = aabb;
            this.posData = posData;
            this.negData = negData;
        }
    }

    class AABBQuantizedNoLeafTree : AABBOptimizedTree
    {
        private IList<AABBQuantizedNoLeafNode> nodes = new List<AABBQuantizedNoLeafNode>();
        private Vector3 centerCoeff = Vector3.One;
        private Vector3 extentsCoeff = Vector3.One;

        public void Load(BinaryReader reader, bool endianMismatch = false)
        {
            uint numNodes = ReadDword(reader, endianMismatch);
            nodes = new List<AABBQuantizedNoLeafNode>((int)numNodes);
            for(int i = 0; i < numNodes; i++)
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
    };
}

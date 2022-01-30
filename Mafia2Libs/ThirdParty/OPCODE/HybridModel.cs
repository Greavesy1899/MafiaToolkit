using ResourceTypes.Collisions.PhysX;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static ResourceTypes.Collisions.PhysX.SerializationUtils;

namespace ThirdParty.OPCODE
{
    /// <summary>
    /// An hybrid collision model
    /// </summary>
    /// <remarks>
    /// See full description in the original sources to get a general idea (<c>OPC_HybridModel.cpp</c>)
    /// </remarks>
    class HybridModel : ModelBase
    {
        /// <summary>
        /// Leaf descriptor
        /// </summary>
        struct LeafTriangles
        {
            /// <summary>
            /// Packed data. Contains number of triangles in the leaf and triangle index for this leaf.  
            /// </summary>
            private uint data;

            public LeafTriangles(uint data)
            {
                this.data = data;
            }

            public static implicit operator uint(LeafTriangles lt) => lt.data;
            public static implicit operator LeafTriangles(uint val) => new LeafTriangles(val);
        }

        private const uint SupportedHybridModelVersion = 0;

        private bool littleEndian = true;

        private uint numLeaves = 1;
        private IList<LeafTriangles> leafTriangles = new List<LeafTriangles>();
        private IList<uint> primitiveIndices = new List<uint>();

        public override void Load_Collision(BinaryReader reader)
        {
            base.Load_Collision(reader);

            char h0, h1, h2;
            ReadChunk(out h0, out h1, out h2, out littleEndian, reader);
            if ((h0 != 'H') && (h1 != 'B') && (h2 != 'M'))
            {
                throw new PhysXException("Invalid 'HBM' header");
            }

            bool platformMismatch = !littleEndian;

            uint version = ReadDword(reader, platformMismatch);
            if (version != SupportedHybridModelVersion)
            {
                throw new PhysXException($"Unsupported hybrid model version {version}");
            }

            numLeaves = ReadDword(reader, platformMismatch);
            if (numLeaves > 1)
            {
                uint leafTrianglesMaxIndex = ReadDword(reader, platformMismatch);
                IList<uint> leafTrianglesAsUints = ReadIndices(leafTrianglesMaxIndex, numLeaves, reader, platformMismatch);
                leafTriangles = leafTrianglesAsUints.Select(elem => (LeafTriangles)elem).ToList();
            }

            uint numPrimitives = ReadDword(reader, platformMismatch);
            // NOTE: In Mafia 2 numPrimitives always == 0
            if (numPrimitives > 0)
            {
                uint primitiveMaxIndex = ReadDword(reader, platformMismatch);
                primitiveIndices = ReadIndices(primitiveMaxIndex, numPrimitives, reader, platformMismatch);
            }
        }

        public override void Save_Collision(BinaryWriter writer, Endian endian = Endian.Little)
        {
            base.Save_Collision(writer, endian);

            if (numLeaves == 0)
            {
                throw new OpcodeException("HybridModel model should contain at least one leaf");
            }

            uint actualNumLeaves = (uint)leafTriangles.Count;
            if (numLeaves > 1 && actualNumLeaves > 1 && numLeaves != actualNumLeaves)
            {
                throw new OpcodeException($"Number of leaves {numLeaves} does not match actual number of leaves {actualNumLeaves}");
            }

            bool isLittleEndian = endian == Endian.Little;
            bool platformMismatch = endian == Endian.Big;

            WriteChunk('H', 'B', 'M', isLittleEndian, writer);
            WriteDword(SupportedHybridModelVersion, writer, platformMismatch);

            WriteDword(numLeaves, writer, platformMismatch);
            if (numLeaves > 1)
            {
                WriteIndices(leafTriangles.Select(lt => (uint)lt).ToList(), writer, platformMismatch);
            }

            WriteDword((uint)primitiveIndices.Count, writer, platformMismatch);
            if (primitiveIndices.Count > 0)
            {
                WriteIndices(primitiveIndices, writer, platformMismatch);
            }
        }

        /// <summary>
        /// Gets the number of bytes used by the tree.
        /// </summary>
        /// <returns>Amount of bytes used</returns>
        public override uint GetUsedBytes()
        {
            return base.GetUsedBytes()
                + 4 // magic + endian flag
                + 4 // version
                + 4 // numLeaves
                + (numLeaves > 1 ? GetUsedBytesByIndices(leafTriangles.Select(lt => (uint)lt).ToList()) : 0) // leafTriangles
                + 4 // numPrimitives
                + GetUsedBytesByIndices(primitiveIndices); // primitiveIndices
        }
    }
}

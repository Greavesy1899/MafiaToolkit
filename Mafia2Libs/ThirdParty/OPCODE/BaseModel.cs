using ResourceTypes.Collisions.PhysX;
using System;
using System.IO;
using static ResourceTypes.Collisions.PhysX.SerializationUtils;

namespace ThirdParty.OPCODE
{
    // NOTE: Could be migrated to Gibbed.IO.Endian to reduce
    // the number of semantically identical types
    public enum Endian
    {
        Little,
        Big
    }

    public interface IOpcodeSerializable
    {
        /* Load the serializable data whilst respecting Collision format */
        void Load_Collision(BinaryReader reader);
        /* Save the serializable data whilst respecting Collision format */
        void Save_Collision(BinaryWriter writer, Endian endian = Endian.Little);
        uint GetUsedBytes();
    }

    /// <summary>
    /// The base class for collision models
    /// </summary>
    abstract class ModelBase : IOpcodeSerializable
    {
        [Flags]
        enum ModelFlag
        {
            // ReSharper disable InconsistentNaming
            /// <summary>
            /// Compressed/uncompressed tree
            /// </summary>
            OPC_QUANTIZED = (1 << 0),
            /// <summary>
            /// Leaf/NoLeaf tree
            /// </summary>
            OPC_NO_LEAF = (1 << 1),
            /// <summary>
            /// Special case for 1-node models
            /// </summary>
            OPC_SINGLE_NODE = (1 << 2)
            // ReSharper restore InconsistentNaming
        };

        private const uint SupportedModelVersion = 1;

        private ModelFlag modelCode = ModelFlag.OPC_SINGLE_NODE;
        private AABBOptimizedTree tree = new DummyTree();

        public virtual void Load_Collision(BinaryReader reader)
        {
            char h0, h1, h2;
            bool littleEndian;

            ReadChunk(out h0, out h1, out h2, out littleEndian, reader);
            if ((h0 != 'O') && (h1 != 'P') && (h2 != 'C'))
            {
                throw new PhysXException("Invalid 'OPC' header");
            }

            bool platformMismatch = !littleEndian;

            uint version = ReadDword(reader, platformMismatch);
            if (version != SupportedModelVersion)
            {
                throw new PhysXException($"Unsupported model version {version}");
            }

            modelCode = (ModelFlag)ReadDword(reader, platformMismatch);

            CreateTree();
            tree.Load_Collision(reader, platformMismatch);
        }

        /// <summary>
        /// Creates an optimized tree according to the modelCode flags
        /// </summary>
        /// <remarks>
        /// Actually only AABBQuantizedNoLeafTree is used in Mafia 2
        /// </remarks>
        private void CreateTree()
        {
            if (modelCode.HasFlag(ModelFlag.OPC_SINGLE_NODE))
            {
                // special case: tree with one single node (not serialized)
                tree = new DummyTree();
            }
            else if (modelCode.HasFlag(ModelFlag.OPC_NO_LEAF))
            {
                if (modelCode.HasFlag(ModelFlag.OPC_QUANTIZED))
                {
                    tree = new AABBQuantizedNoLeafTree();
                }
                else
                {
                    //tree = new AABBNoLeafTree();
                    throw new NotImplementedException("AABBNoLeafTree is not implemented yet");
                }
            }
            else
            {
                if (modelCode.HasFlag(ModelFlag.OPC_QUANTIZED))
                {
                    //tree = new AABBQuantizedTree();
                    throw new NotImplementedException("AABBQuantizedTree is not implemented yet");
                }
                else
                {
                    //tree = new AABBCollisionTree();
                    throw new NotImplementedException("AABBCollisionTree is not implemented yet");
                }
            }
        }

        public virtual void Save_Collision(BinaryWriter writer, Endian endian = Endian.Little)
        {
            bool isLittleEndian = endian == Endian.Little;
            bool platformMismatch = endian == Endian.Big;

            WriteChunk('O', 'P', 'C', isLittleEndian, writer);
            WriteDword(SupportedModelVersion, writer, platformMismatch);

            WriteDword((uint)modelCode, writer, platformMismatch);

            tree.Save_Collision(writer, platformMismatch);
        }

        public virtual uint GetUsedBytes()
        {
            return 4 // magic + endian flag
                + 4 // version
                + 4 // modelCode
                + tree.GetUsedBytes();
        }
    }
}

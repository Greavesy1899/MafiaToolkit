/* Copyright (c) 2017 Rick (rick 'at' gibbed 'dot' us)
 * 
 * This software is provided 'as-is', without any express or implied
 * warranty. In no event will the authors be held liable for any damages
 * arising from the use of this software.
 * 
 * Permission is granted to anyone to use this software for any purpose,
 * including commercial applications, and to alter it and redistribute it
 * freely, subject to the following restrictions:
 * 
 * 1. The origin of this software must not be misrepresented; you must not
 *    claim that you wrote the original software. If you use this software
 *    in a product, an acknowledgment in the product documentation would
 *    be appreciated but is not required.
 * 
 * 2. Altered source versions must be plainly marked as such, and must not
 *    be misrepresented as being the original software.
 * 
 * 3. This notice may not be removed or altered from any source
 *    distribution.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Gibbed.IO;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

namespace Gibbed.Illusion.FileFormats
{
    public class BlockReaderStream : Stream
    {
        private readonly Stream _BaseStream;
        private readonly List<Block> _Blocks;
        private Block _CurrentBlock;
        private long _CurrentPosition;

        private BlockReaderStream(Stream baseStream)
        {
            if (baseStream == null)
            {
                throw new ArgumentNullException("baseStream");
            }

            this._BaseStream = baseStream;
            this._Blocks = new List<Block>();
            this._CurrentPosition = 0;
        }

        public void FreeLoadedBlocks()
        {
            foreach (var block in this._Blocks)
            {
                block.FreeLoadedData();
            }
        }

        private void AddUncompressedBlock(long virtualOffset, uint virtualSize, long dataOffset)
        {
            this._Blocks.Add(new UncompressedBlock(virtualOffset, virtualSize, dataOffset));
        }

        private void AddCompressedBlock(long virtualOffset, uint virtualSize, long dataOffset, uint dataCompressedSize)
        {
            this._Blocks.Add(new CompressedBlock(virtualOffset, virtualSize, dataOffset, dataCompressedSize));
        }

        private bool LoadBlock(long offset)
        {
            if (this._CurrentBlock == null || this._CurrentBlock.IsValidOffset(offset) == false)
            {
                Block block = this._Blocks.SingleOrDefault(candidate => candidate.IsValidOffset(offset));
                if (block == null)
                {
                    this._CurrentBlock = null;
                    return false;
                }
                this._CurrentBlock = block;
            }

            return this._CurrentBlock.Load(this._BaseStream);
        }

        public void SaveUncompressed(Stream output)
        {
            byte[] data = new byte[1024];

            long totalSize = this._Blocks.Max(candidate => candidate.Offset + candidate.Size);

            output.SetLength(totalSize);

            foreach (Block block in this._Blocks)
            {
                output.Seek(block.Offset, SeekOrigin.Begin);
                this.Seek(block.Offset, SeekOrigin.Begin);

                long total = block.Size;
                while (total > 0)
                {
                    int read = this.Read(data, 0, (int)Math.Min(total, data.Length));
                    output.Write(data, 0, read);
                    total -= read;
                }
            }

            output.Flush();
        }

        #region Stream
        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void Flush()
        {
            this._BaseStream.Flush();
        }

        public override long Length
        {
            get { throw new NotSupportedException(); }
        }

        public override long Position
        {
            get { return this._CurrentPosition; }
            set { throw new NotSupportedException(); }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int totalRead = 0;

            while (totalRead < count)
            {
                if (this.LoadBlock(this._CurrentPosition) == false)
                {
                    throw new InvalidOperationException();
                }

                int read = this._CurrentBlock.Read(
                    this._BaseStream,
                    this._CurrentPosition,
                    buffer,
                    offset + totalRead,
                    count - totalRead);

                totalRead += read;
                this._CurrentPosition += read;
            }

            return totalRead;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            if (origin == SeekOrigin.End)
            {
                throw new NotSupportedException();
            }

            if (origin == SeekOrigin.Current)
            {
                if (offset == 0)
                {
                    return this._CurrentPosition;
                }

                offset = this._CurrentPosition + offset;
            }

            /*
            :effort: in fixing seeks that hit the end of data instead of over it
            if (this.LoadBlock(offset) == false)
            {
                throw new InvalidOperationException();
            }
            */

            this._CurrentPosition = offset;
            return this._CurrentPosition;
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }
        #endregion

        private abstract class Block
        {
            public long Offset { get; protected set; }
            public uint Size { get; protected set; }

            public Block(long offset, uint size)
            {
                this.Offset = offset;
                this.Size = size;
            }

            public bool IsValidOffset(long offset)
            {
                return offset >= this.Offset &&
                       offset < this.Offset + this.Size;
            }

            public abstract bool Load(Stream input);
            public abstract void FreeLoadedData();
            public abstract int Read(Stream input, long baseOffset, byte[] buffer, int offset, int count);
        }

        private class UncompressedBlock : Block
        {
            private readonly long _DataOffset;
            private bool _IsLoaded;
            private byte[] _Data;

            public UncompressedBlock(long virtualOffset, uint virtualSize, long dataOffset)
                : base(virtualOffset, virtualSize)
            {
                this._DataOffset = dataOffset;
                this._IsLoaded = false;
                this._Data = null;
            }

            public override void FreeLoadedData()
            {
                this._IsLoaded = false;
                this._Data = null;
            }

            public override bool Load(Stream input)
            {
                if (this._IsLoaded == true)
                {
                    return true;
                }

                input.Seek(this._DataOffset, SeekOrigin.Begin);
                this._Data = new byte[this.Size];
                if (input.Read(this._Data, 0, this._Data.Length) != this._Data.Length)
                {
                    throw new InvalidOperationException();
                }

                this._IsLoaded = true;
                return true;
            }

            public override int Read(Stream input, long baseOffset, byte[] buffer, int offset, int count)
            {
                if (baseOffset >= this.Offset + this.Size)
                {
                    return 0;
                }

                this.Load(input);

                int relativeOffset = (int)(baseOffset - this.Offset);
                int read = (int)Math.Min(this.Size - relativeOffset, count);
                Array.ConstrainedCopy(this._Data, relativeOffset, buffer, offset, read);
                return read;
            }
        }

        private class CompressedBlock : Block
        {
            private readonly long _DataOffset;
            private uint _DataCompressedSize;
            private bool _IsLoaded;
            private byte[] _Data;

            public CompressedBlock(long virtualOffset, uint virtualSize, long dataOffset, uint dataCompressedSize)
                : base(virtualOffset, virtualSize)
            {
                this._DataOffset = dataOffset;
                this._DataCompressedSize = dataCompressedSize;

                this._IsLoaded = false;
                this._Data = null;
            }

            public override void FreeLoadedData()
            {
                this._IsLoaded = false;
                this._Data = null;
            }

            public override bool Load(Stream input)
            {
                if (this._IsLoaded == true)
                {
                    return true;
                }

                input.Seek(this._DataOffset, SeekOrigin.Begin);
                this._Data = new byte[this.Size];
                var inflater = new InflaterInputStream(input);
                if (inflater.Read(this._Data, 0, this._Data.Length) != this._Data.Length)
                {
                    throw new InvalidOperationException();
                }

                this._IsLoaded = true;
                return true;
            }

            public override int Read(Stream input, long baseOffset, byte[] buffer, int offset, int count)
            {
                if (baseOffset >= this.Offset + this.Size)
                {
                    return 0;
                }

                this.Load(input);

                int relativeOffset = (int)(baseOffset - this.Offset);
                int read = (int)Math.Min(this.Size - relativeOffset, count);
                Array.ConstrainedCopy(this._Data, relativeOffset, buffer, offset, read);
                return read;
            }
        }

        private struct CompressedBlockHeader
        {
            public uint UncompressedSize;
            public uint Unknown04;
            public uint Unknown08;
            public uint Unknown0C;
            public uint CompressedSize;
            public uint Unknown14;
            public uint Unknown18;
            public uint Unknown1C;

            public static CompressedBlockHeader Read(Stream input, Endian endian)
            {
                CompressedBlockHeader instance;
                instance.UncompressedSize = input.ReadValueU32(endian);
                instance.Unknown04 = input.ReadValueU32(endian);
                instance.Unknown08 = input.ReadValueU32(endian);
                instance.Unknown0C = input.ReadValueU32(endian);
                instance.CompressedSize = input.ReadValueU32(endian);
                instance.Unknown14 = input.ReadValueU32(endian);
                instance.Unknown18 = input.ReadValueU32(endian);
                instance.Unknown1C = input.ReadValueU32(endian);
                return instance;
            }
        }

        public const uint Signature = 0x6C7A4555; // 'zlEU'

        public static BlockReaderStream FromStream(Stream baseStream, Endian endian)
        {
            var instance = new BlockReaderStream(baseStream);

            var magic = baseStream.ReadValueU32(endian);
            var alignment = baseStream.ReadValueU32(endian);
            var flags = baseStream.ReadValueU8();

            if (magic != Signature || /*alignment != 0x4000 ||*/ flags != 4)
            {
                throw new InvalidOperationException();
            }

            long virtualOffset = 0;
            while (true)
            {
                uint size = baseStream.ReadValueU32(endian);
                bool isCompressed = baseStream.ReadValueU8() != 0;

                if (size == 0)
                {
                    break;
                }

                if (isCompressed == true)
                {
                    var compressedBlockHeader = CompressedBlockHeader.Read(baseStream, endian);
                    if (compressedBlockHeader.Unknown04 != 32 ||
                        compressedBlockHeader.Unknown08 != 81920 ||
                        compressedBlockHeader.Unknown0C != 135200769 ||
                        compressedBlockHeader.Unknown14 != 0 ||
                        compressedBlockHeader.Unknown18 != 0 ||
                        compressedBlockHeader.Unknown1C != 0)
                    {
                        throw new InvalidOperationException();
                    }

                    if (size - 32 != compressedBlockHeader.CompressedSize)
                    {
                        throw new InvalidOperationException();
                    }

                    instance.AddCompressedBlock(virtualOffset,
                                                compressedBlockHeader.UncompressedSize,
                                                baseStream.Position,
                                                compressedBlockHeader.CompressedSize);
                    baseStream.Seek(compressedBlockHeader.CompressedSize, SeekOrigin.Current);
                }
                else
                {
                    instance.AddUncompressedBlock(virtualOffset, size, baseStream.Position);
                    baseStream.Seek(size, SeekOrigin.Current);
                }

                virtualOffset += alignment;
            }

            return instance;
        }
    }
}

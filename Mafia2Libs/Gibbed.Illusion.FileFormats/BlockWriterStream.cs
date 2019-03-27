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
using System.IO;
using Gibbed.IO;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

namespace Gibbed.Illusion.FileFormats
{
    public class BlockWriterStream : Stream
    {
        public const uint Signature = 0x6C7A4555; // 'zlEU'

        private readonly Endian _Endian;
        private readonly uint _Alignment;
        private readonly Stream _BaseStream;
        private readonly byte[] _BlockBytes;
        private int _BlockOffset;
        private readonly bool _IsCompressing;

        private BlockWriterStream(Stream baseStream, uint alignment, Endian endian, bool compress)
        {
            if (baseStream == null)
            {
                throw new ArgumentNullException("baseStream");
            }

            this._BaseStream = baseStream;
            this._Alignment = alignment;
            this._Endian = endian;
            this._BlockBytes = new byte[alignment];
            this._BlockOffset = 0;
            this._IsCompressing = compress;
        }

        #region Stream
        public override bool CanRead
        {
            get { return false; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override void Flush()
        {
            this.FlushBlock();
        }

        public override long Length
        {
            get { throw new NotSupportedException(); }
        }

        public override long Position
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            long remaining = count;
            while (remaining > 0)
            {
                var write = (int)Math.Min(remaining, this._Alignment - this._BlockOffset);
                Array.Copy(buffer, offset, this._BlockBytes, this._BlockOffset, write);
                this._BlockOffset += write;
                remaining -= write;
                offset += write;

                if (this._BlockOffset >= this._Alignment)
                {
                    this.FlushBlock();
                }
            }
        }

        private void FlushBlock()
        {
            if (this._BlockOffset == 0)
            {
                return;
            }

            if (this._IsCompressing == false || this.FlushCompressedBlock() == false)
            {
                /*var blockLength = Array.FindLastIndex(this._BlockBytes, this._BlockOffset - 1, b => b != 0);
                blockLength = 1 + (blockLength < 0 ? 0 : blockLength);*/
                var blockLength = this._BlockOffset;
                this._BaseStream.WriteValueS32(blockLength, this._Endian);
                this._BaseStream.WriteValueU8(0);
                this._BaseStream.Write(this._BlockBytes, 0, blockLength);
                this._BlockOffset = 0;
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

            public void Write(Stream output, Endian endian)
            {
                output.WriteValueU32(this.UncompressedSize, endian);
                output.WriteValueU32(this.Unknown04, endian);
                output.WriteValueU32(this.Unknown08, endian);
                output.WriteValueU32(this.Unknown0C, endian);
                output.WriteValueU32(this.CompressedSize, endian);
                output.WriteValueU32(this.Unknown14, endian);
                output.WriteValueU32(this.Unknown18, endian);
                output.WriteValueU32(this.Unknown1C, endian);
            }
        }

        private bool FlushCompressedBlock()
        {
            using (var data = new MemoryStream())
            {
                /*var blockLength = Array.FindLastIndex(this._BlockBytes, this._BlockOffset - 1, b => b != 0);
                blockLength = 1 + (blockLength < 0 ? 0 : blockLength);*/
                var blockLength = this._BlockOffset;

                var zlib = new DeflaterOutputStream(data);
                zlib.Write(this._BlockBytes, 0, blockLength);
                zlib.Finish();
                data.Flush();

                var compressedLength = (int)data.Length;
                if (data.Length < blockLength)
                {
                    this._BaseStream.WriteValueS32(32 + compressedLength, this._Endian);
                    this._BaseStream.WriteValueU8(1);
                    CompressedBlockHeader compressedBlockHeader;
                    compressedBlockHeader.UncompressedSize = (uint)blockLength;
                    compressedBlockHeader.Unknown04 = 32;
                    compressedBlockHeader.Unknown08 = 81920;
                    compressedBlockHeader.Unknown0C = 135200769;
                    compressedBlockHeader.CompressedSize = (uint)compressedLength;
                    compressedBlockHeader.Unknown14 = 0;
                    compressedBlockHeader.Unknown18 = 0;
                    compressedBlockHeader.Unknown1C = 0;
                    compressedBlockHeader.Write(this._BaseStream, this._Endian);
                    this._BaseStream.Write(data.GetBuffer(), 0, compressedLength);
                    this._BlockOffset = 0;
                    return true;
                }
            }
            return false;
        }

        public void Finish()
        {
            this._BaseStream.WriteValueS32(0, this._Endian);
            this._BaseStream.WriteValueU8(0);
        }
        #endregion

        public static BlockWriterStream ToStream(Stream baseStream, uint alignment, Endian endian, bool compress)
        {
            var instance = new BlockWriterStream(baseStream, alignment, endian, compress);
            baseStream.WriteValueU32(Signature, endian);
            baseStream.WriteValueU32(alignment, endian);
            baseStream.WriteValueU8(4);
            return instance;
        }
    }
}

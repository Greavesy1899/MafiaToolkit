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
using System.Diagnostics;
using System.IO;
using Gibbed.IO;
using OodleSharp;
using Utils.Settings;
using ZLibNet;

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
        private readonly bool _bUseOodle;

        private BlockWriterStream(Stream baseStream, uint alignment, Endian endian, bool compress, bool bUseOodle)
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
            this._Alignment = alignment;
            if (bUseOodle)
            {
                this._bUseOodle = ToolkitSettings.bUseOodleCompression;
            }
        }

        #region Stream
        public override bool CanRead {
            get { return false; }
        }

        public override bool CanSeek {
            get { return false; }
        }

        public override bool CanWrite {
            get { return true; }
        }

        public override void Flush()
        {
            this.FlushBlock();
        }

        public override long Length {
            get { throw new NotSupportedException(); }
        }

        public override long Position {
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
                var blockLength = this._BlockOffset;
                this._BaseStream.WriteValueS32(blockLength, this._Endian);
                this._BaseStream.WriteValueU8(0);
                this._BaseStream.Write(this._BlockBytes, 0, blockLength);
                this._BlockOffset = 0;
            }
        }

        private bool FlushCompressedBlock()
        {
            using (var data = new MemoryStream())
            {
                var blockLength = this._BlockOffset;

                if (this._bUseOodle)
                {
                    return FlushOodleCompressedBlock(data, blockLength);
                }
                else
                {
                    return FlushZlibCompressedBlock(data, blockLength);
                }
            }
        }

        private bool FlushZlibCompressedBlock(MemoryStream data, int blockLength)
        {
            var zlib = new ZLibStream(data, CompressionMode.Compress, CompressionLevel.Level9);
            zlib.Write(this._BlockBytes, 0, blockLength);
            zlib.Flush();
            var compressedLength = (int)data.Length;
            if (data.Length < blockLength)
            {
                this._BaseStream.WriteValueS32(32 + compressedLength, this._Endian);
                this._BaseStream.WriteValueU8(1);
                CompressedBlockHeader compressedBlockHeader = new CompressedBlockHeader();
                compressedBlockHeader.SetZlibPreset();
                compressedBlockHeader.UncompressedSize = (uint)blockLength; //TODO: I think this should actually be alignment?
                compressedBlockHeader.CompressedSize = (uint)compressedLength;
                compressedBlockHeader.ChunkSize = (short)_Alignment;
                compressedBlockHeader.Unknown0C = 135200769;
                compressedBlockHeader.Chunks[0] = (ushort)compressedBlockHeader.CompressedSize;
                compressedBlockHeader.Write(this._BaseStream, this._Endian);
                this._BaseStream.Write(data.GetBuffer(), 0, compressedLength);
                this._BlockOffset = 0;
                zlib.Close();
                zlib.Dispose();
                return true;
            }

            return false;
        }

        private bool FlushOodleCompressedBlock(MemoryStream data, int blockLength)
        {
            byte[] compressed = Oodle.Compress(this._BlockBytes, blockLength, OodleFormat.Kraken, OodleCompressionLevel.Normal);
            Debug.Assert(compressed.Length != 0, "Compressed Block should not be empty");
            data.WriteBytes(compressed);

            var compressedLength = (int)data.Length;
            if(data.Length < blockLength)
            {
                this._BaseStream.WriteValueS32(128 + compressedLength, this._Endian);
                this._BaseStream.WriteValueU8(1);
                CompressedBlockHeader compressedBlockHeader = new CompressedBlockHeader();
                compressedBlockHeader.SetOodlePreset();
                compressedBlockHeader.UncompressedSize = (uint)blockLength;
                compressedBlockHeader.CompressedSize = (uint)compressedLength;
                compressedBlockHeader.ChunkSize = 1;
                compressedBlockHeader.Unknown0C = (uint)blockLength;
                compressedBlockHeader.Chunks[0] = (ushort)compressedBlockHeader.CompressedSize;
                Console.WriteLine(compressedBlockHeader);
                compressedBlockHeader.Write(this._BaseStream, this._Endian);
                this._BaseStream.Write(new byte[96], 0, 96); // Empty padding.
                this._BaseStream.Write(data.GetBuffer(), 0, compressedLength);
                this._BlockOffset = 0;
                return true;
            }

            return false;
        }

        public void Finish()
        {
            this._BaseStream.WriteValueS32(0, this._Endian);
            this._BaseStream.WriteValueU8(0);
        }
        #endregion

        public static BlockWriterStream ToStream(Stream baseStream, uint alignment, Endian endian, bool compress, bool bUseOodle)
        {
            var instance = new BlockWriterStream(baseStream, alignment, endian, compress, bUseOodle);
            baseStream.WriteValueU32(Signature, endian);          
            var headerAlignment = (instance._bUseOodle && compress ? (alignment | 0x1000000) : alignment);
            baseStream.WriteValueU32(headerAlignment, endian);
            baseStream.WriteValueU8(4);
            return instance;
        }
    }
}
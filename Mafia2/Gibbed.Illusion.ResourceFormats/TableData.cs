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
using Mafia2;
using System.IO;
using Gibbed.Illusion.FileFormats;
using Gibbed.IO;

namespace Gibbed.Mafia2.ResourceFormats
{
    public class TableData : IResourceType
    {
        public ulong NameHash;
        public string Name;
        public uint Unk1;
        public uint Unk2;
        private uint rowSize;
        private uint rowCount;
        public byte[] Data;

        public List<Row> Rows = new List<Row>();
        public List<Column> Columns = new List<Column>();

        public override string ToString()
        {
            return this.Name;
        }

        public void Serialize(ushort version, Stream input, Endian endian)
        {
            input.WriteValueU64(NameHash, endian);
            input.WriteStringU16(Name, endian);

            if (version >= 2)
            {
                throw new NotSupportedException();
            }
            else
            {
                input.WriteValueU16((ushort)Columns.Count, endian);
                input.WriteValueU32(Unk1, endian);
                input.WriteValueU32(Unk2, endian);
                input.WriteValueU32(rowSize, endian);
                input.WriteValueU32(rowCount, endian);
                input.WriteBytes(Data);
                input = null;
            }
        }

        public void Deserialize(ushort version, Stream input, Endian endian)
        {
            this.NameHash = input.ReadValueU64(endian);
            this.Name = input.ReadStringU16(endian);

            if (version >= 2)
            {
                throw new NotSupportedException();
            }
            else
            {
                var columnCount = input.ReadValueU16(endian);

                Unk1 = input.ReadValueU32(endian);
                Unk2 = input.ReadValueU32(endian);

                rowSize = input.ReadValueU32(endian);
                rowCount = input.ReadValueU32(endian);
                var data = input.ReadToMemoryStream((int)(rowSize * rowCount));
                Data = data.ReadBytes((int)data.Length);

                this.Columns = new List<Column>();
                for (uint i = 0; i < columnCount; i++)
                {
                    this.Columns.Add(new Column()
                    {
                        NameHash = input.ReadValueU32(endian),
                        Type = (ColumnType)input.ReadValueU8(),
                        Unknown2 = input.ReadValueU8(),
                        Unknown3 = input.ReadValueU16(endian),
                    });
                }

                input = null;

                this.Rows.Clear();
                for (uint i = 0; i < rowCount; i++)
                {
                    var row = new Row();

                    data.Seek(i * rowSize, SeekOrigin.Begin);
                    foreach (var column in this.Columns)
                    {
                        if ((byte)column.Type > 163)
                        {
                            throw new FormatException();
                        }

                        switch (column.Type)
                        {
                            case ColumnType.Boolean:
                            {
                                var value = data.ReadValueU32(endian);
                                if (value != 0 && value != 1)
                                {
                                    throw new FormatException();
                                }
                                row.Values.Add(value != 0);
                                break;
                            }

                            case ColumnType.Float32:
                            {
                                var value = data.ReadValueF32(endian);
                                row.Values.Add(value);
                                break;
                            }

                            case ColumnType.Signed32:
                            {
                                var value = data.ReadValueS32(endian);
                                row.Values.Add(value);
                                break;
                            }

                            case ColumnType.Unsigned32:
                            {
                                var value = data.ReadValueU32(endian);
                                row.Values.Add(value);
                                break;
                            }

                            case ColumnType.Flags32:
                            {
                                var value = data.ReadValueU32(endian);
                                row.Values.Add(value);
                                break;
                            }

                            case ColumnType.Hash64:
                            {
                                var value = data.ReadValueU64(endian);
                                row.Values.Add(value);
                                break;
                            }

                            case ColumnType.String8:
                            {
                                string value = data.ReadString(8, true);
                                row.Values.Add(value);
                                break;
                            }

                            case ColumnType.String16:
                            {
                                string value = data.ReadString(16, true);
                                row.Values.Add(value);
                                break;
                            }

                            case ColumnType.String32:
                            {
                                string value = data.ReadString(32, true);
                                row.Values.Add(value);
                                break;
                            }

                            case ColumnType.String64:
                            {
                                string value = data.ReadString(64, true);
                                row.Values.Add(value);
                                break;
                            }

                            case ColumnType.Color:
                            {
                                float r = data.ReadValueF32(endian);
                                float g = data.ReadValueF32(endian);
                                float b = data.ReadValueF32(endian);
                                // TODO: de-stupidize this
                                row.Values.Add(string.Format("{0}, {1}, {2}", r, g, b));
                                break;
                            }

                            case ColumnType.Hash64AndString32:
                            {
                                var hash = data.ReadValueU64(endian);
                                string value = data.ReadString(32, true);
                                row.Values.Add(value);
                                break;
                            }

                            default:
                            {
                                throw new FormatException();
                            }
                        }
                    }

                    this.Rows.Add(row);
                }
            }
        }

        public void Deserialize(BinaryReader reader)
        {
            this.NameHash = reader.ReadUInt64();
            this.Name = Functions.ReadString16(reader);

            short columnCount = reader.ReadInt16();

            Unk1 = reader.ReadUInt32();
            Unk2 = reader.ReadUInt32();

            rowSize = reader.ReadUInt32();
            rowCount = reader.ReadUInt32();

            this.Columns = new List<Column>();
            for (uint i = 0; i < columnCount; i++)
            {
                this.Columns.Add(new Column()
                {
                    NameHash = reader.ReadUInt32(),
                    Type = (ColumnType)reader.ReadUInt16(),
                    Unknown2 = reader.ReadByte(),
                    Unknown3 = reader.ReadUInt16(),
                });
            }

            Console.WriteLine("");

            this.Rows.Clear();
            for (uint i = 0; i < rowCount; i++)
            {
                var row = new Row();

                //reader.BaseStream.Seek(i * rowSize, SeekOrigin.Begin);
                foreach (var column in this.Columns)
                {
                    if ((byte)column.Type > 163)
                    {
                        throw new FormatException();
                    }

                    switch (column.Type)
                    {
                        case ColumnType.Boolean:
                            {
                                var value = reader.ReadUInt32();
                                if (value != 0 && value != 1)
                                {
                                    throw new FormatException();
                                }
                                row.Values.Add(value != 0);
                                break;
                            }

                        case ColumnType.Float32:
                            {
                                var value = reader.ReadSingle();
                                row.Values.Add(value);
                                break;
                            }

                        case ColumnType.Signed32:
                            {
                                var value = reader.ReadInt32();
                                row.Values.Add(value);
                                break;
                            }

                        case ColumnType.Unsigned32:
                            {
                                var value = reader.ReadUInt32();
                                row.Values.Add(value);
                                break;
                            }

                        case ColumnType.Flags32:
                            {
                                var value = reader.ReadUInt32();
                                row.Values.Add(value);
                                break;
                            }

                        case ColumnType.Hash64:
                            {
                                var value = reader.ReadUInt64();
                                row.Values.Add(value);
                                break;
                            }

                        case ColumnType.String8:
                            {
                                string value = Functions.ReadString8(reader);
                                row.Values.Add(value);
                                break;
                            }

                        case ColumnType.String16:
                            {
                                string value = Functions.ReadString16(reader);
                                row.Values.Add(value);
                                break;
                            }

                        case ColumnType.String32:
                            {
                                string value = Functions.ReadString32(reader);
                                row.Values.Add(value);
                                break;
                            }

                        case ColumnType.String64:
                            {
                                string value = Functions.ReadString64(reader);
                                row.Values.Add(value);
                                break;
                            }

                        case ColumnType.Color:
                            {
                                float r = reader.ReadSingle();
                                float g = reader.ReadSingle();
                                float b = reader.ReadSingle();
                                // TODO: de-stupidize this
                                row.Values.Add(string.Format("{0}, {1}, {2}", r, g, b));
                                break;
                            }

                        case ColumnType.Hash64AndString32:
                            {
                                var hash = reader.ReadUInt64();
                                string value = Functions.ReadString32(reader);
                                row.Values.Add(value);
                                break;
                            }

                        default:
                            {
                                throw new FormatException();
                            }
                    }
                }

                this.Rows.Add(row);
            }
        }

        public class Column
        {
            public uint NameHash;
            public ColumnType Type;
            public byte Unknown2;
            public ushort Unknown3;

            public override string ToString()
            {
                return string.Format("{0:X8} : {1} ({2}, {3})",
                                     this.NameHash,
                                     this.Type,
                                     this.Unknown2,
                                     this.Unknown3);
            }
        }

        public class Row
        {
            public List<object> Values = new List<object>();

            public override string ToString()
            {
                var values = new string[this.Values.Count];
                for (int i = 0; i < this.Values.Count; i++)
                {
                    values[i] = this.Values[i].ToString();
                }
                return string.Join(", ", values);
            }
        }

        public enum ColumnType : byte
        {
            Boolean = 1,
            Float32 = 2,
            Signed32 = 3,
            Unsigned32 = 4,
            Flags32 = 5,
            Hash64 = 6,
            String8 = 8,
            String16 = 16,
            String32 = 32,
            String64 = 64,
            Color = 66,
            Hash64AndString32 = 132,
        }

        public static Type GetValueTypeForColumnType(ColumnType type)
        {
            switch (type)
            {
                case ColumnType.Boolean:
                {
                    return typeof(bool);
                }

                case ColumnType.Float32:
                {
                    return typeof(float);
                }

                case ColumnType.Signed32:
                {
                    return typeof(int);
                }

                case ColumnType.Unsigned32:
                {
                    return typeof(uint);
                }

                case ColumnType.Flags32:
                {
                    return typeof(uint);
                }

                case ColumnType.Hash64:
                {
                    return typeof(ulong);
                }

                case ColumnType.String8:
                case ColumnType.String16:
                case ColumnType.String32:
                case ColumnType.String64:
                {
                    return typeof(string);
                }

                /*
                case ColumnType.Color:
                {
                    return typeof(Color);
                }
                */

                case ColumnType.Hash64AndString32:
                {
                    return typeof(string);
                }
            }

            throw new ArgumentException("unhandled type", "type");
        }
    }
}

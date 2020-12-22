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
using System.Windows;
using Gibbed.Illusion.FileFormats;
using Gibbed.Illusion.FileFormats.Hashing;
using Gibbed.IO;
using Utils.StringHelpers;

namespace Gibbed.Mafia2.ResourceFormats
{
    public class TableData : IResourceType
    {
        public ulong NameHash;
        public string Name;
        public uint Unk1;
        public uint Unk2;
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

            if(version >= 2)
            {
                input.WriteBytes(new byte[10]);
                input.WriteValueS32(-1);
                input.WriteValueS32(0);
            }

            input.WriteValueU16((ushort)Columns.Count, endian);
            input.WriteValueU32(Unk1, endian);
            input.WriteValueU32(Unk2, endian);
            input.WriteValueU32((uint)(Data.Length / Rows.Count));
            input.WriteValueU32((uint)Rows.Count);

            for (int i = 0; i < Rows.Count; i++)
            {
                for (int x = 0; x < Columns.Count; x++)
                {
                    Column column = Columns[x];
                    switch (column.Type)
                    {
                        case ColumnType.Boolean:
                            uint value = Convert.ToUInt32(Rows[i].Values[x]);
                            input.WriteValueU32(value);
                            break;
                        case ColumnType.Float32:
                            input.WriteValueF32((float)Rows[i].Values[x]);
                            break;
                        case ColumnType.Signed32:
                            input.WriteValueS32((int)Rows[i].Values[x]);
                            break;
                        case ColumnType.Unsigned32:
                            input.WriteValueU32((uint)Rows[i].Values[x]);
                            break;
                        case ColumnType.Flags32:
                            input.WriteValueU32((uint)Rows[i].Values[x]);
                            break;
                        case ColumnType.Hash64:
                            input.WriteValueU64((ulong)Rows[i].Values[x]);
                            break;
                        case ColumnType.String8:
                            input.WriteString(Rows[i].Values[x].ToString(), 8);
                            break;
                        case ColumnType.String16:
                            input.WriteString(Rows[i].Values[x].ToString(), 16);
                            break;
                        case ColumnType.String32:
                            input.WriteString(Rows[i].Values[x].ToString(), 32, System.Text.Encoding.GetEncoding(1250));
                            break;
                        case ColumnType.String64:
                            input.WriteString(Rows[i].Values[x].ToString(), 64);
                            break;
                        case ColumnType.Color:
                            string[] colors = (Rows[i].Values[x] as string).Split(new char[1] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            input.WriteValueF32(float.Parse(colors[0]));
                            input.WriteValueF32(float.Parse(colors[1]));
                            input.WriteValueF32(float.Parse(colors[2]));
                            break;
                        case ColumnType.Hash64AndString32:
                            string name = (string)Rows[i].Values[x];
                            ulong hash = FNV64.Hash(name);
                            input.WriteValueU64(hash);
                            input.WriteString(Rows[i].Values[x].ToString(), 32);
                            break;
                        default:
                            throw new FormatException();
                            break;
                    }
                }
            }

            for (int i = 0; i < Columns.Count; i++)
            {
                Columns[i].Serialize(input, endian);
            }
        }

        public bool Validate()
        {
            bool bIsTableValid = true;

            for (int i = 0; i < Rows.Count; i++)
            {
                for (int x = 0; x < Columns.Count; x++)
                {
                    Column column = Columns[x];
                    object value = Rows[i].Values[x];
                    bool bIsCellValid = true;

                    switch (column.Type)
                    {
                        case ColumnType.Boolean:
                            ConvertToType<bool>(value, ref bIsCellValid);
                            break;
                        case ColumnType.Float32:
                            ConvertToType<float>(value, ref bIsCellValid);
                            break;
                        case ColumnType.Signed32:
                            ConvertToType<int>(value, ref bIsCellValid);
                            break;
                        case ColumnType.Unsigned32:
                        case ColumnType.Flags32:
                            ConvertToType<uint>(value, ref bIsCellValid);
                            break;
                        case ColumnType.Hash64:
                            ConvertToType<ulong>(value, ref bIsCellValid);
                            break;
                        case ColumnType.String8:
                        case ColumnType.String16:
                        case ColumnType.String32:
                        case ColumnType.String64:
                            // TODO: Should we check this?
                            break;
                        case ColumnType.Color:
                            string[] colors = (Rows[i].Values[x] as string).Split(new char[1] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            if(colors.Length == 3)
                            {
                                foreach(var colour in colors)
                                {
                                    ConvertToType<float>(colour, ref bIsCellValid);
                                }
                            }
                            break;
                        case ColumnType.Hash64AndString32:
                            // TODO: Should we check this?
                            break;
                        default:
                            throw new FormatException();
                    }

                    if(!bIsCellValid)
                    {
                        string ErrorMessage = string.Format("Error validating cell X: {0} Y: {1}", i, x);
                        MessageBox.Show(ErrorMessage, "Toolkit", MessageBoxButton.OK);
                    }

                    // TODO: Do we want to do this? is it better to iterate through them all and tell them 
                    // which ones failed validation?

                    // If cell is invalid break the validation, as we know the table contains invalid data
                    bIsTableValid = bIsCellValid;
                    if(!bIsTableValid)
                    {
                        return bIsTableValid;
                    }
                }
            }

            return bIsTableValid;
        }

        private T ConvertToType<T>(object ObjectToConvert, ref bool bIsValid)
        {
            // Get type we can to cast to.
            T TypeToCast = Activator.CreateInstance<T>();
            Type TypeOfObject = TypeToCast.GetType();

            // Try and attempt to cast
            T Output = Activator.CreateInstance<T>();

            try
            {
                Output = (T)Convert.ChangeType(ObjectToConvert, TypeOfObject);
            }
            catch (Exception ex)
            {
                Type TypeOfPassedObject = ObjectToConvert.GetType();
                string ErrorMessage = string.Format("Failed to cast object of type {0} to {1}", TypeOfObject.Name, TypeOfPassedObject.Name);
                MessageBox.Show(ErrorMessage, "Toolkit", MessageBoxButton.OK);
                bIsValid = false;
            }

            return Output;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(NameHash);
            StringHelpers.WriteString16(writer, Name);
            writer.Write((ushort)Columns.Count);
            writer.Write(Unk1);
            writer.Write(Unk2);
            writer.Write((uint)CalculateRowSize());
            writer.Write((uint)Rows.Count);

            for (int i = 0; i < Rows.Count; i++)
            {
                for (int x = 0; x < Columns.Count; x++)
                {
                    Column column = Columns[x];
                    object value = Rows[i].Values[x];
                    bool isValid = true;

                    switch (column.Type)
                    {
                        case ColumnType.Boolean:
                            writer.Write(ConvertToType<bool>(value, ref isValid));
                            break;
                        case ColumnType.Float32:
                            writer.Write(ConvertToType<float>(value, ref isValid));
                            break;
                        case ColumnType.Signed32:
                            writer.Write(ConvertToType<int>(value, ref isValid));
                            break;
                        case ColumnType.Unsigned32:
                        case ColumnType.Flags32:
                            writer.Write(ConvertToType<uint>(value, ref isValid));
                            break;
                        case ColumnType.Hash64:
                            writer.Write(ConvertToType<ulong>(value, ref isValid));
                            break;
                        case ColumnType.String8:
                            StringHelpers.WriteStringBuffer(writer, 8, (string)Rows[i].Values[x]);
                            break;
                        case ColumnType.String16:
                            StringHelpers.WriteStringBuffer(writer, 16, (string)Rows[i].Values[x]);
                            break;
                        case ColumnType.String32:
                            StringHelpers.WriteStringBuffer(writer, 32, (string)Rows[i].Values[x], ' ', System.Text.Encoding.GetEncoding(1250));
                            break;
                        case ColumnType.String64:
                            StringHelpers.WriteStringBuffer(writer, 64, (string)Rows[i].Values[x]);
                            break;
                        case ColumnType.Color:
                            string[] colors = (Rows[i].Values[x] as string).Split(new char[1] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach(var colour in colors)
                            {
                                writer.Write(ConvertToType<float>(colour, ref isValid));

                            }
                            break;
                        case ColumnType.Hash64AndString32:
                            string name = (string)Rows[i].Values[x];
                            ulong hash = FNV64.Hash(name);
                            writer.Write(hash);
                            StringHelpers.WriteStringBuffer(writer, 32, !string.IsNullOrEmpty(name) ? name : "");
                            break;
                        default:
                            throw new FormatException();
                    }
                }
            }

            for (int i = 0; i < Columns.Count; i++)
                Columns[i].Serialize(writer);
        }

        public void Deserialize(ushort version, Stream input, Endian endian)
        {
            this.NameHash = input.ReadValueU64(endian);
            this.Name = input.ReadStringU16(endian);

            if(version >= 2)
            {
                input.ReadBytes(18);
            }

            var columnCount = input.ReadValueU16(endian);

            Unk1 = input.ReadValueU32(endian);
            Unk2 = input.ReadValueU32(endian);

            var rowSize = input.ReadValueU32(endian);
            var rowCount = input.ReadValueU32(endian);
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

                    object DeserializedObject = column.DeserializeType(data, endian);
                    row.Values.Add(DeserializedObject);
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

            public void Serialize(Stream input, Endian endian)
            {
                input.WriteValueU32(NameHash);
                input.WriteValueU8((byte)Type);
                input.WriteValueU8(Unknown2);
                input.WriteValueU16(Unknown3);
            }

            public void Serialize(BinaryWriter writer)
            {
                writer.Write(NameHash);
                writer.Write((byte)Type);
                writer.Write(Unknown2);
                writer.Write(Unknown3);
            }

            public object DeserializeType(MemoryStream data, Endian endian)
            {
                object DeserializedObject = null;

                switch (Type)
                {
                    case ColumnType.Boolean:
                        {
                            var value = data.ReadValueU32(endian);
                            if (value != 0 && value != 1)
                            {
                                throw new FormatException();
                            }
                            DeserializedObject = value != 0;
                            break;
                        }

                    case ColumnType.Float32:
                        {
                            var value = data.ReadValueF32(endian);
                            DeserializedObject = value;
                            break;
                        }

                    case ColumnType.Signed32:
                        {
                            var value = data.ReadValueS32(endian);
                            DeserializedObject = value;
                            break;
                        }

                    case ColumnType.Unsigned32:
                        {
                            var value = data.ReadValueU32(endian);
                            DeserializedObject = value;
                            break;
                        }

                    case ColumnType.Flags32:
                        {
                            var value = data.ReadValueU32(endian);
                            DeserializedObject = value;
                            break;
                        }

                    case ColumnType.Hash64:
                        {
                            var value = data.ReadValueU64(endian);
                            DeserializedObject = value;
                            break;
                        }

                    case ColumnType.String8:
                        {
                            string value = data.ReadString(8, true);
                            DeserializedObject = value;
                            break;
                        }

                    case ColumnType.String16:
                        {
                            string value = data.ReadString(16, true);
                            DeserializedObject = value;
                            break;
                        }

                    case ColumnType.String32:
                        {
                            string value = data.ReadString(32, true, System.Text.Encoding.GetEncoding(1250));
                            DeserializedObject = value;
                            break;
                        }

                    case ColumnType.String64:
                        {
                            string value = data.ReadString(64, true);
                            DeserializedObject = value;
                            break;
                        }

                    case ColumnType.Color:
                        {
                            float r = data.ReadValueF32(endian);
                            float g = data.ReadValueF32(endian);
                            float b = data.ReadValueF32(endian);
                            DeserializedObject = string.Format("{0} {1} {2}", r, g, b);
                            break;
                        }

                    case ColumnType.Hash64AndString32:
                        {
                            var hash = data.ReadValueU64(endian);
                            string value = data.ReadString(32, true);
                            DeserializedObject = value;
                            break;
                        }

                    default:
                        {
                            throw new FormatException();
                        }
                }

                return DeserializedObject;
            }

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

        private int CalculateRowSize()
        {
            int rowSize = 0;

            foreach (Column col in Columns)
            {
                switch (col.Type)
                {
                    case ColumnType.Boolean:
                    case ColumnType.Float32:
                    case ColumnType.Signed32:
                    case ColumnType.Unsigned32:
                    case ColumnType.Flags32:
                        rowSize += 4;
                        break;
                    case ColumnType.Hash64:
                        rowSize += 8;
                        break;
                    case ColumnType.String8:
                        rowSize += 8;
                        break;
                    case ColumnType.Color:
                        rowSize += 12;
                        break;
                    case ColumnType.String16:
                        rowSize += 16;
                        break;
                    case ColumnType.String32:
                        rowSize += 32;
                        break;
                    case ColumnType.Hash64AndString32:
                        rowSize += 40;
                        break;
                    case ColumnType.String64:
                        rowSize += 64;
                        break;
                    default:
                        throw new FormatException();
                }
            }
            return rowSize;
        }
    }
}
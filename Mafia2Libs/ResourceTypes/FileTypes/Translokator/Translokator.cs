﻿using ResourceTypes.Actors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Numerics;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using Utils.Extensions;
using Utils.Helpers.Reflection;
using Utils.Logging;
using Utils.StringHelpers;
using Utils.Types;
using Utils.VorticeUtils;
using Vortice.Mathematics;

namespace ResourceTypes.Translokator
{
    public class Grid
    {
        short key;
        Vector3 origin;
        Vector2 cellSize;
        int width;
        int height;
        ushort[] data;

        public short Key {
            get { return key; }
            set { key = value; }
        }
        [TypeConverter(typeof(Vector3Converter)), ReadOnly(true)]
        public Vector3 Origin {
            get { return origin; }
            set { origin = value; }
        }
        [TypeConverter(typeof(Vector2Converter)), ReadOnly(true)]
        public Vector2 CellSize {
            get { return cellSize; }
            set { cellSize = value; }
        }
        public int Width {
            get { return width; }
            set { width = value; }
        }
        public int Height {
            get { return height; }
            set { height = value; }
        }
        public ushort[] Data {
            get { return data; }
            set { data = value; }
        }
    }

    public class Instance
    {
        Vector3 position;
        Vector3 rotation;
        Quaternion quatRot;
        float scale;
        ushort w0;
        ushort w1;
        ushort w2;
        int d5;
        ushort id;
        ushort d4;
        public int RefID;

        [TypeConverter(typeof(Vector3Converter))]
        public Vector3 Position {
            get { return position; }
            set { position = value; }
        }

        [TypeConverter(typeof(Vector3Converter))]
        public Vector3 Rotation {
            get { return rotation; }
            set { rotation = value; UpdateQuaternion(); }
        }

        public Quaternion Quaternion {
            get { return quatRot; }
            private set { quatRot = value; }
        }

        public float Scale {
            get { return scale; }
            set { scale = value; }
        }

        public ushort ID {
            get { return id; }
            set { id = value; }
        }
        [Browsable(false)]
        public ushort W0 {
            get { return w0; }
            set { w0 = value; }
        }
        [Browsable(false)]
        public ushort W1 {
            get { return w1; }
            set { w1 = value; }
        }
        [Browsable(false)]
        public ushort W2 {
            get { return w2; }
            set { w2 = value; }
        }
        [Browsable(false)]
        public ushort D4 {
            get { return d4; }
            set { d4 = value; }
        }
        [Browsable(false)]
        public int D5 {
            get { return d5; }
            set { d5 = value; }
        }

        public Instance()
        {
            Scale = 1.0f;
        }

        public Instance(Instance other)
        {
            position = other.position;
            rotation = other.rotation;
            scale = other.scale;
            Quaternion = other.Quaternion;
            w0 = other.w0;
            w1 = other.w1;
            w2 = other.w2;
            id = other.id;
            d4 = other.d4;
            d5 = other.d5;
        }

        private void UpdateQuaternion()
        {
            Quaternion q = Quaternion.Identity;

            float pitch = MathHelper.ToRadians(Rotation.X);
            float yaw = MathHelper.ToRadians(Rotation.Y);
            float roll = MathHelper.ToRadians(Rotation.Z);

            float v12 = pitch * 0.5f;
            float v8 = MathF.Sin(v12);
            float v11 = v8;
            float v13 = MathF.Cos(v12);
            float v10 = v13;
            float v14 = yaw * 0.5f;
            float v18 = MathF.Sin(v14);
            float v15 = MathF.Cos(v14);
            float v9 = v15;
            float v16 = roll * 0.5f;
            float v19 = MathF.Sin(v16);
            float v17 = MathF.Cos(v16);
            float v4 = v17 * v9;
            float v5 = v19 * v18;
            q.X = v10 * v5 + v11 * v4;
            float v6 = v17 * v18;
            float v7 = v9 * v19;
            q.Y = v11 * v7 + v10 * v6;
            q.Z = (v7 * v10 - v6 * v11);
            q.W = -(v4 * v10 - v5 * v11);

            Quaternion = q;
        }
    }

    public class Object
    {
        short unk02;
        HashName name;
        byte[] unkBytes1;
        float gridMax;
        float gridMin;
        Instance[] instances;

        public short Unk02 {
            get { return unk02; }
            set { unk02 = value; }
        }
        public HashName Name {
            get { return name; }
            set { name = value; }
        }
        [Browsable(false)]
        public byte[] UnkBytes1 {
            get { return unkBytes1; }
            set { unkBytes1 = value; }
        }
        public float GridMax {
            get { return gridMax; }
            set { gridMax = value; }
        }
        public float GridMin {
            get { return gridMin; }
            set { gridMin = value; }
        }
        [Browsable(false)]
        public Instance[] Instances {
            get { return instances; }
            set { instances = value; }
        }

        public Object()
        {
            name = new HashName();
            instances = new Instance[0];
        }

        public Object(Object other)
        {
            instances = new Instance[0];
            unk02 = other.unk02;
            name = other.name;
            unkBytes1 = other.unkBytes1;
            gridMax = other.gridMax;
            gridMin = other.gridMin;
        }
    }

    public class ObjectGroup
    {
        public ActorTypes ActorType { get; set; }
        public short Unk01 { get; set; }
        public Object[] Objects { get; set; }

        public ObjectGroup()
        {
            Objects = new Object[0];
            ActorType = ActorTypes.None;
        }
    }

    public struct ObjectGroupMetaInfo
    {
        public ActorTypes ActorTypeID { get; set; }
        public short Unk0 { get; set; }
    }

    public class TranslokatorLoader
    {
        public Grid[] Grids { get; set; }
        public ObjectGroup[] ObjectGroups { get; set; }

        int version;
        int unk1;
        short unk2;
        BoundingBox bounds;

        public int Version {
            get { return version; }
            set { version = value; }
        }
        public int Unk1 {
            get { return unk1; }
            set { unk1 = value; }
        }
        public short Unk2 {
            get { return unk2; }
            set { unk2 = value; }
        }
        [PropertyIgnoreByReflector]
        public BoundingBox Bounds {
            get { return bounds; }
            set { bounds = value; }
        }

        public TranslokatorLoader()
        {

        }

        public TranslokatorLoader(FileInfo info)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(info.FullName, FileMode.Open)))
            {
                ReadFromFile(reader);
            }
        }

        private void DecompressRotation(Instance instance)
        {
            var v7 = 0.0;
            var v8 = 3.141592741012573;
            var Y = 0.0;
            var X = 0.0;

            if ((instance.D4 & 0x10) != 0)
            {
                var v4 = instance.D5 >> 9;
                var v5 = v4 & 0x1FF;
                X = (instance.D5 & 0x1FF) * 0.001956947147846222 * v8 * 2.0 - v8;
                Y = v5 * 0.001956947147846222 * v8 * 2.0 - v8; //0.001956947147846222 == 1.0f/511.0f
                v7 = 0.001956947147846222 * ((v4 >> 9) & 0x1FF) * v8;
            }
            else
            {
                var v9 = instance.D5 & 0x1FF | 16 * (instance.D4 & 0xE0);
                var v10 = instance.D5 >> 9;
                var v5 = v10 & 0x1FF | 2 * (instance.D4 & 0xF00);
                X = v9 * 0.0002442002587486058 * v8 * 2.0 - v8; //0.0002442002587486058 == 1.0f/4095.0f
                Y = v5 * 0.0001220852136611938 * v8 * 2.0 - v8; //0.0001220852136611938 == 1.0f/8191.0f
                v7 = 0.0001220852136611938 * ((instance.D4 & 0xF000 | (v10 >> 6) & 0xFF8) >> 3) * v8;
            }
            var Z = 2.0f * v7 - v8;
            X = MathHelper.ToDegrees((float)X);
            Y = MathHelper.ToDegrees((float)Y);
            Z = MathHelper.ToDegrees((float)Z);
            instance.Rotation = new Vector3((float)X, (float)Y, (float)Z);
        }

        private void CompressRotation(Instance instance)
        {
            MathHelper.ToRadians(instance.Rotation.X);
            double x = MathHelper.ToRadians(instance.Rotation.X);
            double y = MathHelper.ToRadians(instance.Rotation.Y);
            double z = MathHelper.ToRadians(instance.Rotation.Z);
            var v8 = 3.141592741012573;
            if (instance.Scale != 1)
            {
                x = (x / Math.PI + 1.0f) / 2.0f * 511.0f;
                y = (y / Math.PI + 1.0f) / 2.0f * 511.0f;
                z = (z / Math.PI + 1.0f) / 2.0f * 511.0f;
            }
            else
            {
                x = ((x / Math.PI + 1.0f) / 2.0f);
                y = ((y / Math.PI + 1.0f) / 2.0f);
                z = ((z / Math.PI + 1.0f) / 2.0f);
                x *= 4095f;
                y *= 8191f;
                z *= 8191f;
                var xxs = Convert.ToInt16(x);
                var yys = Convert.ToInt16(y);
                var zzs = Convert.ToInt16(z);
                var higherX = (xxs >> 9);
                var higherY = (yys >> 9);
                var higherZ = (zzs >> 9);
                instance.D4 |= (ushort)((higherZ << 12) | (higherY << 8) | (higherX << 5));
            }

            var xs = Convert.ToInt16(x);
            var lowerX = (ushort)(xs & 0x1FF);
            var ys = Convert.ToInt16(y);
            var lowerY = (ushort)(ys & 0x1FF);
            var zs = Convert.ToInt16(z);
            var lowerZ = (ushort)(zs & 0x1FF);
            instance.D5 |= (lowerZ << 18) | (lowerY << 9) | (lowerX);
        }
        public Vector3 DecompressPosition(byte[] transform, Instance instance, Vector3 tmin, Vector3 tmax, bool debug = false)
        {
            Vector3 position = new Vector3();
            var _1_65535 = 1.0f / 65535.0f;
            ushort x = BitConverter.ToUInt16(transform, 0);
            ushort y = BitConverter.ToUInt16(transform, 2);
            ushort z = BitConverter.ToUInt16(transform, 4);
            var xFrac = x * _1_65535;
            var yFrac = y * _1_65535;
            var zFrac = z * _1_65535;
            var xFull = ((transform[9] >> 6) & 3);
            var yfull = (float)(transform[12] & 3);
            var zfull = (float)((transform[12] >> 2) & 3);
            var xpositive = (transform[9] & 0x08) == 0;
            var ypositive = (transform[9] & 0x10) == 0;
            var zpositive = (transform[9] & 0x20) == 0;
            Vector3 tref = new Vector3((xpositive ? tmax.X : -tmin.X) * 0.25f, (ypositive ? tmax.Y : -tmin.Y) * 0.25f, (zpositive ? tmax.Z : -tmin.Z) * 0.25f);
            Vector3 final = new Vector3((xFull + xFrac) * tref.X, (yfull + yFrac) * tref.Y, (zfull + zFrac) * tref.Z);
            position = new Vector3(xpositive ? final.X : -final.X, ypositive ? final.Y : -final.Y, zpositive ? final.Z : -final.Z);
            return position;
        }
        public void CompressPosition(Instance instance, Vector3 tmin, Vector3 tmax, bool debug = false)
        {
            var vector = instance.Position;
            var xPositive = vector.X > 0.0f ? true : false;
            var yPositive = vector.Y > 0.0f ? true : false;
            var zPositive = vector.Z > 0.0f ? true : false;
            Vector3 tref = new Vector3((xPositive ? tmax.X : tmin.X) * 0.25f, (yPositive ? tmax.Y : tmin.Y) * 0.25f, (zPositive ? tmax.Z : tmin.Z) * 0.25f);
            Vector3 preFinal = new Vector3(vector.X / tref.X, vector.Y / tref.Y, vector.Z / tref.Z);
            preFinal.X = Math.Abs(preFinal.X);
            preFinal.Y = Math.Abs(preFinal.Y);
            preFinal.Z = Math.Abs(preFinal.Z);
            var xFull = Convert.ToInt32(Math.Floor(preFinal.X));
            var yFull = Convert.ToInt32(Math.Floor(preFinal.Y));
            var zFull = Convert.ToInt32(Math.Floor(preFinal.Z));
            bool x4 = false, y4 = false, z4 = false;
            if (xFull == 4)
            {
                x4 = true;
                xFull = 3;
            }
            if (yFull == 4)
            {
                y4 = true;
                yFull = 3;
            }
            if (zFull == 4)
            {
                z4 = true;
                zFull = 3;
            }
            xFull = xFull >= 4 ? 3 : xFull;
            yFull = yFull >= 4 ? 3 : yFull;
            zFull = zFull >= 4 ? 3 : zFull;
            var iXPos = xPositive == true ? 0 : 1;
            var iYPos = yPositive == true ? 0 : 1;
            var iZPos = zPositive == true ? 0 : 1;
            instance.D4 |= (ushort)((zFull << 2) | (yFull));
            instance.D5 |= (xFull << 30);
            instance.D5 |= (iZPos << 29);
            instance.D5 |= (iYPos << 28);
            instance.D5 |= (iXPos << 27);
            var xFrac = preFinal.X > 1.0f ? preFinal.X - Math.Floor(preFinal.X) : preFinal.X;
            var yFrac = preFinal.Y > 1.0f ? preFinal.Y - Math.Floor(preFinal.Y) : preFinal.Y;
            var zFrac = preFinal.Z > 1.0f ? preFinal.Z - Math.Floor(preFinal.Z) : preFinal.Z;
            var preX = x4 == true ? 65535 : xFrac * 65535.0f;
            var preY = y4 == true ? 65535 : yFrac * 65535.0f;
            var preZ = z4 == true ? 65535 : zFrac * 65535.0f;
            instance.W0 = Convert.ToUInt16(preX);
            instance.W1 = Convert.ToUInt16(preY);
            instance.W2 = Convert.ToUInt16(preZ);
        }
        public void DecompressScale(Instance transform)
        {
            var scale = 1.0f;

            if ((transform.D4 & 0x10) != 0)
            {
                var s = transform.D4 & 0xFFE0;
                var e = (s >> 10) & 0x1F;
                if (e != 0)
                {
                    var sign = (s << 16) & 0x80000000;
                    var exponent = ((e + 127 - 15) << 23);
                    var mantissa = (s << 13) & 0x7C0000;
                    var t = sign | exponent | mantissa;
                    var bytes = BitConverter.GetBytes(t);
                    scale = BitConverter.ToSingle(bytes, 0);
                }
            }
            transform.Scale = scale;
        }
        public void CompressScale(Instance transform)
        {
            if (transform.Scale != 1)
            {
                byte[] data = BitConverter.GetBytes(transform.Scale);
                var scalei = BitConverter.ToInt32(data, 0);
                var sign = scalei >> 31;
                var exponent = ((scalei >> 23) - 127 + 15) & 0x1F;
                var mantissa = (scalei >> 18) & 0x1F;

                transform.D4 |= (ushort)((sign << 15) | (exponent << 10) | (mantissa << 5) | (1u << 4));
            }
        }
        private void CompileData()
        {
            #region calculate bounding box
            Vector3 Min = Vector3.Zero;
            Vector3 Max = Vector3.Zero;
            ushort numInstance = 0;

            for (int i = 0; i < ObjectGroups.Length; i++)
            {
                ObjectGroup objectGroup = ObjectGroups[i];

                for (int x = 0; x < objectGroup.Objects.Length; x++)
                {
                    Object obj = objectGroup.Objects[x];

                    obj.UnkBytes1 = new byte[31 - obj.Name.ToString().Length];

                    foreach(Instance CurrentInstance in obj.Instances)
                    {
                        if (CurrentInstance.Position.X < Min.X)
                        {
                            Min.X = CurrentInstance.Position.X;
                        }

                        if (CurrentInstance.Position.X > Max.X)
                        {
                            Max.X = CurrentInstance.Position.X;
                        }

                        if (CurrentInstance.Position.Y < Min.Y)
                        {
                            Min.Y = CurrentInstance.Position.Y;
                        }

                        if (CurrentInstance.Position.Y > Max.Y)
                        {
                            Max.Y = CurrentInstance.Position.Y;
                        }

                        if (CurrentInstance.Position.Z < Min.Z)
                        {
                            Min.Z = CurrentInstance.Position.Z;
                        }

                        if (CurrentInstance.Position.Z > Max.Z)
                        {
                            Max.Z = CurrentInstance.Position.Z;
                        }

                        numInstance++;
                    }
                }
            }

            bounds = new BoundingBox(Min, Max);
            #endregion calculate bounding box

            #region rebuild grid bounds
            for (int i = 0; i < Grids.Length; i++)
            {
                var grid = Grids[i];
                grid.CellSize = new Vector2(bounds.GetWidth() / grid.Width, bounds.GetHeight() / grid.Height);
                grid.Data = new ushort[grid.Width * grid.Height];
                grid.Origin = bounds.Min;
            }
            #endregion rebuild grid bounds

            #region encode instance data / build grid
            for (int i = 0; i < ObjectGroups.Length; i++)
            {
                ObjectGroup objectGroup = ObjectGroups[i];

                for (int x = 0; x < objectGroup.Objects.Length; x++)
                {
                    Object obj = objectGroup.Objects[x];

                    for (int y = 0; y < obj.Instances.Length; y++)
                    {
                        Instance instance = obj.Instances[y];
                        var other = new Instance(instance);
                        instance.D4 = 0;
                        instance.D5 = 0;
                        instance.W0 = 0;
                        instance.W1 = 0;
                        instance.W2 = 0;
                        instance.ID = numInstance;
                        CompressPosition(instance, bounds.Min, bounds.Max);
                        CompressScale(instance);
                        CompressRotation(instance);

                        for (int a = 0; a < Grids.Length; a++)
                        {
                            var grid = Grids[a];

                            if (obj.GridMax == grid.Key)
                            {
                                var offsetX = instance.Position.X - grid.Origin.X;
                                var offsetY = instance.Position.Y - grid.Origin.Y;

                                var gridX = (ushort)Math.Abs(Math.Floor(offsetX / grid.CellSize.X));
                                var gridY = (ushort)Math.Abs(Math.Floor(offsetY / grid.CellSize.Y));
                                gridX = (ushort)Math.Min(gridX, grid.Width - 1);
                                gridY = (ushort)Math.Min(gridY, grid.Height - 1);
                                grid.Data[gridX + (gridY * grid.Width)]++;
                            }
                        }

                        numInstance++;
                    }
                }
            }
            #endregion encode instance data / build grid
        }

        public void RebuildGridData()
        {
            for (int i = 0; i < Grids.Length; i++)
            {
                var grid = Grids[i];
                grid.CellSize = new Vector2(bounds.GetWidth() / grid.Width, bounds.GetHeight() / grid.Height);
                grid.Data = new ushort[grid.Width * grid.Height];
                grid.Origin = bounds.Min;
            }

            for (int i = 0; i < ObjectGroups.Length; i++)
            {
                ObjectGroup objectGroup = ObjectGroups[i];

                for (int x = 0; x < objectGroup.Objects.Length; x++)
                {
                    Object obj = objectGroup.Objects[x];

                    for (int y = 0; y < obj.Instances.Length; y++)
                    {
                        Instance instance = obj.Instances[y];

                        for (int a = 0; a < Grids.Length; a++)
                        {
                            var grid = Grids[a];

                            if (obj.GridMax == grid.Key)
                            {
                                var offsetX = instance.Position.X - grid.Origin.X;
                                var offsetY = instance.Position.Y - grid.Origin.Y;

                                var gridX = (ushort)Math.Abs(Math.Floor(offsetX / grid.CellSize.X));
                                var gridY = (ushort)Math.Abs(Math.Floor(offsetY / grid.CellSize.Y));
                                gridX = (ushort)Math.Min(gridX, grid.Width - 1);
                                gridY = (ushort)Math.Min(gridY, grid.Height - 1);
                                grid.Data[gridX + (gridY * grid.Width)]++;
                            }
                        }
                    }
                }
            }
        }


        public void ReadFromFile(BinaryReader reader)
        {
            version = reader.ReadInt32();
            unk1 = reader.ReadInt32();
            unk2 = reader.ReadInt16();
            bounds = BoundingBoxExtenders.ReadFromFile(reader);
            int NumGroups = reader.ReadInt32();
            
            // Load Grids
            Grids = new Grid[NumGroups];
            for (int i = 0; i < NumGroups; i++)
            {
                Grid grid = new Grid();
                grid.Key = reader.ReadInt16();
                grid.Origin = Vector3Utils.ReadFromFile(reader);
                grid.CellSize = Vector2Extenders.ReadFromFile(reader);
                grid.Width = reader.ReadInt32();
                grid.Height = reader.ReadInt32();
                grid.Data = new ushort[grid.Width * grid.Height];

                for (int y = 0; y < grid.Data.Length; y++)
                {
                    grid.Data[y] = reader.ReadUInt16();
                }

                Grids[i] = grid;
            }

            // Load MetaInfo, pushed into ObjectGroup when loading
            int NumMetaInfos = (reader.ReadInt32());
            ObjectGroupMetaInfo[] MetaInfo = new ObjectGroupMetaInfo[NumMetaInfos];
            for (int i = 0; i < MetaInfo.Length; i++)
            {
                MetaInfo[i].ActorTypeID = (ActorTypes)reader.ReadInt16();
                MetaInfo[i].Unk0 = reader.ReadInt16();
            }

            // Now load the groups
            int NumObjectGroups = reader.ReadInt32();
            ObjectGroups = new ObjectGroup[NumObjectGroups];
            for (int i = 0; i < NumObjectGroups; i++)
            {
                ObjectGroup objectGroup = new ObjectGroup();
                objectGroup.ActorType = (ActorTypes)reader.ReadInt16();
                objectGroup.Unk01 = MetaInfo[i].Unk0;

                // Should be identical
                ActorTypes MetaInfoActorType = MetaInfo[i].ActorTypeID;
                ToolkitAssert.Ensure(objectGroup.ActorType == MetaInfoActorType, "The ActorType for the ObjectGroup must be identical");

                // Load the group objects.
                int NumObjects = reader.ReadInt32();
                objectGroup.Objects = new Object[NumObjects];
                for (int x = 0; x < NumObjects; x++)
                {
                    Object obj = new Object();
                    ushort NumInstances2 = reader.ReadUInt16();
                    obj.Unk02 = reader.ReadInt16();

                    obj.Name = new HashName();
                    ulong hashvalue = reader.ReadUInt64();
                    obj.Name.Hash = hashvalue;
                    obj.Name.String = StringHelpers.ReadString(reader);
                    ToolkitAssert.Ensure(obj.Name.Hash == hashvalue, "Object Hash and Name should be identical");

                    obj.UnkBytes1 = reader.ReadBytes(31 - obj.Name.ToString().Length);
                    obj.GridMax = reader.ReadSingle();
                    obj.GridMin = reader.ReadSingle();
                    uint NumInstances = reader.ReadUInt32();
                    obj.Instances = new Instance[NumInstances];

                    for (int y = 0; y < obj.Instances.Length; y++)
                    {
                        byte[] packed = reader.ReadBytes(14);
                        Instance instance = new Instance();
                        instance.W0 = BitConverter.ToUInt16(packed, 0);
                        instance.W1 = BitConverter.ToUInt16(packed, 2);
                        instance.W2 = BitConverter.ToUInt16(packed, 4);
                        instance.D5 = BitConverter.ToInt32(packed, 6);
                        instance.ID = BitConverter.ToUInt16(packed, 10);
                        instance.D4 = BitConverter.ToUInt16(packed, 12);
                        DecompressScale(instance);
                        DecompressRotation(instance);                    
                        instance.Position = DecompressPosition(packed, instance, bounds.Min, bounds.Max);

                        obj.Instances[y] = instance;
                    }

                    objectGroup.Objects[x] = obj;
                }

                ObjectGroups[i] = objectGroup;
            }
        }

        public void WriteToFile(FileInfo info)
        {
            CompileData();

            using (BinaryWriter writer = new BinaryWriter(File.Open(info.FullName, FileMode.Create)))
            {
                InternalWriteToFile(writer);
            }
        }
        private void InternalWriteToFile(BinaryWriter writer)
        {
            writer.Write(version);
            writer.Write(unk1);
            writer.Write(unk2);
            BoundingBoxExtenders.WriteToFile(bounds, writer);
            writer.Write(Grids.Length);

            // Save grids
            for (int i = 0; i < Grids.Length; i++)
            {
                Grid grid = Grids[i];
                writer.Write(grid.Key);
                Vector3Utils.WriteToFile(grid.Origin, writer);
                Vector2Extenders.WriteToFile(grid.CellSize, writer);
                writer.Write(grid.Width);
                writer.Write(grid.Height);

                for (int y = 0; y != grid.Data.Length; y++)
                {
                    writer.Write(grid.Data[y]);
                }
            }

            // Save ObjectGroup MetaInfo
            writer.Write(ObjectGroups.Length);
            for(int i = 0; i < ObjectGroups.Length; i++)
            {
                writer.Write((short)ObjectGroups[i].ActorType);
                writer.Write(ObjectGroups[i].Unk01);
            }

            // Save ObjectGroups
            writer.Write(ObjectGroups.Length);
            for (int i = 0; i < ObjectGroups.Length; i++)
            {
                ObjectGroup objectGroup = ObjectGroups[i];
                writer.Write((short)objectGroup.ActorType);
                writer.Write(objectGroup.Objects.Length);

                for (int x = 0; x < objectGroup.Objects.Length; x++)
                {
                    Object obj = objectGroup.Objects[x];
                    writer.Write((ushort)obj.Instances.Length);
                    writer.Write(obj.Unk02);
                    writer.Write(obj.Name.Hash);
                    StringHelpers.WriteString(writer, obj.Name.String);
                    writer.Write(obj.UnkBytes1);
                    writer.Write(obj.GridMax);
                    writer.Write(obj.GridMin);
                    writer.Write((uint)obj.Instances.Length);

                    for (int y = 0; y < obj.Instances.Length; y++)
                    {
                        Instance instance = obj.Instances[y];
                        writer.Write(instance.W0);
                        writer.Write(instance.W1);
                        writer.Write(instance.W2);
                        writer.Write(instance.D5);
                        writer.Write(instance.ID);
                        writer.Write(instance.D4);
                    }
                    objectGroup.Objects[x] = obj;
                }

                ObjectGroups[i] = objectGroup;
            }
        }

        public void ConvertToXML(string Filename)
        {
            XElement Root = ReflectionHelpers.ConvertPropertyToXML(this);
            Root.Save(Filename);
        }

        public void ConvertFromXML(string Filename)
        {
            XElement LoadedDoc = XElement.Load(Filename);
            TranslokatorLoader FileContents = ReflectionHelpers.ConvertToPropertyFromXML<TranslokatorLoader>(LoadedDoc);

            // Copy data taken from loaded XML
            Grids = FileContents.Grids;
            ObjectGroups = FileContents.ObjectGroups;
            Version = FileContents.Version;
            Unk1 = FileContents.Unk1;
            Unk2 = FileContents.Unk2;
            Bounds = FileContents.Bounds;
        }
    }
}

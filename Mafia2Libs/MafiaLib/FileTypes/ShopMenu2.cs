using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.ComponentModel;
using Utils.Extensions;
using System.IO;
using SharpDX;
using Utils.SharpDXExtensions;

namespace ResourceTypes.City
{
    public class ShopMenu2
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class LocalisedString
        {
            uint id;
            string text;

            public uint ID {
                get { return id; }
                set { id = value; }
            }
            public string Text {
                get { return text; }
                set { text = value; }
            }

            public LocalisedString(uint id) 
            { 
                this.id = id; 
            }

            public override string ToString()
            {
                return string.Format("{0} {1}", ID, Text);
            }
        }
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class Shop
        {
            string name;
            LocalisedString unk0;
            int id;

            public string Name {
                get { return name; }
                set { name = value; }
            }
            public LocalisedString Unk0 {
                get { return unk0; }
                set { unk0 = value; }
            }
            public int ID {
                get { return id; }
                set { id = value; }
            }

            public Shop()
            {
                unk0 = new LocalisedString(0);
                Name = "";
            }

            public override string ToString()
            {
                return string.Format("{0} {1} {2}", ID, Name, Unk0);
            }
        }

        public class ShopMenu
        {
            int id;
            int unk0;
            string unk1;
            string path;
            LocalisedString unkDB0;
            int unkZero0;
            int unk2;
            int unk3;
            int unkZero01;
            int unkZero02;

            List<ItemConfig> items = new List<ItemConfig>();

            public int ID {
                get { return id; }
                set { id = value; }
            }
            public int Unk0 {
                get { return unk0; }
                set { unk0 = value; }
            }
            public string Unk1 {
                get { return unk1; }
                set { unk1 = value; }
            }
            public string Path {
                get { return path; }
                set { path = value; }
            }
            public LocalisedString UnkDB0 {
                get { return unkDB0; }
                set { unkDB0 = value; }
            }
            public int UnkZero0 {
                get { return unkZero0; }
                set { unkZero0 = value; }
            }
            public int Unk2 {
                get { return unk2; }
                set { unk2 = value; }
            }
            public int Unk3 {
                get { return unk3; }
                set { unk3 = value; }
            }
            public int UnkZero01 {
                get { return unkZero01; }
                set { unkZero01 = value; }
            }
            public int UnkZero02 {
                get { return unkZero02; }
                set { unkZero02 = value; }
            }
            public List<ItemConfig> Items {
                get { return items; }
                set { items = value; }
            }

            public ShopMenu()
            {
                items = new List<ItemConfig>();
                unkDB0 = new LocalisedString(0);
                unk1 = "";
                path = "";
            }

            public override string ToString()
            {
                return string.Format("{3} {0} {1} {2}", Unk1, Path, Items.Count, ID);
            }
        }

        public class Item
        {
            string name;
            ushort key;
            byte unk0;

            public string Name {
                get { return name; }
                set { name = value; }
            }
            [Browsable(false)]
            public ushort Key {
                get { return key; }
                set { key = value; }
            }
            public byte Unk0 {
                get { return unk0; }
                set { unk0 = value; }
            }

            public Item()
            {
                name = "";
            }
        }

        public class ItemConfig
        {
            [TypeConverter(typeof(ExpandableObjectConverter))]
            public class ItemCamera
            {
                public Vector3 Position { get; set; }
                public Quaternion Rotation { get; set; }
                public float Unk01 { get; set; }

                public void ReadFromFile(MemoryStream stream, bool isBigEndian)
                {
                    Position = Vector3Extenders.ReadFromFile(stream, isBigEndian);
                    Rotation = QuaternionExtensions.ReadFromFile(stream, isBigEndian);
                    Unk01 = stream.ReadSingle(isBigEndian);
                }

                public void WriteToFile(MemoryStream stream, bool isBigEndian)
                {
                    Position.WriteToFile(stream, isBigEndian);
                    Rotation.WriteToFile(stream, isBigEndian);
                    stream.Write(Unk01, isBigEndian);
                }
            }

            LocalisedString unkDB0;
            LocalisedString unkDB1;
            LocalisedString unkDB2;
            LocalisedString unkDB3;
            LocalisedString unkDB4;
            LocalisedString unkDB5;
            int itemUnk0;
            int itemPrice;
            int itemUnk1;
            int itemChangeTimeIn;
            int itemChangeTimeOut;
            ItemCamera cameraConfig;
            byte unkByte;
            float[] unkMatrix;

            int count1;
            Item[] section1;
            int count2;
            Item[] section2;

            public LocalisedString UnkDB0 {
                get { return unkDB0; }
                set { unkDB0 = value; }
            }
            public LocalisedString UnkDB1 {
                get { return unkDB1; }
                set { unkDB1 = value; }
            }
            public LocalisedString UnkDB2 {
                get { return unkDB2; }
                set { unkDB2 = value; }
            }
            public LocalisedString UnkDB3 {
                get { return unkDB3; }
                set { unkDB3 = value; }
            }
            public LocalisedString UnkDB4 {
                get { return unkDB4; }
                set { unkDB4 = value; }
            }
            public LocalisedString UnkDB5 {
                get { return unkDB5; }
                set { unkDB5 = value; }
            }
            [Category("Item Information")]
            public int ItemUnk0 {
                get { return itemUnk0; }
                set { itemUnk0 = value; }
            }
            [Category("Item Information")]
            public int ItemPrice {
                get { return itemPrice; }
                set { itemPrice = value; }
            }
            [Category("Item Information")]
            public int ItemUnk1 {
                get { return itemUnk1; }
                set { itemUnk1 = value; }
            }
            [Category("Item Information")]
            public int ItemChangeTimeIn {
                get { return itemChangeTimeIn; }
                set { itemChangeTimeIn = value; }
            }
            [Category("Item Information")]
            public int ItemChangeTimeOut {
                get { return itemChangeTimeOut; }
                set { itemChangeTimeOut = value; }
            }
            public ItemCamera CameraConfig {
                get { return cameraConfig; }
                set { cameraConfig = value; }
            }
            public byte UnkByte {
                get { return unkByte; }
                set { unkByte = value; }
            }
            public float[] UnkMatrixFloats {
                get { return unkMatrix; }
                set { unkMatrix = value; }
            }
            public Item[] Section1 {
                get { return section1; }
                set { section1 = value; }
            }
            public Item[] Section2 {
                get { return section2; }
                set { section2 = value; }
            }

            public ItemConfig()
            {
                CameraConfig = new ItemCamera();
                unkMatrix = new float[10];
                section1 = new Item[0];
                section2 = new Item[0];
                unkDB0 = new LocalisedString(0);
                unkDB1 = new LocalisedString(0);
                unkDB2 = new LocalisedString(0); 
                unkDB3 = new LocalisedString(0);
                unkDB4 = new LocalisedString(0);
                unkDB5 = new LocalisedString(0);
            }
        }

        const int magic = 1936223538;
        const int version = 4;

        string stringPool = "";
        Dictionary<int, string> dicPool = new Dictionary<int, string>();
        public List<Shop> shops = new List<Shop>();
        int[] unkOffsets;
        int[] unkIDs;
        public List<ShopMenu> shopItems = new List<ShopMenu>();
        Dictionary<uint, string> textDB = new Dictionary<uint, string>();

        public ShopMenu2()
        {

        }

        private void ReadTextDB()
        {
            if (File.Exists("TextDatabase.dat"))
            {
                string[] lines = File.ReadAllLines("TextDatabase.dat");

                foreach (var line in lines)
                {
                    if (!string.IsNullOrEmpty(line))
                    {
                        string[] split = line.Split(':');
                        split[0] = Regex.Replace(split[0], @"_", "");
                        textDB.Add(uint.Parse(split[0]), split[1]);
                    }
                }
                lines = null;
            }
        }

        private void GetFromDB(LocalisedString loc)
        {
            string result;
            textDB.TryGetValue(loc.ID, out result);

            if (textDB.Count == 0)
                loc.Text = "TextDatabase is not loaded!";
            else
                loc.Text = result;
        }

        public void ReadFromFile(string file)
        {
            using (var stream = new MemoryStream(File.ReadAllBytes(file)))
            {
                ReadTextDB();
                var isBigEndian = false;
                if (stream.ReadInt32(isBigEndian) != magic)
                    return;

                if (stream.ReadInt32(isBigEndian) != version)
                    return;

                var size = stream.ReadInt32(isBigEndian);

                while (true)
                {
                    int offset = (int)stream.Position - 12; // header is 12 bytes.

                    if (offset == size)
                        break;

                    string name = stream.ReadString(); //read string
                    dicPool.Add(offset, name); //add offset as unique key and string
                }

                var numShops = stream.ReadInt32(isBigEndian);

                for (int i = 0; i < numShops; i++)
                {
                    Shop shop = new Shop();
                    shop.Name = stream.ReadString();
                    shop.Unk0 = new LocalisedString(stream.ReadUInt32(isBigEndian));
                    GetFromDB(shop.Unk0);
                    shop.ID = stream.ReadInt32(isBigEndian);
                    shops.Add(shop);
                }

                var num = stream.ReadInt32(isBigEndian);
                long startOffset = stream.Position;
                unkIDs = new int[num];
                unkOffsets = new int[num];

                for (int x = 0; x < num; x++)
                {
                    unkIDs[x] = stream.ReadInt32(isBigEndian);
                    unkOffsets[x] = stream.ReadInt32(isBigEndian);
                }

                for (int i = 0; i < num; i++)
                {
                    var metaInfo = new ShopMenu();
                    metaInfo.ID = stream.ReadInt32(isBigEndian);
                    metaInfo.Unk0 = stream.ReadInt32(isBigEndian);
                    metaInfo.Unk1 = stream.ReadString();
                    metaInfo.Path = stream.ReadString();
                    metaInfo.UnkDB0 = new LocalisedString(stream.ReadUInt32(isBigEndian));
                    GetFromDB(metaInfo.UnkDB0);
                    metaInfo.UnkZero0 = stream.ReadInt32(isBigEndian);
                    metaInfo.Unk2 = stream.ReadInt32(isBigEndian);
                    metaInfo.Unk3 = stream.ReadInt32(isBigEndian);
                    metaInfo.UnkZero01 = stream.ReadInt32(isBigEndian);
                    metaInfo.UnkZero02 = stream.ReadInt32(isBigEndian);

                    var itemNum = stream.ReadInt32(isBigEndian);

                    for (int x = 0; x < itemNum; x++)
                    {
                        var item = new ItemConfig();
                        item.UnkDB0 = new LocalisedString(stream.ReadUInt32(isBigEndian));
                        GetFromDB(item.UnkDB0);
                        item.UnkDB1 = new LocalisedString(stream.ReadUInt32(isBigEndian));
                        GetFromDB(item.UnkDB1);
                        item.UnkDB2 = new LocalisedString(stream.ReadUInt32(isBigEndian));
                        GetFromDB(item.UnkDB2);
                        item.UnkDB3 = new LocalisedString(stream.ReadUInt32(isBigEndian));
                        GetFromDB(item.UnkDB3);
                        item.UnkDB4 = new LocalisedString(stream.ReadUInt32(isBigEndian));
                        GetFromDB(item.UnkDB4);
                        item.UnkDB5 = new LocalisedString(stream.ReadUInt32(isBigEndian));
                        GetFromDB(item.UnkDB5);

                        item.ItemUnk0 = stream.ReadInt32(isBigEndian);
                        item.ItemPrice = stream.ReadInt32(isBigEndian);
                        item.ItemUnk1 = stream.ReadInt32(isBigEndian);
                        item.ItemChangeTimeIn = stream.ReadInt32(isBigEndian);
                        item.ItemChangeTimeOut = stream.ReadInt32(isBigEndian);

                        item.CameraConfig = new ItemConfig.ItemCamera();
                        item.CameraConfig.ReadFromFile(stream, isBigEndian);

                        item.UnkByte = stream.ReadByte8();
                        item.UnkMatrixFloats = new float[10];
                        for (int z = 0; z < 10; z++)
                            item.UnkMatrixFloats[z] = stream.ReadSingle(isBigEndian);

                        var section1Size = stream.ReadInt32(isBigEndian);
                        item.Section1 = new Item[section1Size];
                        for (int z = 0; z < section1Size; z++)
                        {
                            var it = new Item();
                            it.Key = stream.ReadUInt16(isBigEndian);
                            var text = "";
                            dicPool.TryGetValue(it.Key, out text);
                            it.Name = text;
                            it.Unk0 = stream.ReadByte8();
                            item.Section1[z] = it;
                        }

                        int section2Size = stream.ReadInt32(isBigEndian);
                        item.Section2 = new Item[section2Size];
                        for (int z = 0; z < section2Size; z++)
                        {
                            var it = new Item();
                            it.Key = stream.ReadUInt16(isBigEndian);
                            var text = "";
                            dicPool.TryGetValue(it.Key, out text);
                            it.Name = text;
                            it.Unk0 = stream.ReadByte8();
                            item.Section2[z] = it;
                        }
                            metaInfo.Items.Add(item);
                    }
                    shopItems.Add(metaInfo);
                }
            }
        }

        public void WriteToFile(string file)
        {
            BuildUpdateKeyStringBuffer();
            using (var stream = new MemoryStream())
            {
                var isBigEndian = false;
                stream.Write(magic, isBigEndian);
                stream.Write(version, isBigEndian);
                stream.Write(stringPool.Length, isBigEndian);
                stream.Write(stringPool.ToCharArray());
                stream.Write(shops.Count, isBigEndian);

                for (int i = 0; i < shops.Count; i++)
                {
                    Shop shop = shops[i];
                    stream.WriteString(shop.Name);
                    stream.Write(shop.Unk0.ID, isBigEndian);
                    stream.Write(shop.ID, isBigEndian);
                }

                stream.Write(shopItems.Count, isBigEndian);
                long startOffset = stream.Position;


                for (int x = 0; x < shopItems.Count; x++)
                {
                    stream.Write(-1, isBigEndian);
                    stream.Write(-1, isBigEndian);
                }

                long previousFinish = 0;

                for (int i = 0; i < shopItems.Count; i++)
                {
                    var metaInfo = shopItems[i];

                    previousFinish = stream.Position;
                    long currentPos = stream.Position;
                    long offset = (startOffset + (i * 8));
                    stream.Position = offset;
                    stream.Write(metaInfo.ID, isBigEndian);
                    stream.Write((int)(currentPos - startOffset), isBigEndian);
                    stream.Position = currentPos;
                    stream.Write(metaInfo.ID, isBigEndian);
                    stream.Write(metaInfo.Unk0, isBigEndian);
                    stream.WriteString(metaInfo.Unk1);
                    stream.WriteString(metaInfo.Path);
                    stream.Write(metaInfo.UnkDB0.ID, isBigEndian);
                    stream.Write(metaInfo.UnkZero0, isBigEndian);
                    stream.Write(metaInfo.Unk2, isBigEndian);
                    stream.Write(metaInfo.Unk3, isBigEndian);
                    stream.Write(metaInfo.UnkZero01, isBigEndian);
                    stream.Write(metaInfo.UnkZero02, isBigEndian);
                    stream.Write(metaInfo.Items.Count, isBigEndian);

                    for (int x = 0; x < metaInfo.Items.Count; x++)
                    {
                        var item = metaInfo.Items[x];
                        stream.Write(item.UnkDB0.ID, isBigEndian);
                        stream.Write(item.UnkDB1.ID, isBigEndian);
                        stream.Write(item.UnkDB2.ID, isBigEndian);
                        stream.Write(item.UnkDB3.ID, isBigEndian);
                        stream.Write(item.UnkDB4.ID, isBigEndian);
                        stream.Write(item.UnkDB5.ID, isBigEndian);

                        stream.Write(item.ItemUnk0, isBigEndian);
                        stream.Write(item.ItemPrice, isBigEndian);
                        stream.Write(item.ItemUnk1, isBigEndian);
                        stream.Write(item.ItemChangeTimeIn, isBigEndian);
                        stream.Write(item.ItemChangeTimeOut, isBigEndian);

                        item.CameraConfig.WriteToFile(stream, isBigEndian);

                        stream.WriteByte(item.UnkByte);
                        for (int z = 0; z < 10; z++)
                        {
                            stream.Write(item.UnkMatrixFloats[z], isBigEndian);
                        }

                        stream.Write(item.Section1.Length, isBigEndian);
                        for (int z = 0; z < item.Section1.Length; z++)
                        {
                            var it = item.Section1[z];
                            stream.Write(it.Key, isBigEndian);
                            stream.WriteByte(it.Unk0);
                        }

                        stream.Write(item.Section2.Length, isBigEndian);
                        for (int z = 0; z < item.Section2.Length; z++)
                        {
                            var it = item.Section2[z];
                            stream.Write(it.Key, isBigEndian);
                            stream.WriteByte(it.Unk0);
                        }
                    }
                }
                File.WriteAllBytes(file, stream.ToArray());
            }
        }

        private void BuildUpdateKeyStringBuffer()
        {
            //fix this
            List<string> addedNames = new List<string>();
            List<ushort> addedPos = new List<ushort>();
            stringPool = "";

            for (int i = 0; i != shopItems.Count; i++)
            {
                var shop = shopItems[i];
                for (int y = 0; y != shop.Items.Count; y++)
                {
                    var itemd = shop.Items[y];
                    for (int x = 0; x != itemd.Section1.Length; x++)
                    {
                        int index = addedNames.IndexOf(itemd.Section1[x].Name);
                        if (index == -1)
                        {
                            addedNames.Add(itemd.Section1[x].Name);
                            addedPos.Add((ushort)stringPool.Length);
                            itemd.Section1[x].Key = (ushort)stringPool.Length;
                            stringPool += itemd.Section1[x].Name + "\0";
                        }
                        else
                        {
                            itemd.Section1[x].Key = addedPos[index];
                        }
                    }
                    for (int x = 0; x != itemd.Section2.Length; x++)
                    {
                        int index = addedNames.IndexOf(itemd.Section2[x].Name);
                        if (index == -1)
                        {
                            addedNames.Add(itemd.Section2[x].Name);
                            addedPos.Add((ushort)stringPool.Length);
                            itemd.Section2[x].Key = (ushort)stringPool.Length;
                            stringPool += itemd.Section2[x].Name + "\0";
                        }
                        else
                        {
                            itemd.Section2[x].Key = addedPos[index];
                        }
                    }
                }
            }
        }
    }
}
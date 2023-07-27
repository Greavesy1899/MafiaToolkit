using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Numerics;
using System.Text.RegularExpressions;
using Utils.Extensions;
using Utils.VorticeUtils;

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

            public LocalisedString(LocalisedString OtherString)
            {
                id = OtherString.id;
                text = OtherString.text;
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
            public int ID { get; set; }
            public int Unk0 { get; set; }
            public string Unk1 { get; set; }
            public string Path { get; set; }
            public LocalisedString UnkDB0 { get; set; }
            public int UnkZero0 { get; set; }
            public int Unk2 { get; set; }
            public int Unk3 { get; set; }
            public int UnkZero01 { get; set; }
            public int UnkZero02 { get; set; }
            public List<ItemConfig> Items { get; set; }

            public ShopMenu()
            {
                Items = new List<ItemConfig>();
                UnkDB0 = new LocalisedString(0);
                Unk1 = string.Empty;
                Path = string.Empty;
            }

            public ShopMenu(ShopMenu OtherShopMenu)
            {
                ID = OtherShopMenu.ID;
                Unk0 = OtherShopMenu.Unk0;
                Unk1 = OtherShopMenu.Unk1;
                Path = OtherShopMenu.Path;
                UnkDB0 = new LocalisedString(OtherShopMenu.UnkDB0);
                UnkZero0 = OtherShopMenu.UnkZero0;
                Unk2 = OtherShopMenu.Unk2;
                Unk3 = OtherShopMenu.Unk3;
                UnkZero01 = OtherShopMenu.UnkZero01;
                UnkZero02 = OtherShopMenu.UnkZero02;

                Items = new List<ItemConfig>();
                foreach (ItemConfig OtherItem in OtherShopMenu.Items)
                {
                    ItemConfig CopiedItem = new ItemConfig(OtherItem);
                    Items.Add(CopiedItem);
                }
            }

            public override string ToString()
            {
                return string.Format("{3} {0} {1} {2}", Unk1, Path, Items.Count, ID);
            }
        }

        public class Item
        {
            public string Name { get; set; }
            [Browsable(false)]
            public ushort Key { get; set; }
            public byte Unk0 { get; set; }

            public Item()
            {
                Name = string.Empty;
                Key = 0;
                Unk0 = 0;
            }

            public Item(Item OtherItem)
            {
                Name = OtherItem.Name;
                Key = OtherItem.Key;
                Unk0 = OtherItem.Unk0;
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

                public ItemCamera()
                {
                    Position = Vector3.Zero;
                    Rotation = Quaternion.Identity;
                    Unk01 = 0.0f;
                }

                public ItemCamera(ItemCamera Other)
                {
                    Position = Other.Position;
                    Rotation = Other.Rotation;
                    Unk01 = Other.Unk01;
                }

                public void ReadFromFile(MemoryStream stream, bool isBigEndian)
                {
                    Position = Vector3Utils.ReadFromFile(stream, isBigEndian);
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

            public LocalisedString Name { get; set; }
            public LocalisedString ShortDescription { get; set; }
            public LocalisedString UnkDB2 { get; set; }
            public LocalisedString UnkDB3 { get; set; }
            public LocalisedString PromptText { get; set; }
            public LocalisedString UnkDB5 { get; set; }
            [Category("Item Information")]
            public int ItemUnk0 { get; set; }
            [Category("Item Information")]
            public int ItemPrice { get; set; }
            [Category("Item Information")]
            public int ItemUnk1 { get; set; }
            [Category("Item Information")]
            public int ItemChangeTimeIn { get; set; }
            [Category("Item Information")]
            public int ItemChangeTimeOut { get; set; }
            public ItemCamera CameraConfig { get; set; }
            public byte UnkByte { get; set; }
            public float[] UnkMatrixFloats { get; set; }
            public Item[] Section1 { get; set; }
            public Item[] Section2 { get; set; }

            public ItemConfig()
            {
                CameraConfig = new ItemCamera();
                UnkMatrixFloats = new float[10];
                Section1 = new Item[0];
                Section2 = new Item[0];
                Name = new LocalisedString(0);
                ShortDescription = new LocalisedString(0);
                UnkDB2 = new LocalisedString(0); 
                UnkDB3 = new LocalisedString(0);
                PromptText = new LocalisedString(0);
                UnkDB5 = new LocalisedString(0);
            }

            public ItemConfig(ItemConfig OtherItemConfig)
            {
                CameraConfig = new ItemCamera(OtherItemConfig.CameraConfig);

                // Copy Matrix using default C# copy
                UnkMatrixFloats = new float[10];
                Array.Copy(OtherItemConfig.UnkMatrixFloats, UnkMatrixFloats, OtherItemConfig.UnkMatrixFloats.Length);

                // Copy over item data
                ItemUnk0 = OtherItemConfig.ItemUnk0;
                ItemPrice = OtherItemConfig.ItemPrice;
                ItemUnk1 = OtherItemConfig.ItemUnk1;
                ItemChangeTimeIn = OtherItemConfig.ItemChangeTimeIn;
                ItemChangeTimeOut = OtherItemConfig.ItemChangeTimeOut;
                UnkByte = OtherItemConfig.UnkByte;

                // Copy sections
                Section1 = new Item[OtherItemConfig.Section1.Length];
                Section2 = new Item[OtherItemConfig.Section2.Length];
                
                for(int i = 0; i < Section1.Length; i++)
                {
                    Section1[i] = new Item(OtherItemConfig.Section1[i]);
                }

                for (int i = 0; i < Section2.Length; i++)
                {
                    Section2[i] = new Item(OtherItemConfig.Section2[i]);
                }

                // Copy all localised Strings
                Name = new LocalisedString(OtherItemConfig.Name);
                ShortDescription = new LocalisedString(OtherItemConfig.ShortDescription);
                UnkDB2 = new LocalisedString(OtherItemConfig.UnkDB2);
                UnkDB3 = new LocalisedString(OtherItemConfig.UnkDB3);
                PromptText = new LocalisedString(OtherItemConfig.PromptText);
                UnkDB5 = new LocalisedString(OtherItemConfig.UnkDB5);
            }
        }

        public List<Shop> Shops { get; set; }
        public List<ShopMenu> ShopItems { get; set; }

        private string stringPool = "";
        private Dictionary<int, string> dicPool;
        private Dictionary<uint, string> textDB;

        private const int magic = 1936223538;
        private const int version = 4;

        public ShopMenu2()
        {
            stringPool = string.Empty;
            dicPool = new Dictionary<int, string>();
            Shops = new List<Shop>();
            ShopItems = new List<ShopMenu>();
            textDB = new Dictionary<uint, string>();
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
            }
        }

        private void GetFromDB(LocalisedString loc)
        {
            string result = string.Empty;
            if (textDB.TryGetValue(loc.ID, out result))
            {
                loc.Text = result;
                return;
            }

            loc.Text = "TextDatabase is not loaded!";
        }

        public void ReadFromFile(string file)
        {
            using (var stream = new MemoryStream(File.ReadAllBytes(file)))
            {
                ReadTextDB();
                var isBigEndian = false;
                if (stream.ReadInt32(isBigEndian) != magic)
                {
                    return;
                }

                if (stream.ReadInt32(isBigEndian) != version)
                {
                    return;
                }

                var size = stream.ReadInt32(isBigEndian);

                while (true)
                {
                    int offset = (int)stream.Position - 12; // header is 12 bytes.

                    if (offset == size)
                    {
                        break;
                    }

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
                    Shops.Add(shop);
                }

                var num = stream.ReadInt32(isBigEndian);
                int[] unkIDs = new int[num];
                int[] unkOffsets = new int[num];

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
                        item.Name = new LocalisedString(stream.ReadUInt32(isBigEndian));
                        GetFromDB(item.Name);
                        item.ShortDescription = new LocalisedString(stream.ReadUInt32(isBigEndian));
                        GetFromDB(item.ShortDescription);
                        item.UnkDB2 = new LocalisedString(stream.ReadUInt32(isBigEndian));
                        GetFromDB(item.UnkDB2);
                        item.UnkDB3 = new LocalisedString(stream.ReadUInt32(isBigEndian));
                        GetFromDB(item.UnkDB3);
                        item.PromptText = new LocalisedString(stream.ReadUInt32(isBigEndian));
                        GetFromDB(item.PromptText);
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
                        {
                            item.UnkMatrixFloats[z] = stream.ReadSingle(isBigEndian);
                        }

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
                    ShopItems.Add(metaInfo);
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
                stream.Write(Shops.Count, isBigEndian);

                for (int i = 0; i < Shops.Count; i++)
                {
                    Shop shop = Shops[i];
                    stream.WriteString(shop.Name);
                    stream.Write(shop.Unk0.ID, isBigEndian);
                    stream.Write(shop.ID, isBigEndian);
                }

                stream.Write(ShopItems.Count, isBigEndian);
                long startOffset = stream.Position;


                for (int x = 0; x < ShopItems.Count; x++)
                {
                    stream.Write(-1, isBigEndian);
                    stream.Write(-1, isBigEndian);
                }

                for (int i = 0; i < ShopItems.Count; i++)
                {
                    var metaInfo = ShopItems[i];

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
                        stream.Write(item.Name.ID, isBigEndian);
                        stream.Write(item.ShortDescription.ID, isBigEndian);
                        stream.Write(item.UnkDB2.ID, isBigEndian);
                        stream.Write(item.UnkDB3.ID, isBigEndian);
                        stream.Write(item.PromptText.ID, isBigEndian);
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

            for (int i = 0; i != ShopItems.Count; i++)
            {
                var shop = ShopItems[i];
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
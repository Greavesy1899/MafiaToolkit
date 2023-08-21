using ResourceTypes.EntityDataStorage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Numerics;
using System.Security.Policy;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Utils.Extensions;
using Utils.Helpers.Reflection;
using Utils.VorticeUtils;

namespace ResourceTypes.City
{
    public class ShopMenu2
    {
        public class SHPMQuaternionConverter : TypeConverter
        {
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
            }

            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                return destinationType == typeof(string) || base.CanConvertTo(context, destinationType);
            }

            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                object result = null;
                string stringValue = value as string;

                if (!string.IsNullOrEmpty(stringValue))
                {
                    float[] values = ConverterUtils.ConvertStringToFloats(stringValue, 3);
                    result = new SHPMQuaternion(values);
                }

                return result ?? base.ConvertFrom(context, culture, value);
            }

            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            {
                object result = null;
                SHPMQuaternion quat = (SHPMQuaternion)value;

                if (destinationType == typeof(string))
                {
                    result = quat.ToString();
                }

                return result ?? base.ConvertTo(context, culture, value, destinationType);
            }
        }

        [TypeConverter(typeof(SHPMQuaternionConverter)), PropertyClassAllowReflection]
        public class SHPMQuaternion
        {
            [PropertyForceAsAttribute]
            public float X { get; set; }
            [PropertyForceAsAttribute]
            public float Y { get; set; }
            [PropertyForceAsAttribute]
            public float Z { get; set; }
            [PropertyForceAsAttribute]
            public float W { get; set; }

            public SHPMQuaternion() { }

            public SHPMQuaternion(float[] Values)
            {
                X = Values[0];
                Y = Values[1];
                Z = Values[2];
                W = Values[3];
            }

            public void ReadFromFile(MemoryStream Stream, bool bIsBigEndian)
            {
                X = Stream.ReadSingle(bIsBigEndian);
                Y = Stream.ReadSingle(bIsBigEndian);
                Z = Stream.ReadSingle(bIsBigEndian);
                W = Stream.ReadSingle(bIsBigEndian);
            }

            public void WriteToFile(MemoryStream Stream, bool bIsBigEndian)
            {
                Stream.Write(X, bIsBigEndian);
                Stream.Write(Y, bIsBigEndian);
                Stream.Write(Z, bIsBigEndian);
                Stream.Write(W, bIsBigEndian);
            }

            public override string ToString()
            {
                return string.Format("X:{0} Y:{1} Z:{2} W:{3}", X, Y, Z, W);
            }
        }

        public class SHPMVector3Converter : TypeConverter
        {
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
            }

            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                return destinationType == typeof(string) || base.CanConvertTo(context, destinationType);
            }

            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                object result = null;
                string stringValue = value as string;

                if (!string.IsNullOrEmpty(stringValue))
                {
                    float[] values = ConverterUtils.ConvertStringToFloats(stringValue, 3);
                    result = new SHPMVector3(values);
                }

                return result ?? base.ConvertFrom(context, culture, value);
            }

            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            {
                object result = null;
                SHPMVector3 vector3 = (SHPMVector3)value;

                if (destinationType == typeof(string))
                {
                    result = vector3.ToString();
                }

                return result ?? base.ConvertTo(context, culture, value, destinationType);
            }
        }

        [TypeConverter(typeof(SHPMVector3Converter)), PropertyClassAllowReflection]
        public class SHPMVector3
        {
            [PropertyForceAsAttribute]
            public float X { get; set; }
            [PropertyForceAsAttribute]
            public float Y { get; set; }
            [PropertyForceAsAttribute]
            public float Z { get; set; }

            public SHPMVector3() { }

            public SHPMVector3(float[] Values)
            {
                X = Values[0];
                Y = Values[1];
                Z = Values[2];
            }

            public void ReadFromFile(MemoryStream Stream, bool bIsBigEndian)
            {
                X = Stream.ReadSingle(bIsBigEndian);
                Y = Stream.ReadSingle(bIsBigEndian);
                Z = Stream.ReadSingle(bIsBigEndian);
            }

            public void WriteToFile(MemoryStream Stream, bool bIsBigEndian)
            {
                Stream.Write(X, bIsBigEndian);
                Stream.Write(Y, bIsBigEndian);
                Stream.Write(Z, bIsBigEndian);
            }

            public override string ToString()
            {
                return string.Format("X:{0} Y:{1} Z:{2}", X, Y, Z);
            }
        }

        [TypeConverter(typeof(ExpandableObjectConverter)), PropertyClassAllowReflection]
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

            public LocalisedString()
            {
                this.id = 0;
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
                Name = string.Empty;
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
            [Browsable(false)]
            public ItemConfig[] Items { get; set; }

            public ShopMenu()
            {
                Items = null;
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

                Items = new ItemConfig[OtherShopMenu.Items.Length];
                for (int i = 0; i < OtherShopMenu.Items.Length; i++)
                {
                    ItemConfig CopiedItem = new ItemConfig(OtherShopMenu.Items[i]);
                    Items[i] = CopiedItem;
                }
            }

            public override string ToString()
            {
                return string.Format("{3} {0} {1} {2}", Unk1, Path, Items.Length, ID);
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
            [TypeConverter(typeof(ExpandableObjectConverter)), PropertyClassAllowReflection]
            public class ItemCamera
            {
                public SHPMVector3 Position { get; set; }
                public SHPMQuaternion Rotation { get; set; }
                public float Unk01 { get; set; }

                public ItemCamera()
                {
                    Position = new SHPMVector3();
                    Rotation = new SHPMQuaternion();
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
                    Position.ReadFromFile(stream, isBigEndian);
                    Rotation.ReadFromFile(stream, isBigEndian);
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

        public Shop[] Shops { get; set; }
        public ShopMenu[] ShopItems { get; set; }

        private string stringPool = "";
        private Dictionary<int, string> dicPool;
        private Dictionary<uint, string> textDB;

        private const int magic = 1936223538;
        private const int version = 4;

        public ShopMenu2()
        {
            stringPool = string.Empty;
            dicPool = new Dictionary<int, string>();
            Shops = null;
            ShopItems = null;
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
            ReadTextDB();

            using (MemoryStream stream = new MemoryStream(File.ReadAllBytes(file)))
            {
                // TODO: Support Big Endian formats.
                // We can check magic & version
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
                Shops = new Shop[numShops];

                for (int i = 0; i < numShops; i++)
                {
                    Shop shop = new Shop();
                    shop.Name = stream.ReadString();
                    shop.Unk0 = new LocalisedString(stream.ReadUInt32(isBigEndian));
                    GetFromDB(shop.Unk0);
                    shop.ID = stream.ReadInt32(isBigEndian);
                    Shops[i] = shop;
                }

                var num = stream.ReadInt32(isBigEndian);
                int[] unkIDs = new int[num];
                int[] unkOffsets = new int[num];
                ShopItems = new ShopMenu[num];

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
                    metaInfo.Items = new ItemConfig[itemNum];

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
                        metaInfo.Items[x] = item;
                    }
                    ShopItems[i] = metaInfo;
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
                stream.Write(Shops.Length, isBigEndian);

                for (int i = 0; i < Shops.Length; i++)
                {
                    Shop shop = Shops[i];
                    stream.WriteString(shop.Name);
                    stream.Write(shop.Unk0.ID, isBigEndian);
                    stream.Write(shop.ID, isBigEndian);
                }

                stream.Write(ShopItems.Length, isBigEndian);
                long startOffset = stream.Position;


                for (int x = 0; x < ShopItems.Length; x++)
                {
                    stream.Write(-1, isBigEndian);
                    stream.Write(-1, isBigEndian);
                }

                for (int i = 0; i < ShopItems.Length; i++)
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
                    stream.Write(metaInfo.Items.Length, isBigEndian);

                    for (int x = 0; x < metaInfo.Items.Length; x++)
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

        public void ConvertToXML(string Filename)
        {
            XElement Root = ReflectionHelpers.ConvertPropertyToXML(this);
            Root.Save(Filename);
        }

        public void ConvertFromXML(string Filename)
        {
            XElement LoadedDoc = XElement.Load(Filename);
            ShopMenu2 FileContents = ReflectionHelpers.ConvertToPropertyFromXML<ShopMenu2>(LoadedDoc);

            // Copy data taken from loaded XML
            Shops = FileContents.Shops;
            ShopItems = FileContents.ShopItems;
            stringPool = string.Empty;
            dicPool = new Dictionary<int, string>();
        }

        private void BuildUpdateKeyStringBuffer()
        {
            //fix this
            List<string> addedNames = new List<string>();
            List<ushort> addedPos = new List<ushort>();
            stringPool = "";

            for (int i = 0; i != ShopItems.Length; i++)
            {
                var shop = ShopItems[i];
                for (int y = 0; y != shop.Items.Length; y++)
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
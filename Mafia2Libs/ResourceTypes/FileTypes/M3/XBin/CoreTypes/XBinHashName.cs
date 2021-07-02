using Gibbed.Illusion.FileFormats.Hashing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using Utils.Extensions;
using Utils.Helpers;
using Utils.Helpers.Reflection;

namespace ResourceTypes.M3.XBin
{
    public static class XBinHashStorage
    {
        static Dictionary<ulong, string> HashStorage;

        public static void LoadStorage()
        {
            HashStorage = new Dictionary<ulong, string>();
            HashStorage.Add(14695981039346656037, "");

            string[] LoadedLines = File.ReadAllLines("Resources//GameData//XBin_Hashes.txt");

            foreach(string Line in LoadedLines)
            {
                ulong FNVHash = FNV64.Hash(Line);
                HashStorage.TryAdd(FNVHash, Line);
            }
        }

        public static string GetNameFromHash(ulong Hash, out bool bSuccessful)
        {
            string Value = "";
            bSuccessful = HashStorage.TryGetValue(Hash, out Value);
            return Value != null ? Value : "";
        }
    }

    public class XBinHashNameConverter : ExpandableObjectConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(XBinHashName) || base.CanConvertTo(context, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            object Result = null;
            string StringValue = value as string;

            string[] Splits = StringValue.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            ulong Hash = 0;
            bool bIsOnlyUlong = ulong.TryParse(Splits[0], out Hash);

            XBinHashName HashName = new XBinHashName();

            // If it is indeed only the hash, quickly return out
            if (bIsOnlyUlong)
            {
                HashName.Hash = Hash;
                HashName.ForceToCheckStorage();
                Result = HashName;
                return Result ?? base.ConvertFrom(context, culture, value);
            }

            string RemovedBrackets = Splits[1].Replace("[", "");
            RemovedBrackets = Splits[1].Replace("]", "");

            HashName.Name = Splits[0];
            HashName.Hash = ulong.Parse(RemovedBrackets);

            return Result ?? base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            object result = null;
            XBinHashName HashName = (XBinHashName)value;

            if (destinationType == typeof(string))
            {
                result = HashName.ToString();
            }

            return result ?? base.ConvertTo(context, culture, value, destinationType);
        }
    }

    [TypeConverter(typeof(XBinHashNameConverter)), PropertyClassAllowReflection()]
    public class XBinHashName
    {
        private string _name;

        [PropertyForceAsAttribute]
        public ulong Hash { get; set; }
        [LocalisedDescription("$XBIN_PROP_DESC_NAME_UNSTORED"), PropertyForceAsAttribute]
        public string Name {
            get { return _name; }
            set { SetName(value); }
        }

        public XBinHashName()
        {
            Hash = 0;
            _name = "";
        }

        public static XBinHashName ConstructAndReadFromFile(BinaryReader reader)
        {
            XBinHashName NewObject = new XBinHashName();
            NewObject.ReadFromFile(reader);
            return NewObject;
        }

        public void ReadFromFile(BinaryReader reader)
        {
            Hash = reader.ReadUInt64();

            bool bSuccess = false;
            _name = XBinHashStorage.GetNameFromHash(Hash, out bSuccess);
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(Hash);
        }

        public void SetName(string Value)
        {
            _name = Value;
            Hash = FNV64.Hash(Name);
        }

        public void ForceToCheckStorage()
        {
            bool bSuccess = false;
            string ReturnedName = XBinHashStorage.GetNameFromHash(Hash, out bSuccess);

            if(bSuccess)
            {
                _name = ReturnedName;
            }
        }

        public override string ToString()
        {
            if(!string.IsNullOrEmpty(Name))
            {
                return string.Format("{0} [{1}]", Name, Hash);
            }

            return Hash.ToString();
        }
    }
}

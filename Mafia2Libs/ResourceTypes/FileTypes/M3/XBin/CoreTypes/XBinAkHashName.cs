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
    public class XBinAkHashNameConverter : ExpandableObjectConverter
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

            uint Hash = 0;
            bool bIsOnlyUlong = uint.TryParse(Splits[0], out Hash);

            XBinAkHashName HashName = new XBinAkHashName();

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
            HashName.Hash = uint.Parse(RemovedBrackets);

            return Result ?? base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            object result = null;
            XBinAkHashName HashName = (XBinAkHashName)value;

            if (destinationType == typeof(string))
            {
                result = HashName.ToString();
            }

            return result ?? base.ConvertTo(context, culture, value, destinationType);
        }
    }

    [TypeConverter(typeof(XBinAkHashNameConverter)), PropertyClassAllowReflection()]
    public class XBinAkHashName
    {
        private string _name;

        [PropertyForceAsAttribute]
        public uint Hash { get; set; }
        [LocalisedDescription("$XBIN_PROP_DESC_NAME_UNSTORED"), PropertyForceAsAttribute]
        public string Name
        {
            get { return _name; }
            set { SetName(value); }
        }

        public XBinAkHashName()
        {
            Hash = 0;
            _name = "";
        }

        public static XBinAkHashName ConstructAndReadFromFile(BinaryReader reader)
        {
            XBinAkHashName NewObject = new XBinAkHashName();
            NewObject.ReadFromFile(reader);
            return NewObject;
        }

        public void ReadFromFile(BinaryReader reader)
        {
            Hash = reader.ReadUInt32();

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
            Hash = FNV32.Hash(Name);
        }

        public void ForceToCheckStorage()
        {
            bool bSuccess = false;
            string ReturnedName = XBinHashStorage.GetNameFromHash(Hash, out bSuccess);

            if (bSuccess)
            {
                _name = ReturnedName;
            }
        }

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(Name))
            {
                return string.Format("{0} [{1}]", Name, Hash);
            }

            return Hash.ToString();
        }
    }
}

using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using Utils.Extensions;
using Utils.Helpers.Reflection;

namespace ResourceTypes.XBin.Types
{
    public class XBinVector2Converter : TypeConverter
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
                float[] values = ConverterUtils.ConvertStringToFloats(stringValue, 2);
                result = new XBinVector3(values);
            }

            return result ?? base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            object result = null;
            XBinVector2 vector2 = (XBinVector2)value;

            if (destinationType == typeof(string))
            {
                result = vector2.ToString();
            }

            return result ?? base.ConvertTo(context, culture, value, destinationType);
        }
    }

    [TypeConverter(typeof(XBinVector2Converter)), PropertyClassAllowReflection]
    public class XBinVector2
    {
        [PropertyForceAsAttribute]
        public float X { get; set; }
        [PropertyForceAsAttribute]
        public float Y { get; set; }

        public XBinVector2() { }

        public XBinVector2(float[] Values)
        {
            X = Values[0];
            Y = Values[1];
        }

        public void ReadFromFile(BinaryReader reader)
        {
            X = reader.ReadSingle();
            Y = reader.ReadSingle();
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(X);
            writer.Write(Y);
        }

        public override string ToString()
        {
            return string.Format("X: {0} Y: {1}", X, Y);
        }
    }
}

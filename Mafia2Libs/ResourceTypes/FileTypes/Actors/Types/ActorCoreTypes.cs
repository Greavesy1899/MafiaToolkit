using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Extensions;
using Utils.Helpers.Reflection;

namespace ResourceTypes.Actors
{
    public class EDSVector3Converter : TypeConverter
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
                result = new EDSVector3(values);
            }

            return result ?? base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            object result = null;
            EDSVector3 vector3 = (EDSVector3)value;

            if (destinationType == typeof(string))
            {
                result = vector3.ToString();
            }

            return result ?? base.ConvertTo(context, culture, value, destinationType);
        }
    }

    [TypeConverter(typeof(EDSVector3Converter)), PropertyClassAllowReflection]
    public class EDSVector3
    {
        [PropertyForceAsAttribute]
        public float X { get; set; }
        [PropertyForceAsAttribute]
        public float Y { get; set; }
        [PropertyForceAsAttribute]
        public float Z { get; set; }

        public EDSVector3() { }

        public EDSVector3(float[] Values)
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
}

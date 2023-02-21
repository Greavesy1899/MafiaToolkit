using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using Utils.Extensions;
using Utils.Helpers.Reflection;

namespace Toolkit.Mathematics
{
    public class Vec3Converter : TypeConverter
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
                result = new Vec3(values);
            }

            return result ?? base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            object result = null;
            Vec3 vector3 = (Vec3)value;

            if (destinationType == typeof(string))
            {
                result = vector3.ToString();
            }

            return result ?? base.ConvertTo(context, culture, value, destinationType);
        }
    }

    /**
     * Type which is expected to be used with XML serialisation.
     * The problem we have we System.Numeric.Vector3 is that it does not
     * reflect well into XML. Therefore we can use this "barebones" class
     * to make the transition easier. Unfortunately, it cannot be called 
     * Vector3 either due to the clash in other Toolkit files such as Collision.cs
     */
    [TypeConverter(typeof(Vec3Converter)), PropertyClassAllowReflection]
    public class Vec3
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public Vec3() { }

        public Vec3(float[] Values)
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

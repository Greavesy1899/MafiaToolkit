using BitStreams;
using System;
using System.ComponentModel;
using System.Globalization;
using Utils.Extensions;
using Utils.Helpers.Reflection;

namespace ResourceTypes.Prefab
{
    [TypeConverter(typeof(C_Vector3Converter)), PropertyClassAllowReflection]
    public class C_Vector3
    {
        [PropertyForceAsAttribute]
        public float X { get; set; }
        [PropertyForceAsAttribute]
        public float Y { get; set; }
        [PropertyForceAsAttribute]
        public float Z { get; set; }

        public C_Vector3()
        {
            X = Y = Z = 0.0f;
        }

        public C_Vector3(float InX, float InY, float InZ)
        {
            X = InX;
            Y = InY;
            Z = InZ;
        }

        public void Load(BitStream MemStream)
        {
            X = MemStream.ReadSingle();
            Y = MemStream.ReadSingle();
            Z = MemStream.ReadSingle();
        }

        public void Save(BitStream MemStream)
        {
            MemStream.WriteSingle(X);
            MemStream.WriteSingle(Y);
            MemStream.WriteSingle(Z);
        }

        public void Multiply(float InScalar)
        {
            X *= InScalar;
            Y *= InScalar;
            Z *= InScalar;
        }

        public void Divide(float InScalar)
        {
            X /= InScalar;
            Y /= InScalar;
            Z /= InScalar;
        }

        public C_Vector3 Clone()
        {
            C_Vector3 NewVector = new C_Vector3();
            NewVector.X = X;
            NewVector.Y = Y;
            NewVector.Z = Z;

            return NewVector;
        }

        public static C_Vector3 Construct(BitStream MemStream)
        {
            C_Vector3 Vector = new C_Vector3();
            Vector.Load(MemStream);
            return Vector;
        }

        public override string ToString()
        {
            return string.Format("X:{0} Y:{1} Z:{2}", X, Y, Z);
        }
    }

    public class C_Vector3Converter : ExpandableObjectConverter
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
                result = new C_Vector3(values[0], values[1], values[2]);
            }

            return result ?? base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            object result = null;
            C_Vector3 vector3 = (C_Vector3)value;

            if (destinationType == typeof(string))
            {
                result = string.Format("X:{0} Y:{1} Z:{2}", vector3.X, vector3.Y, vector3.Z);
            }

            return result ?? base.ConvertTo(context, culture, value, destinationType);
        }
    }
}

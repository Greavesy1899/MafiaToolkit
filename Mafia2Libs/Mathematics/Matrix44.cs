using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Numerics;
using System.Reflection;
using Utils.Extensions;
using Utils.Helpers.Reflection;

namespace Toolkit.Mathematics
{
    public class Matrix44Converter : TypeConverter
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
                Type t = typeof(Matrix4x4);

                Matrix4x4 Matrix = new();
                object M = Matrix;

                stringValue = stringValue.Replace("\n", "");
                stringValue = stringValue.Replace(" ", "");
                stringValue = stringValue.Replace(",", "|");
                stringValue = stringValue.Replace(")(", "_");
                stringValue = stringValue.Replace("(", "");
                stringValue = stringValue.Replace(")", "");

                string[] Rows = stringValue.Split("_");

                for (int i = 0; i < Rows.Length; i++)
                {
                    string[] Values = Rows[i].Split("|");

                    for (int j = 0; j < Values.Length; j++)
                    {
                        string PropertyName = "M" + (i + 1).ToString() + (j + 1).ToString();
                        float Value = Values[j].ToSingle();

                        FieldInfo fi = t.GetField(PropertyName);
                        fi.SetValue(M, Value);
                    }
                }

                Matrix = (Matrix4x4)M;
                result = new Matrix44(Matrix);
            }

            return result ?? base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            object result = null;
            Matrix44 matrix = (Matrix44)value;

            if (destinationType == typeof(string))
            {
                result = matrix.ToString();
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
    [TypeConverter(typeof(Matrix44Converter)), PropertyClassAllowReflection]
    public class Matrix44
    {
        public float M11 { get; set; } = 1.0f;
        public float M12 { get; set; } = 0.0f;
        public float M13 { get; set; } = 0.0f;
        public float M14 { get; set; } = 0.0f;
        public float M21 { get; set; } = 0.0f;
        public float M22 { get; set; } = 1.0f;
        public float M23 { get; set; } = 0.0f;
        public float M24 { get; set; } = 0.0f;
        public float M31 { get; set; } = 0.0f;
        public float M32 { get; set; } = 0.0f;
        public float M33 { get; set; } = 1.0f;
        public float M34 { get; set; } = 0.0f;
        public float M41 { get; set; } = 0.0f;
        public float M42 { get; set; } = 0.0f;
        public float M43 { get; set; } = 0.0f;
        public float M44 { get; set; } = 1.0f;

        public Matrix44() { }

        public Matrix44(float[] Values)
        {
            M11 = Values[0];
            M12 = Values[1];
            M13 = Values[2];
            M14 = Values[3];
            M21 = Values[4];
            M22 = Values[5];
            M23 = Values[6];
            M24 = Values[7];
            M31 = Values[8];
            M32 = Values[9];
            M33 = Values[10];
            M34 = Values[11];
            M41 = Values[12];
            M42 = Values[13];
            M43 = Values[14];
            M44 = Values[15];
        }

        public Matrix44(Matrix4x4 M)
        {
            M11 = M.M11;
            M12 = M.M12;
            M13 = M.M13;
            M14 = M.M14;
            M21 = M.M21;
            M22 = M.M22;
            M23 = M.M23;
            M24 = M.M24;
            M31 = M.M31;
            M32 = M.M32;
            M33 = M.M33;
            M34 = M.M34;
            M41 = M.M41;
            M42 = M.M42;
            M43 = M.M43;
            M44 = M.M44;
        }

        public void ReadFromFile(MemoryStream Stream, bool bIsBigEndian)
        {
            M11 = Stream.ReadSingle(bIsBigEndian);
            M12 = Stream.ReadSingle(bIsBigEndian);
            M13 = Stream.ReadSingle(bIsBigEndian);
            M41 = Stream.ReadSingle(bIsBigEndian);
            M21 = Stream.ReadSingle(bIsBigEndian);
            M22 = Stream.ReadSingle(bIsBigEndian);
            M23 = Stream.ReadSingle(bIsBigEndian);
            M42 = Stream.ReadSingle(bIsBigEndian);
            M31 = Stream.ReadSingle(bIsBigEndian);
            M32 = Stream.ReadSingle(bIsBigEndian);
            M33 = Stream.ReadSingle(bIsBigEndian);
            M43 = Stream.ReadSingle(bIsBigEndian);
        }

        public void WriteToFile(MemoryStream Stream, bool bIsBigEndian)
        {
            Stream.Write(M11, bIsBigEndian);
            Stream.Write(M12, bIsBigEndian);
            Stream.Write(M13, bIsBigEndian);
            Stream.Write(M41, bIsBigEndian);
            Stream.Write(M21, bIsBigEndian);
            Stream.Write(M22, bIsBigEndian);
            Stream.Write(M23, bIsBigEndian);
            Stream.Write(M42, bIsBigEndian);
            Stream.Write(M31, bIsBigEndian);
            Stream.Write(M32, bIsBigEndian);
            Stream.Write(M33, bIsBigEndian);
            Stream.Write(M43, bIsBigEndian);
        }

        public void ReadFromFile(BinaryReader br)
        {
            M11 = br.ReadSingle();
            M12 = br.ReadSingle();
            M13 = br.ReadSingle();
            M41 = br.ReadSingle();
            M21 = br.ReadSingle();
            M22 = br.ReadSingle();
            M23 = br.ReadSingle();
            M42 = br.ReadSingle();
            M31 = br.ReadSingle();
            M32 = br.ReadSingle();
            M33 = br.ReadSingle();
            M43 = br.ReadSingle();
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write(M11);
            bw.Write(M12);
            bw.Write(M13);
            bw.Write(M41);
            bw.Write(M21);
            bw.Write(M22);
            bw.Write(M23);
            bw.Write(M42);
            bw.Write(M31);
            bw.Write(M32);
            bw.Write(M33);
            bw.Write(M43);
        }

        public override string ToString()
        {
            return "(" + M11 + ", " + M12 + ", " + M13 + ", " + M14 + ") " +
                   "(" + M21 + ", " + M22 + ", " + M23 + ", " + M24 + ") " +
                   "(" + M31 + ", " + M32 + ", " + M33 + ", " + M34 + ") " +
                   "(" + M41 + ", " + M42 + ", " + M43 + ", " + M44 + ")";
        }
    }
}

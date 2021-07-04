using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;

namespace ResourceTypes.OC3.FaceFX
{
    /*
     * FaceFx archives store the majority of the strings in a table,
     * so we encapsulate the text in FxName. This way, when we are saving
     * the archive, we can store the text directly into the table.
     */
    [TypeConverter(typeof(FxNameConverter))]
    public class FxName
    {
        public string Text { get; set; }

        public FxName()
        {
            Text = "Null";
        }

        public void ReadFromFile(FxArchive OwningArchive, BinaryReader Reader)
        {
            uint StringTableIndex = Reader.ReadUInt32();
            Text = OwningArchive.GetFromStringTable(StringTableIndex);
        }

        public void WriteToFile(FxArchive OwningArchive, BinaryWriter Writer)
        {
            int StringTableIndex = OwningArchive.GetFromStringTable(Text);
            Writer.Write((uint)StringTableIndex);
        }

        public void AddToStringTable(FxArchive OwningArchive)
        {
            OwningArchive.AddToStringTable(Text);
        }

        public void SetIndex(FxArchive OwningArchive, uint Index)
        {
            Text = OwningArchive.GetFromStringTable(Index);
        }

        public override string ToString()
        {
            return Text;
        }
    }

    /*
     * TypeConverter so FxName's in the PropertyGrid can be edited easier.
     * Essentially this allows bi-directional String -> FxName conversions.
     */
    public class FxNameConverter : TypeConverter
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
            // Convert value to string
            string StringValue = value as string;

            // Construct new FxName, store value inside the FxName and return
            FxName NameObject = new FxName();
            NameObject.Text = StringValue;

            return NameObject;
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            // This one is easy - it's already converted to an editable string via .ToString()
            return value.ToString();
        }
    }
}
using SharpDX;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Utils.Extensions
{
    public sealed class MTreeView : TreeView
    {

        //fix from: (gotta love stack overflow!)
        //https://stackoverflow.com/questions/14647216/c-sharp-treeview-ignore-double-click-only-at-checkbox
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0203 && CheckBoxes)
            {
                var localPos = this.PointToClient(Cursor.Position);
                var hitTestInfo = this.HitTest(localPos);
                if (hitTestInfo.Location == TreeViewHitTestLocations.StateImage)
                {
                    m.Msg = 0x0201;
                }
            }
            base.WndProc(ref m);
        }
    }

    public class MTableColumn : DataGridViewColumn
    {
        private byte unk2;
        private ushort unk3;
        private Gibbed.Mafia2.ResourceFormats.TableData.ColumnType m2Type;
        private uint hash;

        public byte Unk2 {
            get { return unk2; }
            set { unk2 = value; }
        }
        public ushort Unk3 {
            get { return unk3; }
            set { unk3 = value; }
        }
        public Gibbed.Mafia2.ResourceFormats.TableData.ColumnType TypeM2 {
            get { return m2Type; }
            set { m2Type = value; }
        }
        public uint NameHash {
            get { return hash; }
            set { hash = value; }
        }
    }

    public class MToolStripStatusLabel : ToolStripStatusLabel
    {
        public void SetTextWithTimeStamp(string InText)
        {
            string Message = string.Format("{0} - {1}", DateTime.Now.ToLongTimeString(), InText);
            Text = Message;
        }
    }

    public static class ConverterUtils
    {
        private static string[] replacementList = { "X", "Y", "Z", "W", ":" };
        public static float[] ConvertStringToFloats(string text, int count)
        {
            //remove letters
            foreach(var replacement in replacementList)
            {
                text = text.Replace(replacement, "");
            }

            text = text.Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator);

            //create array, split text to array and convert to floats.
            float[] floats = new float[count];
            string[] components = text.Split(' ');
            for(int i = 0; i < count; i++)
            {
                var value = 0.0f;
                if(!float.TryParse(components[i], NumberStyles.Any, CultureInfo.InvariantCulture, out value))
                {
                    throw new InvalidCastException(string.Format("Failed to convert {0}", components[i]));
                }
                floats[i] = value;
            }
            return floats;
        }
    }

    public class Vector2Converter : TypeConverter
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
                result = new Vector2(values);
            }

            return result ?? base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            object result = null;
            Vector2 vector2 = (Vector2)value;

            if (destinationType == typeof(string))
            {
                result = vector2.ToString();
            }

            return result ?? base.ConvertTo(context, culture, value, destinationType);
        }
    }

    public class Vector3Converter : TypeConverter
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
                result = new Vector3(values);
            }

            return result ?? base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            object result = null;
            Vector3 vector3 = (Vector3)value;

            if (destinationType == typeof(string))
            {
                result = vector3.ToString();
            }

            return result ?? base.ConvertTo(context, culture, value, destinationType);
        }
    }

    public class Vector4Converter : TypeConverter
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
                float[] values = ConverterUtils.ConvertStringToFloats(stringValue, 4);
                result = new Vector4(values);
            }

            return result ?? base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            object result = null;
            Vector4 vector4 = (Vector4)value;

            if (destinationType == typeof(string))
            {
                result = vector4.ToString();
            }

            return result ?? base.ConvertTo(context, culture, value, destinationType);
        }
    }

    public class QuaternionConverter : TypeConverter
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
                float[] values = ConverterUtils.ConvertStringToFloats(stringValue, 4);
                result = new Quaternion(values);
            }

            return result ?? base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            object result = null;
            Quaternion quaternion = (Quaternion)value;

            if (destinationType == typeof(string))
            {
                result = quaternion.ToString();
            }

            return result ?? base.ConvertTo(context, culture, value, destinationType);
        }
    }

    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.All)]
    public class NumericUpDownToolStrip : ToolStripControlHost
    {
        private Container components = null;
        private NumericUpDown numericUpDown;

        [Category("Data")]
        public decimal Value {
            get { return numericUpDown.Value; }
            set { numericUpDown.Value = value; }
        }

        [Category("Data")]
        public decimal Minimum {
            get { return numericUpDown.Minimum; }
            set { numericUpDown.Minimum = value; }
        }

        [Category("Data")]
        public decimal Maximum {
            get { return numericUpDown.Maximum; }
            set { numericUpDown.Maximum = value; }
        }

        [Category("Data")]
        public int DecimalPlaces {
            get { return numericUpDown.DecimalPlaces; }
            set { numericUpDown.DecimalPlaces = value; }
        }

        [Category("Data")]
        public decimal Increment {
            get { return numericUpDown.Increment; }
            set { numericUpDown.Increment = value; }
        }

        private EventHandler onValueChanged;

        public event EventHandler ValueChanged {
            add {
                onValueChanged += value;
            }
            remove {
                onValueChanged -= value;
            }
        }

        public NumericUpDownToolStrip() : base(new NumericUpDown())
        {
            InitializeComponent();
            numericUpDown = (NumericUpDown)Control;
        }

        protected override void OnSubscribeControlEvents(Control c)
        {
            base.OnSubscribeControlEvents(c);
            NumericUpDown nud = (NumericUpDown)c;
            nud.ValueChanged += new EventHandler(OnValueChanged);
        }

        protected override void OnUnsubscribeControlEvents(Control c)
        {
            base.OnUnsubscribeControlEvents(c);
            NumericUpDown nud = (NumericUpDown)c;
            nud.ValueChanged -= new EventHandler(OnValueChanged);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();

            base.Dispose(disposing);
        }

        
        private void OnValueChanged(object sender, EventArgs e)
        {
            onValueChanged?.Invoke(this, e);
        }

        private void InitializeComponent()
        {
            components = new Container();
        }
    }

    public class FlagCheckedListBox : CheckedListBox
    {
        private Container components = null;

        public FlagCheckedListBox()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            // TODO: Add any initialization after the InitForm call

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                    components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code
        private void InitializeComponent()
        {
            // 
            // FlaggedCheckedListBox
            // 
            this.CheckOnClick = true;

        }
        #endregion

        // Adds an integer value and its associated description
        public FlagCheckedListBoxItem Add(int v, string c)
        {
            FlagCheckedListBoxItem item = new FlagCheckedListBoxItem(v, c);
            Items.Add(item);
            return item;
        }

        public FlagCheckedListBoxItem Add(FlagCheckedListBoxItem item)
        {
            Items.Add(item);
            return item;
        }

        protected override void OnItemCheck(ItemCheckEventArgs e)
        {
            base.OnItemCheck(e);

            if (isUpdatingCheckStates)
                return;

            // Get the checked/unchecked item
            FlagCheckedListBoxItem item = Items[e.Index] as FlagCheckedListBoxItem;
            // Update other items
            UpdateCheckedItems(item, e.NewValue);
        }

        // Checks/Unchecks items depending on the give bitvalue
        protected void UpdateCheckedItems(int value)
        {

            isUpdatingCheckStates = true;

            // Iterate over all items
            for (int i = 0; i < Items.Count; i++)
            {
                FlagCheckedListBoxItem item = Items[i] as FlagCheckedListBoxItem;

                if (item.value == 0)
                {
                    SetItemChecked(i, value == 0);
                }
                else
                {

                    // If the bit for the current item is on in the bitvalue, check it
                    if ((item.value & value) == item.value && item.value != 0)
                        SetItemChecked(i, true);
                    // Otherwise uncheck it
                    else
                        SetItemChecked(i, false);
                }
            }

            isUpdatingCheckStates = false;

        }

        // Updates items in the checklistbox
        // composite = The item that was checked/unchecked
        // cs = The check state of that item
        protected void UpdateCheckedItems(FlagCheckedListBoxItem composite, CheckState cs)
        {

            // If the value of the item is 0, call directly.
            if (composite.value == 0)
                UpdateCheckedItems(0);


            // Get the total value of all checked items
            int sum = 0;
            for (int i = 0; i < Items.Count; i++)
            {
                FlagCheckedListBoxItem item = Items[i] as FlagCheckedListBoxItem;

                // If item is checked, add its value to the sum.
                if (GetItemChecked(i))
                    sum |= item.value;
            }

            // If the item has been unchecked, remove its bits from the sum
            if (cs == CheckState.Unchecked)
                sum = sum & (~composite.value);
            // If the item has been checked, combine its bits with the sum
            else
                sum |= composite.value;

            // Update all items in the checklistbox based on the final bit value
            UpdateCheckedItems(sum);

        }

        private bool isUpdatingCheckStates = false;

        // Gets the current bit value corresponding to all checked items
        public int GetCurrentValue()
        {
            int sum = 0;

            for (int i = 0; i < Items.Count; i++)
            {
                FlagCheckedListBoxItem item = Items[i] as FlagCheckedListBoxItem;

                if (GetItemChecked(i))
                    sum |= item.value;
            }

            return sum;
        }

        Type enumType;
        Enum enumValue;

        // Adds items to the checklistbox based on the members of the enum
        private void FillEnumMembers()
        {
            foreach (string name in Enum.GetNames(enumType))
            {
                object val = Enum.Parse(enumType, name);
                int intVal = (int)Convert.ChangeType(val, typeof(int));

                Add(intVal, name);
            }
        }

        // Checks/unchecks items based on the current value of the enum variable
        private void ApplyEnumValue()
        {
            int intVal = (int)Convert.ChangeType(enumValue, typeof(int));
            UpdateCheckedItems(intVal);

        }

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
        public Enum EnumValue {
            get {
                object e = Enum.ToObject(enumType, GetCurrentValue());
                return (Enum)e;
            }
            set {

                Items.Clear();
                enumValue = value; // Store the current enum value
                enumType = value.GetType(); // Store enum type
                FillEnumMembers(); // Add items for enum members
                ApplyEnumValue(); // Check/uncheck items depending on enum value

            }
        }


    }

    // Represents an item in the checklistbox
    public class FlagCheckedListBoxItem
    {
        public FlagCheckedListBoxItem(int v, string c)
        {
            value = v;
            caption = c;
        }

        public override string ToString()
        {
            return caption;
        }

        // Returns true if the value corresponds to a single bit being set
        public bool IsFlag {
            get {
                return ((value & (value - 1)) == 0);
            }
        }

        // Returns true if this value is a member of the composite bit value
        public bool IsMemberFlag(FlagCheckedListBoxItem composite)
        {
            return (IsFlag && ((value & composite.value) == value));
        }

        public int value;
        public string caption;
    }


    // UITypeEditor for flag enums
    public class FlagEnumUIEditor : UITypeEditor
    {
        // The checklistbox
        private FlagCheckedListBox flagEnumCB;

        public FlagEnumUIEditor()
        {
            flagEnumCB = new FlagCheckedListBox();
            flagEnumCB.BorderStyle = BorderStyle.None;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (context != null
                && context.Instance != null
                && provider != null)
            {

                IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

                if (edSvc != null)
                {

                    Enum e = (Enum)Convert.ChangeType(value, context.PropertyDescriptor.PropertyType);
                    flagEnumCB.EnumValue = e;
                    edSvc.DropDownControl(flagEnumCB);
                    return flagEnumCB.EnumValue;

                }
            }
            return null;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }


    }

    //Taken from stackoverflow, not my code https://stackoverflow.com/questions/2273577/how-to-go-from-treenode-fullpath-data-and-get-the-actual-treenode;
    public static class TreeNodeCollectionUtils
    {
        public static TreeNode FindTreeNodeByFullPath(this TreeNodeCollection collection, string fullPath, StringComparison comparison = StringComparison.InvariantCultureIgnoreCase)
        {
            var foundNode = collection.Cast<TreeNode>().FirstOrDefault(tn => string.Equals(tn.FullPath, fullPath, comparison));
            if (null == foundNode)
            {
                foreach (var childNode in collection.Cast<TreeNode>())
                {
                    var foundChildNode = FindTreeNodeByFullPath(childNode.Nodes, fullPath, comparison);
                    if (null != foundChildNode)
                    {
                        return foundChildNode;
                    }
                }
            }

            return foundNode;
        }
    }

    public static class ColorExtender
    {
        public static Vector3 Normalize(this System.Drawing.Color color)
        {
            return new Vector3(color.R / 255.0f, color.G / 255.0f, color.B / 255.0f);
        }
    }


    public static class TreeNodeExtender
    {
        public static bool CheckIfParentsAreValid(this TreeNode node)
        {
            bool bValid = false;
            TreeNode currentParent = node.Parent;

            while (currentParent != null)
            {
                bValid = currentParent.Checked;
                currentParent = currentParent.Parent;

                if(!bValid)
                {
                    return bValid;
                }
            }
            return bValid;
        }
    }
    public static class DictionaryExtensions
    {
        public static int IndexOfValue<Tkey, TValue>(this Dictionary<Tkey, TValue> dic, int key)
        {
            int index = 0;
            foreach (KeyValuePair<Tkey, TValue> entry in dic)
            {
                if (Convert.ToInt32(entry.Key) == key)
                    return index;
                else
                    index++;
            }
            return -1;
        }

        public static bool AddRange<TKey, TValue>(this Dictionary<TKey, TValue> Dic, Dictionary<TKey, TValue> OtherDic)
        {
            bool bResult = true;

            foreach(var Pair in OtherDic)
            {
                bResult = Dic.TryAdd(Pair.Key, Pair.Value);
            }

            return bResult;
        }

        public static bool TryAdd<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key, TValue value)
        {
            bool bHasKey = dic.ContainsKey(key);

            if(!bHasKey)
            {
                dic.Add(key, value);
                return true;
            }

            return false;
        }

        public static bool TryRemove<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key)
        {
            bool bHasKey = dic.ContainsKey(key);

            if(bHasKey)
            {
                return dic.Remove(key);
            }

            return false;
        }

        public static TValue TryGet<Tkey, TValue>(this Dictionary<Tkey, TValue> dic, Tkey key)
        {
            if(dic.ContainsKey(key))
            {
                return dic[key];
            }

            return default(TValue);
        }
    }

    public static class FileInfoUtils
    {
        public static string ConvertToMemorySize(this long value)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = value;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }

            // Adjust the format string to your preferences. For example "{0:0.#}{1}" would
            // show a single decimal place, and no space.
            return string.Format("{0:0.##} {1}", len, sizes[order]);
        }

        public static string CalculateFileSize(this FileInfo file)
        {
            return file.Length.ConvertToMemorySize();
        }
    }

    public static class BinaryReaderExtender
    {
        public static uint ReadInt24(this BinaryReader reader)
        {
            byte[] bytes = reader.ReadBytes(3);
            int value = bytes[0] | (bytes[1] << 8) | (bytes[2] << 16);
            return (uint)value;
        }
    }

    public static class BinaryWriterExtender
    {
        public static void WriteInt24(this BinaryWriter writer, uint value)
        {
            byte[] bytes = new byte[4];
            bytes[0] = (byte)(value & 0xFF);
            bytes[1] = (byte)(value >> 8);
            bytes[2] = (byte)(value >> 16);
            bytes[3] = (byte)(value >= 128 ? 0x80 : 0); //not sure this is correct..
            writer.Write(bytes);
        }
    }
}
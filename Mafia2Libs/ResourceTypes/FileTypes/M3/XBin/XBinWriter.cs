using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Utils.Extensions;

namespace ResourceTypes.M3.XBin
{
    public class XBinWriter : BinaryWriter
    {
        private struct StringPtr
        {
            public long FileOffset { get; set; }
            public string RequiredString { get; set; }
        }

        private string RawStringBuffer;
        private Dictionary<string, int> StringBuffer;
        private Dictionary<string, long> ObjectPtrsToFix;
        private List<StringPtr> PtrsToFix;

        public XBinWriter(Stream output) : base(output)
        {
            RawStringBuffer = "";
            StringBuffer = new Dictionary<string, int>();
            ObjectPtrsToFix = new Dictionary<string, long>();
            PtrsToFix = new List<StringPtr>();
        }

        public XBinWriter(Stream output, Encoding encoding) : base(output, encoding)
        {
            RawStringBuffer = "";
            StringBuffer = new Dictionary<string, int>();
            ObjectPtrsToFix = new Dictionary<string, long>();
            PtrsToFix = new List<StringPtr>();
        }

        public XBinWriter(Stream output, Encoding encoding, bool leaveOpen) : base(output, encoding, leaveOpen)
        {
            RawStringBuffer = "";
            StringBuffer = new Dictionary<string, int>();
            ObjectPtrsToFix = new Dictionary<string, long>();
            PtrsToFix = new List<StringPtr>();
        }

        protected XBinWriter()
        {
            RawStringBuffer = "";
            StringBuffer = new Dictionary<string, int>();
            ObjectPtrsToFix = new Dictionary<string, long>();
            PtrsToFix = new List<StringPtr>();
        }

        public override void Close()
        {
            Debug.Assert(PtrsToFix.Count == 0, "Should have no StringPtrs to fix!");
            Debug.Assert(ObjectPtrsToFix.Count == 0, "Should have no ObjectPtrs to fix!");
            base.Close();
        }

        // Functions to help XBin
        public void PushStringPtr(string Text)
        {
            // Create a Ptr struct for the string
            StringPtr Ptr = new StringPtr();
            Ptr.FileOffset = BaseStream.Position;
            Ptr.RequiredString = Text;

            // Push the Ptr to the array.
            PtrsToFix.Add(Ptr);

            // Write temporary -1
            Write(-1);
        }

        public void PushObjectPtr(string Identifier)
        {
            // Push object to the array
            ObjectPtrsToFix.Add(Identifier, BaseStream.Position);

            // Write temporary -1
            Write(-1);
        }

        public void FixUpObjectPtr(string Identifier)
        {
            long CurrentPosition = BaseStream.Position;

            long Value = 0;
            if(ObjectPtrsToFix.TryGetValue(Identifier, out Value))
            {
                BaseStream.Seek(Value, SeekOrigin.Begin);
                uint ValueToWrite = (uint)(CurrentPosition - Value);
                Write(ValueToWrite);

                BaseStream.Seek(CurrentPosition, SeekOrigin.Begin);

                ObjectPtrsToFix.Remove(Identifier);
            }
        }

        public void FixUpStringPtrs()
        {
            long BufferStartOffset = BaseStream.Position;

            foreach(StringPtr Ptr in PtrsToFix)
            {
                int Offset = GetBufferOffsetForString(Ptr.RequiredString);

                BaseStream.Seek(Ptr.FileOffset, SeekOrigin.Begin);
                int StringOffset = (int)(BufferStartOffset + Offset);
                int Gap = (int)(StringOffset - Ptr.FileOffset);
                Write(Gap);
            }

            BaseStream.Seek(BufferStartOffset, SeekOrigin.Begin);
            Utils.StringHelpers.StringHelpers.WriteString(this, RawStringBuffer);
            PtrsToFix.Clear();
        }

        private void AddStringToBuffer(string Text)
        {
            StringBuffer.Add(Text, RawStringBuffer.Length);
            RawStringBuffer += (Text + '\0');
        }

        private int GetBufferOffsetForString(string Text)
        {
            if(StringBuffer.ContainsKey(Text))
            {
                return StringBuffer[Text];
            }
            else
            {
                AddStringToBuffer(Text);
                return StringBuffer[Text];
            }
        }
    }
}

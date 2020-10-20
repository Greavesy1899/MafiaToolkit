using System.Collections.Generic;
using System.IO;
using System.Text;

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
        private List<StringPtr> PtrsToFix;

        public XBinWriter(Stream output) : base(output)
        {
            RawStringBuffer = "";
            StringBuffer = new Dictionary<string, int>();
            PtrsToFix = new List<StringPtr>();
        }

        public XBinWriter(Stream output, Encoding encoding) : base(output, encoding)
        {
            RawStringBuffer = "";
            StringBuffer = new Dictionary<string, int>();
            PtrsToFix = new List<StringPtr>();
        }

        public XBinWriter(Stream output, Encoding encoding, bool leaveOpen) : base(output, encoding, leaveOpen)
        {
            RawStringBuffer = "";
            StringBuffer = new Dictionary<string, int>();
            PtrsToFix = new List<StringPtr>();
        }

        protected XBinWriter()
        {
            RawStringBuffer = "";
            StringBuffer = new Dictionary<string, int>();
            PtrsToFix = new List<StringPtr>();
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

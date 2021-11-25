using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace ResourceTypes.Navigation
{
    // TODO: XBin has the same functionality - maybe we can make something like TBinaryWriter?
    public class NavigationWriter : BinaryWriter
    {
        private struct ObjectPtr
        {
            public long FileOffset { get; set; } // Offset to save to
            public long BaseOffset { get; set; } // Position we should be offsetting from
        }

        private Dictionary<string, ObjectPtr> ObjectPtrs;

        //~ Constructors
        protected NavigationWriter()
        {
            ObjectPtrs = new Dictionary<string, ObjectPtr>();
        }

        public NavigationWriter(Stream output, Encoding encoding) : base(output, encoding)
        {
            ObjectPtrs = new Dictionary<string, ObjectPtr>();
        }

        public NavigationWriter(Stream output) : base(output)
        {
            ObjectPtrs = new Dictionary<string, ObjectPtr>();
        }

        public NavigationWriter(Stream output, Encoding encoding, bool leaveOpen) : base(output, encoding, leaveOpen)
        {
            ObjectPtrs = new Dictionary<string, ObjectPtr>();
        }

        //~ Overridden Functions
        public override void Close()
        {
            Debug.Assert(ObjectPtrs.Count == 0, "Should have no ObjectPtrs to fix!");

            base.Close();
        }

        //~ Functions
        public void PushLooseObjectPtr(string UniqueID, long PositionToOffsetFrom = 0)
        {
            // create pointer
            ObjectPtr NewPtr = new ObjectPtr();
            NewPtr.FileOffset = BaseStream.Position;
            NewPtr.BaseOffset = PositionToOffsetFrom;

            Debug.Assert(!ObjectPtrs.ContainsKey(UniqueID), "Cannot add duplicate key into the ObjectPtrs dictionary.");

            Write(-1);

            ObjectPtrs.Add(UniqueID, NewPtr);
        }

        public void SolveLooseObjectPtr(string UniqueID)
        {
            Debug.Assert(ObjectPtrs.ContainsKey(UniqueID), "Cannot solve loose object if it doesn't exist.");

            // get ptr
            ObjectPtr ExistingPtr = ObjectPtrs[UniqueID];

            // cache
            long CurrentPosition = BaseStream.Position;
            long ActualOffset = (BaseStream.Position - ExistingPtr.BaseOffset);

            // seek to old position and write new offset
            BaseStream.Position = ExistingPtr.FileOffset;
            Write((uint)ActualOffset);

            // return back to position
            BaseStream.Position = CurrentPosition;

            // remove
            ObjectPtrs.Remove(UniqueID);
        }

        public void RemoveLooseObjectPtr(string UniqueID)
        {
            // don't make users suffer - we should be able to use this without
            // deafening the users with the windows error sound.
            if (ObjectPtrs.ContainsKey(UniqueID))
            {
                ObjectPtrs.Remove(UniqueID);
            }
        }
    }
}

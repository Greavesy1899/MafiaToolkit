using System.Collections.Generic;
using Gibbed.Illusion.FileFormats.Hashing;
using Utils.Logging;

namespace ResourceTypes.ModelHelpers.ModelExporter
{
    public interface IVisitor
    {
        void Accept(MT_Object Object);
    }

    public class MT_ValidationVisitor : IVisitor
    {
        private MT_ValidationTracker ValidationTracker = null;
        private MT_Object CurrentObject = null;

        public MT_ValidationVisitor()
        {
            ValidationTracker = new MT_ValidationTracker();
        }

        public void Accept(MT_Object Object)
        {
            CurrentObject = Object;

            // do stuff

            CurrentObject = null;
        }

        private void AddMessage(MT_MessageType MessageType, string Format, params object[] Args)
        {
            ToolkitAssert.Ensure(ValidationTracker != null, "TrackerObject shouldn't be invalid.");
            ValidationTracker.AddMessage(CurrentObject, MessageType, Format, Args);
        }

        private void AddMessage(MT_MessageType MessageType, string Text)
        {
            ToolkitAssert.Ensure(ValidationTracker != null, "TrackerObject shouldn't be invalid.");
            ValidationTracker.AddMessage(CurrentObject, MessageType, Text);
        }
    }

    public class MT_MaterialCollectorVisitor : IVisitor
    {
        // A dictionary containing all materials collected by this visitor.
        public Dictionary<ulong, MT_MaterialInstance> Materials { get; private set; }

        public MT_MaterialCollectorVisitor()
        {
            Materials = new Dictionary<ulong, MT_MaterialInstance>();
        }

        public void Accept(MT_Object Object)
        {
            if (!Object.ObjectFlags.HasFlag(MT_ObjectFlags.HasLODs))
            {
                // Skip if no MT_Lods
                return;
            }

            // Iterate through all LODs then FaceGroups, find all valid materials.
            // We don't need to iterate through the MT_Object's child as that is done elsewhere.
            foreach (MT_Lod Lod in Object.Lods)
            {
                foreach (MT_FaceGroup FaceGroup in Lod.FaceGroups)
                {
                    if (FaceGroup.Material != null)
                    {
                        MT_MaterialInstance MatIntance = FaceGroup.Material;

                        // Only add the hash if doesn't exist in the dictionary
                        ulong Hash = FNV64.Hash(MatIntance.Name);
                        if (!Materials.ContainsKey(Hash))
                        {
                            // Could be quite costly but I can't const the MaterialInstance
                            Materials.Add(Hash, new MT_MaterialInstance(MatIntance));
                        }
                    }
                }
            }
        }
    }
}

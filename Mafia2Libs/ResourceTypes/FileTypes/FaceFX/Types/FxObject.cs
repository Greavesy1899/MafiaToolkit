using System.IO;

namespace ResourceTypes.OC3.FaceFX
{
    /*
     * Base class of all FaceFx objects
     */
    public abstract class FxObject
    {
        // Standard constructor
        public FxObject() { }

        // Load the object
        public virtual void Deserialize(FxArchive Owner, BinaryReader Reader) { }

        // Save the object
        public virtual void Serialize(FxArchive Owner, BinaryWriter Writer) { }

        public virtual void PopulateStringTable(FxArchive Owner) { }
    }
}

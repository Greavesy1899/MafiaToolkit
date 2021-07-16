using System.IO;

namespace ResourceTypes.OC3.FaceFX
{
    /*
     * Named base class. 
     * Names are stored in the FxArchive StringTable.
     */
    public class FxNamedObject : FxObject
    {
        public FxName Name { get; set; }

        public FxNamedObject() : base()
        {
            Name = new FxName();
        }

        public override void Deserialize(FxArchive Owner, BinaryReader Reader)
        {
            base.Deserialize(Owner, Reader);

            Name.ReadFromFile(Owner, Reader);
        }

        public override void Serialize(FxArchive Owner, BinaryWriter Writer)
        {
            base.Serialize(Owner, Writer);

            Name.WriteToFile(Owner, Writer);
        }

        public override void PopulateStringTable(FxArchive Owner)
        {
            base.PopulateStringTable(Owner);

            Name.AddToStringTable(Owner);
        }
    }
}

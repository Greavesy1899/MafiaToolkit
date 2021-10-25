using Vortice.Mathematics;

namespace ResourceTypes.ModelHelpers.ModelExporter
{
    public class MT_FaceGroup : IValidator
    {
        public BoundingBox Bounds { get; set; }
        public uint StartIndex { get; set; }
        public uint NumFaces { get; set; }
        public MT_MaterialInstance Material { get; set; }

        protected override bool InternalValidate(MT_ValidationTracker TrackerObject)
        {
            bool bIsValid = true;

            if(NumFaces == 0)
            {
                AddMessage(MT_MessageType.Error, "This FaceGroup has no faces.");
                bIsValid = false;
            }

            // Check Material
            if(Material == null)
            {
                AddMessage(MT_MessageType.Error, "This FaceGroup has an invalid Material object.");
                bIsValid = false;
            }
            else
            {
                bool bMaterialValid = Material.ValidateObject(TrackerObject);
                bIsValid &= bMaterialValid;
            }

            return bIsValid;
        }
    }
}

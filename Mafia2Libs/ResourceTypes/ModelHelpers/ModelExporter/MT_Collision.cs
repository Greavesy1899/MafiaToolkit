using System.IO;
using System.Numerics;
using Utils.StringHelpers;
using Utils.VorticeUtils;

namespace ResourceTypes.ModelHelpers.ModelExporter
{
    public class MT_Collision : IValidator
    {
        public Vector3[] Vertices { get; set; }
        public uint[] Indices { get; set; }
        public MT_FaceGroup[] FaceGroups { get; set; }

        //~ IValidator Interface
        protected override bool InternalValidate(MT_ValidationTracker TrackerObject)
        {
            bool bValidity = true;

            if(Vertices.Length == 0)
            {
                AddMessage(MT_MessageType.Error, "This collision object has no vertices.");
                bValidity = false;
            }

            if (Indices.Length == 0)
            {
                AddMessage(MT_MessageType.Error, "This collision object has no indices");
                bValidity = false;
            }

            if (FaceGroups.Length == 0)
            {
                AddMessage(MT_MessageType.Error, "This collision object has no face groups.");
                bValidity = false;
            }

            foreach(var FaceGroup in FaceGroups)
            {
                bool bIsValid = FaceGroup.ValidateObject(TrackerObject);
                bValidity &= bIsValid;
            }

            return bValidity;
        }
    }
}

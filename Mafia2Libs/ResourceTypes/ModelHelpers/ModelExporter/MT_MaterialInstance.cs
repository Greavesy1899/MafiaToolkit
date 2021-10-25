using System;
using ResourceTypes.Materials;

namespace ResourceTypes.ModelHelpers.ModelExporter
{
    [Flags]
    public enum MT_MaterialInstanceFlags
    {
        IsCollision = 1,
        HasDiffuse = 2
    }

    public class MT_MaterialInstance : IValidator
    {
        public MT_MaterialInstanceFlags MaterialFlags { get; set; }
        public string Name { get; set; }
        public string DiffuseTexture { get; set; }

        public MT_MaterialInstance()
        {
            Name = "";
            DiffuseTexture = "";
        }

        protected override bool InternalValidate(MT_ValidationTracker TrackerObject)
        {
            bool bValidity = true;

            // First make sure to handle collisions
            if (MaterialFlags.HasFlag(MT_MaterialInstanceFlags.IsCollision))
            {
                Collisions.CollisionMaterials MaterialChoice = Collisions.CollisionMaterials.Undefined;
                if(!Enum.TryParse(Name, out MaterialChoice))
                {
                    AddMessage(MT_MessageType.Error, "This Material is set to Collision, yet it cannot be converted to CollisionEnum - {0}", Name);
                    bValidity = false;
                }
            }
            else
            {
                // Then handle normal materials
                if (MaterialFlags.HasFlag(MT_MaterialInstanceFlags.HasDiffuse))
                {
                    // check if Material name is valid
                    if (string.IsNullOrEmpty(Name))
                    {
                        AddMessage(MT_MessageType.Warning, "This Material has no name.");
                        bValidity = false;
                    }
                    else
                    {
                        // Check if Material exists in MTLs
                        if (MaterialsManager.LookupMaterialByName(Name) == null)
                        {
                            AddMessage(MT_MessageType.Warning, "The Material [{0}] was not found in the currently loaded MTLs", Name);
                            bValidity = false;
                        }
                    }
                }
            }

            return bValidity;
        }
    }
}

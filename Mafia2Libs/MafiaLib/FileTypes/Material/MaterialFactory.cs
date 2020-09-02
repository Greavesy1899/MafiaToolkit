using System.Diagnostics;
using System.IO;

namespace ResourceTypes.Materials
{
    public static class MaterialFactory
    {
        public static IMaterial ReadMaterialFromFile(BinaryReader reader, VersionsEnumerator version)
        {
            IMaterial Material = null;

            switch(version)
            {
                case VersionsEnumerator.V_57:
                    Material = new Material_v57();
                    Material.ReadFromFile(reader, version);
                    break;
                case VersionsEnumerator.V_58:
                    Material = new Material_v58();
                    Material.ReadFromFile(reader, version);
                    break;
                case VersionsEnumerator.V_63:
                    Material = new Material_v63();
                    Material.ReadFromFile(reader, version);
                    break;
                default:
                    break;
            }

            Debug.Assert(Material != null, string.Format("Failed to read Material Type: {0}", version));
            return Material;
        }
    }
}

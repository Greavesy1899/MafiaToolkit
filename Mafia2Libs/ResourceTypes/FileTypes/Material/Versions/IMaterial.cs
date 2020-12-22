using System.Collections.Generic;
using System.IO;
using Utils.Types;
using System.ComponentModel;
using Utils.Extensions;

namespace ResourceTypes.Materials
{
    // TODO: Consider some unified approach for IMaterialSampler to be stored here.
    public class IMaterial
    {
        public HashName MaterialName { get; set; }
        public string MaterialGUID { get { return MaterialName.ConstructGUID(); } }

        [Editor(typeof(FlagEnumUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public MaterialFlags Flags { get; set; }
        public ulong ShaderID { get; set; }
        public uint ShaderHash { get; set; }
        public List<MaterialParameter> Parameters { get; set; }

        public IMaterial()
        {
            MaterialName = new HashName();
            Parameters = new List<MaterialParameter>();

            // TODO: Remove this from base class, make some kind of factory of Shader Types.
            MaterialName.Set("NEW_MATERIAL");
            ShaderHash = 3388704532;
            ShaderID = 4894707398632176459;
            Flags = (MaterialFlags)31461376;
        }

        public virtual void ReadFromFile(BinaryReader reader, VersionsEnumerator version) { }

        public virtual void WriteToFile(BinaryWriter writer, VersionsEnumerator version) { }

        public virtual List<string> CollectTextures() { return null; }

        public void SetName(string name)
        {
            MaterialName.Set(name);
        }

        public string GetMaterialName()
        {
            return MaterialName.String;
        }

        public ulong GetMaterialHash()
        {
            return MaterialName.Hash;
        }
        public virtual HashName GetTextureByID(string SamplerName)
        {
            return null;
        }

        public virtual bool HasTexture(string Name)
        {
            return false;
        }

        public MaterialParameter GetParameterByKey(string ParameterKey)
        {
            foreach (var param in Parameters)
            {
                if (param.ID == ParameterKey)
                {
                    return param;
                }
            }

            return null;
        }

        public override string ToString()
        {
            return string.Format("{0}", MaterialName.ToString());
        }
    }

    public class IMaterialSampler
    {
        public string ID { get; set; }
        public byte[] SamplerStates { get; set; }

        public IMaterialSampler()
        {
            // TODO: Remove this from base class, make some kind of factory of Shader Types.
            ID = "S000";
            SamplerStates = new byte[6] { 3, 3, 2, 0, 0, 0 };
        }

        public virtual void ReadFromFile(BinaryReader reader, VersionsEnumerator version) { }

        public virtual void WriteToFile(BinaryWriter writer, VersionsEnumerator version) { }

        public virtual string GetFileName()
        {
            return "Invalid";
        }

        public virtual ulong GetFileHash()
        {
            return ulong.MinValue;
        }
    }
}

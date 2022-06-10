using Gibbed.Illusion.FileFormats.Hashing;
using ResourceTypes.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rendering.Core
{
    public class RuntimeMaterialInstance
    {
        private IMaterial GameMaterial = null;

        private Dictionary<uint, IMaterialSampler> SamplerIDToHash;
        private Dictionary<uint, ulong> TextureIDToHash;
        private Dictionary<uint, float[]> ParameterIDToHash;

        public RuntimeMaterialInstance()
        {
            SamplerIDToHash = new Dictionary<uint, IMaterialSampler>();
            TextureIDToHash = new Dictionary<uint, ulong>();
            ParameterIDToHash = new Dictionary<uint, float[]>();
        }

        public void Initialise(IMaterial InMaterial)
        {
            GameMaterial = InMaterial;

            GetAndAddSampler("S000");
            GetAndAddSampler("S001");
            GetAndAddSampler("S011");

            GetAndAddParameter("C002");
            GetAndAddParameter("C005");
        }

        public ulong GetMaterialHash()
        {
            if(GameMaterial != null)
            {
                return GameMaterial.GetMaterialHash();
            }

            return 0;
        }

        public ulong GetSamplerTexture(uint SamplerID)
        {
            if(SamplerIDToHash.ContainsKey(SamplerID))
            {
                return SamplerIDToHash[SamplerID].GetFileHash();
            }

            return 0;
        }

        public float[] GetParamtersFor(uint ParameterID)
        {
            if (SamplerIDToHash.ContainsKey(ParameterID))
            {
                return ParameterIDToHash[ParameterID];
            }

            return null;
        }

        private void GetAndAddSampler(string SamplerID)
        {
            IMaterialSampler Sampler = GameMaterial.GetSamplerByKey(SamplerID);
            if(Sampler != null)
            {
                uint Hash = FNV32.Hash(SamplerID);
                SamplerIDToHash.Add(Hash, Sampler);
            }
        }

        private void GetAndAddParameter(string ParameterID)
        {
            MaterialParameter Param = GameMaterial.GetParameterByKey(ParameterID);
            if(Param != null)
            {
                uint Hash = FNV32.Hash(ParameterID);
                ParameterIDToHash.Add(Hash, Param.Paramaters);
            }
        }
    }
}

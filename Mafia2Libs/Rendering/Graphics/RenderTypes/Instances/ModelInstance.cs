using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Windows.Documents;
using Utils.Types;
using Vortice.Direct3D11;

namespace Rendering.Graphics.Instances
{
    // Třída spravující všechny instance modelů, které sdílí stejnou geometrii
    public class ModelInstanceManager
    {
        // Dictionary pro ukládání modelových instancí podle jejich LOD hashe
        private Dictionary<int, ModelInstances> instancesByLodHash = new Dictionary<int, ModelInstances>();

        // Metoda pro přidání nové instance
        public void AddInstance(int lodHash, ID3D11Buffer indexBuffer, ID3D11Buffer vertexBuffer,RenderModel.LOD[] lod,ID3D11ShaderResourceView ao,HashName aohash)
        {
            // Pokud už existují instance s tímto LOD hashem, přidá se nová, jinak se vytvoří nový seznam
            if (!instancesByLodHash.TryGetValue(lodHash, out ModelInstances modelInstances))
            {
                modelInstances = new ModelInstances();
                instancesByLodHash[lodHash] = modelInstances;
            }
            
            modelInstances.modelInstances.Add(new ModelInstance
            {
                LodHash = lodHash,
                indexBuffer = indexBuffer,
                vertexBuffer = vertexBuffer,
                LODs = lod,
                AoTexture = ao,
                aoHash = aohash,
            });
        }

        // Metoda pro získání všech instancí podle LOD hashe
        public List<ModelInstance> GetInstances(int lodHash)
        {
            return instancesByLodHash.TryGetValue(lodHash, out ModelInstances modelInstances)
                ? modelInstances.modelInstances
                : new List<ModelInstance>(); // Vrátí prázdný seznam, pokud není hash nalezen
        }
        
        public Dictionary<int, ModelInstances> GetAllInstances()
        {
            return instancesByLodHash;
        }

        // Metoda pro kontrolu, zda už instance s daným hashem existují
        public bool HasInstances(int lodHash)
        {
            return instancesByLodHash.ContainsKey(lodHash);
        }
        
        
        public void SetupShaders()
        {
            List<ModelInstances> instances = new List<ModelInstances>();
            instances = instancesByLodHash.Values.ToList();
            foreach (var instance in instances )
            {
                List<ModelInstance> instancelist = new List<ModelInstance>();
                instancelist = instance.modelInstances;
                foreach (var inst in instancelist)
                {
                    for (int x = 0; x != inst.LODs[0].ModelParts.Length; x++)
                    {
                        RenderModel.ModelPart part = inst.LODs[0].ModelParts[x];
                        if (part.Material == null)
                            part.Shader = RenderStorageSingleton.Instance.ShaderManager.shaders[0];
                        else
                        {
                            //Debug.WriteLine(LODs[0].ModelParts[x].Material.MaterialName + "\t" + LODs[0].ModelParts[x].Material.ShaderHash);
                            part.Shader = (RenderStorageSingleton.Instance.ShaderManager.shaders.ContainsKey(inst.LODs[0].ModelParts[x].Material.ShaderHash)
                                ? RenderStorageSingleton.Instance.ShaderManager.shaders[inst.LODs[0].ModelParts[x].Material.ShaderHash]
                                : RenderStorageSingleton.Instance.ShaderManager.shaders[0]);
                        }
                        inst.LODs[0].ModelParts[x] = part;
                    }
                }
            }
        }
            public void InitTextures(ID3D11Device d3d, ID3D11DeviceContext d3dContext)
            {
                List<ModelInstances> instances = new List<ModelInstances>();
                instances = instancesByLodHash.Values.ToList();
                foreach (var instance in instances )
                {
                    List<ModelInstance> instancelist = new List<ModelInstance>();
                    instancelist = instance.modelInstances;
                    foreach (var inst in instancelist)
                    {
                        if (inst.aoHash != null)
                        {
                            ID3D11ShaderResourceView texture;

                            if (!RenderStorageSingleton.Instance.TextureCache.TryGetValue(inst.aoHash.Hash, out texture))
                            {
                                if (!string.IsNullOrEmpty(inst.aoHash.String))
                                {
                                    texture = TextureLoader.LoadTexture(d3d, d3dContext, inst.aoHash.String);
                                    RenderStorageSingleton.Instance.TextureCache.Add(inst.aoHash.Hash, texture);
                                }
                            }

                            inst.AoTexture = texture;
                        }
                        else
                        {
                            inst.AoTexture = RenderStorageSingleton.Instance.TextureCache[0];
                        }

                        for (int i = 0; i < inst.LODs.Length; i++)
                        {
                            for(int x = 0; x < inst.LODs[i].ModelParts.Length; x++)
                            {
                                RenderModel.ModelPart part = inst.LODs[i].ModelParts[x];
                    
                                if(part.Material != null)
                                {
                                    GetTextureFromSampler(d3d, d3dContext, part, "S000");
                                    GetTextureFromSampler(d3d, d3dContext, part, "S001");
                                    GetTextureFromSampler(d3d, d3dContext, part, "S011");
                                }
                            }
                        }
                    }
                }
    }
    
    private void GetTextureFromSampler(ID3D11Device d3d, ID3D11DeviceContext d3dContext, RenderModel.ModelPart part, string SamplerKey)
    {
        HashName sampler = part.Material.GetTextureByID(SamplerKey);
        if (sampler != null)
        {
            ID3D11ShaderResourceView texture;

            ulong SamplerHash = sampler.Hash;
            string SamplerName = sampler.String;

            if (!RenderStorageSingleton.Instance.TextureCache.TryGetValue(SamplerHash, out texture))
            {
                if (!string.IsNullOrEmpty(SamplerName))
                {
                    texture = TextureLoader.LoadTexture(d3d, d3dContext, SamplerName);
                    RenderStorageSingleton.Instance.TextureCache.Add(SamplerHash, texture);
                }
            }
        }
    }
    }
    

    // Třída pro správu seznamu modelových instancí sdílejících stejný LOD
    public class ModelInstances
    {
        public List<ModelInstance> modelInstances = new List<ModelInstance>();
    }

    // Třída reprezentující jednotlivou instanci modelu
    public class ModelInstance
    {
        public RenderModel.LOD[] LODs;
        public int LodHash ;
        public ID3D11Buffer indexBuffer;
        public ID3D11Buffer vertexBuffer;
        public ID3D11Buffer InstanceBuffer; 
        public List<Matrix4x4> Transforms; 
        public ID3D11ShaderResourceView AoTexture;
        public HashName aoHash;
    }
}

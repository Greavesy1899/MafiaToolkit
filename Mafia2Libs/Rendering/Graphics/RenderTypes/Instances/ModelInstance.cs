using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Vortice.Direct3D11;

namespace Rendering.Graphics.Instances
{
    // Třída spravující všechny instance modelů, které sdílí stejnou geometrii
    public class ModelInstanceManager
    {
        // Dictionary pro ukládání modelových instancí podle jejich LOD hashe
        private Dictionary<int, ModelInstances> instancesByLodHash = new Dictionary<int, ModelInstances>();

        // Metoda pro přidání nové instance
        public void AddInstance(int lodHash, ID3D11Buffer indexBuffer, ID3D11Buffer vertexBuffer,RenderModel.LOD[] lod)
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
                LODs = lod
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
    }
}

using System.Collections.Generic;
using Rendering.Graphics;
using Vortice.Direct3D11;

namespace Rendering.Core
{
    public enum PrimitiveType
    {
        Box,
        Line,
    }

    public class PrimitiveManager
    {
        private Dictionary<string, PrimitiveBatch> Batches = null;

        public PrimitiveManager()
        {
            Batches = new Dictionary<string, PrimitiveBatch>();
        }

        public IRenderer GetObject(int RefID)
        {
            IRenderer Result = null;
            foreach (PrimitiveBatch Batch in Batches.Values)
            {
                Result = Batch.GetObject(RefID);

                if (Result != null)
                {
                    return Result;
                }
            }

            return Result;
        }

        public bool RemoveObject(int RefID)
        {
            foreach (PrimitiveBatch Batch in Batches.Values)
            {
                return Batch.RemoveObject(RefID);
            }

            return false;
        }

        public void AddPrimitiveBatch(PrimitiveBatch Batch)
        {
            if (!Batches.ContainsKey(Batch.BatchID))
            {
                Batches.Add(Batch.BatchID, Batch);
            }
        }

        // TODO: Return boolean here?
        public void RemovePrimitiveBatch(PrimitiveBatch Batch)
        {
            RemovePrimitiveBatch(Batch.BatchID);
        }

        // TODO: Return boolean here?
        public void RemovePrimitiveBatch(string BatchID)
        {
            if (Batches.ContainsKey(BatchID))
            {
                Batches.Remove(BatchID);
            }
        }

        public void RenderPrimitives(ID3D11Device InDevice, ID3D11DeviceContext InDeviceContext, Camera InCamera)
        {
            foreach (PrimitiveBatch Batch in Batches.Values)
            {
                Batch.RenderBatch(InDevice, InDeviceContext, InCamera);
            }
        }

        public void Shutdown()
        {
            foreach (PrimitiveBatch Batch in Batches.Values)
            {
                Batch.Shutdown();
            }

            Batches = null;
        }
    }
}
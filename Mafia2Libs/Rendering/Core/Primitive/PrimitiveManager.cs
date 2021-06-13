﻿using Rendering.Graphics;
using SharpDX.Direct3D11;
using System.Collections.Generic;

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
            if(!Batches.ContainsKey(Batch.BatchID))
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

        public void RenderPrimitives(Device InDevice, DeviceContext InDeviceContext, Camera InCamera)
        {
            foreach(PrimitiveBatch Batch in Batches.Values)
            {
                Batch.RenderBatch(InDevice, InDeviceContext, InCamera);
            }
        }

        public void Shutdown()
        {
            foreach(PrimitiveBatch Batch in Batches.Values)
            {
                Batch.Shutdown();
            }

            Batches = null;
        }
    }
}
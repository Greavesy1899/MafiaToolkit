// SPDX-License-Identifier: LicenseRef-PolyForm-Strict-1.0.0
// Copyright (c) 2018-2026 Greavesy

using Rendering.Graphics;
using Vortice.Direct3D11;
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

        public void RemoveAndShutdownPrimitiveBatch(PrimitiveBatch Batch)
        {
            RemoveAndShutdownPrimitiveBatch(Batch.BatchID);
        }

        public void RemoveAndShutdownPrimitiveBatch(string BatchID)
        {
            PrimitiveBatch Batch = null;
            if (Batches.TryGetValue(BatchID, out Batch))
            {
                Batches.Remove(BatchID);
                Batch.Shutdown();
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
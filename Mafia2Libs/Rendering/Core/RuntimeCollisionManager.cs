using Rendering.Graphics;
using ResourceTypes.Collisions;
using System.Collections.Generic;
using System.Numerics;
using Toolkit.Core;
using Utils.ContainerTypes;
using Utils.Logging;

namespace Rendering.Core
{
    /***
     * A simplified entry to store in the dictionary, allows us to link
     * a reference with a Transform. (and possibly extend later..)
     */
    public class RenderInstanceEntry
    {
        public int RefID { get; private set; }
        public Matrix4x4 Transform { get; private set; }
        public bool bIsVisible { get; set; }

        public RenderInstanceEntry()
        {
            RefID = -1;
            Transform = Matrix4x4.Identity;
            bIsVisible = true;
        }

        public void ConstructEntry(Matrix4x4 InTransform)
        {
            RefID = RefManager.GetNewRefID();
            Transform = InTransform;
        }
    }

    /***
     * Rendering manager for objects which act as collisions ingame.
     * This includes static collisions and ItemDesc collisions.
     */
    public class RuntimeCollisionManager
    {
        private GraphicsClass CachedGraphics;
        private DirectX11Class D3D;

        private Dictionary<ulong, RenderStaticCollision> StaticCollisionCache;
        private MultiMap<ulong, RenderInstanceEntry> StaticCollisionInstanceCache;
        private Dictionary<int, ulong> InstanceRefToCollisionHashLookup;

        public RuntimeCollisionManager(GraphicsClass InGraphics, DirectX11Class InD3D)
        {
            CachedGraphics = InGraphics;
            D3D = InD3D;

            StaticCollisionCache = new Dictionary<ulong, RenderStaticCollision>();
            StaticCollisionInstanceCache = new MultiMap<ulong, RenderInstanceEntry>();
            InstanceRefToCollisionHashLookup = new Dictionary<int, ulong>();
        }

        public void RequestStaticCollision(Collision.CollisionModel Model)
        {
            if (!StaticCollisionCache.ContainsKey(Model.Hash))
            {
                // Create Static Collision
                RenderStaticCollision StaticCollision = new RenderStaticCollision();
                StaticCollision.ConvertCollisionToRender(Model.Hash, Model.Mesh);
                StaticCollision.InitBuffers(D3D.Device, D3D.DeviceContext);

                // Add to cache
                StaticCollisionCache.Add(Model.Hash, StaticCollision);
            }
        }

        public int RequestStaticInstance(Collision.Placement CollisionPlacement)
        {
            ToolkitAssert.Ensure(StaticCollisionCache.ContainsKey(CollisionPlacement.Hash), "The StaticCollision object must already exist before requesting a Collision Instance");

            // Construct the instance
            RenderInstanceEntry NewEntry = new RenderInstanceEntry();
            NewEntry.ConstructEntry(CollisionPlacement.Transform);

            // Push into instance dictionaries so we can reference later.
            StaticCollisionInstanceCache.Add(CollisionPlacement.Hash, NewEntry);
            InstanceRefToCollisionHashLookup.Add(NewEntry.RefID, CollisionPlacement.Hash);

            return NewEntry.RefID;
        }

        public void RemoveStaticCollision(ulong CollisionHash)
        {
            if(StaticCollisionCache.ContainsKey(CollisionHash))
            {
                // Iterate, and remove all RefIDs found in this list of instances
                List<RenderInstanceEntry> Entries = StaticCollisionInstanceCache[CollisionHash];
                foreach (RenderInstanceEntry Entry in Entries)
                {
                    InstanceRefToCollisionHashLookup.Remove(Entry.RefID);
                }

                // Clear all the instances, then the Collision itself.
                StaticCollisionInstanceCache.Remove(CollisionHash);
                StaticCollisionCache.Remove(CollisionHash);
            }
        }

        public void RemoveStaticInstance(int RefID)
        {
            RenderInstanceEntry InstanceEntry = GetInstanceEntry(RefID);
            if(InstanceEntry != null)
            {
                // Get CollisionHash, then try to find the entry with the same RefID.
                ulong CollisionHash = InstanceRefToCollisionHashLookup[RefID];
                bool bHasRemoved = StaticCollisionInstanceCache[CollisionHash].Remove(InstanceEntry);
                ToolkitAssert.Ensure(bHasRemoved == true, "We should have removed this Static Collision Instance!");

                // Remove from lookup dictionary
                InstanceRefToCollisionHashLookup.Remove(RefID);
            }
        }

        public void RenderStaticInstances()
        {
            // TODO: I want to improve this, render inside this function.
            // Sort out shader/editor parameters then iterate, render instances.
            // That will be the cheapest way to deal with static collisions.
            // .... or use Dx11 instancing and that would be even better.
            foreach (KeyValuePair<ulong, List<RenderInstanceEntry>> EntryList in StaticCollisionInstanceCache)
            {
                RenderStaticCollision StaticCol = StaticCollisionCache[EntryList.Key];
                foreach(RenderInstanceEntry Entry in EntryList.Value)
                {
                    if (Entry.bIsVisible)
                    {
                        StaticCol.SetTransform(Entry.Transform);
                        StaticCol.Render(D3D, CachedGraphics.Camera);
                    }
                }
            }
        }

        public void SetVisibilityOfStaticCollisionInstance(int RefID, bool bIsVisible)
        {
            // TODO: Move instance into an 'inactive' pool.
            // This will likely need to be done when we deal with instancing.

            // Get the Instance, then set new visibility.
            RenderInstanceEntry InstanceEntry = GetInstanceEntry(RefID);
            if(InstanceEntry != null)
            {
                InstanceEntry.bIsVisible = bIsVisible;
            }
        }

        private RenderInstanceEntry GetInstanceEntry(int RefID)
        {
            if (InstanceRefToCollisionHashLookup.ContainsKey(RefID))
            {
                // Get CollisionHash, then try to find the entry with the same RefID.
                ulong CollisionHash = InstanceRefToCollisionHashLookup[RefID];
                return StaticCollisionInstanceCache[CollisionHash].Find(e => e.RefID == RefID);
            }

            return null;
        }

        public void Shutdown()
        {
            foreach(RenderStaticCollision Entry in StaticCollisionCache.Values)
            {
                Entry.Shutdown();
            }

            StaticCollisionCache.Clear();
            StaticCollisionInstanceCache.Clear();
            InstanceRefToCollisionHashLookup.Clear();
        }
    }
}

using System.Collections.Generic;
using Rendering.Factories;
using Rendering.Graphics;
using ResourceTypes.Actors;
using System.IO;
using System.Numerics;
using Utils.Types;
using Vortice.Mathematics;

namespace ResourceTypes.FrameResource
{
    public class FrameObjectFrame : FrameObjectJoint
    {
        HashName actorHash;
        ActorEntry item;

        public HashName ActorHash {
            get { return actorHash; }
            set { actorHash = value; }
        }
        public ActorEntry Item {
            get { return item; }
            set { item = value; }
        }

        public FrameObjectFrame(FrameObjectFrame other) : base(other)
        {
            actorHash = other.actorHash;
        }

        public FrameObjectFrame(FrameResource OwningResource) : base(OwningResource)
        {
            actorHash = new HashName();
        }

        public override void ReadFromFile(MemoryStream reader, bool isBigEndian)
        {
            base.ReadFromFile(reader, isBigEndian);
            actorHash = new HashName(reader, isBigEndian);
        }

        public override void WriteToFile(BinaryWriter writer)
        {
            // NB: We do not want to save the local transform of an actor into the frame resource.
            // This is because the original Mafia II pipeline removed the transform from linked actors,
            // so we will do the same to avoid any chance of discrepencies which may appear.
            Matrix4x4 OldTransform = LocalTransform;
            LocalTransform = Matrix4x4.Identity;

            base.WriteToFile(writer);
            actorHash.WriteToFile(writer);

            // Now revert back to the original transform
            LocalTransform = OldTransform;
        }

        public override string ToString()
        {
            return string.Format("{0}", Name);
        }

        public override void ConstructRenderable(Dictionary<int, IRenderer> assets)
        {
            BoundingBox TempBox = new BoundingBox(new Vector3(0.5f), new Vector3(0.5f));
            RenderBoundingBox Renderable = RenderableFactory.BuildBoundingBox(TempBox, WorldTransform);
            RenderAdapter = new Rendering.Core.RenderableAdapter();
            RenderAdapter.InitAdaptor(Renderable, this);
        }
    }
}

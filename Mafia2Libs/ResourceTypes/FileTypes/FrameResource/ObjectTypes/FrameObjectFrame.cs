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

        public FrameObjectFrame(MemoryStream reader, bool isBigEndian) : base()
        {
            ReadFromFile(reader, isBigEndian);
        }

        public FrameObjectFrame(FrameObjectFrame other) : base(other)
        {
            actorHash = other.actorHash;
        }

        public FrameObjectFrame() : base()
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
            base.WriteToFile(writer);
            actorHash.WriteToFile(writer);
        }

        protected override void SanitizeOnSave()
        {
            base.SanitizeOnSave();

            if(ActorHash.Hash != 0)
            {
                LocalTransform = Matrix4x4.Identity;
            }
        }

        public override string ToString()
        {
            return string.Format("{0}", Name);
        }

        public override void ConstructRenderable()
        {
            BoundingBox TempBox = new BoundingBox(new Vector3(0.5f), new Vector3(0.5f));
            RenderBoundingBox Renderable = RenderableFactory.BuildBoundingBox(TempBox, WorldTransform);
            RenderAdapter = new Rendering.Core.RenderableAdapter();
            RenderAdapter.InitAdaptor(Renderable, this);
        }
    }
}

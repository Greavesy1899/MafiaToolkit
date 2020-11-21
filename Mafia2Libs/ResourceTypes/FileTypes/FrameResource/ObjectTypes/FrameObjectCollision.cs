using Rendering.Factories;
using Rendering.Graphics;
using System.IO;
using Utils.Extensions;

namespace ResourceTypes.FrameResource
{
    public class FrameObjectCollision : FrameObjectBase
    {

        ulong hash;

        public ulong Hash {
            get { return hash; }
            set { hash = value; }
        }

        public FrameObjectCollision(MemoryStream reader, bool isBigEndian) : base()
        {
            ReadFromFile(reader, isBigEndian);
        }

        public FrameObjectCollision() : base()
        {
            hash = 0;
        }

        public FrameObjectCollision(FrameObjectCollision other) : base(other)
        {
            hash = other.hash;
        }

        public override void ReadFromFile(MemoryStream reader, bool isBigEndian)
        {
            base.ReadFromFile(reader, isBigEndian);
            hash = reader.ReadUInt64(isBigEndian);
        }

        public override void WriteToFile(BinaryWriter writer)
        {
            base.WriteToFile(writer);
            writer.Write(hash);
        }

        public override string ToString()
        {
            return string.Format("{0}", Name);
        }

        public override void ConstructRenderable()
        {
            // We don't want to use this code yet, it's from old-old stuff which needs to be looked at.
            // TODO: Look at bringing this old feature back.
            return;
            RenderStaticCollision CollisionMesh = RenderableFactory.BuildRenderItemDesc(Hash);
            RenderAdapter = new Rendering.Core.RenderableAdapter();
            RenderAdapter.InitAdaptor(CollisionMesh, this);
        }
    }
}
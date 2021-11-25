using Mafia2Tool;
using Rendering.Factories;
using Rendering.Graphics;
using ResourceTypes.ItemDesc;
using System.IO;
using Utils.Extensions;

namespace ResourceTypes.FrameResource
{
    public class FrameObjectCollision : FrameObjectBase
    {
        private ulong _Hash;
        private ItemDescLoader ItemDesc;

        public ulong Hash {
            get { return _Hash; }
            set {
                _Hash = value;
                GetUsedItemDesc();
            }
        }

        public string ItemDescFileName {
            get {
                return ItemDesc?.FileName;
            }
        }

        public FrameObjectCollision(MemoryStream reader, bool isBigEndian) : base()
        {
            ReadFromFile(reader, isBigEndian);
        }

        public FrameObjectCollision() : base()
        {
            _Hash = 0;
            ItemDesc = null;
        }

        public FrameObjectCollision(FrameObjectCollision other) : base(other)
        {
            _Hash = other._Hash;
            ItemDesc = other.ItemDesc;
        }

        public override void ReadFromFile(MemoryStream reader, bool isBigEndian)
        {
            base.ReadFromFile(reader, isBigEndian);
            _Hash = reader.ReadUInt64(isBigEndian);
        }

        public override void WriteToFile(BinaryWriter writer)
        {
            base.WriteToFile(writer);
            writer.Write(_Hash);
        }

        public override void ConstructRenderable()
        {
            GetUsedItemDesc();

            // We don't want to use this code yet, it's from old-old stuff which needs to be looked at.
            // TODO: Look at bringing this old feature back.
            return;
            //RenderStaticCollision CollisionMesh = RenderableFactory.BuildRenderItemDesc(Hash);
            //RenderAdapter = new Rendering.Core.RenderableAdapter();
            //RenderAdapter.InitAdaptor(CollisionMesh, this);
        }

        // TODO: Move this to a different location.
        // It would be better if this didn't access SceneData.
        public void GetUsedItemDesc()
        {
            foreach(ItemDescLoader ItemDesc in SceneData.ItemDescs)
            {
                if(ItemDesc.frameRef == _Hash)
                {
                    this.ItemDesc = ItemDesc;
                    break;
                }
            }
        }

        public override string ToString()
        {
            return string.Format("{0}", Name);
        }
    }
}
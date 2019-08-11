using System.IO;
using ResourceTypes.Actors;
using Utils.Types;

namespace ResourceTypes.FrameResource
{
    public class FrameObjectFrame : FrameObjectJoint
    {
        Hash actorHash;
        Actor.ActorItem item;

        public Hash ActorHash {
            get { return actorHash; }
            set { actorHash = value; }
        }
        public Actor.ActorItem Item {
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
            actorHash = new Hash();
        }

        public override void ReadFromFile(MemoryStream reader, bool isBigEndian)
        {
            base.ReadFromFile(reader, isBigEndian);
            actorHash = new Hash(reader, isBigEndian);
        }

        public override void WriteToFile(BinaryWriter writer)
        {
            base.WriteToFile(writer);
            actorHash.WriteToFile(writer);
        }

        public override string ToString()
        {
            return string.Format("{0}", Name);
        }
    }
}

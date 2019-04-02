using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using Mafia2;
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

        public FrameObjectFrame(BinaryReader reader) : base()
        {
            ReadFromFile(reader);
        }

        public FrameObjectFrame(FrameObjectFrame other) : base(other)
        {
            actorHash = other.actorHash;
        }

        public FrameObjectFrame() : base()
        {
            actorHash = new Hash();
        }

        public override void ReadFromFile(BinaryReader reader)
        {
            base.ReadFromFile(reader);
            actorHash = new Hash(reader);
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

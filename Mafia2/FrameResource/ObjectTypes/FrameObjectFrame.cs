using System.IO;

namespace Mafia2
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

        public override void ReadFromFile(BinaryReader reader)
        {
            base.ReadFromFile(reader);
            actorHash = new Hash(reader);
        }

        public override string ToString()
        {
            return string.Format("{0}", Name);
        }
    }
}

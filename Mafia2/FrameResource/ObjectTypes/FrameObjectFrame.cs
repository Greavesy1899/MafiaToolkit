using System.ComponentModel;
using System.Diagnostics;
using System.IO;

namespace Mafia2
{
    public class FrameObjectFrame : FrameObjectJoint
    {
        Hash actorHash;
        Actor.ActorItem item;
        NameTableFlags nameTableFlags;

        public Hash ActorHash {
            get { return actorHash; }
            set { actorHash = value; }
        }
        public Actor.ActorItem Item {
            get { return item; }
            set { item = value; }
        }

        [Description("Only use this if the object is going to be saved in the FrameNameTable")]
        public NameTableFlags FrameNameTableFlags {
            get { return nameTableFlags; }
            set { nameTableFlags = value; }
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

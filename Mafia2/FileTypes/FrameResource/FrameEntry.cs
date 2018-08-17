using System.Collections.Generic;

namespace Mafia2
{
    public class FrameEntry
    {
        //All frame entries have their own ID, this is used so we can link the entries with each other instead of using Indexes, like Mafia II uses.
        protected int refID;
        protected Dictionary<string, int> refs = new Dictionary<string, int>();

        public int RefID {
            get { return refID; }
        }
        public Dictionary<string, int> Refs {
            get { return refs; }
        }

        /// <summary>
        /// Constructor randomly generates their ID.
        /// </summary>
        public FrameEntry()
        {
            refID = Functions.RandomGenerator.Next();
        }

        /// <summary>
        /// Add reference to this object.
        /// </summary>
        /// <param name="objRef"></param>
        public void AddRef(FrameEntryRefTypes type, int objRef)
        {
            switch (type)
            {
                case FrameEntryRefTypes.Mesh:
                    refs.Add("Mesh", objRef);
                    break;
                case FrameEntryRefTypes.Material:
                    refs.Add("Material", objRef);
                    break;
                case FrameEntryRefTypes.BlendInfo:
                    refs.Add("BlendInfo", objRef);
                    break;
                case FrameEntryRefTypes.Skeleton:
                    refs.Add("Skeleton", objRef);
                    break;
                case FrameEntryRefTypes.SkeletonHierachy:
                    refs.Add("SkeletonHierachy", objRef);
                    break;
                default:
                    refs.Add("UnknownType" + objRef, objRef);
                    break;
            }
        }

        /// <summary>
        /// Remove reference from this object.
        /// </summary>
        /// <param name="objRef"></param>
        public void SubRef(int objRef)
        {
            foreach (KeyValuePair<string, int> entry in refs)
            {
                if (entry.Value == objRef)
                    refs.Remove(entry.Key);
            }
        }
    }
}

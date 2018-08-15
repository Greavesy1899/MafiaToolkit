using System.Collections.Generic;

namespace Mafia2
{
    public class FrameEntry
    {
        //All frame entries have their own ID, this is used so we can link the entries with each other instead of using Indexes, like Mafia II uses.
        protected int refID;
        protected List<int> refs = new List<int>();

        public int RefID {
            get { return refID; }
        }
        public List<int> Refs {
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
        public void AddRef(int objRef)
        {
            refs.Add(objRef);
        }

        /// <summary>
        /// Remove reference from this object.
        /// </summary>
        /// <param name="objRef"></param>
        public void SubRef(int objRef)
        {
            foreach (int num in refs)
            {
                if (objRef == num)
                    refs.Remove(num);
            }
        }
    }
}

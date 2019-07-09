using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Utils.StringHelpers;

namespace ResourceTypes.FrameResource
{
    public enum FrameEntryRefTypes
    {
        Mesh,
        Material,
        BlendInfo,
        Skeleton,
        SkeletonHierachy,
        Parent1,
        Parent2
    }

    public class FrameEntry
    {
        //All frame entries have their own ID, this is used so we can link the entries with each other instead of using Indexes, like Mafia II uses.
        protected int refID;
        protected Dictionary<string, int> refs = new Dictionary<string, int>();

        public const string MeshRef = "Mesh";
        public const string MaterialRef = "Material";
        public const string BlendInfoRef = "BlendInfo";
        public const string SkeletonRef = "Skeleton";
        public const string SkeletonHierRef = "SkeletonHierachy";
        public const string Parent1Ref = "Parent1";
        public const string Parent2Ref = "Parent2";

        [ReadOnly(true)]
        public int RefID {
            set { refID = value; }
            get { return refID; }
        }
        [ReadOnly(true)]
        public Dictionary<string, int> Refs {
            get { return refs; }
        }

        /// <summary>
        /// Constructor randomly generates their ID.
        /// </summary>
        public FrameEntry()
        {
            refID = StringHelpers.RandomGenerator.Next();
        }

        public FrameEntry(FrameEntry entry)
        {
            refID = StringHelpers.RandomGenerator.Next();
            refs = new Dictionary<string, int>();

            for(int i = 0; i != entry.refs.Count; i++)
            {
                refs.Add(entry.refs.ElementAt(i).Key, entry.refs.ElementAt(i).Value);
            }
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
                    refs.Add(MeshRef, objRef);
                    break;
                case FrameEntryRefTypes.Material:
                    refs.Add(MaterialRef, objRef);
                    break;
                case FrameEntryRefTypes.BlendInfo:
                    refs.Add(BlendInfoRef, objRef);
                    break;
                case FrameEntryRefTypes.Skeleton:
                    refs.Add(SkeletonRef, objRef);
                    break;
                case FrameEntryRefTypes.SkeletonHierachy:
                    refs.Add(SkeletonHierRef, objRef);
                    break;
                case FrameEntryRefTypes.Parent1:
                    refs.Add(Parent1Ref, objRef);
                    break;
                case FrameEntryRefTypes.Parent2:
                    refs.Add(Parent2Ref, objRef);
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

        /// <summary>
        /// Remove reference from this object using the type of ref.
        /// </summary>
        /// <param name="objRef"></param>
        public void SubRef(FrameEntryRefTypes refType)
        {
            switch (refType)
            {
                case FrameEntryRefTypes.Mesh:
                    removeRef(MeshRef);
                    break;
                case FrameEntryRefTypes.Material:
                    removeRef(MaterialRef);
                    break;
                case FrameEntryRefTypes.BlendInfo:
                    removeRef(BlendInfoRef);
                    break;
                case FrameEntryRefTypes.Skeleton:
                    removeRef(SkeletonRef);
                    break;
                case FrameEntryRefTypes.SkeletonHierachy:
                    removeRef(SkeletonHierRef);
                    break;
                case FrameEntryRefTypes.Parent1:
                    removeRef(Parent1Ref);
                    break;
                case FrameEntryRefTypes.Parent2:
                    removeRef(Parent2Ref);
                    break;
                default:
                    Console.WriteLine("Unknown type: " + refType);
                    break;
            }
        }

        private void removeRef(string refName)
        {
            if(refs.ContainsKey(refName))
                refs.Remove(refName);
        }

        public override string ToString()
        {
            return "Frame Entry";
        }
    }
}

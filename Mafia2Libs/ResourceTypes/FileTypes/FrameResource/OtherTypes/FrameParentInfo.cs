using System.ComponentModel;

namespace ResourceTypes.FrameResource
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class ParentInfo
    {
        int index;
        string name;
        int refID;

        public int Index {
            get { return index; }
            set { index = value; }
        }

        public string Name {
            get { return name; }
            set { name = value; }
        }

        public int RefID {
            get { return refID; }
            set { refID = value; }
        }

        public enum ParentType
        {
            ParentIndex1,
            ParentIndex2
        }

        public ParentInfo(int index)
        {
            this.index = index;
        }

        public ParentInfo(ParentInfo other)
        {
            index = other.index;
            name = other.name;
            refID = other.refID;
        }

        public void SetParent(FrameEntry ParentEntry, int IndexOfParent)
        {
            SetParent(IndexOfParent, ParentEntry.ToString(), ParentEntry.RefID);
        }

        public void SetParent(int index, string name, int refID)
        {
            this.index = index;
            this.name = name;
            this.refID = refID;
        }

        public void RemoveParent()
        {
            index = -1;
            name = "root";
            refID = 0;
        }

        public override string ToString()
        {
            if (index == -1)
            {
                return string.Format("{0}, root", index);
            }

            return string.Format("{0}, {1}", index, name);
        }
    }
}
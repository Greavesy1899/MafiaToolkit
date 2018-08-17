namespace Mafia2
{
    public class ParentStruct
    {
        int index;
        string name;
        int refID;

        public int Index
        {
            get { return index; }
            set { index = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int RefID {
            get { return refID; }
            set { refID = value; }
        }

        public ParentStruct(int index)
        {
            this.index = index;
        }

        public override string ToString()
        {
            if (index == -1)
                return string.Format("{0}, root", index);
            else
                return string.Format("{0}. {1}", index, name);
        }
    }
}

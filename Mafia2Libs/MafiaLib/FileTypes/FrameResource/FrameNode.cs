using Mafia2;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ResourceTypes.FrameResource
{
    public class FrameNode
    {
        public FrameNode Parent1 { get; private set; }
        public FrameNode Parent2 { get; private set; }
        public Dictionary<int, FrameNode> Children { get; private set; }
        public object Object { get; private set; }

        public FrameNode(object obj)
        {
            Object = obj;
            Children = new Dictionary<int, FrameNode>();
        }

        public bool AddChild(FrameNode childNode, int idx)
        {
            if (Children.ContainsKey(idx) || childNode == null)
                return false;

           childNode.SetParent1(this);
           Children.Add(idx, childNode);
            return true;
        }

        public void SetParent1(FrameNode parent)
        {
            Parent1 = parent;
        }

        public void SetParent2(FrameNode parent)
        {
            Parent2 = parent;
        }

        public void DumpNode()
        {
            Console.WriteLine(Object.ToString());
            Console.WriteLine("Children: " + Children.Count);
            foreach(KeyValuePair<int, FrameNode> child in Children)
            {
                Console.WriteLine(child.Value.ToString());
                child.Value.DumpNode();
            }
        }

        public void ConvertToTreeNode(ref TreeNode treeNode)
        {
            TreeNode thisNode = new TreeNode(Object.ToString());
            thisNode.Tag = Object;

            foreach (KeyValuePair<int, FrameNode> child in Children)
            {
                child.Value.ConvertToTreeNode(ref thisNode);
            }
            treeNode.Nodes.Add(thisNode);
        }

        public override string ToString()
        {
            string parent = (Parent1 == null) ? "null" : Parent1.Object.ToString();
            return string.Format("{0}, {1}, {2}", Object.ToString(), parent, Children.Count);
        }
    }
}

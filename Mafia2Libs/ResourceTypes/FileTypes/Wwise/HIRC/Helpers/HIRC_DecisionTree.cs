using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;
using System.ComponentModel;

namespace ResourceTypes.Wwise.Helpers
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class DecisionTree
    {
        public uint Key { get; set; }
        public int NodeID { get; set; }
        public int Weight { get; set; }
        public int Probability { get; set; }
        public List<DecisionTreeNode> Nodes { get; set; }
        public int isSingle { get; set; }

        public DecisionTree(BinaryReader br, uint treeDataSize)
        {
            if (treeDataSize != 12)
            {
                isSingle = 0;
                Nodes = new List<DecisionTreeNode>();
                Key = br.ReadUInt32();
                NodeID = br.ReadUInt16();
                int nodeCount = br.ReadUInt16();
                Weight = br.ReadUInt16();
                Probability = br.ReadUInt16();

                for (int i = 0; i < nodeCount; i++)
                {
                    Nodes.Add(new DecisionTreeNode(br));
                }
            }
            else
            {
                isSingle = 1;
                Nodes = new List<DecisionTreeNode>();
                Nodes.Add(new DecisionTreeNode(br));
            }
        }

        public DecisionTree()
        {
            isSingle = 0;
            Nodes = new List<DecisionTreeNode>();
            Key = 0;
            NodeID = 0;
            Weight = 0;
            Probability = 0;
        }

        public void WriteToFile(BinaryWriter bw)
        {
            if (isSingle == 0)
            {
                bw.Write(Key);
                bw.Write((short)NodeID);
                bw.Write((short)Nodes.Count);
                bw.Write((short)Weight);
                bw.Write((short)Probability);

                foreach (DecisionTreeNode node in Nodes)
                {
                    bw.Write(node.Key);
                    bw.Write(node.audioNodeID);
                    bw.Write((short)node.Weight);
                    bw.Write((short)node.Probability);
                }
            }
            else
            {
                foreach (DecisionTreeNode node in Nodes)
                {
                    bw.Write(node.Key);
                    bw.Write(node.audioNodeID);
                    bw.Write((short)node.Weight);
                    bw.Write((short)node.Probability);
                }
            }
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class DecisionTreeNode
    {
        public uint Key { get; set; }
        public uint audioNodeID { get; set; }
        public int Weight { get; set; }
        public int Probability { get; set; }
        public DecisionTreeNode(BinaryReader br)
        {
            Key = br.ReadUInt32();
            audioNodeID = br.ReadUInt32();
            Weight = br.ReadUInt16();
            Probability = br.ReadUInt16();
        }

        public DecisionTreeNode()
        {
            Key = 0;
            audioNodeID = 0;
            Weight = 0;
            Probability = 0;
        }
    }
}

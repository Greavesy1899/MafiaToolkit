using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;

namespace ResourceTypes.Wwise.Helpers
{
    public class DecisionTree
    {
        public uint key { get; set; }
        public int nodeId { get; set; }
        public int weight { get; set; }
        public int probability { get; set; }
        public List<DecisionTreeNode> nodes { get; set; }
        public int isSingle { get; set; }

        public DecisionTree(BinaryReader br, uint treeDataSize)
        {
            if (treeDataSize != 12)
            {
                isSingle = 0;
                nodes = new List<DecisionTreeNode>();
                key = br.ReadUInt32();
                nodeId = br.ReadUInt16();
                int nodeCount = br.ReadUInt16();
                weight = br.ReadUInt16();
                probability = br.ReadUInt16();

                for (int i = 0; i < nodeCount; i++)
                {
                    nodes.Add(new DecisionTreeNode(br));
                }
            }
            else
            {
                isSingle = 1;
                nodes = new List<DecisionTreeNode>();
                nodes.Add(new DecisionTreeNode(br));
            }
        }

        public DecisionTree()
        {
            isSingle = 0;
            nodes = new List<DecisionTreeNode>();
            key = 0;
            nodeId = 0;
            weight = 0;
            probability = 0;
        }

        public void WriteToFile(BinaryWriter bw)
        {
            if (isSingle == 0)
            {
                bw.Write(key);
                bw.Write((short)nodeId);
                bw.Write((short)nodes.Count);
                bw.Write((short)weight);
                bw.Write((short)probability);

                foreach (DecisionTreeNode node in nodes)
                {
                    bw.Write(node.key);
                    bw.Write(node.audioNodeId);
                    bw.Write((short)node.weight);
                    bw.Write((short)node.probability);
                }
            }
            else
            {
                foreach (DecisionTreeNode node in nodes)
                {
                    bw.Write(node.key);
                    bw.Write(node.audioNodeId);
                    bw.Write((short)node.weight);
                    bw.Write((short)node.probability);
                }
            }
        }
    }

    public class DecisionTreeNode
    {
        public uint key { get; set; }
        public uint audioNodeId { get; set; }
        public int weight { get; set; }
        public int probability { get; set; }
        public DecisionTreeNode(BinaryReader br)
        {
            key = br.ReadUInt32();
            audioNodeId = br.ReadUInt32();
            weight = br.ReadUInt16();
            probability = br.ReadUInt16();
        }

        public DecisionTreeNode()
        {
            key = 0;
            audioNodeId = 0;
            weight = 0;
            probability = 0;
        }
    }
}

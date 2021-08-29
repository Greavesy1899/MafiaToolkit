using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Windows;
using System.Collections.Generic;
using ResourceTypes.Wwise.Helpers;
using ResourceTypes.Wwise;

namespace ResourceTypes.Wwise.Objects
{
    public class MusicSwitchContainer
    {
        [System.ComponentModel.Browsable(false)]
        public int type { get; set; }
        public uint id { get; set; }
        public byte musicFlags { get; set; } //bit1 = "bOverrideParentMidiTempo", bit2 = "bOverrideParentMidiTarget", bit3 = "bMidiTargetTypeBus" 
        public NodeBase nodeBase { get; set; }
        public List<uint> childIDs { get; set; } //IDs of child HIRC objects
        public double akMeterGridPeriod { get; set; }
        public double akMeterGridOffset { get; set; }
        public float akMeterTempo { get; set; }
        public byte akMeterTimeSigNumBeatsBar { get; set; }
        public byte akMeterTimeSigBeatValue { get; set; }
        public byte akMeterInfoFlag { get; set; }
        public List<byte[]> stingers { get; set; }
        public List<TransitionRule> rules { get; set; }
        public int continuePlayback { get; set; }
        public List<ArgumentGroup> argumentGroups { get; set; }
        public byte mode { get; set; } //0x00 = BestMatch
        public List<DecisionTree> decisionTrees { get; set; }
        public MusicSwitchContainer(HIRCObject parentObject, BinaryReader br, int iType)
        {
            type = iType;
            uint length = br.ReadUInt32();
            id = br.ReadUInt32();
            musicFlags = br.ReadByte();
            nodeBase = new NodeBase(br, parentObject);
            childIDs = new List<uint>();
            uint numChilds = br.ReadUInt32();

            for (int i = 0; i < numChilds; i++)
            {
                uint key = br.ReadUInt32();
                childIDs.Add(key);
            }

            akMeterGridPeriod = br.ReadDouble();
            akMeterGridOffset = br.ReadDouble();
            akMeterTempo = br.ReadSingle();
            akMeterTimeSigNumBeatsBar = br.ReadByte();
            akMeterTimeSigBeatValue = br.ReadByte();
            akMeterInfoFlag = br.ReadByte();
            stingers = new List<byte[]>();
            uint numStingers = br.ReadUInt32();

            for (int i = 0; i < numStingers; i++)
            {
                stingers.Add(br.ReadBytes(24));
            }

            rules = new List<TransitionRule>();
            uint numRules = br.ReadUInt32();

            for (int i = 0; i < numRules; i++)
            {
                rules.Add(new TransitionRule(br, parentObject));
            }

            continuePlayback = br.ReadByte();
            uint treeDepth = br.ReadUInt32();
            argumentGroups = new List<ArgumentGroup>();

            for (int i = 0; i < treeDepth; i++)
            {
                uint id = br.ReadUInt32();
                int value = br.ReadByte();
                argumentGroups.Add(new ArgumentGroup(id, value));
            }

            decisionTrees = new List<DecisionTree>();
            uint treeDataSize = br.ReadUInt32(); //DecisionTree = 12, DecisionTreeNode = 12 --> treeDataSize = 12 * treeDepth + 12 * DecisionTreeNodeCount
            mode = br.ReadByte();

            long initOffset = br.BaseStream.Position;
            long endOffset = br.BaseStream.Position;

            while ((endOffset - initOffset) != treeDataSize)
            {
                decisionTrees.Add(new DecisionTree(br, treeDataSize));
                endOffset = br.BaseStream.Position;
            }
        }

        public MusicSwitchContainer(HIRCObject parentObject)
        {
            type = 0;
            id = 0;
            musicFlags = 0;
            nodeBase = new NodeBase(parentObject);
            childIDs = new List<uint>();
            akMeterGridPeriod = 0;
            akMeterGridOffset = 0;
            akMeterTempo = 0;
            akMeterTimeSigNumBeatsBar = 0;
            akMeterTimeSigBeatValue = 0;
            akMeterInfoFlag = 0;
            stingers = new List<byte[]>();
            rules = new List<TransitionRule>();
            continuePlayback = 0;
            argumentGroups = new List<ArgumentGroup>();
            mode = 0;
            decisionTrees = new List<DecisionTree>();
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write((byte)type);
            bw.Write(GetLength());
            bw.Write(id);
            bw.Write(musicFlags);

            nodeBase.WriteToFile(bw);

            bw.Write(childIDs.Count);

            foreach (uint value in childIDs)
            {
                bw.Write(value);
            }

            bw.Write(akMeterGridPeriod);
            bw.Write(akMeterGridOffset);
            bw.Write(akMeterTempo);
            bw.Write(akMeterTimeSigNumBeatsBar);
            bw.Write(akMeterTimeSigBeatValue);
            bw.Write(akMeterInfoFlag);
            bw.Write(stingers.Count);

            foreach (byte[] stinger in stingers)
            {
                bw.Write(stinger);
            }

            bw.Write(rules.Count);

            foreach (TransitionRule rule in rules)
            {
                rule.WriteToFile(bw);
            }

            bw.Write((byte)continuePlayback);
            bw.Write(argumentGroups.Count);

            foreach (ArgumentGroup group in argumentGroups)
            {
                group.WriteToFile(bw);
            }

            uint treeDataSize = 0;
            long treePos = bw.BaseStream.Position;
            bw.Write(treeDataSize);
            bw.Write(mode);

            foreach (DecisionTree tree in decisionTrees)
            {
                treeDataSize += (uint)tree.nodes.Count * 12;

                if (tree.isSingle == 0)
                {
                    treeDataSize += 12;
                }

                tree.WriteToFile(bw);
            }

            long lastPos = bw.BaseStream.Position;
            bw.Seek((int)treePos, SeekOrigin.Begin);
            bw.Write(treeDataSize);
            bw.Seek((int)lastPos, SeekOrigin.Begin);
        }

        public int GetLength()
        {
            int length = 50 + nodeBase.GetLength() + childIDs.Count * 4 + stingers.Count * 24 + argumentGroups.Count * 5;

            foreach (TransitionRule rule in rules)
            {
                length += rule.GetLength();
            }

            foreach (DecisionTree tree in decisionTrees)
            {
                length += tree.nodes.Count * 12;

                if (tree.isSingle == 0)
                {
                    length += 12;
                }
            }

            return length;
        }
    }
}

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
        public int Type { get; set; }
        public uint ID { get; set; }
        public byte MusicFlags { get; set; } //bit1 = "bOverrideParentMidiTempo", bit2 = "bOverrideParentMidiTarget", bit3 = "bMidiTargetTypeBus" 
        public NodeBase NodeBase { get; set; }
        public List<uint> ChildIDs { get; set; } //IDs of child HIRC objects
        public double AKMeterGridPeriod { get; set; }
        public double AKMeterGridOffset { get; set; }
        public float AKMeterTempo { get; set; }
        public byte AKMeterTimeSigNumBeatsBar { get; set; }
        public byte AKMeterTimeSigBeatValue { get; set; }
        public byte AKMeterInfoFlag { get; set; }
        public List<byte[]> Stingers { get; set; }
        public List<TransitionRule> Rules { get; set; }
        public int ContinuePlayback { get; set; }
        public List<ArgumentGroup> ArgumentGroups { get; set; }
        public byte Mode { get; set; } //0x00 = BestMatch
        public List<DecisionTree> DecisionTrees { get; set; }
        public MusicSwitchContainer(HIRCObject ParentObject, BinaryReader br, int iType)
        {
            Type = iType;
            uint Length = br.ReadUInt32();
            ID = br.ReadUInt32();
            MusicFlags = br.ReadByte();
            NodeBase = new NodeBase(br, ParentObject);
            ChildIDs = new List<uint>();
            uint numChilds = br.ReadUInt32();

            for (int i = 0; i < numChilds; i++)
            {
                uint Key = br.ReadUInt32();
                ChildIDs.Add(Key);
            }

            AKMeterGridPeriod = br.ReadDouble();
            AKMeterGridOffset = br.ReadDouble();
            AKMeterTempo = br.ReadSingle();
            AKMeterTimeSigNumBeatsBar = br.ReadByte();
            AKMeterTimeSigBeatValue = br.ReadByte();
            AKMeterInfoFlag = br.ReadByte();
            Stingers = new List<byte[]>();
            uint numStingers = br.ReadUInt32();

            for (int i = 0; i < numStingers; i++)
            {
                Stingers.Add(br.ReadBytes(24));
            }

            Rules = new List<TransitionRule>();
            uint numRules = br.ReadUInt32();

            for (int i = 0; i < numRules; i++)
            {
                Rules.Add(new TransitionRule(br, ParentObject));
            }

            ContinuePlayback = br.ReadByte();
            uint treeDepth = br.ReadUInt32();
            ArgumentGroups = new List<ArgumentGroup>();

            for (int i = 0; i < treeDepth; i++)
            {
                uint ID = br.ReadUInt32();
                int value = br.ReadByte();
                ArgumentGroups.Add(new ArgumentGroup(ID, value));
            }

            DecisionTrees = new List<DecisionTree>();
            uint treeDataSize = br.ReadUInt32(); //DecisionTree = 12, DecisionTreeNode = 12 --> treeDataSize = 12 * treeDepth + 12 * DecisionTreeNodeCount
            Mode = br.ReadByte();

            long initOffset = br.BaseStream.Position;
            long endOffset = br.BaseStream.Position;

            while ((endOffset - initOffset) != treeDataSize)
            {
                DecisionTrees.Add(new DecisionTree(br, treeDataSize));
                endOffset = br.BaseStream.Position;
            }
        }

        public MusicSwitchContainer(HIRCObject ParentObject)
        {
            Type = 0;
            ID = 0;
            MusicFlags = 0;
            NodeBase = new NodeBase(ParentObject);
            ChildIDs = new List<uint>();
            AKMeterGridPeriod = 0;
            AKMeterGridOffset = 0;
            AKMeterTempo = 0;
            AKMeterTimeSigNumBeatsBar = 0;
            AKMeterTimeSigBeatValue = 0;
            AKMeterInfoFlag = 0;
            Stingers = new List<byte[]>();
            Rules = new List<TransitionRule>();
            ContinuePlayback = 0;
            ArgumentGroups = new List<ArgumentGroup>();
            Mode = 0;
            DecisionTrees = new List<DecisionTree>();
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write((byte)Type);
            bw.Write(GetLength());
            bw.Write(ID);
            bw.Write(MusicFlags);

            NodeBase.WriteToFile(bw);

            bw.Write(ChildIDs.Count);

            foreach (uint value in ChildIDs)
            {
                bw.Write(value);
            }

            bw.Write(AKMeterGridPeriod);
            bw.Write(AKMeterGridOffset);
            bw.Write(AKMeterTempo);
            bw.Write(AKMeterTimeSigNumBeatsBar);
            bw.Write(AKMeterTimeSigBeatValue);
            bw.Write(AKMeterInfoFlag);
            bw.Write(Stingers.Count);

            foreach (byte[] stinger in Stingers)
            {
                bw.Write(stinger);
            }

            bw.Write(Rules.Count);

            foreach (TransitionRule rule in Rules)
            {
                rule.WriteToFile(bw);
            }

            bw.Write((byte)ContinuePlayback);
            bw.Write(ArgumentGroups.Count);

            foreach (ArgumentGroup Group in ArgumentGroups)
            {
                Group.WriteToFile(bw);
            }

            uint treeDataSize = 0;
            long treePos = bw.BaseStream.Position;
            bw.Write(treeDataSize);
            bw.Write(Mode);

            foreach (DecisionTree tree in DecisionTrees)
            {
                treeDataSize += (uint)tree.Nodes.Count * 12;

                if (tree.isSingle == 0)
                {
                    treeDataSize += 12;
                }

                tree.WriteToFile(bw);
            }

            long LastPos = bw.BaseStream.Position;
            bw.Seek((int)treePos, SeekOrigin.Begin);
            bw.Write(treeDataSize);
            bw.Seek((int)LastPos, SeekOrigin.Begin);
        }

        public int GetLength()
        {
            int Length = 50 + NodeBase.GetLength() + ChildIDs.Count * 4 + Stingers.Count * 24 + ArgumentGroups.Count * 5;

            foreach (TransitionRule rule in Rules)
            {
                Length += rule.GetLength();
            }

            foreach (DecisionTree tree in DecisionTrees)
            {
                Length += tree.Nodes.Count * 12;

                if (tree.isSingle == 0)
                {
                    Length += 12;
                }
            }

            return Length;
        }
    }
}

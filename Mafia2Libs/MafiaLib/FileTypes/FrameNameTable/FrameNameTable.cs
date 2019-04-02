using System.Collections.Generic;
using ResourceTypes.FrameResource;
using Mafia2;
using System.IO;
using System.Linq;
using Utils.StringHelpers;

namespace ResourceTypes.FrameNameTable
{
    public class FrameNameTable
    {
        int bufferSize;
        int dataSize;
        Data[] frameData;
        string fileName;
        Dictionary<int, string> names = new Dictionary<int, string>();
        string m_buffer = "";

        public Dictionary<int, string> Names 
            {
            get { return names; }
            set { names = value; }
        }
        public Data[] FrameData {
            get { return frameData; }
            set { frameData = value; }
        }
        public string FileName {
            get { return fileName; }
            set { fileName = value; }
        }


        ///<summary>
        /// Empty FrameNameTable; best to use when rebuilding the data.
        /// </summary>
        public FrameNameTable() { }

        /// <summary>
        /// Construct data by reading a "FrameNameTable"
        /// </summary>
        /// <param name="file">filepath</param>
        public FrameNameTable(string file)
        {
            fileName = file;
            using (BinaryReader reader = new BinaryReader(File.Open(file, FileMode.Open)))
            {
                ReadFromFile(reader);
            }
        }

        /// <summary>
        /// Builds the file from the FrameResource passed into this function.
        /// </summary>
        /// <param name="resource">the frame resource which the data is coming from.</param>
        public void BuildDataFromResource(FrameResource.FrameResource resource)
        {
            List<Data> tableData = new List<Data>();
            int[] scenePos;
            string[] sceneNames;

            if (resource.Header.IsScene)
            {
                scenePos = new int[resource.Header.NumFolderNames + 1];
                sceneNames = new string[resource.Header.NumFolderNames + 1];

                //add the actual scenes from the header, and then the <scenes> one.
                for (int i = 0; i != resource.Header.NumFolderNames; i++)
                {
                    m_buffer += resource.Header.SceneFolders[i].Name.String;
                    m_buffer += "\0";
                    scenePos[i] = m_buffer.Length - resource.Header.SceneFolders[i].Name.String.Length - 1;
                    sceneNames[i] = resource.Header.SceneFolders[i].Name.String;
                }

                string scene = "<scene>\0";
                m_buffer += scene;
                scenePos[scenePos.Length - 1] = m_buffer.Length - scene.Length;
                sceneNames[sceneNames.Length - 1] = scene;
            }
            else
            {
                scenePos = new int[1];
                sceneNames = new string[1];
                m_buffer += "<scene>\0";
                scenePos[0] = 0;
                sceneNames[0] = "<scene>\0";
            }

            for (int i = 0; i != resource.FrameObjects.Count; i++)
            {
                bool addToTable = false;
                object block = resource.FrameObjects.ElementAt(i).Value;

                //possible types to save? might change in the future however.
                if (block.GetType().BaseType == typeof(FrameObjectBase) || block.GetType().BaseType == typeof(FrameObjectJoint) || block.GetType().BaseType == typeof(FrameObjectSingleMesh))
                {
                    if ((block as FrameObjectBase).IsOnFrameTable)
                    {
                        addToTable = true;
                    }
                }

                if (addToTable)
                {
                    FrameObjectBase fBase = (block as FrameObjectBase);

                    if (fBase.ParentIndex1.Index == -1)
                    {
                        Data data = new Data();
                        data.Flags = fBase.FrameNameTableFlags;

                        //auto <scene>
                        int sceneIndex = scenePos.Length - 1;

                        //check if this is a scene. If it is, then we get the index for the scene names and pos.
                        if (resource.Header.IsScene && fBase.ParentIndex2.Index != -1)
                            sceneIndex = fBase.ParentIndex2.Index;

                        //set parent index.
                        data.Parent = (short)scenePos[sceneIndex];

                        //add name to string and set namepos1 For namepos2, check if this is a scene. If so, then use 0xFFFF.
                        data.NamePos1 = (ushort)m_buffer.Length;
                        m_buffer += fBase.Name.String;
                        m_buffer += "\0";
                        data.NamePos2 = (resource.Header.IsScene) ? (ushort)0xFFFF : data.NamePos1;

                        //set frameIndex. minus the blockID and then subtract it from the total number of blocks.
                        data.FrameIndex = (short)i;

                        tableData.Add(data);
                    }
                }

            }

            frameData = tableData.ToArray();
            dataSize = tableData.Count;
            bufferSize = m_buffer.Length;
        }

        /// <summary>
        /// Adds names to the nametables.
        /// </summary>
        public void AddNames()
        {
            for (int i = 0; i != frameData.Length; i++)
            {
                frameData[i].Name = names[frameData[i].NamePos1];

                if (names.ContainsKey(frameData[i].Parent))
                    frameData[i].ParentName = names[frameData[i].Parent];
            }
        }

        /// <summary>
        /// Read the data from the file and store the read data.
        /// </summary>
        /// <param name="reader"></param>
        public void ReadFromFile(BinaryReader reader)
        {
            bufferSize = reader.ReadInt32();

            while (true)
            {
                int offset = (int)reader.BaseStream.Position - 4; // header is 4 bytes.

                if (offset == bufferSize)
                    break;

                string name = StringHelpers.ReadString(reader); //read string
                names.Add(offset, name); //add offset as unique key and string
            }

            dataSize = reader.ReadInt32();
            frameData = new Data[dataSize];

            for (int i = 0; i != frameData.Length; i++)
            {
                frameData[i] = new Data(reader);
            }
            AddNames();
        }

        /// <summary>
        /// write the data to the file and save the data.
        /// </summary>
        /// <param name="writer"></param>
        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(bufferSize);
            writer.Write(m_buffer.ToCharArray());
            writer.Write(dataSize);

            for(int i = 0; i != frameData.Length; i++)
                frameData[i].WriteToFile(writer);
        }

        public class Data
        {
            string parentName;
            string name;                   
            short parent;  
            ushort namepos1;
            ushort namepos2;
            short frameIndex;
            NameTableFlags flags;

            public string ParentName {
                get { return parentName; }
                set { parentName = value; }
            }
            public string Name {
                get { return name; }
                set { name = value; }
            }
            public short Parent {
                get { return parent; }
                set { parent = value; }
            }
            public ushort NamePos1 {
                get { return namepos1; }
                set { namepos1 = value; }
            }
            public ushort NamePos2 {
                get { return namepos2; }
                set { namepos2 = value; }
            }
            public short FrameIndex {
                get { return frameIndex; }
                set { frameIndex = value; }
            }
            public NameTableFlags Flags {
                get { return flags; }
                set { flags = value; }
            }

            /// <summary>
            /// Constructs an empty data bank, so you can add your own data.
            /// </summary>
            public Data() { }

            /// <summary>
            /// Constructor which then reads data from a given file.
            /// </summary>
            /// <param name="reader"></param>
            public Data(BinaryReader reader)
            {
                ReadFromFile(reader);
            }

            public void ReadFromFile(BinaryReader reader)
            {
                parent = reader.ReadInt16();
                namepos1 = reader.ReadUInt16();
                namepos2 = reader.ReadUInt16();
                frameIndex = reader.ReadInt16();
                flags = (NameTableFlags)reader.ReadInt16();
            }

            public void WriteToFile(BinaryWriter writer)
            {
                writer.Write(parent);
                writer.Write(namepos1);
                writer.Write(namepos2);
                writer.Write(frameIndex);

                writer.Write((short)flags);
            }

            public override string ToString()
            {
                return string.Format("{0}, {1}, Frame Index: {2}", parentName, name, frameIndex);
            }
        }
    }
}

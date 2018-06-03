using System;
using System.Collections.Generic;
using System.IO;

namespace Mafia2
{
    public class ItemDesc
    {
        public ulong frameRef; //links into FrameResources. Only checked collisions.
        public byte unk_byte; //ALWAYS 2
        public CollisionTypes colType;
        public CollisionBase cBase;
        public object collision;

        //remove later
        public long size;
        public string fileName;

        public void WriteToEDC()
        {
            if ((int)colType > 3)
                return;

            CustomEDC edc = new CustomEDC(colType, collision, cBase.Matrix);

            using (BinaryWriter writer = new BinaryWriter(File.Create("collisions/" + cBase.Hash + ".edc")))
            {
                writer.Write(cBase.Hash.ToString());
                writer.Write(cBase.Matrix.Position.X);
                writer.Write(cBase.Matrix.Position.Y);
                writer.Write(cBase.Matrix.Position.Z);
                writer.Write(cBase.Matrix.Rotation.Vector.X);
                writer.Write(cBase.Matrix.Rotation.Vector.Y);
                writer.Write(cBase.Matrix.Rotation.Vector.Z);
                writer.Write((byte)colType);
                edc.WriteToFile(writer);
            }
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}", frameRef, size);
        }
    }
    public class ItemDescParse
    {
        List<ItemDesc> itemDescList = new List<ItemDesc>();

        public ItemDescParse()
        {
            Parse();
            BuildCollisions();
        }

        public void Parse()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(Directory.GetCurrentDirectory());

            FileInfo[] files = dirInfo.GetFiles();

            List<string> ItemDesc = new List<string>();

            foreach (FileInfo file in files)
            {
                if (file.Name.Contains("ItemDesc_"))
                    ParseItemDesc(file.Name);
            }
        }

        public void BuildCollisions()
        {
            for(int i = 0; i != itemDescList.Count; i++)
            {
                itemDescList[i].WriteToEDC();
            }
        }
        private void ParseItemDesc(string name)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(name, FileMode.Open)))
            {
                ItemDesc item = new ItemDesc();
                item.size = reader.BaseStream.Length;
                item.fileName = name;

                item.frameRef = reader.ReadUInt64();
                item.unk_byte = reader.ReadByte();
                item.colType = (CollisionTypes)reader.ReadByte();
                item.cBase = new CollisionBase(reader);

                if (item.colType == CollisionTypes.Box)
                    item.collision = new CollisionBox(reader);
                else if (item.colType == CollisionTypes.Sphere)
                    item.collision = new CollisionSphere(reader);
                else if (item.colType == CollisionTypes.Capsule)
                    item.collision = new CollisionCapsule(reader);
                else
                    Console.WriteLine("Unsupported type {0}", item.colType);

                itemDescList.Add(item);
            }
        }
    }
}

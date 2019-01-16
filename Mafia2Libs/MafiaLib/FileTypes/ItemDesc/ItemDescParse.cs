using System;
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

        public ItemDesc(string fileName)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.Open)))
            {
                ReadFromFile(reader);
            }
        }

        public void ReadFromFile(BinaryReader reader)
        {
            frameRef = reader.ReadUInt64();
            unk_byte = reader.ReadByte();
            colType = (CollisionTypes)reader.ReadByte();
            cBase = new CollisionBase(reader);

            if (colType == CollisionTypes.Box)
                collision = new CollisionBox(reader);
            else if (colType == CollisionTypes.Sphere)
                collision = new CollisionSphere(reader);
            else if (colType == CollisionTypes.Capsule)
                collision = new CollisionCapsule(reader);
            else if (colType == CollisionTypes.Unk7)
                collision = new CollisionUnk7(reader);
            else
                Console.WriteLine("Unsupported type {0}", colType);
        }

        public void WriteToEDC()
        {
            //if ((int)colType > 3)
            //    return;

            //CustomEDC edc = new CustomEDC(colType, collision, cBase.Matrix);

            //using (BinaryWriter writer = new BinaryWriter(File.Create("collisions/" + frameRef + ".edc")))
            //{
            //    writer.Write(frameRef.ToString());
            //    writer.Write(cBase.Matrix.Position.X);
            //    writer.Write(cBase.Matrix.Position.Y);
            //    writer.Write(cBase.Matrix.Position.Z);
            //    writer.Write(cBase.Matrix.Rotation.Euler.X);
            //    writer.Write(cBase.Matrix.Rotation.Euler.Y);
            //    writer.Write(cBase.Matrix.Rotation.Euler.Z);
            //    writer.Write((byte)colType);
            //    edc.WriteToFile(writer);
            //}
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}", frameRef, colType);
        }
    }
}

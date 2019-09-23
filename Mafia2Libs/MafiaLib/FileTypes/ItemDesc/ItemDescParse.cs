using System;
using System.IO;
using Utils;
using Utils.Logging;
using Utils.Types;

namespace ResourceTypes.ItemDesc
{
    public class ItemDescLoader
    {
        public ulong frameRef; //links into FrameResources. Only checked collisions.
        public byte unk_byte; //ALWAYS 2
        public CollisionTypes colType;
        public ulong idHash;
        public short colMaterial;
        public TransformMatrix Matrix;
        public byte unkByte;
        public object collision;

        public ItemDescLoader(string fileName)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.Open)))
            {
                ReadFromFile(reader);
            }
            OverwriteConvexWithCooked("cooked.bin", fileName);
        }

        public void ReadFromFile(BinaryReader reader)
        {
            frameRef = reader.ReadUInt64();
            unk_byte = reader.ReadByte();
            colType = (CollisionTypes)reader.ReadByte();
            idHash = reader.ReadUInt64();
            colMaterial = reader.ReadInt16();
            Matrix = new TransformMatrix(reader);
            unkByte = reader.ReadByte();

            if (colType == CollisionTypes.Box)
                collision = new CollisionBox(reader);
            else if (colType == CollisionTypes.Sphere)
                collision = new CollisionSphere(reader);
            else if (colType == CollisionTypes.Capsule)
                collision = new CollisionCapsule(reader);
            else if (colType == CollisionTypes.Convex)
                collision = new CollisionConvex(reader);
            else
                throw new Exception("Unsupported colType: " + colType);
        }

        public void OverwriteConvexWithCooked(string cookedName, string output)
        {
            if (colType == CollisionTypes.Convex)
            {
                FBXHelper.CookConvexCollision("uncooked.bin", "cooked.bin");
                byte[] data = File.ReadAllBytes(cookedName);

                using (BinaryWriter writer = new BinaryWriter(File.Open(output, FileMode.Create)))
                {
                    writer.Write(frameRef);
                    writer.Write(unk_byte);
                    writer.Write((byte)colType);
                    writer.Write(idHash);
                    writer.Write(colMaterial);
                    Matrix.WriteToFile(writer);
                    writer.Write(unkByte);
                    writer.Write((ushort)data.Length);
                    writer.Write(data);
                }
                if (File.Exists("cooked.bin")) File.Delete("cooked.bin");
                if (File.Exists("uncooked.bin")) File.Delete("uncooked.bin");
                Log.WriteLine("Recooked ItemDesc", LoggingTypes.MESSAGE, LogCategoryTypes.APPLICATION);
            }
            
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}", frameRef, colType);
        }
    }
}

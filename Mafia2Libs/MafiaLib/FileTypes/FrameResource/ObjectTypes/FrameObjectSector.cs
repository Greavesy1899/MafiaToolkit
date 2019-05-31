using System;
using System.ComponentModel;
using System.IO;
using SharpDX;
using Utils.SharpDXExtensions;
using Utils.Types;

namespace ResourceTypes.FrameResource
{
    public class FrameObjectSector : FrameObjectJoint
    {
        int unk_08_int;
        int planesSize;
        Vector4[] planes;
        BoundingBox bounds;
        Vector3 unk_13_vector3;
        Vector3 unk_14_vector3;
        Hash sectorName;

        public int Unk08 {
            get { return unk_08_int; }
            set { unk_08_int = value; }
        }

        [Browsable(false)]
        public int PlanesSize {
            get { return planesSize; }
            set { planesSize = value; }
        }
        public Vector4[] Planes {
            get { return planes; }
            set { planes = value; }
        }
        public BoundingBox Bounds {
            get { return bounds ; }
            set { bounds = value; }
        }
        public Vector3 Unk13 {
            get { return unk_13_vector3; }
            set { unk_13_vector3 = value; }
        }
        public Vector3 Unk14 {
            get { return unk_14_vector3; }
            set { unk_14_vector3 = value; }
        }
        public Hash SectorName {
            get { return sectorName; }
            set { sectorName = value; }
        }

        public float BoundsMinimumX {
            get { return bounds.Minimum.X; }
            set { bounds.Minimum.X = value; }
        }
        public float BoundsMinimumY {
            get { return bounds.Minimum.Y; }
            set { bounds.Minimum.Y = value; }
        }
        public float BoundsMinimumZ {
            get { return bounds.Minimum.Z; }
            set { bounds.Minimum.Z = value; }
        }
        public float BoundsMaximumX {
            get { return bounds.Maximum.X; }
            set { bounds.Maximum.X = value; }
        }
        public float BoundsMaximumY {
            get { return bounds.Maximum.Y; }
            set { bounds.Maximum.Y = value; }
        }
        public float BoundsMaximumZ {
            get { return bounds.Maximum.Z; }
            set { bounds.Maximum.Z = value; }
        }

        public FrameObjectSector() : base()
        {
            bounds = new BoundingBox();
            unk_13_vector3 = new Vector3(0);
            unk_14_vector3 = new Vector3(0);
            sectorName = new Hash();
        }

        public FrameObjectSector(FrameObjectSector other) : base(other)
        {
            bounds = other.bounds;
            unk_13_vector3 = other.unk_13_vector3;
            unk_14_vector3 = other.unk_14_vector3;
            sectorName = new Hash(sectorName.String);
        }

        public FrameObjectSector(BinaryReader reader) : base()
        {
            ReadFromFile(reader);
        }

        public override void ReadFromFile(BinaryReader reader)
        {
            base.ReadFromFile(reader);
            unk_08_int = reader.ReadInt32();
            planesSize = reader.ReadInt32();

            planes = new Vector4[planesSize];
            for (int i = 0; i != planes.Length; i++)
                planes[i] = Vector4Extenders.ReadFromFile(reader);

            bounds = BoundingBoxExtenders.ReadFromFile(reader);
            unk_13_vector3 = Vector3Extenders.ReadFromFile(reader);
            unk_14_vector3 = Vector3Extenders.ReadFromFile(reader);
            sectorName = new Hash(reader);

        }

        public override void WriteToFile(BinaryWriter writer)
        {
            base.WriteToFile(writer);
            writer.Write(unk_08_int);
            writer.Write(planes.Length);

            for (int i = 0; i != planes.Length; i++)
                planes[i].WriteToFile(writer);

            bounds.WriteToFile(writer);
            unk_13_vector3.WriteToFile(writer);
            unk_14_vector3.WriteToFile(writer);
            sectorName.WriteToFile(writer);
        }

        public void FillPlanesArray()
        {
            planes = new Vector4[6];
            planes[0] = new Vector4(-1, 0, 0, bounds.Maximum.X);
            planes[1] = new Vector4(1, 0, 0, bounds.Maximum.X);
            planes[2] = new Vector4(0, -1, 0, bounds.Maximum.Y);
            planes[3] = new Vector4(0, 1, 0, bounds.Maximum.Y);
            planes[4] = new Vector4(0, 0, -1, bounds.Maximum.Z);
            planes[5] = new Vector4(0, 0, 1, bounds.Maximum.Z);
            //planes[0] = new Vector4(0, 1, 0, Math.Abs(bounds.Minimum.X));
            //planes[1] = new Vector4(1, 0, 0, Math.Abs(bounds.Minimum.Y));
            //planes[2] = new Vector4(0, 0, -1, bounds.Maximum.Z);
            //planes[3] = new Vector4(0, 0, 1, Math.Abs(bounds.Minimum.Y));
            //planes[4] = new Vector4(-1, 0, 0, bounds.Maximum.X);
            //planes[5] = new Vector4(0, -1, 0, bounds.Maximum.Y);
        }

        public override string ToString()
        {
            return Name.String;
        }
    }
}

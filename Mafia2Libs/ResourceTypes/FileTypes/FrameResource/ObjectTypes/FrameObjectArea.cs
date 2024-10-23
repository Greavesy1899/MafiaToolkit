using Rendering.Factories;
using Rendering.Graphics;
using System.ComponentModel;
using System.IO;
using System.Numerics;
using Mafia2Tool;
using Utils.Extensions;
using Utils.VorticeUtils;
using Vortice.Mathematics;

namespace ResourceTypes.FrameResource
{
    public class FrameObjectArea : FrameObjectJoint
    {
        int unk01;
        int planesSize;
        Vector4[] planes;
        BoundingBox bounds;

        public int Unk01 {
            get { return unk01; }
            set { unk01 = value; }
        }

        [Browsable(false)]
        public int PlaneSize {
            get { return planesSize; }
            set { planesSize = value; }
        }

        public Vector4[] Planes {
            get { return planes; }
            set { planes = value; }
        }
        public BoundingBox Bounds {
            get { return bounds; }
            set { bounds = value; }
        }
        [TypeConverter(typeof(Vector3Converter))]
        public Vector3 BoundaryBoxMinimum
        {
            get { return bounds.Min; }
            set { bounds.SetMinimum(value); }
        }
        [TypeConverter(typeof(Vector3Converter))]
        public Vector3 BoundaryBoxMaximum
        {
            get { return bounds.Max; }
            set { bounds.SetMaximum(value); }
        }

        public FrameObjectArea(FrameResource OwningResource) : base(OwningResource)
        {
            unk01 = 0;
            planesSize = 0;
            planes = new Vector4[planesSize];
            bounds = new BoundingBox();
        }

        public FrameObjectArea(FrameObjectArea other) : base(other)
        {
            unk01 = other.unk01;
            planesSize = other.planesSize;
            planes = other.planes;
            bounds = other.bounds;
        }

        public override void ReadFromFile(MemoryStream reader, bool isBigEndian)
        {
            base.ReadFromFile(reader, isBigEndian);
            unk01 = reader.ReadInt32(isBigEndian);
            planesSize = reader.ReadInt32(isBigEndian);
            planes = new Vector4[planesSize];

            for (int i = 0; i != planes.Length; i++)
            {
                planes[i] = Vector4Extenders.ReadFromFile(reader, isBigEndian);
            }

            bounds = BoundingBoxExtenders.ReadFromFile(reader, isBigEndian);
        }

        public override void WriteToFile(BinaryWriter writer)
        {
            base.WriteToFile(writer);
            writer.Write(unk01);
            writer.Write(planes.Length);

            for (int i = 0; i != planes.Length; i++)
            {
                planes[i].WriteToFile(writer);
            }

            bounds.WriteToFile(writer);
        }

        public void FillPlanesArray()
        {
            planes = new Vector4[6];
            planes[0] = new Vector4(-1, 0, 0, bounds.Max.X);
            planes[1] = new Vector4(1, 0, 0, bounds.Max.X);
            planes[2] = new Vector4(0, -1, 0, bounds.Max.Y);
            planes[3] = new Vector4(0, 1, 0, bounds.Max.Y);
            planes[4] = new Vector4(0, 0, -1, bounds.Max.Z);
            planes[5] = new Vector4(0, 0, 1, bounds.Max.Z);
        }

        public override string ToString()
        {
            return Name.String;
        }

        public override void ConstructRenderable()
        {
            RenderBoundingBox Renderable = RenderableFactory.BuildBoundingBox(Bounds, WorldTransform);
            RenderAdapter = new Rendering.Core.RenderableAdapter();
            RenderAdapter.InitAdaptor(Renderable, this);
        }
    }
}

using System.IO;

namespace Mafia2
{
    public class CollisionUnk7
    {
        int size;
        string nxs;
        string cvxm;
        int num1;
        int num2;
        string ice_0;
        string clhl;
        int num3;
        string ice_1;
        string cvhl;

        int type2;
        int nPoints;
        int nTriangles;
        int nEdges;
        int nPolygons;
        int nPolyPoints;
        int nPolyPoints2;

        Vector3[] points;
        int maxPointIndex;
        byte[] triangles;

        short zeroShort;

        short[] vertDef;
        Vector3 centerOfMass;

        Polygon[] polygons;

        public CollisionUnk7(BinaryReader reader)
        {
            ReadFromFile(reader);
        }

        public void ReadFromFile(BinaryReader reader)
        {
            size = reader.ReadInt16();
            nxs = new string(reader.ReadChars(4));
            cvxm = new string(reader.ReadChars(4));
            num1 = reader.ReadInt32();
            num2 = reader.ReadInt32();
            ice_0 = new string(reader.ReadChars(4));
            clhl = new string(reader.ReadChars(4));
            num3 = reader.ReadInt32();
            ice_1 = new string(reader.ReadChars(4));
            cvhl = new string(reader.ReadChars(4));

            type2 = reader.ReadInt32();

            if (type2 != 5)
                throw new System.Exception("Type2 expected 5");

            nPoints = reader.ReadInt32();
            nTriangles = reader.ReadInt32();
            nEdges = reader.ReadInt32();
            nPolygons = reader.ReadInt32();
            nPolyPoints = reader.ReadInt32();
            nPolyPoints2 = reader.ReadInt32();

            points = new Vector3[nPoints];
            for (int i = 0; i != points.Length; i++)
            {
                points[i] = new Vector3(reader);
            }

            maxPointIndex = reader.ReadInt32();

            triangles = new byte[nTriangles*3];
            for (int i = 0; i != triangles.Length; i++)
            {
                triangles[i] = reader.ReadByte();
            }

            zeroShort = reader.ReadInt16();

            vertDef = new short[nPoints*2];
            for (int i = 0; i != vertDef.Length; i++)
            {
                vertDef[i] = reader.ReadInt16();
            }

            centerOfMass = new Vector3(reader);

            polygons = new Polygon[nPolygons];
            for (int i = 0; i != polygons.Length; i++)
            {
                polygons[i] = new Polygon(reader);
            }
        }

        public void WriteToFile(BinaryWriter writer)
        {
           //writer.Write(vector.X);
            //writer.Write(vector.Y);
            //writer.Write(vector.Z);
        }
    }
}

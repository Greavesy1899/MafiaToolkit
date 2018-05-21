using Mafia2;
using System.IO;

public struct TransformMatrix {
    Matrix33 rotation;
    Vector3 position;

    public Matrix33 Rotation {
        get { return rotation; }
        set { rotation = value; }
    }
    public Vector3 Position {
        get { return position; }
        set { position = value; }
    }

    public TransformMatrix(BinaryReader reader) {

        float[] m1 = new float[] { reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle() };
        float x = reader.ReadSingle();
        float[] m2 = new float[] { reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle() };
        float y = reader.ReadSingle();
        float[] m3 = new float[] { reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle() };
        float z = reader.ReadSingle();

        rotation = new Matrix33(m1, m2, m3);
        position = new Vector3(x, y, z);
    }

    public override string ToString() {
        return string.Format("{0}, {1}", "TO_ADD", position);
    }
}

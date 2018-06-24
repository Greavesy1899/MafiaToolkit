﻿using Mafia2;
using System.ComponentModel;
using System.IO;

[TypeConverter(typeof(ExpandableObjectConverter))]
public class TransformMatrix
{
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

    public TransformMatrix() { }
    public TransformMatrix(BinaryReader reader)
    {
        ReadFromFile(reader);
    }

    public void ReadFromFile(BinaryReader reader)
    {
        Vector3 m1 = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        float x = reader.ReadSingle();
        Vector3 m2 = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        float y = reader.ReadSingle();
        Vector3 m3 = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        float z = reader.ReadSingle();

        rotation = new Matrix33(m1, m2, m3);
        position = new Vector3(x, y, z);
    }

    public void WriteToFile(BinaryWriter writer)
    {
        rotation.M1.WriteToFile(writer);
        writer.Write(position.X);
        rotation.M2.WriteToFile(writer);
        writer.Write(position.Y);
        rotation.M3.WriteToFile(writer);
        writer.Write(position.Z);
    }

    public override string ToString()
    {
        return string.Format("{0}, {1}", rotation, position);
    }
}

using Mafia2;
using System.ComponentModel;
using System.IO;

[TypeConverter(typeof(ExpandableObjectConverter))]
public class TransformMatrix
{
    public Matrix33 Rotation { get; set; }
    public Vector3 Position { get; set; }

    /// <summary>
    /// Construct empty TransformMatrix.
    /// </summary>
    public TransformMatrix() { }

    /// <summary>
    /// Construct TransformMatrix from parsed data.
    /// </summary>
    /// <param name="reader"></param>
    public TransformMatrix(BinaryReader reader)
    {
        ReadFromFile(reader);
    }

    /// <summary>
    /// Read TransformMatrix from the file.
    /// </summary>
    /// <param name="reader"></param>
    public void ReadFromFile(BinaryReader reader)
    {
        Vector3 m1 = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        float x = reader.ReadSingle();
        Vector3 m2 = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        float y = reader.ReadSingle();
        Vector3 m3 = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        float z = reader.ReadSingle();

        Rotation = new Matrix33(m1, m2, m3);
        Position = new Vector3(x, y, z);
    }

    /// <summary>
    /// Used for Max.
    /// </summary>
    /// <param name="writer"></param>
    public void WriteToFile(BinaryWriter writer)
    {
        writer.Write(Rotation.Rows[0, 0]);
        writer.Write(Rotation.Rows[0, 1]);
        writer.Write(Rotation.Rows[0, 2]);
        writer.Write(Position.X);
        writer.Write(Rotation.Rows[1, 0]);
        writer.Write(Rotation.Rows[1, 1]);
        writer.Write(Rotation.Rows[1, 2]);
        writer.Write(Position.Y);
        writer.Write(Rotation.Rows[2, 0]);
        writer.Write(Rotation.Rows[2, 1]);
        writer.Write(Rotation.Rows[2, 2]);
        writer.Write(Position.Z);
    }

    /// <summary>
    /// Use this to write to the FrameResource.
    /// </summary>
    /// <param name="writer"></param>
    public void WriteToFrame(BinaryWriter writer)
    {
        writer.Write(Rotation.Rows[0, 0]);
        writer.Write(Rotation.Rows[1, 0]);
        writer.Write(Rotation.Rows[2, 0]);
        writer.Write(Position.X);
        writer.Write(Rotation.Rows[0, 1]);
        writer.Write(Rotation.Rows[1, 1]);
        writer.Write(Rotation.Rows[2, 1]);
        writer.Write(Position.Y);
        writer.Write(Rotation.Rows[0, 2]);
        writer.Write(Rotation.Rows[1, 2]);
        writer.Write(Rotation.Rows[2, 2]);
        writer.Write(Position.Z);
    }

    public override string ToString()
    {
        return $"{Rotation} {Position}";
    }
}

using Mafia2;
using System;

public struct Matrix33 {
    Vector3 m1;
    Vector3 m2;
    Vector3 m3;

    Vector3 angle;

    public Vector3 M1 {
        get { return m1; }
        set { m1 = value; }
    }
    public Vector3 M2 {
        get { return m2; }
        set { m2 = value; }
    }
    public Vector3 M3 {
        get { return m3; }
        set { m3 = value; }
    }
    public Vector3 Vector {
        get { return angle; }
        set { angle = value; }
    }

    public Matrix33(Vector3 m1, Vector3 m2, Vector3 m3) {
        this.m1 = m1;
        this.m2 = m2;
        this.m3 = m3;
        angle = new Vector3();
        Vector = ConvertToAngle();
    }

    public Vector3 ConvertToAngle()
    {
        double x = (Math.Acos((m1.X + m2.Y + m3.Z - 1) / 2)) * 180 / Math.PI;
        double y = (Math.Acos((m1.Y + m2.Y + m3.Y - 1) / 2)) * 180 / Math.PI;
        double z = (Math.Acos((m1.Z + m2.Y + m3.X - 1) / 2)) * 180 / Math.PI;

        return new Vector3((float)x, (float)y, (float)z);
    }

    public override string ToString()
    {
        return string.Format("{0},", m1);
    }


}

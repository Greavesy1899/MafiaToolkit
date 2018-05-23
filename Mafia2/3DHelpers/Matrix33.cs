//This could get redone. Instead of using float arrays, maybe just use vectors.
using Mafia2;

public struct Matrix33 {
    Vector3 m1;
    Vector3 m2;
    Vector3 m3;

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

    public Matrix33(Vector3 m1, Vector3 m2, Vector3 m3) {
        this.m1 = m1;
        this.m2 = m2;
        this.m3 = m3;
    }

    public override string ToString()
    {
        return string.Format("{0}", m1);
    }


}

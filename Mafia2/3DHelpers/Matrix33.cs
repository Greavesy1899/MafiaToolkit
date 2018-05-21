//This could get redone. Instead of using float arrays, maybe just use vectors.
public struct Matrix33 {
    float[] m1;
    float[] m2;
    float[] m3;

    public float[] M1 {
        get { return m1; }
        set { m1 = value; }
    }
    public float[] M2 {
        get { return m2; }
        set { m2 = value; }
    }
    public float[] M3 {
        get { return m3; }
        set { m3 = value; }
    }

    public Matrix33(float[] m1, float[] m2, float[] m3) {
        this.m1 = m1;
        this.m2 = m2;
        this.m3 = m3;
    }
}

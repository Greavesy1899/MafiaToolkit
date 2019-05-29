namespace ResourceTypes.BufferPools
{
    public enum BufferType
    {
        Vertex = 2,
        Index = 3,
    }

    public class BufferLocationStruct
    {
        private int poolLoc;
        private int bufferLoc;

        public int PoolLocation {
            get { return poolLoc; }
            set { poolLoc = value; }
        }

        public int BufferLocation {
            get { return bufferLoc; }
            set { bufferLoc = value; }
        }

        public BufferLocationStruct(int i, int c)
        {
            poolLoc = i;
            bufferLoc = c;
        }
    }
}

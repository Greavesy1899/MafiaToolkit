using System;

namespace ResourceTypes.FrameNameTable
{
    [Flags]
    public enum NameTableFlags : int
    {
        flag_1 = 1,
        flag_2 = 2,
        flag_4 = 4,
        flag_8 = 8,
        flag_16 = 16,
        flag_32 = 32,
        flag_64 = 64,
        flag_128 = 128,
        flag_256 = 256,
        flag_512 = 512,
        flag_1024 = 1024,
        flag_2048 = 2048,
        flag_4096 = 4096,
    }
}

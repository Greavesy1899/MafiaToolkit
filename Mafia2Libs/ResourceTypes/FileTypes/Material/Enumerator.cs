using System;

namespace ResourceTypes.Materials
{
    public enum VersionsEnumerator
    {
        Nill = -1,
        V_57 = 57,
        V_58 = 58,
        V_63 = 63,
    }

    [Flags]
    public enum MaterialFlags : uint //No idea which ones are used.
    {
        flag0 = 0x10,
        flag_1 = 1,
        Alpha = 2,
        flag_4 = 4,
        flag_8 = 8,
        Disable_ZWriting = 16,
        flag_32 = 32,
        flag_64 = 64,
        flag_128 = 128,
        flag_256 = 256,
        flag_512 = 512,
        flag_1024 = 1024,
        flag_2048 = 2048,
        CastShadows = 4096,
        flag_8192 = 8192,
        flag_16384 = 16384,
        flag_32768 = 32768,
        flag_65536 = 65536,
        flag_131072 = 131072,
        flag_262144 = 262144,
        flag_524288 = 524288,
        flag_1048576 = 1048576,
        flag_2097152 = 2097152,
        flag_4194304 = 4194304,
        flag_8388608 = 8388608,
        flag_16777216 = 16777216,
        flag_33554432 = 33554432,
        flag_67108864 = 67108864,
        flag_134217728 = 134217728,
        flag_268435456 = 268435456,
        flag_536870912 = 536870912,
        flag_1073741824 = 1073741824
        //flag_2147483648 = 2147483648
    }

    public enum SearchTypesHashes
    {
        MaterialHash = 2,
        ShaderID = 3,
        ShaderHash = 4,
    }

    public enum SearchTypesString
    {
        MaterialName = 0,
        TextureName = 1
    }
}

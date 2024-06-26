﻿using System.IO;

namespace ResourceTypes.Cutscene.KeyParams
{
    public class KeyType_Temp : IKeyType
    {
        public byte[] KeyData;

        public override void ReadFromFile(BinaryReader br)
        {
            base.ReadFromFile(br);
            KeyData = br.ReadBytes((int)br.BaseStream.Length - 4);
        }

        public override void WriteToFile(BinaryWriter bw)
        {
            base.WriteToFile(bw);
            bw.Write(KeyData);
        }

        public override string ToString()
        {
            return string.Format("Type: {0} NOT_REVERSED", KeyType);
        }
    }
}

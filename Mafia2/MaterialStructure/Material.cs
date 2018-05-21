using System;
using System.IO;
using Gibbed.IO;
using System.ComponentModel;

namespace Mafia2 {
    public class Material {

        ulong materialNumID;
        string materialNumStr;
        string materialName;

        byte ufo1;
        byte ufo2;
        int ufo3;
        byte ufo4;
        int ufo5;
        int ufo6;

        ulong shaderID;
        string shaderName;

        uint flags;

        int sp_count;
        ShaderParameter[] sp;

        int sps_count;
        ShaderParameterSampler[] sps;

        int arrayNum;

        [Category("Material IDs"), ReadOnly(true)]
        public ulong MaterialNumID {
            get { return materialNumID; }
            set { materialNumID = value; }
        }
        [Category("Material IDs")]
        public string MaterialNumStr {
            get { return materialNumStr; }
            set { materialNumStr = value; }
        }
        [Category("Material IDs")]
        public string MaterialName {
            get { return materialName; }
            set { materialName = value; }
        }

        [Category("UFOs")]
        public byte UFO1 {
            get { return ufo1; }
            set { ufo2 = value; }
        }

        [Category("UFOs")]
        public byte UFO2 {
            get { return ufo2; }
            set { ufo2 = value; }
        }

        [Category("UFOs")]
        public int UFO3 {
            get { return ufo3; }
            set { ufo3 = value; }
        }

        [Category("UFOs")]
        public byte UFO4 {
            get { return ufo4; }
            set { ufo4 = value; }
        }

        [Category("UFOs")]
        public int UFO5 {
            get { return ufo5; }
            set { ufo5 = value; }
        }

        [Category("UFOs")]
        public int UFO6 {
            get { return ufo6; }
            set { ufo6 = value; }
        }

        [Category("Shaders")]
        public ulong ShaderID {
            get { return shaderID; }
            set { shaderID = value; }
        }

        [Category("Shaders")]
        public string ShaderName {
            get { return shaderName; }
            set { shaderName = value; }
        }

        [Category("Flags")]
        public uint Flags {
            get { return flags; }
            set { flags = value; }
        }

        [Category("SP")]
        public ShaderParameter[] SP {
            get { return sp; }
            set {
                sp = value;
                sp_count = value.Length;
            }
        }

        [Category("SPS")]
        public ShaderParameterSampler[] SPS {
            get { return sps; }
            set {
                sps = value;
                sps_count = value.Length;
            }
        }

        [ReadOnly(true)]
        public int ArrayNum {
            get { return arrayNum; }
            set { arrayNum = value; }
        }


        public Material(BinaryReader reader) {
            ReadFromFile(reader);
        }

        public override string ToString() {
            return string.Format("{0}\t{1}", materialNumStr, materialName);
        }

        public void ReadFromFile(BinaryReader reader) {

            materialNumID = reader.ReadUInt64();
            materialNumStr = string.Format("{0:X16}", materialNumID.Swap());

            int nameLength = reader.ReadInt32();
            materialName = new string(reader.ReadChars(nameLength));

            ufo1 = reader.ReadByte();
            ufo2 = reader.ReadByte();
            ufo3 = reader.ReadInt32();
            ufo4 = reader.ReadByte();
            ufo5 = reader.ReadInt32();
            ufo6 = reader.ReadInt32();

            shaderID = reader.ReadUInt64().Swap();
            shaderName = string.Format("{0:X16}", shaderID);

            flags = reader.ReadUInt32();

            sp_count = reader.ReadInt32();
            sp = new ShaderParameter[sp_count];
            for(int i = 0; i != sp_count; i++) {
                sp[i] = new ShaderParameter(reader);
            }

            sps_count = reader.ReadInt32();
            sps = new ShaderParameterSampler[sps_count];
            for (int i = 0; i != sps_count; i++) {
                sps[i] = new ShaderParameterSampler(reader);
            }

        }

        public void WriteToFile(FileStream stream) {
            //material hash code and name.
            stream.WriteValueU64(materialNumID.Swap());
            stream.WriteValueS32(materialName.Length);
            stream.WriteString(materialName);

            //UFO values
            stream.WriteValueU8(ufo1);
            stream.WriteValueU8(ufo2);
            stream.WriteValueS32(ufo3);
            stream.WriteValueU8(ufo4);
            stream.WriteValueS32(ufo5);
            stream.WriteValueS32(ufo6);

            //Shader and flags
            stream.WriteValueU64(shaderID.Swap());
            stream.WriteValueU32(flags);

            //Shader Parameter
            stream.WriteValueS32(sp_count);
            foreach (ShaderParameter sp in sp) {
                sp.WriteToFile(stream);
            }

            //Shader Parameter Samplers
            stream.WriteValueS32(sps_count);
            foreach (ShaderParameterSampler sps in sps) {
                sps.WriteToFile(stream);
            }
        }

    }

    public struct ShaderParameter {

        string chunk;
        int floatCount;
        float[] floats;

        public string Chunk {
            get { return chunk; }
            set { chunk = value; }
        }
        public float[] Floats {
            get { return floats; }
            set {
                floats = value;
                floatCount = value.Length;
            }
        }

        public ShaderParameter(BinaryReader reader) {

            chunk = new string(reader.ReadChars(4));

            floatCount = reader.ReadInt32() / 4;

            floats = new float[floatCount];
            for (int i = 0; i != floatCount; i++) {
                floats[i] = reader.ReadSingle();
            }
        }

        public override string ToString() {
            return string.Format("{0}, {1}", chunk, floatCount);
        }

        public void WriteToFile(FileStream stream) {
            stream.WriteString(chunk);
            stream.WriteValueS32(floatCount * 4);
            foreach(float f in floats) {
                stream.WriteValueF32(f);
            }
        }
    }

    public struct ShaderParameterSampler {

        string chunk;
        string ufo_x2;
        string flags;
        string file;

        ulong unk;

        public string Chunk {
            get { return chunk; }
            set { chunk = value; }
        }
        public string UFO_X2 {
            get { return ufo_x2; }
            set { ufo_x2 = value; }
        }
        public string Flags {
            get { return flags; }
            set { flags = value; }
        }
        public string File {
            get { return file; }
            set { file = value; }
        }
        public ulong Umk {
            get { return unk; }
            set { unk = value; }
        }

        public ShaderParameterSampler(BinaryReader reader) {

            chunk = new string(reader.ReadChars(4));

            ufo_x2 = reader.ReadInt32() + " " + reader.ReadInt32();
            unk = reader.ReadUInt64();
            flags = BitConverter.ToString(reader.ReadBytes(16)).Replace("-", "");

            int size = reader.ReadInt32();
            file = new string(reader.ReadChars(size));
        }

        public override string ToString() {
            return string.Format("{0}, {1}", chunk, file);
        }

        public void WriteToFile(FileStream stream) {
            stream.WriteString(chunk);
            stream.WriteValueS32(int.Parse(ufo_x2.Split(' ')[0]));
            stream.WriteValueS32(int.Parse(ufo_x2.Split(' ')[1]));
            stream.WriteValueU64(unk);
            stream.WriteBytes(Functions.ConvertHexStringToByteArray(flags));
            stream.WriteValueU32((uint)file.Length);
            stream.WriteString(file);
        }
    }
}

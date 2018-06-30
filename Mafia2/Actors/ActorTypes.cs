using System;
using System.IO;

namespace Mafia2
{
    public class ActorPinup
    {
        int pinupNum;

        public int PinupNum {
            get { return pinupNum; }
            set { pinupNum = value; }
        }

        public ActorPinup(BinaryReader reader)
        {
            pinupNum = reader.ReadInt32();
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}", ActorTypes.Pinup, pinupNum);
        }
    }

    public class ActorScriptEntity
    {
        string scriptName;
        int unk01;

        public string ScriptName {
            get { return scriptName; }
            set { scriptName = value; }
        }
        public int Unk01 {
            get { return unk01; }
            set { unk01 = value; }
        }

        public ActorScriptEntity(BinaryReader reader)
        {
            scriptName = new string(reader.ReadChars(96)).TrimEnd('\0');
            unk01 = reader.ReadInt32();
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}", ActorTypes.ScriptEntity, scriptName);
        }
    }
}

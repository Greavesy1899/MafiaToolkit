using System;

namespace ResourceTypes.Collisions.Opcode
{
    class OpcodeException : Exception
    {
        public OpcodeException(string message): base(message)
        {
        }
    }
}

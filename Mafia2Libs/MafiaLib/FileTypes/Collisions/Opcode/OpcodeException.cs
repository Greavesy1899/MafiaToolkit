using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceTypes.Collisions.Opcode
{
    class OpcodeException : Exception
    {
        public OpcodeException(string message): base(message)
        {
        }
    }
}

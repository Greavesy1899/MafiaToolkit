using System;

namespace ResourceTypes.Collisions.PhysX
{
    public class PhysXException : Exception
    {
        public PhysXException(string message) : base(message)
        {
        }
    }
}

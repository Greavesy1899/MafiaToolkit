using System.Collections.Generic;
using System.IO;

namespace Mafia2
{
    //Built upon EDM, but EDC is for collisions.
    public class CustomEDC
    {
        CollisionTypes type;
        object collision;
        TransformMatrix matrix;

        public CustomEDC(CollisionTypes type, object collision, TransformMatrix matrix)
        {
            this.type = type;
            this.collision = collision;
            this.matrix = matrix;
        }

        public void WriteToFile(BinaryWriter writer)
        {
            if (type == CollisionTypes.Box)
                (collision as CollisionBox).WriteToFile(writer);
            else if (type == CollisionTypes.Capsule)
                (collision as CollisionCapsule).WriteToFile(writer);
            else if (type == CollisionTypes.Sphere)
                (collision as CollisionSphere).WriteToFile(writer);
        }
    }
}

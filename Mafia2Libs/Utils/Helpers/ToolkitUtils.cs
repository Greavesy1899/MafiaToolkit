using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Helpers
{
    public static class ToolkitUtils
    {
        public static bool IsSubclassOf(object InObject, Type InType)
        {
            Type ObjectType = InObject.GetType();
            if(ObjectType.IsSubclassOf(InType))
            {
                return true;
            }

            return false;
        }
    }
}

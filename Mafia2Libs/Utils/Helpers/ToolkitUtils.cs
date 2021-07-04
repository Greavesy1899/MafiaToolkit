using System;

namespace Utils.Helpers
{
    public static class ToolkitUtils
    {
        public static bool Is<T>(object InObject)
        {
            Type ObjectType = InObject.GetType();
            if (InObject is T)
            {
                return true;
            }

            return false;
        }

        public static bool IsSubclassOf<T>(object InObject)
        {
            Type ObjectType = InObject.GetType();
            if(ObjectType.IsSubclassOf(typeof(T)))
            {
                return true;
            }

            return false;
        }

        public static T Cast<T>(object InObject)
        {
            if(IsSubclassOf<T>(InObject) || Is<T>(InObject))
            {
                if (InObject is T)
                {
                    return (T)InObject;
                }
            }

            return default(T);
        }
    }
}

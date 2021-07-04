using System;

namespace Utils.Helpers
{
    public static class ToolkitUtils
    {
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
            if(IsSubclassOf<T>(InObject))
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

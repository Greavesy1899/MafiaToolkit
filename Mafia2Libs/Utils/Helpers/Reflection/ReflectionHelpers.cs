using System;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace Utils.Helpers.Reflection
{
    public class ReflectionHelpers
    {
        public static T ConvertToPropertyFromXML<T>(XElement Node)
        {
            T TypedObject = Activator.CreateInstance<T>();

            foreach (PropertyInfo Info in TypedObject.GetType().GetProperties())
            {
                // Check if this Property has been flagged to be ignored.
                if (!AllowPropertyToReflect(Info))
                {
                    continue;
                }

                // Should this property be read from an Attribute.
                bool bForceAsAttribute = ForcePropertyAsAttribute(Info);

                if (Info.PropertyType.IsArray)
                {
                    // Get Element.
                    XElement Element = Node.Element(Info.Name);

                    // Create an Array using the element type of the array, with the number of elements to set the length.
                    Array ArrayObject = Array.CreateInstance(Info.PropertyType.GetElementType(), Element.Elements().Count());

                    // Iterate through the elements, construct the object using our reflection system and push them into the array.
                    for (int i = 0; i < ArrayObject.Length; i++)
                    {
                        object ElementObject = InternalConvertProperty(Element.Elements().ElementAt(i), Info.PropertyType.GetElementType());
                        ArrayObject.SetValue(ElementObject, i);
                    }

                    // Finally, replace the array on our TypedObject.
                    TypedObject.GetType().GetProperty(Info.Name).SetValue(TypedObject, ArrayObject);
                    continue;
                }

                // Arrays CANNOT be arrays. So we first check if it is an array, and then try and parts this parts which can be an attribute.
                string NodeContent = bForceAsAttribute ?  Node.Attribute(Info.Name).Value : Node.Element(Info.Name).Value;
                if (!string.IsNullOrEmpty(NodeContent))
                {
                    if (Info.PropertyType.IsEnum)
                    {
                        object Value = Enum.Parse(Info.PropertyType, NodeContent);
                        Info.SetValue(TypedObject, Value);
                        continue;
                    }
                    else
                    {
                        Info.SetValue(TypedObject, Convert.ChangeType(NodeContent, Info.PropertyType));
                    }               
                }
            }

            return TypedObject;
        }

        private static object InternalConvertProperty(XElement Node, Type ElementType)
        {
            object TypedObject = Activator.CreateInstance(ElementType);

            if(ElementType.GetProperties().Length == 0)
            {
                TypedObject = Convert.ChangeType(Node.Value, ElementType);
                return TypedObject;
            }

            var Properties = ElementType.GetProperties();
            foreach (PropertyInfo Info in ElementType.GetProperties())
            {
                // Check if this Property has been flagged to be ignored.
                if (!AllowPropertyToReflect(Info))
                {
                    continue;
                }

                // Should this property be read from an Attribute.
                bool bForceAsAttribute = ForcePropertyAsAttribute(Info);    

                if (Info.PropertyType.IsClass && AllowClassReflection(Info.PropertyType))
                {
                    // Get Element
                    XElement Element = Node.Element(Info.Name);
                    object ClassObject = InternalConvertProperty(Element, Info.PropertyType);
                    Info.SetValue(TypedObject, ClassObject);
                    continue;
                }

                string NodeContent = bForceAsAttribute ? Node.Attribute(Info.Name).Value : Node.Element(Info.Name).Value;

                if (!string.IsNullOrEmpty(NodeContent))
                {
                    if (Info.PropertyType.IsEnum)
                    {
                        object Value = Enum.Parse(Info.PropertyType, NodeContent);
                        Info.SetValue(TypedObject, Value);
                        continue;
                    }
                    else if(Info.PropertyType.IsArray)
                    {
                        // Get Element.
                        XElement Element = Node.Element(Info.Name);

                        // Create an Array using the element type of the array, with the number of elements to set the length.
                        Array ArrayObject = Array.CreateInstance(Info.PropertyType.GetElementType(), Element.Elements().Count());

                        // Iterate through the elements, construct the object using our reflection system and push them into the array.
                        for (int i = 0; i < ArrayObject.Length; i++)
                        {
                            object ElementObject = InternalConvertProperty(Element.Elements().ElementAt(i), Info.PropertyType.GetElementType());
                            ArrayObject.SetValue(ElementObject, i);
                        }

                        // Finally, replace the array on our TypedObject.
                        TypedObject.GetType().GetProperty(Info.Name).SetValue(TypedObject, ArrayObject);
                        continue;
                    }
                    else if(Info.PropertyType.IsClass && AllowClassReflection(Info.PropertyType))
                    {
                        // Get Element
                        XElement Element = Node.Element(Info.Name);
                        object ClassObject = InternalConvertProperty(Element, Info.PropertyType);
                        Info.SetValue(TypedObject, ClassObject);
                    }
                    else
                    {
                        Info.SetValue(TypedObject, Convert.ChangeType(NodeContent, Info.PropertyType));
                    }
                }
            }

            return TypedObject;
        }

        /*
         * Utility function to convert object from C# -> XML.
         */
        private static XElement InternalConvertProperty<TObject>(TObject PropertyData, Type ObjectType, string PropertyName)
        {
            // If Object is an Array, we get the Array and iterate through elements.
            if (ObjectType.IsArray)
            {
                XElement RootElement = new XElement("Root");
                Array ArrayContent = (Array)Convert.ChangeType(PropertyData, ObjectType);

                foreach (object Element in ArrayContent)
                {
                    XElement Entry = ConvertPropertyToXML(Element);
                    RootElement.Add(Entry);
                }

                return RootElement;
            }
            else if(AllowClassReflection(ObjectType))
            {
                XElement Element = new XElement(PropertyName);
                ConvertObject(Element, PropertyData, ObjectType);
                return Element;
            }
            else
            {
                XElement Element = new XElement(ObjectType.Name);
                ConvertObject(Element, PropertyData, ObjectType);
                return Element;
            }
        }

        /*
         * Utility function to convert object from C# -> XML.
         */
        private static void ConvertObject<TObject>(XElement Element, TObject PropertyData, Type ObjectType)
        {
            // If the ObjectType has no properties, then just attempt to write.
            // TODO: Consider if this is actually a good idea?
            // Maybe there is a way of determine if it is a type like char, byte, int32 etc.
            if (ObjectType.GetProperties().Length == 0)
            {
                // Set the value and early return. We know we have no properties so no need to carry on.
                Element.SetValue(PropertyData);
            }

            foreach (PropertyInfo Info in ObjectType.GetProperties())
            {
                // Check if this Property has been flagged to be ignored.
                if (!AllowPropertyToReflect(Info))
                {
                    continue;
                }

                // Should this property be saved as an Attribute.
                bool bForceAsAttribute = ForcePropertyAsAttribute(Info);

                // Is this an Array, if so, we have to iterate.
                if (Info.PropertyType.IsArray)
                {
                    XElement RootElement = new XElement(Info.Name);
                    Array ArrayContent = (Array)PropertyData.GetType().GetProperty(Info.Name).GetValue(PropertyData);

                    foreach (object ArrayElement in ArrayContent)
                    {
                        XElement Entry = ConvertPropertyToXML(ArrayElement);
                        RootElement.Add(Entry);
                    }

                    Element.Add(RootElement);
                }
                else if (Info.PropertyType.IsClass && AllowClassReflection(Info.PropertyType))
                {
                    object ClassObject = PropertyData.GetType().GetProperty(Info.Name).GetValue(PropertyData);
                    Element.Add(InternalConvertProperty(ClassObject, Info.PropertyType, Info.Name));
                }
                else
                {
                    object info = PropertyData.GetType().GetProperty(Info.Name).GetValue(PropertyData);

                    // Sanity check for null
                    info = (info != null ? info : "");

                    if (bForceAsAttribute)
                    {
                        Element.Add(new XAttribute(Info.Name, info));
                    }
                    else
                    {
                        Element.Add(new XElement(Info.Name, info));
                    }
                }
            }
        }

        public static XElement ConvertPropertyToXML<TObject>(TObject PropertyData)
        {
            return InternalConvertProperty(PropertyData, PropertyData.GetType(), "N/A");
        }

        private static bool ForcePropertyAsAttribute(PropertyInfo Info)
        {
            // Is our Attribute Valid?
            Attribute PropertyAttritbute = Info.GetCustomAttribute(typeof(PropertyForceAsAttributeAttribute));

            if (PropertyAttritbute != null)
            {
                PropertyInfo[] PropertyInfos = Info.PropertyType.GetProperties();

                // Check if this property has nested properties.
                //Debug.Assert(PropertyInfos.Length == 0, "ERROR: Cannot save property with nested properties as attribute.",
                 //   "We cannot save a property with more child properties. Please remove the attribute from this property: " + Info.Name);

                return true;
            }

            return false;
        }

        private static bool AllowPropertyToReflect(PropertyInfo Info)
        {
            // Is our Attribute Valid?
            Attribute PropertyAttritbute = Info.GetCustomAttribute(typeof(PropertyIgnoreByReflector));
            return PropertyAttritbute == null;
        }

        private static bool AllowClassReflection(Type Info)
        {
            // Is our Class allowed to reflect?
            Attribute PropertyAttritbute = Info.GetCustomAttribute(typeof(PropertyClassAllowReflection));
            return PropertyAttritbute != null;
        }
    }
}

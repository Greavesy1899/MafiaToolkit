using System;
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
                XElement Element = Node.Element(Info.Name);
                if (Element != null)
                {
                    if (Info.PropertyType.IsEnum)
                    {
                        object Value = Enum.Parse(Info.PropertyType, Element.Value);
                        Info.SetValue(TypedObject, Value);
                        continue;
                    }

                    Info.SetValue(TypedObject, Convert.ChangeType(Element.Value, Info.PropertyType));
                }
            }

            return TypedObject;
        }

        public static XElement ConvertPropertyToXML<TObject>(TObject PropertyData)
        {
            Type ObjectType = PropertyData.GetType();

            // If Object is an Array, we get the Array and iterate through elements.
            if (ObjectType.IsArray)
            {
                XElement RootElement = new XElement("root");
                Array ArrayContent = (Array)Convert.ChangeType(PropertyData, ObjectType);

                foreach (object Element in ArrayContent)
                {
                    XElement Entry = ConvertPropertyToXML(Element);
                    RootElement.Add(Entry);
                }

                return RootElement;
            }
            else
            {
                XElement Element = new XElement(ObjectType.Name);

                foreach (PropertyInfo Info in ObjectType.GetProperties())
                {
                    // Check if this Property has been flagged to be ignored.
                    if(!AllowPropertyToReflect(Info))
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
                    else
                    {
                        object info = PropertyData.GetType().GetProperty(Info.Name).GetValue(PropertyData);

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

                return Element;
            }
        }

        private static bool ForcePropertyAsAttribute(PropertyInfo Info)
        {
            // Is our Attribute Valid?
            Attribute PropertyAttritbute = Info.GetCustomAttribute(typeof(PropertyForceAsAttributeAttribute));

            if (PropertyAttritbute != null)
            {
                Console.WriteLine("Hello");
            }
            return PropertyAttritbute != null;
        }

        private static bool AllowPropertyToReflect(PropertyInfo Info)
        {
            // Is our Attribute Valid?
            Attribute PropertyAttritbute = Info.GetCustomAttribute(typeof(PropertyIgnoreByReflector));
            return PropertyAttritbute == null;
        }
    }
}

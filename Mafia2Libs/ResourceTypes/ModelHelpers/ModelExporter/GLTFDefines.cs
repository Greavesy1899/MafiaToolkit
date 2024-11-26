using SharpGLTF.Schema2;
using System.Text.Json.Nodes;

namespace ResourceTypes.ModelHelpers.ModelExporter
{
    public static class GLTFDefines
    {
        public static bool GetValueFromNode<TType>(Node InNode, string InPropertyName, out TType OutValue)
        {
            OutValue = default(TType);

            if (InNode == null)
            {
                // node is invalid
                return false;
            }

            if (InNode.Extras == null)
            {
                // node does not have extras
                return false;
            }

            JsonNode PropertyNode = InNode.Extras[InPropertyName];
            if (PropertyNode == null)
            {
                // specified property does not exist
                return false;
            }

            if (PropertyNode is not JsonValue)
            {
                // not a JsonValue type
                return false;
            }

            JsonValue PropertyValue = PropertyNode.AsValue();
            if (PropertyValue.TryGetValue<TType>(out OutValue))
            {
                // we got the value
                return true;
            }

            // value in node does not match type expected
            return false;
        }
    }
}

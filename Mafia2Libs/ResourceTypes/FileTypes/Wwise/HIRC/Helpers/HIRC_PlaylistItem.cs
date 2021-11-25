using System;
using System.Xml;
using System.Xml.Linq;
using System.ComponentModel;

namespace ResourceTypes.Wwise.Helpers
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlaylistItem
    {
        public uint VertexOffset { get; set; }
        public uint VertexCount { get; set; }

        public PlaylistItem(uint iVertexOffset, uint iNumVertices)
        {
            VertexOffset = iVertexOffset;
            VertexCount = iNumVertices;
        }

        public PlaylistItem()
        {
            VertexOffset = 0;
            VertexCount = 0;
        }
    }
}

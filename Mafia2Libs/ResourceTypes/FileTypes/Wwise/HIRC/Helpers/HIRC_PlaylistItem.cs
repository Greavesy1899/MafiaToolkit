using System;
using System.Xml;
using System.Xml.Linq;

namespace ResourceTypes.Wwise.Helpers
{
    public class PlaylistItem
    {
        public uint vertexOffset { get; set; }
        public uint numVertices { get; set; }

        public PlaylistItem(uint iVertexOffset, uint iNumVertices)
        {
            vertexOffset = iVertexOffset;
            numVertices = iNumVertices;
        }

        public PlaylistItem()
        {
            vertexOffset = 0;
            numVertices = 0;
        }
    }
}

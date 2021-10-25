using System.IO;
using System.Numerics;
using Utils.StringHelpers;
using Utils.VorticeUtils;

namespace ResourceTypes.ModelHelpers.ModelExporter
{
    public class MT_Collision : IValidator
    {
        public Vector3[] Vertices { get; set; }
        public uint[] Indices { get; set; }
        public MT_FaceGroup[] FaceGroups { get; set; }

        /** IO Functions */
        public bool ReadFromFile(BinaryReader reader)
        {
            // Attempt to read Vertices
            uint NumVertices = reader.ReadUInt32();
            Vertices = new Vector3[NumVertices];
            for (int i = 0; i < NumVertices; i++)
            {
                Vertices[i] = Vector3Utils.ReadFromFile(reader);
            }

            // Read FaceGroups
            uint NumFaceGroups = reader.ReadUInt32();
            FaceGroups = new MT_FaceGroup[NumFaceGroups];
            for (int i = 0; i < NumFaceGroups; i++)
            {
                // Attempt to read FaceGroup
                MT_FaceGroup NewFaceGroup = new MT_FaceGroup();
                NewFaceGroup.StartIndex = reader.ReadUInt32();
                NewFaceGroup.NumFaces = reader.ReadUInt32();

                // Read FaceGroup Material
                MT_MaterialInstance NewMaterial = new MT_MaterialInstance();
                NewMaterial.MaterialFlags = (MT_MaterialInstanceFlags)reader.ReadInt32();
                NewMaterial.Name = StringHelpers.ReadString8(reader);
                NewFaceGroup.Material = NewMaterial;
                FaceGroups[i] = NewFaceGroup;
            }

            // Read Indices
            uint NumIndices = reader.ReadUInt32();
            Indices = new uint[NumIndices];
            for (int i = 0; i < NumIndices; i++)
            {
                Indices[i] = reader.ReadUInt32();
            }

            return true;
        }

        public void WriteToFile(BinaryWriter writer)
        {
            // Attempt to write vertices
            writer.Write(Vertices.Length);
            for (int i = 0; i < Vertices.Length; i++)
            {
                Vector3 Vertex = Vertices[i];
                Vertex.WriteToFile(writer);
            }

            // Attempt to write FaceGroups
            writer.Write(FaceGroups.Length);
            for (int i = 0; i < FaceGroups.Length; i++)
            {
                // Write FaceGroup
                MT_FaceGroup FaceGroup = FaceGroups[i];
                writer.Write(FaceGroup.StartIndex);
                writer.Write(FaceGroup.NumFaces);

                // Write Material Instance
                MT_MaterialInstance MaterialInstance = FaceGroup.Material;
                writer.Write((int)MaterialInstance.MaterialFlags);
                StringHelpers.WriteString8(writer, MaterialInstance.Name);

                if (MaterialInstance.MaterialFlags.HasFlag(MT_MaterialInstanceFlags.HasDiffuse))
                {
                    StringHelpers.WriteString8(writer, MaterialInstance.DiffuseTexture);
                }
            }

            // Attempt to write Indices
            writer.Write(Indices.Length);
            for (int i = 0; i < Indices.Length; i++)
            {
                writer.Write(Indices[i]);
            }
        }

        //~ IValidator Interface
        protected override bool InternalValidate(MT_ValidationTracker TrackerObject)
        {
            bool bValidity = true;

            if(Vertices.Length == 0)
            {
                AddMessage(MT_MessageType.Error, "This collision object has no vertices.");
                bValidity = false;
            }

            if (Indices.Length == 0)
            {
                AddMessage(MT_MessageType.Error, "This collision object has no indices");
                bValidity = false;
            }

            if (FaceGroups.Length == 0)
            {
                AddMessage(MT_MessageType.Error, "This collision object has no face groups.");
                bValidity = false;
            }

            foreach(var FaceGroup in FaceGroups)
            {
                bool bIsValid = FaceGroup.ValidateObject(TrackerObject);
                bValidity &= bIsValid;
            }

            return bValidity;
        }
    }
}

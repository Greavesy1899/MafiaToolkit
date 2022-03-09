using System;
using System.Collections.Generic;
using Gibbed.Illusion.FileFormats.Hashing;
using ResourceTypes.FrameResource;
using ResourceTypes.BufferPools;
using Utils.Types;
using ResourceTypes.ModelHelpers.ModelExporter;
using System.IO;
using Utils.Settings;
using System.Windows.Forms;
using ResourceTypes.Materials;
using System.Numerics;
using Vortice.Mathematics;
using Utils.VorticeUtils;
using System.Diagnostics;

namespace Utils.Models
{
    public class ModelWrapper
    {
        FrameObjectSingleMesh frameMesh; //model can be either "FrameObjectSingleMesh"
        FrameObjectModel frameModel; //Or "FrameObjectModel"
        IndexBuffer[] indexBuffers; //Holds the buffer which will then be saved/replaced later
        VertexBuffer[] vertexBuffers; //Holds the buffers which will then be saved/replaced later
        MT_Object modelObject;
        private bool useSingleMesh; //False means ModelMesh, True means SingleMesh;

        public FrameObjectSingleMesh FrameMesh
        {
            get { return frameMesh; }
        }

        public IndexBuffer[] IndexBuffers
        {
            get { return indexBuffers; }
            set { indexBuffers = value; }
        }

        public VertexBuffer[] VertexBuffers
        {
            get { return vertexBuffers; }
            set { vertexBuffers = value; }
        }

        public MT_Object ModelObject
        {
            get { return modelObject; }
            set { modelObject = value; }
        }

        public ModelWrapper(FrameObjectSingleMesh frameMesh, IndexBuffer[] indexBuffers, VertexBuffer[] vertexBuffers)
        {
            this.frameMesh = frameMesh;
            this.indexBuffers = indexBuffers;
            this.vertexBuffers = vertexBuffers;
            modelObject = new MT_Object();
            modelObject.ObjectName = frameMesh.Name.ToString();
            //model.AOTexture = frameMesh.OMTextureHash.String; // missing support
            modelObject.BuildFromCooked(frameMesh, vertexBuffers, indexBuffers);
        }

        public ModelWrapper(FrameObjectModel frameModel, IndexBuffer[] indexBuffers, VertexBuffer[] vertexBuffers)
        {
            this.frameModel = frameModel;
            this.indexBuffers = indexBuffers;
            this.vertexBuffers = vertexBuffers;
            modelObject = new MT_Object();
            modelObject.ObjectName = frameModel.Name.ToString();
            //model.AOTexture = frameMesh.OMTextureHash.String; // missing support
            modelObject.BuildFromCooked(frameModel, vertexBuffers, indexBuffers);
        }

        public ModelWrapper(FrameObjectBase FrameObject)
        {
            modelObject = new MT_Object();
            modelObject.ObjectName = FrameObject.Name.ToString();
            modelObject.BuildStandardObject(FrameObject);
        }

        /// <summary>
        /// Construct an empty model.
        /// </summary>
        public ModelWrapper()
        {
            ModelObject = new MT_Object();
        }

        public void SetFrameMesh(FrameObjectSingleMesh Mesh)
        {
            frameMesh = Mesh;
        }

        private float GetMaxInverted(float Val1, float Val2)
        {
            float AbsVal1 = MathF.Abs(Val1);
            float AbsVal2 = MathF.Abs(Val2);
            return -MathF.Max(AbsVal1, AbsVal2);
        }

        /// <summary>
        /// Update decompression offset and position.
        /// </summary>
        public void CalculateDecompression()
        {
            FrameGeometry frameGeometry = frameMesh.Geometry;

            BoundingBox bounds = new BoundingBox();
            bounds.SetMinimum(frameMesh.Boundings.Minimum);
            bounds.SetMaximum(frameMesh.Boundings.Maximum);
            frameGeometry.DecompressionOffset = new Vector3(bounds.Minimum.X, bounds.Minimum.Y, bounds.Minimum.Z);

            double MaxX = bounds.Maximum.X - bounds.Minimum.X;
            double MaxY = bounds.Maximum.Y - bounds.Minimum.Y;
            double MaxZ = bounds.Maximum.Z - bounds.Minimum.Z;

            float Max = (float)Math.Max(MaxX, Math.Max(MaxY, MaxZ * 2.0f));
            frameGeometry.DecompressionFactor = Max / 0xFFFF;

            double fMaxSize = Math.Max(MaxX, Math.Max(MaxY, MaxZ * 2.0f));
            Console.WriteLine("Decompress value before: " + fMaxSize);
            double result = Math.Log(fMaxSize) / Math.Log(2.0f);
            double pow = Math.Ceiling(result);
            double factor = Math.Pow(2.0f, pow);
            float OldOutput = (float)(factor / 0x10000);
            float NewOutput = frameGeometry.DecompressionFactor;

            Debug.WriteLine(string.Format("{0} => {1}", OldOutput, NewOutput));
        }

        public void BuildIndexBuffer()
        {
            if (ModelObject.Lods == null)
            {
                return;
            }

            for (int i = 0; i < ModelObject.Lods.Length; i++)
            {
                MT_Lod LodObject = ModelObject.Lods[i];
                var indexFormat = (LodObject.Over16BitLimit() ? 2 : 1);
                IndexBuffers[i] = new IndexBuffer(FNV64.Hash("M2TK." + ModelObject.ObjectName + ".IB" + i));
                indexBuffers[i].SetData(LodObject.Indices);
                indexBuffers[i].SetFormat(indexFormat);
            }
        }

        /// <summary>
        /// Builds vertex buffer from the mesh data.
        /// </summary>
        public void BuildVertexBuffer()
        {
            FrameGeometry frameGeometry = frameMesh.Geometry;

            if (frameGeometry.LOD == null)
            {
                return;
            }

            for (int i = 0; i != ModelObject.Lods.Length; i++)
            {
                FrameLOD frameLod = frameGeometry.LOD[i];
                int vertexSize;
                Dictionary<VertexFlags, FrameLOD.VertexOffset> vertexOffsets = frameLod.GetVertexOffsets(out vertexSize);
                byte[] vBuffer = new byte[vertexSize * frameLod.NumVerts];

                for (int v = 0; v != ModelObject.Lods[i].Vertices.Length; v++)
                {
                    Vertex vert = ModelObject.Lods[i].Vertices[v];

                    if (frameLod.VertexDeclaration.HasFlag(VertexFlags.Position))
                    {
                        int startIndex = v * vertexSize + vertexOffsets[VertexFlags.Position].Offset;
                        vert.WritePositionData(vBuffer, startIndex, frameGeometry.DecompressionFactor, frameGeometry.DecompressionOffset);
                    }

                    if (frameLod.VertexDeclaration.HasFlag(VertexFlags.Tangent))
                    {
                        int startIndex = v * vertexSize + vertexOffsets[VertexFlags.Position].Offset;
                        vert.WriteTangentData(vBuffer, startIndex);
                    }

                    if (frameLod.VertexDeclaration.HasFlag(VertexFlags.Normals))
                    {
                        int startIndex = v * vertexSize + vertexOffsets[VertexFlags.Normals].Offset;
                        vert.WriteNormalData(vBuffer, startIndex);
                    }

                    if (frameLod.VertexDeclaration.HasFlag(VertexFlags.Color))
                    {
                        int startIndex = v * vertexSize + vertexOffsets[VertexFlags.Color].Offset;
                        vert.WriteColourData(vBuffer, startIndex, 0);
                    }

                    if (frameLod.VertexDeclaration.HasFlag(VertexFlags.Color1))
                    {
                        int startIndex = v * vertexSize + vertexOffsets[VertexFlags.Color1].Offset;
                        vert.WriteColourData(vBuffer, startIndex, 1);
                    }

                    if (frameLod.VertexDeclaration.HasFlag(VertexFlags.DamageGroup))
                    {
                        int startIndex = v * vertexSize + vertexOffsets[VertexFlags.DamageGroup].Offset;
                        vert.WriteDamageGroup(vBuffer, startIndex);
                    }

                    if (frameLod.VertexDeclaration.HasFlag(VertexFlags.TexCoords0))
                    {
                        int startIndex = v * vertexSize + vertexOffsets[VertexFlags.TexCoords0].Offset;
                        vert.WriteUvData(vBuffer, startIndex, 0);
                    }

                    if (frameLod.VertexDeclaration.HasFlag(VertexFlags.TexCoords1))
                    {
                        int startIndex = v * vertexSize + vertexOffsets[VertexFlags.TexCoords1].Offset;
                        vert.WriteUvData(vBuffer, startIndex, 1);
                    }

                    if (frameLod.VertexDeclaration.HasFlag(VertexFlags.TexCoords2))
                    {
                        int startIndex = v * vertexSize + vertexOffsets[VertexFlags.TexCoords2].Offset;
                        vert.WriteUvData(vBuffer, startIndex, 2);
                    }

                    if (frameLod.VertexDeclaration.HasFlag(VertexFlags.ShadowTexture))
                    {
                        int startIndex = v * vertexSize + vertexOffsets[VertexFlags.ShadowTexture].Offset;
                        vert.WriteUvData(vBuffer, startIndex, 3);
                    }

                }

                VertexBuffers[i] = new VertexBuffer(FNV64.Hash("M2TK." + ModelObject.ObjectName + ".VB" + i));
                VertexBuffers[i].Data = vBuffer;
            }
        }

        public void ReadObjectFromFbx(string file)
        {
            // Change extension, pass to M2FBX
            string m2tFile = file.Remove(file.Length - 4, 4) + ".m2t";
            int result = FBXHelper.ConvertFBX(file, m2tFile);

            // Read the MT object.
            ModelObject = MT_ObjectHandler.ReadObjectFromFile(file);

            // Delete the recently-created MT file.
            if (File.Exists(m2tFile))
            {
                File.Delete(m2tFile);
            }
        }

        public void ReadObjectFromM2T(string file)
        {
            ModelObject = MT_ObjectHandler.ReadObjectFromFile(file);
        }

        public void ExportObject()
        {
            string SavePath = ToolkitSettings.ExportPath;

            if (!Directory.Exists(ToolkitSettings.ExportPath))
            {
                // Ask if we can create it
                DialogResult Result = MessageBox.Show("The path does not exist. Do you want to create it?", "Toolkit", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (Result == DialogResult.Yes)
                {
                    Directory.CreateDirectory(SavePath);
                }
                else
                {
                    // Can't export file with no valid directory.
                    MessageBox.Show("Cannot export a mesh with no valid directory. Please change your directory.", "Toolkit", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            // Export Object
            string FileName = ToolkitSettings.ExportPath + "\\" + ModelObject.ObjectName;
            ExportBundle(FileName);
            switch (ToolkitSettings.Format)
            {
                case 0:
                    ExportObjectToFbx(FileName, false);
                    break;
                case 1:
                    ExportObjectToFbx(FileName, true);
                    break;
                default:
                    break;
            }
        }

        private void ExportObjectToFbx(string File, bool bIsBinary)
        {
            FBXHelper.ConvertMTB(File + ".mtb", File + ".fbx");
        }

        private void ExportBundle(string FileToWrite)
        {
            MT_ObjectBundle BundleObject = new MT_ObjectBundle();
            BundleObject.Objects = new MT_Object[1];
            BundleObject.Objects[0] = ModelObject;

            using (BinaryWriter writer = new BinaryWriter(File.Open(FileToWrite + ".mtb", FileMode.Create)))
            {
                MT_ObjectHandler.WriteBundleToFile(writer, BundleObject);
            }
        }

        public void UpdateObjectsFromModel()
        {
            FrameGeometry frameGeometry = frameMesh.Geometry;
            FrameMaterial frameMaterial = frameMesh.Material;

            frameGeometry.NumLods = (byte)ModelObject.Lods.Length;

            if (frameGeometry.LOD == null)
            {
                frameGeometry.LOD = new FrameLOD[ModelObject.Lods.Length];
            }

            frameMaterial.NumLods = (byte)ModelObject.Lods.Length;
            frameMaterial.LodMatCount = new int[ModelObject.Lods.Length];
            frameMaterial.Materials = new List<MaterialStruct[]>();

            for (int x = 0; x < ModelObject.Lods.Length; x++)
            {
                frameMaterial.Materials.Add(new MaterialStruct[frameMaterial.LodMatCount[x]]);
            }
            for (int x = 0; x < ModelObject.Lods.Length; x++)
            {
                MT_Lod LodObject = ModelObject.Lods[x];

                var lod = new FrameLOD();
                lod.Distance = 1E+12f;
                lod.BuildNewPartition();
                lod.BuildNewMaterialSplit();
                lod.SplitInfo.NumVerts = LodObject.Vertices.Length;
                lod.NumVerts = LodObject.Vertices.Length;
                lod.SplitInfo.NumFaces = LodObject.Indices.Length / 3;
                lod.VertexDeclaration = LodObject.VertexDeclaration;

                //burst split info.
                lod.SplitInfo.IndexStride = (LodObject.Over16BitLimit() ? 4 : 2);
                lod.SplitInfo.NumMatSplit = LodObject.FaceGroups.Length;
                lod.SplitInfo.NumMatBurst = LodObject.FaceGroups.Length;
                lod.SplitInfo.MaterialSplits = new FrameLOD.MaterialSplit[LodObject.FaceGroups.Length];
                lod.SplitInfo.MaterialBursts = new FrameLOD.MaterialBurst[LodObject.FaceGroups.Length];
                frameGeometry.LOD[x] = lod;

                int faceIndex = 0;
                frameMaterial.LodMatCount[x] = LodObject.FaceGroups.Length;
                frameMaterial.Materials[x] = new MaterialStruct[LodObject.FaceGroups.Length];
                for (int i = 0; i < LodObject.FaceGroups.Length; i++)
                {
                    MT_FaceGroup CurrentFaceGroup = LodObject.FaceGroups[i];

                    frameMaterial.Materials[x][i] = new MaterialStruct();
                    frameMaterial.Materials[x][i].StartIndex = (int)CurrentFaceGroup.StartIndex;
                    frameMaterial.Materials[x][i].NumFaces = (int)CurrentFaceGroup.NumFaces;
                    frameMaterial.Materials[x][i].Unk3 = 0;

                    IMaterial FoundMaterial = MaterialsManager.LookupMaterialByName(CurrentFaceGroup.Material.Name);
                    if (FoundMaterial != null)
                    {
                        frameMaterial.Materials[x][i].MaterialName = FoundMaterial.GetMaterialName();
                        frameMaterial.Materials[x][i].MaterialHash = FoundMaterial.GetMaterialHash();
                    }

                    faceIndex += (int)CurrentFaceGroup.NumFaces;

                    frameGeometry.LOD[x].SplitInfo.MaterialBursts[i].Bounds = new short[6]
                    {
                        Convert.ToInt16(CurrentFaceGroup.Bounds.Minimum.X),
                        Convert.ToInt16(CurrentFaceGroup.Bounds.Minimum.Y),
                        Convert.ToInt16(CurrentFaceGroup.Bounds.Minimum.Z),
                        Convert.ToInt16(CurrentFaceGroup.Bounds.Maximum.X),
                        Convert.ToInt16(CurrentFaceGroup.Bounds.Maximum.Y),
                        Convert.ToInt16(CurrentFaceGroup.Bounds.Maximum.Z)
                    };

                    // TODO: Figure out what this actually means.
                    if (ModelObject.Lods[x].FaceGroups.Length == 1)
                    {
                        string MaterialName = ModelObject.Lods[0].FaceGroups[0].Material.Name;
                        frameGeometry.LOD[x].SplitInfo.Hash = FNV64.Hash(MaterialName);
                    }

                    frameGeometry.LOD[x].SplitInfo.MaterialBursts[i].FirstIndex = 0;
                    frameGeometry.LOD[x].SplitInfo.MaterialBursts[i].LeftIndex = -1;
                    frameGeometry.LOD[x].SplitInfo.MaterialBursts[i].RightIndex = -1;
                    frameGeometry.LOD[x].SplitInfo.MaterialBursts[i].SecondIndex =
                        Convert.ToUInt16(CurrentFaceGroup.NumFaces - 1);
                    frameGeometry.LOD[x].SplitInfo.MaterialSplits[i].BaseIndex = (int)CurrentFaceGroup.StartIndex;
                    frameGeometry.LOD[x].SplitInfo.MaterialSplits[i].FirstBurst = i;
                    frameGeometry.LOD[x].SplitInfo.MaterialSplits[i].NumBurst = 1;
                }
            }
        }

        public void CreateObjectsFromModel()
        {
            FrameGeometry frameGeometry = frameMesh.Geometry;
            FrameMaterial frameMaterial = frameMesh.Material;

            //set lods for all data.
            indexBuffers = new IndexBuffer[ModelObject.Lods.Length];
            vertexBuffers = new VertexBuffer[ModelObject.Lods.Length];

            List<Vertex[]> vertData = new List<Vertex[]>();
            for (int i = 0; i != ModelObject.Lods.Length; i++)
            {
                vertData.Add(ModelObject.Lods[i].Vertices);
            }

            frameMesh.Boundings = BoundingBoxExtenders.CalculateBounds(vertData);
            frameMaterial.Bounds = frameMesh.Boundings;
            CalculateDecompression();
            UpdateObjectsFromModel();
            BuildIndexBuffer();
            BuildVertexBuffer();

            for (int i = 0; i < ModelObject.Lods.Length; i++)
            {
                var lod = frameGeometry.LOD[i];

                var size = 0;
                lod.GetVertexOffsets(out size);
                if (vertexBuffers[i].Data.Length != (size * lod.NumVerts)) throw new SystemException();
                lod.IndexBufferRef = new HashName("M2TK." + ModelObject.ObjectName + ".IB" + i);
                lod.VertexBufferRef = new HashName("M2TK." + ModelObject.ObjectName + ".VB" + i);
            }

            // TODO: Remove this code from this function and insert into CreateSkinnedObjectsFromModel.
            // Then get FrameObjectModel to direct call the aforementioned function.
            // This function should then get called by that function, and leave this for static assets.
            if (ModelObject.ObjectFlags.HasFlag(MT_ObjectFlags.HasSkinning))
            {
                CreateSkinnedObjectsFromModel();
            }
        }

        public void CreateSkinnedObjectsFromModel()
        {
            // MT_Object data
            MT_Skeleton SkeletonObject = ModelObject.Skeleton;

            // Get game-asset data
            FrameObjectModel ModelFrame = (frameMesh as FrameObjectModel);
            FrameSkeleton SkeletonBlock = ModelFrame.GetSkeletonObject();
            FrameSkeletonHierachy HierarchyBlock = ModelFrame.GetSkeletonHierarchyObject();
            FrameBlendInfo BlendInfoBlock = ModelFrame.GetBlendInfoObject();

            HierarchyBlock.LastChildIndices = new byte[255];
            HierarchyBlock.ParentIndices = new byte[255];
            HierarchyBlock.UnkData = new byte[255];

            List<int> SkeletonSequence = new List<int>();
            SkeletonSequence.Add(0);

            int[] JointBoneIDs = new int[SkeletonObject.Joints.Length];
            int[] IDRemapTable = new int[1024];

            for (int i = 0; i < SkeletonSequence.Count; i++)
            {
                MT_Joint JointObject = SkeletonObject.Joints[i];

                // handle last child indices array
                HierarchyBlock.LastChildIndices[i] = (byte)(i > 0 ? HierarchyBlock.LastChildIndices[i - 1] : 0);

                for (int z = 0; z < SkeletonObject.Joints.Length; z++)
                {
                    bool bCheckOne = SkeletonObject.Joints[z].ParentJointIndex == JointBoneIDs[i];
                    if (bCheckOne)
                    {
                        int NewSlot = SkeletonSequence.Count;
                        HierarchyBlock.ParentIndices[NewSlot] = (byte)i;
                        HierarchyBlock.LastChildIndices[i] = (byte)NewSlot;
                        JointBoneIDs[z] = NewSlot;

                        if (JointBoneIDs[z] >= 0)
                        {
                            IDRemapTable[JointBoneIDs[z]] = NewSlot;
                        }

                        SkeletonSequence.Add(z);
                    }
                }

                HierarchyBlock.UnkData[i] = (byte)i;
            }

            /*
            int NumJoints = SkeletonObject.Joints.Length;
            SkeletonBlock.BoneNames = new HashName[NumJoints];
            SkeletonBlock.NumBones = new int[4];
            SkeletonBlock.UnkLodData = new int[1];
            SkeletonBlock.BoneLODUsage = new byte[NumJoints];

            SkeletonBlock.NumBlendIDs = NumJoints;
            SkeletonBlock.NumUnkCount2 = NumJoints;
            SkeletonBlock.UnkLodData[0] = NumJoints;


            for (int i = 0; i < 4; i++)
            {
                SkeletonBlock.NumBones[i] = NumJoints;
            }

            for (int i = 0; i < NumJoints; i++)
            {
                HashName bone = new HashName();
                bone.Set(SkeletonObject.Joints[i].Name);
                SkeletonBlock.BoneNames[i] = bone;

                if (ModelObject.Lods.Length == 1)
                {
                    SkeletonBlock.BoneLODUsage[i] = 1;
                }
            }

            HierarchyBlock.ParentIndices = new byte[NumJoints];
            HierarchyBlock.LastChildIndices = new byte[NumJoints];
            HierarchyBlock.UnkData = new byte[NumJoints];
            SkeletonBlock.JointTransforms = new Matrix[NumJoints];

            HierarchyBlock.UnkData[0] = (byte)(NumJoints + 1);

            for (int i = 0; i < NumJoints; i++)
            {
                MT_Joint JointObject = SkeletonObject.Joints[i];

                HierarchyBlock.ParentIndices[i] = (byte)JointObject.ParentJointIndex;
                HierarchyBlock.UnkData[i] = (byte)(i != NumJoints ? i : 0);
                SkeletonBlock.JointTransforms[i] = MatrixExtensions.SetMatrix(JointObject.Rotation, JointObject.Scale, JointObject.Position);
            }*/
        }
    }
}

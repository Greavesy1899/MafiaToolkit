using Gibbed.Illusion.FileFormats.Hashing;
using ResourceTypes.BufferPools;
using ResourceTypes.FrameResource;
using ResourceTypes.Materials;
using ResourceTypes.ModelHelpers.ModelExporter;
using SharpGLTF.Schema2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Utils.Types;
using Utils.VorticeUtils;
using Vortice.Mathematics;

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

            modelObject = MT_Object.TryBuildObject(frameMesh);
        }

        public ModelWrapper(FrameObjectModel frameModel, IndexBuffer[] indexBuffers, VertexBuffer[] vertexBuffers)
        {
            this.frameModel = frameModel;
            this.indexBuffers = indexBuffers;
            this.vertexBuffers = vertexBuffers;

            modelObject = MT_Object.TryBuildObject(frameModel);
        }

        public ModelWrapper(FrameObjectBase FrameObject)
        {
            modelObject = MT_Object.TryBuildObject(FrameObject);
        }

        public ModelWrapper(FrameHeaderScene FrameScene)
        {
            modelObject = MT_Object.TryBuildObject(FrameScene);
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

        public (float DecompressionFactor, Vector3 DecompressionOffset) GetDecompFactor(BoundingBox boundingBox)
        {
            float Size = boundingBox.Depth * 2.0f;

            float decompressionFactor = Size / 65520.0f;
            float offset = 8 * decompressionFactor;

            Vector3 decompressionOffset = new Vector3(boundingBox.Min.X - offset, boundingBox.Min.Y - offset, boundingBox.Min.Z - offset / 2.0f);

            Vector3 MinusOffset = boundingBox.Max - decompressionOffset;
            Vector3 Factorised = MinusOffset / decompressionFactor;

            if (Factorised.X > ushort.MaxValue || Factorised.Y > ushort.MaxValue || Factorised.Z > ushort.MaxValue)
            {
                decompressionFactor = 256.0f / 65536.0f;
                offset = 4 * decompressionFactor;
            }

            MinusOffset = boundingBox.Max - decompressionOffset;
            Factorised = MinusOffset / decompressionFactor;

            if (Factorised.X > ushort.MaxValue || Factorised.Y > ushort.MaxValue || Factorised.Z > ushort.MaxValue)
            {
                List<float> values = new() { boundingBox.Height, boundingBox.Width, boundingBox.Depth };

                Size = values.Max();

                decompressionFactor = Size / 65520.0f;
                offset = 8 * decompressionFactor;
            }

            decompressionOffset = new Vector3(boundingBox.Min.X - offset, boundingBox.Min.Y - offset, boundingBox.Min.Z - offset / 2.0f);

            return (decompressionFactor, decompressionOffset);
        }

        /// <summary>
        /// Update decompression offset and position.
        /// </summary>
        public void CalculateDecompression()
        {
            FrameGeometry frameGeometry = frameMesh.Geometry;
            frameGeometry.DecompressionFactor = 1.525879E-05f;
            frameGeometry.DecompressionOffset = Vector3.Zero;

            BoundingBox bounds = new BoundingBox();
            bounds.SetMinimum(frameMesh.Boundings.Min);
            bounds.SetMaximum(frameMesh.Boundings.Max);

            (float, Vector3) Values = GetDecompFactor(bounds);
            frameGeometry.DecompressionFactor = Values.Item1;
            frameGeometry.DecompressionOffset = Values.Item2;
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

                    // TODO: delete once validation is complete
                    byte[] data = new byte[vertexSize];
                    Array.Copy(vBuffer, (v * vertexSize), data, 0, vertexSize);
                    Vertex TestVertex = VertexTranslator.DecompressVertex(data, frameLod.VertexDeclaration, frameGeometry.DecompressionOffset, frameGeometry.DecompressionFactor, vertexOffsets);
                }

                VertexBuffers[i] = new VertexBuffer(FNV64.Hash("M2TK." + ModelObject.ObjectName + ".VB" + i));
                VertexBuffers[i].Data = vBuffer;
            }
        }

        public void ExportObject(string SavePath, int FilterIndex)
        {          
            if(ModelObject != null)
            {
                ExportBundle(SavePath);
            }
        }

        private void ExportBundle(string FileToWrite)
        {
            MT_ObjectBundle BundleObject = new MT_ObjectBundle();
            BundleObject.Objects = new MT_Object[1];
            BundleObject.Objects[0] = ModelObject;

            // TODO: Default is ValidationMode.Strict, which faulters on Normals
            WriteSettings WriteContext = new WriteSettings();
            WriteContext.Validation = SharpGLTF.Validation.ValidationMode.TryFix;
            WriteContext.JsonIndented = true;

            ModelRoot CompiledModel = BundleObject.BuildGLTF();
            CompiledModel.SaveGLB(FileToWrite, WriteContext);
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
                        Convert.ToInt16(CurrentFaceGroup.Bounds.Min.X),
                        Convert.ToInt16(CurrentFaceGroup.Bounds.Min.Y),
                        Convert.ToInt16(CurrentFaceGroup.Bounds.Min.Z),
                        Convert.ToInt16(CurrentFaceGroup.Bounds.Max.X),
                        Convert.ToInt16(CurrentFaceGroup.Bounds.Max.Y),
                        Convert.ToInt16(CurrentFaceGroup.Bounds.Max.Z)
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

            // Apply bounding box for FrameGeometry and FrameMaterial
            BoundingBox NewBounds = ModelObject.GetLODBounds();
            frameMesh.Boundings = NewBounds;
            frameMaterial.Bounds = NewBounds;

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
            // TEMP - generate remappings and apply to all LODs + Skinned mesh
            var RemappedBlendInfos = ModelObject.TestGenerateBoneRemappings();

            // MT_Object data
            MT_Skeleton SkeletonObject = ModelObject.Skeleton;

            // Get game-asset data
            FrameObjectModel ModelFrame = (frameMesh as FrameObjectModel);
            FrameSkeleton SkeletonBlock = ModelFrame.GetSkeletonObject();
            FrameSkeletonHierachy HierarchyBlock = ModelFrame.GetSkeletonHierarchyObject();
            FrameBlendInfo BlendInfoBlock = ModelFrame.GetBlendInfoObject();

            // Build skeleton hierarchy block
            HierarchyBlock.LastChildIndices = new byte[SkeletonObject.Joints.Length];
            HierarchyBlock.ParentIndices = new byte[SkeletonObject.Joints.Length];
            HierarchyBlock.Unk01 = 0;

            // Skeleton block - allocate enough for all joints
            SkeletonBlock.BoneNames = new HashName[SkeletonObject.Joints.Length];
            SkeletonBlock.JointTransforms = new Matrix4x4[SkeletonObject.Joints.Length];
            SkeletonBlock.BoneLODUsage = new byte[SkeletonObject.Joints.Length];

            // Not sure what UnkData is - but the assumption is that the first slot has number of bones
            // Then a list from 1 - N Bones. Last slot in the array is 0.
            HierarchyBlock.UnkData = new byte[SkeletonObject.Joints.Length + 1];
            byte UnkDataItr = 1;

            for (int i = 0; i < SkeletonObject.Joints.Length; i++)
            {
                MT_Joint CurrentJoint = SkeletonObject.Joints[i];

                // Work on Skeleton Block
                SkeletonBlock.BoneNames[i] = new HashName(CurrentJoint.Name);

                // generate joint transform from MT_Joint
                Matrix4x4 RotTransform = Matrix4x4.CreateFromQuaternion(CurrentJoint.Rotation);
                Matrix4x4 LocTransform = Matrix4x4.CreateScale(CurrentJoint.Scale);
                Matrix4x4 SclTransform = Matrix4x4.CreateTranslation(CurrentJoint.Position);
                Matrix4x4 XForm = RotTransform * LocTransform * SclTransform;
                SkeletonBlock.JointTransforms[i] = XForm;

                // Create usage list. UsageFlags should automatically be generated upon loading the GLTF.
                SkeletonBlock.BoneLODUsage[i] = (byte)CurrentJoint.UsageFlags;

                // TODO: World transform -> how tf do you do this??

                // Work on Hierarchy Block
                HierarchyBlock.ParentIndices[i] = (CurrentJoint.ParentJointIndex == -1 ? byte.MaxValue : (byte)CurrentJoint.ParentJointIndex);
                HierarchyBlock.UnkData[i + 1] = UnkDataItr++;
            }

            // Hierarchy Block - Finish off UnkData by assigning first and last slot
            HierarchyBlock.UnkData[0] = (byte)SkeletonObject.Joints.Length;
            HierarchyBlock.UnkData[SkeletonObject.Joints.Length] = 0;

            // Skeleton Block - Finalise remaining data
            // Fill in additional data
            SkeletonBlock.NumLods = ModelObject.Lods.Length;
            SkeletonBlock.NumUnkCount2 = SkeletonObject.Joints.Length;
            SkeletonBlock.IDType = 3; // unknown - 3 is typically hashes? For cars and crane.

            // for some reason all slots in this array equal same amount of bones..
            SkeletonBlock.NumBones = new int[4];
            SkeletonBlock.NumBones[0] = SkeletonBlock.NumBones[1] = SkeletonBlock.NumBones[2] = SkeletonBlock.NumBones[3] = SkeletonObject.Joints.Length;

            // TODO: Once BlendInfo is done, there are a few pieces of data which must be stored in Skeleton
            SkeletonBlock.NumBlendIDs = 0;
            SkeletonBlock.MappingForBlendingInfos = null; // maybe in here too, one for each lod?
            SkeletonBlock.UnkLodData = null; // again, one for each lod, number of blend infos?

            // now lets begin generating skinned data for each LOD
            BlendInfoBlock.BoneIndexInfos = new FrameBlendInfo.BoneIndexInfo[ModelObject.Lods.Length];         
            for(int Idx = 0; Idx < ModelObject.Lods.Length; Idx++)
            {
                MT_Lod CurrentLod = ModelObject.Lods[Idx];
                FrameBlendInfo.BoneIndexInfo LodIndexInfo = new FrameBlendInfo.BoneIndexInfo();
                LodIndexInfo.IDs = RemappedBlendInfos[Idx].IDs;
                LodIndexInfo.NumIDs = RemappedBlendInfos[Idx].NumIDs;
                LodIndexInfo.BonesPerPool = RemappedBlendInfos[Idx].BonesPerPool;

                // generate each weighted info for each facegroup found in the LOD
                LodIndexInfo.SkinnedMaterialInfo = new FrameBlendInfo.SkinnedMaterialInfo[CurrentLod.FaceGroups.Length];
                for (int MatIdx = 0; MatIdx < LodIndexInfo.SkinnedMaterialInfo.Length; MatIdx++)
                {
                    MT_FaceGroup CurrentFaceGroup = CurrentLod.FaceGroups[MatIdx];

                    FrameBlendInfo.SkinnedMaterialInfo SkinnedMatInfo = new FrameBlendInfo.SkinnedMaterialInfo();
                    SkinnedMatInfo.NumWeightsPerVertex = CurrentFaceGroup.WeightsPerVertex;

                    // TODO: We currently do not understand BoneSlot mappings therefore default to zero
                    SkinnedMatInfo.AssignedPoolIndex = RemappedBlendInfos[Idx].SkinnedMaterialInfo[MatIdx].AssignedPoolIndex;

                    LodIndexInfo.SkinnedMaterialInfo[MatIdx] = SkinnedMatInfo;
                }

                // assign
                BlendInfoBlock.BoneIndexInfos[Idx] = LodIndexInfo;
            }
        }
    }
}

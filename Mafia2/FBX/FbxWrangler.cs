using Fbx;
using System;
using System.Collections.Generic;

namespace Mafia2.FBX
{
    public class FbxWrangler
    {
        //not the best idea, but hey ho, it works.
        private static List<ConnectionStruct> connections = new List<ConnectionStruct>();

        public class FbxPresetNodes
        {
            /// <summary>
            /// Builds 'FBXHeaderExtension' for new FBX.
            /// </summary>
            public static FbxNode BuildHeaderExtensionNode()
            {
                FbxNode node = new FbxNode();
                node.Name = "FBXHeaderExtension";

                //FBXHeaderVersion, FBXVersion, EncryptionType
                node.Nodes.Add(new FbxNode().CreateNode("FBXHeaderVersion", 1003));
                node.Nodes.Add(new FbxNode().CreateNode("FBXVersion", 7400));
                node.Nodes.Add(new FbxNode().CreateNode("EncryptionType", 0));

                //CreationTimeStamp
                FbxNode creationTimeStamp = new FbxNode() { Name = "CreationTimeStamp" };
                creationTimeStamp.Nodes.Add(new FbxNode().CreateNode("Version", 1000));
                creationTimeStamp.Nodes.Add(new FbxNode().CreateNode("Year", DateTime.Now.Year));
                creationTimeStamp.Nodes.Add(new FbxNode().CreateNode("Month", DateTime.Now.Month));
                creationTimeStamp.Nodes.Add(new FbxNode().CreateNode("Day", DateTime.Now.Month));
                creationTimeStamp.Nodes.Add(new FbxNode().CreateNode("Hour", DateTime.Now.Hour));
                creationTimeStamp.Nodes.Add(new FbxNode().CreateNode("Minute", DateTime.Now.Minute));
                creationTimeStamp.Nodes.Add(new FbxNode().CreateNode("Second", DateTime.Now.Second));
                creationTimeStamp.Nodes.Add(new FbxNode().CreateNode("Millisecond", DateTime.Now.Millisecond));
                node.Nodes.Add(creationTimeStamp);

                //Creator
                node.Nodes.Add(new FbxNode().CreateNode("Creator", "FBX SDK/FBX Plugins version 2017.0.1"));

                //SceneInfo
                FbxNode sceneInfo = new FbxNode().CreateNode("SceneInfo", "SceneInfo::GlobalInfo");
                sceneInfo.Properties.Add("UserData");
                sceneInfo.Nodes.Add(new FbxNode().CreateNode("Type", "UserData"));
                sceneInfo.Nodes.Add(new FbxNode().CreateNode("Version", 100));
                //MetaData CHILD of SceneInfo.
                FbxNode metaData = new FbxNode().CreateNode("MetaData", null);
                metaData.Nodes.Add(new FbxNode().CreateNode("Title", ""));
                metaData.Nodes.Add(new FbxNode().CreateNode("Subject", ""));
                metaData.Nodes.Add(new FbxNode().CreateNode("Author", ""));
                metaData.Nodes.Add(new FbxNode().CreateNode("Keywords", ""));
                metaData.Nodes.Add(new FbxNode().CreateNode("Revision", ""));
                metaData.Nodes.Add(new FbxNode().CreateNode("Comment", ""));
                sceneInfo.Nodes.Add(metaData);
                //Properties70 CHILD of SceneInfo
                FbxNode properties70 = new FbxNode().CreateNode("Properties70", null);
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "DocumentUrl", "KString", "Url", "", "C:\\URL.FBX" }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "SrcDocumentUrl", "KString", "Url", "", "C:\\URL.FBX" }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "Original", "Compound", "", "" }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "Original|ApplicationVendor", "KString", "", "", "Autodesk" }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "Original|ApplicationName", "KString", "", "", "3ds Max" }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "Original|ApplicationVersion", "KString", "", "", "2017" }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "Original|DateTime_GMT", "DateTime", "", "", DateTime.Now.ToString() }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "Original|FileName", "KString", "", "", "C:\\URL.FBX" }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "LastSaved", "Compound", "", "" }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "LastSaved|ApplicationVendor", "KString", "", "", "Autodesk" }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "LastSaved|ApplicationName", "KString", "", "", "3ds Max" }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "LastSaved|ApplicationVersion", "KString", "", "", "2017" }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "LastSaved|DateTime_GMT", "DateTime", "", "", DateTime.Now.ToString() }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "Original|ApplicationActiveProject", "KString", "", "", "C:\\URL.FBX" }));
                sceneInfo.Nodes.Add(properties70);
                node.Nodes.Add(sceneInfo);
                return node;
            }

            /// <summary>
            /// Builds 'CreationTime' for new FBX.
            /// </summary>
            public static void BuildCreationTimeNode()
            {
                FbxNode node = new FbxNode();
                node.Name = "CreationTime";
                node.Value = DateTime.Now;
                node.Properties.Add(node.Value);
            }

            /// <summary>
            /// Builds 'Creator' for new FBX
            /// </summary>
            public static void BuildCreatorNode()
            {
                FbxNode node = new FbxNode();
                node.Name = "CreatorNode";
                node.Value = "Toolkit Wranger v1";
                node.Properties.Add(node.Value);
            }

            /// <summary>
            /// Builds 'GlobalSettings' node for FBX.
            /// </summary>
            public static FbxNode BuildGlobalSettingsNode()
            {
                FbxNode globalSettings = new FbxNode().CreateNode("GlobalSettings", null);
                globalSettings.Nodes.Add(new FbxNode().CreateNode("Version", 1000));
                FbxNode properties = new FbxNode().CreateNode("Properties70", null);
                properties.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "UpAxis", "int", "Integer", "", 1 }));
                properties.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "UpAxisSign", "int", "Integer", "", 1 }));
                properties.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "FrontAxis", "int", "Integer", "", 2 }));
                properties.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "FrontAxisSign", "int", "Integer", "", 1 }));
                properties.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "CoordAxis", "int", "Integer", "", 0 }));
                properties.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "CoordAxisSign", "int", "Integer", "", 1 }));
                properties.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "OriginalUpAxis", "int", "Integer", "", 2 }));
                properties.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "OriginalUpAxisSign", "int", "Integer", "", 1 }));
                properties.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "UnitScaleFactor", "double", "Number", "", 2.54 }));
                properties.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "OriginalUnitScaleFactor", "double", "Number", "", 2.54 }));
                properties.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "AmbientColor", "ColorRGB", "Color", "", 0, 0, 0 }));
                properties.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "DefaultCamera", "KString", "", "", "Producer Perspective" }));
                properties.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "TimeMode", "enum", "", "", 6 }));
                properties.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "TimeProtocol", "enum", "", "", 2 }));
                properties.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "SnapOnFrameMode", "enum", "", "", 0 }));
                properties.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "TimeSpanStart", "KTime", "Time", "", 0 }));
                properties.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "TimeSpanStop", "KTime", "Time", "", 153953860000 }));
                properties.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "CustomFrameRate", "double", "Number", "", -1 }));
                properties.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "TimeMarker", "Compound", "", "" }));
                properties.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "CurrentTimeMarker", "int", "Integer", "", -1 }));
                globalSettings.Nodes.Add(properties);
                return globalSettings;
            }

            /// <summary>
            /// Builds 'Documents' node for FBX.
            /// </summary>
            public static FbxNode BuildDocumentsNode()
            {
                FbxNode node = new FbxNode() { Name = "Documents" };
                node.Nodes.Add(new FbxNode().CreateNode("Count", 1));
                node.Nodes.Add(new FbxNode().CreatePropertyNode("Document", new object[] { 2045365264, "", "Scene" }));
                FbxNode properties70 = new FbxNode().CreateNode("Properties70", null);
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "SourceObject", "object", "", "" }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "ActiveAnimStackName", "KString", "", "", "" }));
                node["Document"].Nodes.Add(properties70);
                node["Document"].Nodes.Add(new FbxNode().CreateNode("RootNode", 0));
                return node;
            }

            /// <summary>
            /// Builds 'Reference' node for FBX.
            /// </summary>
            /// <returns></returns>
            public static FbxNode BuildReferenceNode()
            {
                FbxNode node = new FbxNode().CreateNode("References", null);
                return node;
            }

            /// <summary>
            /// Builds 'Definitions' node for FBX.
            /// </summary>
            /// <param name="doGlobal"></param>
            /// <param name="doAnims"></param>
            /// <param name="doModels"></param>
            /// <returns></returns>
            public static FbxNode BuildDefinitionsNode(bool doGlobal, bool doAnims, bool doModels, int numMats = 0)
            {
                int count = 0;
                if (doGlobal)
                    count++;
                if (doAnims)
                    count += 2;
                if (doModels)
                    count += numMats*4;

                FbxNode node = new FbxNode().CreateNode("Definitions", null);
                node.Nodes.Add(new FbxNode().CreateNode("Version", 100));
                node.Nodes.Add(new FbxNode().CreateNode("Count", count));

                if (doGlobal)
                {
                    FbxNode global = new FbxNode().CreateNode("ObjectType", "GlobalSettings");
                    global.Nodes.Add(new FbxNode().CreateNode("Count", 1));
                    node.Nodes.Add(global);
                }

                if (doModels)
                {
                    node.Nodes.Add(BuildModelDefinitionNode());
                    node.Nodes.Add(BuildGeometryDefinitionNode());
                    node.Nodes.Add(BuildMaterialDefinitionNode(numMats));
                    node.Nodes.Add(BuildTextureDefinitionNode(numMats));
                    node.Nodes.Add(BuildVideoDefinitionNode(numMats));
                }

                return node;
            }

            /// <summary>
            /// Build 'Model' Definition Node.
            /// </summary>
            /// <returns></returns>
            private static FbxNode BuildModelDefinitionNode()
            {
                FbxNode node = new FbxNode().CreateNode("ObjectType", "Model");
                node.Nodes.Add(new FbxNode().CreateNode("Count", 1));

                FbxNode propTemplate = new FbxNode().CreateNode("PropertyTemplate", "FbxNode");
                FbxNode properties70 = new FbxNode().CreateNode("Properties70", null);
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "QuaternionInterpolate", "enum", "", "", 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "RotationOffset", "Vector3D", "Vector", "", 0, 0, 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "RotationPivot", "Vector3D", "Vector", "", 0, 0, 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "ScalingOffset", "Vector3D", "Vector", "", 0, 0, 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "ScalingPivot", "Vector3D", "Vector", "", 0, 0, 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "TranslationActive", "bool", "", "", 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "TranslationMin", "Vector3D", "Vector", "", 0, 0, 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "TranslationMax", "Vector3D", "Vector", "", 0, 0, 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "TranslationMinX", "bool", "", "", 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "TranslationMinY", "bool", "", "", 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "TranslationMinZ", "bool", "", "", 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "TranslationMaxX", "bool", "", "", 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "TranslationMaxY", "bool", "", "", 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "TranslationMaxZ", "bool", "", "", 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "RotationOrder", "enum", "", "", 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "RotationSpaceForLimitOnly", "bool", "", "", 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "RotationStiffnessX", "double", "Number", "", 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "RotationStiffnessY", "double", "Number", "", 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "RotationStiffnessZ", "double", "Number", "", 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "AxisLen", "double", "Number", "", 10 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "PreRotation", "Vector3D", "Vector", "", 0, 0, 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "PostRotation", "Vector3D", "Vector", "", 0, 0, 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "RotationActive", "bool", "", "", 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "RotationMin", "Vector3D", "Vector", "", 0, 0, 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "RotationMax", "Vector3D", "Vector", "", 0, 0, 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "RotationMinX", "bool", "", "", 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "RotationMinY", "bool", "", "", 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "RotationMinZ", "bool", "", "", 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "RotationMaxX", "bool", "", "", 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "RotationMaxY", "bool", "", "", 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "RotationMaxZ", "bool", "", "", 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "InheritType", "enum", "", "", 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "ScalingActive", "bool", "", "", 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "ScalingMin", "Vector3D", "Vector", "", 0, 0, 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "ScalingMax", "Vector3D", "Vector", "", 1, 1, 1 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "ScalingMinX", "bool", "", "", 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "ScalingMinY", "bool", "", "", 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "ScalingMinZ", "bool", "", "", 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "ScalingMaxX", "bool", "", "", 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "ScalingMaxY", "bool", "", "", 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "ScalingMaxZ", "bool", "", "", 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "GeometricTranslation", "Vector3D", "Vector", "", 0, 0, 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "GeometricRotation", "Vector3D", "Vector", "", 0, 0, 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "GeometricScaling", "Vector3D", "Vector", "", 1, 1, 1 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "MinDampRangeX", "double", "Number", "", 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "MinDampRangeY", "double", "Number", "", 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "MinDampRangeZ", "double", "Number", "", 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "MaxDampRangeX", "double", "Number", "", 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "MaxDampRangeY", "double", "Number", "", 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "MaxDampRangeZ", "double", "Number", "", 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "MinDampStrengthX", "double", "Number", "", 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "MinDampStrengthY", "double", "Number", "", 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "MinDampStrengthZ", "double", "Number", "", 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "MaxDampStrengthX", "double", "Number", "", 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "MaxDampStrengthY", "double", "Number", "", 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "MaxDampStrengthZ", "double", "Number", "", 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "PreferedAngleX", "double", "Number", "", 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "PreferedAngleY", "double", "Number", "", 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "PreferedAngleZ", "double", "Number", "", 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "LookAtProperty", "object", "", "" }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "UpVectorProperty", "object", "", "" }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "Show", "bool", "", "", 1 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "NegativePercentShapeSupport", "bool", "", "", 1 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "DefaultAttributeIndex", "int", "Integer", "", -1 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "Freeze", "bool", "", "", 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "LODBox", "bool", "", "", 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "Lcl Translation", "Lcl Translation", "", "A", 0, 0, 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "Lcl Rotation", "Lcl Rotation", "", "A", 0, 0, 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "Lcl Scaling", "Lcl Scaling", "", "A", 1, 1, 1 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "Visibility", "Visibility", "", "A", 1 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "Visibility Inheritance", "Visibility Inheritance", "", "", 1 }));
                propTemplate.Nodes.Add(properties70);
                node.Nodes.Add(propTemplate);
                return node;
            }

            /// <summary>
            /// Build 'Geometry' Definition Node.
            /// </summary>
            /// <returns></returns>
            private static FbxNode BuildGeometryDefinitionNode()
            {
                FbxNode node = new FbxNode().CreateNode("ObjectType", "Geometry");
                node.Nodes.Add(new FbxNode().CreateNode("Count", 1));

                FbxNode propTemplate = new FbxNode().CreateNode("PropertyTemplate", "FbxMesh");
                FbxNode properties70 = new FbxNode().CreateNode("Properties70", null);
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "Color", "ColorRGB", "Color", "", 0.8, 0.8, 0.8 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "BBoxMin", "Vector3D", "Vector", "", 0, 0, 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "BBoxMax", "Vector3D", "Vector", "", 0, 0, 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "Primary Visibility", "bool", "", "", 1 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "Casts Shadows", "bool", "", "", 1 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "Receive Shadows", "bool", "", "", 1 }));

                propTemplate.Nodes.Add(properties70);
                node.Nodes.Add(propTemplate);
                return node;
            }

            /// <summary>
            /// Build 'Material' Definition Node.
            /// </summary>
            /// <returns></returns>
            private static FbxNode BuildMaterialDefinitionNode(int count)
            {
                FbxNode node = new FbxNode().CreateNode("ObjectType", "Material");
                node.Nodes.Add(new FbxNode().CreateNode("Count", count));

                FbxNode propTemplate = new FbxNode().CreateNode("PropertyTemplate", "FbxSurfacePhong");
                FbxNode properties70 = new FbxNode().CreateNode("Properties70", null);
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "ShadingModel", "KString", "", "", "Phong" }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "MultiLayer", "bool", "", "", 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "EmissiveColor", "Color", "", "A", 0, 0, 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "EmissiveFactor", "Number", "", "A", 1 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "AmbientColor", "Color", "", "A", 0.2, 0.2, 0.2 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "AmbientFactor", "Number", "", "A", 1 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "DiffuseColor", "Color", "", "A", 0.8, 0.8, 0.8 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "DiffuseFactor", "Number", "", "A", 1 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "Bump", "Vector3D", "Vector", "", 0, 0, 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "NormalMap", "Vector3D", "Vector", "", 0, 0, 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "BumpFactor", "double", "Number", "", 1 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "TransparentColor", "Color", "", "A", 0, 0, 0 }));      
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "TransparencyFactor", "Number", "", "A", 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "DisplacementColor", "ColorRGB", "Color", "", 0, 0, 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "DisplacementFactor", "double", "Number", "", 1 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "VectorDisplacementColor", "ColorRGB", "Color", "", 0, 0, 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "VectorDisplacementFactor", "double", "Number", "", 1 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "SpecularColor", "Color", "", "A", 0.2, 0.2, 0.2 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "SpecularFactor", "Number", "", "A", 1 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "ShininessExponent", "Number", "", "A", 20 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "ReflectionColor", "Color", "", "A", 0, 0, 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "ReflectionFactor", "Number", "", "A", 1 }));
                propTemplate.Nodes.Add(properties70);
                node.Nodes.Add(propTemplate);
                return node;
            }

            /// <summary>
            /// Build 'Texture' Definition Node.
            /// </summary>
            /// <returns></returns>
            private static FbxNode BuildTextureDefinitionNode(int count)
            {
                FbxNode node = new FbxNode().CreateNode("ObjectType", "Texture");
                node.Nodes.Add(new FbxNode().CreateNode("Count", count));

                FbxNode propTemplate = new FbxNode().CreateNode("PropertyTemplate", "FbxFileTexture");
                FbxNode properties70 = new FbxNode().CreateNode("Properties70", null);
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "TextureTypeUse", "enum", "", "", 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "Texture alpha", "Number", "", "A", 1 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "CurrentMappingType", "enum", "", "", 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "WrapModeU", "enum", "", "", 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "WrapModeV", "enum", "", "", 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "UVSwap", "bool", "", "", 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "PremultiplyAlpha", "bool", "", "", 1 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "Translation", "Vector", "", "A", 0, 0, 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "Rotation", "Vector", "", "A", 0, 0, 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "Scaling", "Vector", "", "A", 1, 1, 1 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "TextureRotationPivot", "Vector3D", "Vector", "", 0, 0, 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "TextureScalingPivot", "Vector3D", "Vector", "", 0, 0, 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "CurrentTextureBlendMode", "enum", "", "", 1 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "UVSet", "KString", "", "", "default" }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "UseMaterial", "bool", "", "", 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "UseMipMap", "bool", "", "", 0 }));
                propTemplate.Nodes.Add(properties70);
                node.Nodes.Add(propTemplate);
                return node;
            }

            /// <summary>
            /// Build 'Video' Definition Node.
            /// </summary>
            /// <returns></returns>
            private static FbxNode BuildVideoDefinitionNode(int count)
            {
                FbxNode node = new FbxNode().CreateNode("ObjectType", "Video");
                node.Nodes.Add(new FbxNode().CreateNode("Count", count));

                FbxNode propTemplate = new FbxNode().CreateNode("PropertyTemplate", "FbxVideo");
                FbxNode properties70 = new FbxNode().CreateNode("Properties70", null);
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "ImageSequence", "bool", "", "", 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "ImageSequenceOffset", "int", "Integer", "", 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "FrameRate", "double", "Number", "", 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "LastFrame", "int", "Integer", "", 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "Width", "int", "Integer", "", 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "Height", "int", "Integer", "", 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "Path", "KString", "XRefUrl", "", "" }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "StartFrame", "int", "Integer", "", 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "StopFrame", "int", "Integer", "", 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "PlaySpeed", "double", "Number", "", 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "Offset", "KTime", "Time", "", 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "InterlaceMode", "enum", "", "", 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "FreeRunning", "bool", "", "", 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "Loop", "bool", "", "", 0 }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "AccessMode", "enum", "", "", 0 }));
                propTemplate.Nodes.Add(properties70);
                node.Nodes.Add(propTemplate);
                return node;
            }
        }

        public static void BuildEmptyFBX()
        {
            connections = new List<ConnectionStruct>();
            FbxDocument doc = new FbxDocument();
            doc.Version = FbxVersion.v7_4;
            doc.Nodes.Add(FbxPresetNodes.BuildHeaderExtensionNode());
            doc.Nodes.Add(FbxPresetNodes.BuildGlobalSettingsNode());
            doc.Nodes.Add(FbxPresetNodes.BuildDocumentsNode());
            doc.Nodes.Add(FbxPresetNodes.BuildDefinitionsNode(true, false, true));
        }

        public static void BuildFBXFromModel(Model model, string path, bool saveBinary)
        {
            int numMats = model.Lods[0].Parts.Length;

            FbxDocument doc = new FbxDocument();
            doc.Version = FbxVersion.v7_4;
            doc.Nodes.Add(FbxPresetNodes.BuildHeaderExtensionNode());
            doc.Nodes.Add(FbxPresetNodes.BuildGlobalSettingsNode());
            doc.Nodes.Add(FbxPresetNodes.BuildDocumentsNode());
            doc.Nodes.Add(FbxPresetNodes.BuildDefinitionsNode(true, false, true, numMats));
            doc.Nodes.Add(BuildObjectNode(model));
            doc.Nodes.Add(BuildConnections());

            if (saveBinary)
                FbxIO.WriteBinary(doc, path + model.FrameMesh.Name + ".fbx");
            else
                FbxIO.WriteAscii(doc, path + model.FrameMesh.Name + ".fbx");
        }

        private static FbxNode BuildObjectNode(Model model)
        {
            int geomID = Functions.RandomGenerator.Next();
            int modelID = Functions.RandomGenerator.Next();

            FbxNode node = new FbxNode().CreateNode("Objects", null);

            //do geom stuff
            FbxNode geom = new FbxNode().CreateNode("Geometry", geomID);
            geom.Properties.Add("Geometry::");
            geom.Properties.Add("Mesh");
            FbxNode properties = new FbxNode().CreateNode("Properties70", null);
            properties.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "Color", "ColorRGB", "Color", "", 0.6, 0.6, 0.9 }));
            geom.Nodes.Add(properties);

            double[] verts = new double[model.Lods[0].Vertices.Length * 3];
            int verticesPos = 0;
            for (int i = 0; i < model.Lods[0].Vertices.Length * 3; i += 3)
            {
                verts[i] = model.Lods[0].Vertices[verticesPos].Position.X;
                verts[i + 1] = model.Lods[0].Vertices[verticesPos].Position.Y;
                verts[i + 2] = model.Lods[0].Vertices[verticesPos].Position.Z;
                verticesPos++;
            }

            List<int> triangles = new List<int>();
            for (int i = 0; i < model.Lods[0].Parts.Length; i++)
            {
                for (int x = 0; x < model.Lods[0].Parts[i].Indices.Length; x++)
                {
                    triangles.Add(model.Lods[0].Parts[i].Indices[x].S1);
                    triangles.Add(model.Lods[0].Parts[i].Indices[x].S2);
                    triangles.Add(~model.Lods[0].Parts[i].Indices[x].S3);
                }
            }

            FbxNode vertNode = new FbxNode().CreateNode("Vertices", verts);
            FbxNode polyVertIndex = new FbxNode().CreateNode("PolygonVertexIndex", triangles.ToArray());
            geom.Nodes.Add(vertNode);
            geom.Nodes.Add(polyVertIndex);
            geom.Nodes.Add(new FbxNode().CreateNode("GeometryVersion", 124));

            //check for normals before exporting.
            if (model.Lods[0].VertexDeclaration.HasFlag(VertexFlags.Normals))
                geom.Nodes.Add(BuildLayerElementNormalNode(model, triangles.Count));

            //check for UV before exporting.
            if (model.Lods[0].VertexDeclaration.HasFlag(VertexFlags.TexCoords0))
                geom.Nodes.Add(BuildLayerElementUVNode(model));

            geom.Nodes.Add(BuildLayerElementMaterial(model));

            geom.Nodes.Add(new FbxNode().CreateNode("Layer", 0));
            geom["Layer"].Nodes.Add(new FbxNode().CreateNode("Version", 100));

            //make sure normals exist before adding the layer.
            if (model.Lods[0].VertexDeclaration.HasFlag(VertexFlags.Normals))
                geom["Layer"].Nodes.Add(BuildNamedLayer("LayerElementNormal"));

            geom["Layer"].Nodes.Add(BuildNamedLayer("LayerElementMaterial"));

            //make sure UVs exist before adding the layer.
            if (model.Lods[0].VertexDeclaration.HasFlag(VertexFlags.TexCoords0))
                geom["Layer"].Nodes.Add(BuildNamedLayer("LayerElementUV"));

            node.Nodes.Add(geom);

            //Do model stuff.
            node.Nodes.Add(new FbxNode().CreateNode("Model", modelID));
            node["Model"].Properties.Add("Model::" + model.FrameMesh.Name.String);
            node["Model"].Properties.Add("Model");
            node["Model"].Nodes.Add(new FbxNode().CreateNode("Version", 232));
            FbxNode properties70 = new FbxNode().CreateNode("Properties70", null);
            properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "PreRotation", "Vector3D", "Vector", "", -90, -0, 0 }));
            properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "RotationActive", "bool", "", "", 1 }));
            properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "InheritType", "enum", "", "", 1 }));
            properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "ScalingMax", "Vector3D", "Vector", "", 0, 0, 0 }));
            properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "DefaultAttributeIndex", "int", "Integer", "", 0 }));
            properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "mr displacement use global settings", "Bool", "", "AU", 1 }));
            properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "mr displacement view dependent", "Bool", "", "AU", 1 }));
            properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "mr displacement method", "Integer", "", "AU", 6, 6, 6 }));
            properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "mr displacement smoothing on", "Bool", "", "AU", 1 }));
            properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "mr displacement edge length", "Number", "", "AU", 2, 2, 2 }));
            properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "mr displacement max displace", "Number", "", "AU", 20, 20, 20 }));
            properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "mr displacement parametric subdivision level", "Integer", "", "AU", 5, 5, 5 }));
            properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "MaxHandle", "int", "Integer", "UH", 2 }));

            node["Model"].Nodes.Add(properties70);
            //node["Model"].Nodes.Add(new FbxNode().CreateNode("Shading", 'T'));
            node["Model"].Nodes.Add(new FbxNode().CreateNode("Culling", "CullingOff"));
            connections = new List<ConnectionStruct>();
            connections.Add(new ConnectionStruct("OO", modelID, 0));
            connections.Add(new ConnectionStruct("OO", geomID, modelID));

            int[] matIDs = new int[model.Lods[0].Parts.Length];

            for(int i = 0; i != model.Lods[0].Parts.Length; i++)
            {
                matIDs[i] = Functions.RandomGenerator.Next();
                node.Nodes.Add(BuildMaterialNode(model.Lods[0].Parts[i].Material, matIDs[i]));
                connections.Add(new ConnectionStruct("OO", matIDs[i], modelID));
            }

            for (int i = 0; i != model.Lods[0].Parts.Length; i++)
            {
                int vidID = Functions.RandomGenerator.Next();
                int texID = Functions.RandomGenerator.Next();
                node.Nodes.Add(BuildVideoNode(model.Lods[0].Parts[i].Material, vidID));
                node.Nodes.Add(BuildTextureNode(model.Lods[0].Parts[i].Material, texID));
                connections.Add(new ConnectionStruct("OP", texID, matIDs[i], "DiffuseColor"));
                connections.Add(new ConnectionStruct("OO", vidID, texID));
            }

            return node;
        }

        /// <summary>
        /// Build Connections.
        /// </summary>
        /// <returns></returns>
        private static FbxNode BuildConnections()
        {
            FbxNode node = new FbxNode().CreateNode("Connections", null);

            for(int i = 0; i != connections.Count; i++)
            {
                node.Nodes.Add(BuildConnection(connections[i]));
            }

            return node;
        }

        /// <summary>
        /// Build LayerElementNormal.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="trianglesCount"></param>
        /// <returns></returns>
        private static FbxNode BuildLayerElementNormalNode(Model model, int trianglesCount)
        {
            FbxNode node;
            //Do normal (layer stuff)
            node = new FbxNode().CreateNode("LayerElementNormal", 0);
            node.Nodes.Add(new FbxNode().CreateNode("Version", 102));
            node.Nodes.Add(new FbxNode().CreateNode("Name", ""));
            node.Nodes.Add(new FbxNode().CreateNode("MappingInformationType", "ByVertice"));
            node.Nodes.Add(new FbxNode().CreateNode("ReferenceInformationType", "Direct"));

            double[] normals = new double[model.Lods[0].Vertices.Length * 3];
            int normalPos = 0;
            for (int i = 0; i < model.Lods[0].Vertices.Length * 3; i += 3)
            {
                normals[i] = model.Lods[0].Vertices[normalPos].Normal.X;
                normals[i + 1] = model.Lods[0].Vertices[normalPos].Normal.Y;
                normals[i + 2] = model.Lods[0].Vertices[normalPos].Normal.Z;
                normalPos++;
            }
            node.Nodes.Add(new FbxNode().CreateNode("Normals", normals));
            node.Nodes.Add(new FbxNode().CreateNode("NormalsW", new double[trianglesCount]));
            return node;
        }

        /// <summary>
        /// Build LayerElementUV.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="trianglesCount"></param>
        /// <returns></returns>
        private static FbxNode BuildLayerElementUVNode(Model model)
        {
            FbxNode node = new FbxNode().CreateNode("LayerElementUV", 0);
            node.Nodes.Add(new FbxNode().CreateNode("Version", 101));
            node.Nodes.Add(new FbxNode().CreateNode("Name", "UVChannel_1"));
            node.Nodes.Add(new FbxNode().CreateNode("MappingInformationType", "ByVertice"));
            node.Nodes.Add(new FbxNode().CreateNode("ReferenceInformationType", "Direct"));

            double[] uvs = new double[model.Lods[0].Vertices.Length * 2];
            int uvPos = 0;
            for (int i = 0; i < model.Lods[0].Vertices.Length * 2; i += 2)
            {
                uvs[i] = model.Lods[0].Vertices[uvPos].UVs[0].X;
                uvs[i + 1] = 1f - model.Lods[0].Vertices[uvPos].UVs[0].Y;
                uvPos++;
            }
            node.Nodes.Add(new FbxNode().CreateNode("UV", uvs));
            return node;
        }

        /// <summary>
        /// Build LayerElementMaterial
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private static FbxNode BuildLayerElementMaterial(Model model)
        {
            FbxNode node = new FbxNode().CreateNode("LayerElementMaterial", 0);
            node.Nodes.Add(new FbxNode().CreateNode("Version", 101));
            node.Nodes.Add(new FbxNode().CreateNode("Name", ""));
            node.Nodes.Add(new FbxNode().CreateNode("MappingInformationType", "ByPolygon"));
            node.Nodes.Add(new FbxNode().CreateNode("ReferenceInformationType", "IndexToDirect"));

            List<int> matIDs = new List<int>();
            for (int i = 0; i < model.Lods[0].Parts.Length; i++)
            {
                for (int x = 0; x < model.Lods[0].Parts[i].Indices.Length; x++)
                {
                    matIDs.Add(i);
                }                
            }
            node.Nodes.Add(new FbxNode().CreateNode("Materials", matIDs.ToArray()));
            return node;
        }

        /// <summary>
        /// Build layer with specified name.
        /// </summary>
        /// <param name="layerName"></param>
        /// <returns></returns>
        private static FbxNode BuildNamedLayer(string layerName)
        {
            FbxNode node = new FbxNode().CreateNode("LayerElement", null);
            node.Nodes.Add(new FbxNode().CreateNode("Type", layerName));
            node.Nodes.Add(new FbxNode().CreateNode("TypedIndex", 0));
            return node;
        }

        /// <summary>
        /// Build named 'Material' Node
        /// </summary>
        /// <param name="material"></param>
        /// <returns></returns>
        private static FbxNode BuildMaterialNode(string material, int matID)
        {
            FbxNode node = new FbxNode().CreateNode("Material", matID);
            node.Properties.Add("Material::" + material);
            node.Properties.Add("");
            node.Nodes.Add(new FbxNode().CreateNode("Version", 102));
            node.Nodes.Add(new FbxNode().CreateNode("ShadingModel", "phong"));
            node.Nodes.Add(new FbxNode().CreateNode("MultiLayer", 0));
            FbxNode properties70 = new FbxNode().CreateNode("Properties70", null);
            properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "ShadingModel", "KString", "", "", "phong" }));
            properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "EmissiveFactor", "Number", "", "A", 0 }));
            properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "AmbientColor", "Color", "", "A", 0.6, 0.6, 0.6 }));
            properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "DiffuseColor", "Color", "", "A", 0.6, 0.6, 0.6 }));
            properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "TransparentColor", "Color", "", "A", 1, 1, 1 }));
            properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "SpecularColor", "Color", "", "A", 0.9, 0.9, 0.9 }));
            properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "SpecularFactor", "Number", "", "A", 0 }));
            properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "ShininessExponent", "Number", "", "A", 2 }));
            properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "Emissive", "Vector3D", "Vector", "", 0, 0, 0 }));
            properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "Ambient", "Vector3D", "Vector", "", 0.6, 0.6, 0.6 }));
            properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "Diffuse", "Vector3D", "Vector", "", 0.6, 0.6, 0.6 }));
            properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "Specular", "Vector3D", "Vector", "", 0, 0, 0 }));
            properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "Shininess", "double", "Number", "", 2 }));
            properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "Opacity", "double", "Number", "", 1 }));
            properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "Reflectivity", "double", "Number", "", 0 }));
            node.Nodes.Add(properties70);
            return node;
        }

        /// <summary>
        /// Build named 'Video' Node
        /// </summary>
        /// <param name="video"></param>
        /// <returns></returns>
        private static FbxNode BuildVideoNode(string video, int vidID)
        {
            string fileName = "C:\\" + video;
            string relative = "textures\\" + video;

            FbxNode node = new FbxNode().CreateNode("Video", vidID);
            node.Properties.Add("Video::" + video);
            node.Properties.Add("Clip");
            node.Nodes.Add(new FbxNode().CreateNode("Type", "Clip"));
            FbxNode properties70 = new FbxNode().CreateNode("Properties70", null);
            properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "Path", "KString", "XRefUrl", "", fileName }));
            node.Nodes.Add(properties70);
            node.Nodes.Add(new FbxNode().CreateNode("UseMipMap", 0));
            node.Nodes.Add(new FbxNode().CreateNode("Filename", fileName));
            node.Nodes.Add(new FbxNode().CreateNode("RelativeFilename", relative));
            return node;
        }

        /// <summary>
        /// Build named 'Texture' Node
        /// </summary>
        /// <param name="texture"></param>
        /// <returns></returns>
        private static FbxNode BuildTextureNode(string texture, int texID)
        {
            string textureName = "Texture::" + texture;
            string fileName = "C:\\" + texture;
            string relative = "textures\\" + texture;

            FbxNode node = new FbxNode().CreateNode("Texture", texID);
            node.Properties.Add(textureName);
            node.Properties.Add("");
            node.Nodes.Add(new FbxNode().CreateNode("Type", "TextureVideoClip"));
            node.Nodes.Add(new FbxNode().CreateNode("Version", 202));
            node.Nodes.Add(new FbxNode().CreateNode("TextureName", textureName));
            FbxNode properties70 = new FbxNode().CreateNode("Properties70", null);
            properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "UVSet", "KString", "", "", "UVChannel_1" }));
            properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "UseMaterial", "bool", "", "", 1 }));
            node.Nodes.Add(properties70);
            node.Nodes.Add(new FbxNode().CreateNode("Media", "Video::"+texture));
            node.Nodes.Add(new FbxNode().CreateNode("FileName", fileName));
            node.Nodes.Add(new FbxNode().CreateNode("RelativeFilename", relative));
            node.Nodes.Add(new FbxNode().CreateNode("ModelUVTranslation", 0));
            node["ModelUVTranslation"].Properties.Add(0);
            node.Nodes.Add(new FbxNode().CreateNode("ModelUVScaling", 1));
            node["ModelUVScaling"].Properties.Add(1);
            node.Nodes.Add(new FbxNode().CreateNode("Texture_Alpha_Source", "Alpha_Black"));
            node.Nodes.Add(new FbxNode().CreateNode("Cropping", 1));
            node["Cropping"].Properties.Add(1);
            node["Cropping"].Properties.Add(1);
            node["Cropping"].Properties.Add(1);
            return node;
        }

        private static FbxNode BuildConnection(ConnectionStruct connection)
        {
            FbxNode node = new FbxNode().CreateNode("C", null);
            node.Properties.Add(connection.Type);
            node.Properties.Add(connection.ID1);
            node.Properties.Add(connection.ID2);

            if (connection.ExtraMat != null)
                node.Properties.Add(connection.ExtraMat);

            return node;
        }

        private struct ConnectionStruct
        {
            string type;
            int id1;
            int id2;
            object extraMat;

            public string Type { get { return type; } set { type = value; } }
            public int ID1 { get { return id1; } set { id1 = value; } }
            public int ID2 { get { return id2; } set { id2 = value; } }
            public object ExtraMat { get { return extraMat; } set { extraMat = value; } }

            public ConnectionStruct(string type, int id1, int id2, object extraMat = null)
            {
                this.type = type;
                this.id1 = id1;
                this.id2 = id2;
                this.extraMat = extraMat;
            }
        }
    }
}

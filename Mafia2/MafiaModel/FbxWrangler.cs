using Fbx;
using System;
using System.Collections.Generic;

namespace Mafia2
{
    public class FbxWrangler
    {
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
                node.Nodes.Add(new FbxNode().CreateNode("Creator", "Toolkit Model Wrangler"));

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
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[5] { "DocumentUrl", "KString", "Url", "", "C:\\URL.FBX" }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[5] { "SrcDocumentUrl", "KString", "Url", "", "C:\\URL.FBX" }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[4] { "Original", "Compound", "", "" }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[5] { "Original|ApplicationVendor", "KString", "", "", "Autodesk"}));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[5] { "Original|ApplicationName", "KString", "", "", "3ds Max" }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[5] { "Original|ApplicationVersion", "KString", "", "", "2017" }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[5] { "Original|DateTime_GMT", "DateTime", "", "", DateTime.Now.ToString() }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[5] { "Original|FileName", "KString", "", "", "C:\\URL.FBX" }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[4] { "LastSaved", "Compound", "", "" }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[5] { "LastSaved|ApplicationVendor", "KString", "", "", "Autodesk" }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[5] { "LastSaved|ApplicationName", "KString", "", "", "3ds Max" }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[5] { "LastSaved|ApplicationVersion", "KString", "", "", "2017" }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[5] { "LastSaved|DateTime_GMT", "DateTime", "", "", DateTime.Now.ToString() }));
                properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[5] { "Original|ApplicationActiveProject", "KString", "", "", "C:\\URL.FBX" }));
                sceneInfo.Nodes.Add(properties70);
                node.Nodes.Add(sceneInfo);
                return node;
            }

            /// <summary>
            /// Builds 'FileID' for new FBX.
            /// </summary>
            public static void BuildFileIDNode()
            {
                FbxNode node = new FbxNode();
                node.Name = "FileId";
                node.Value = new byte[] {43, 182, 47, 230, 186, 41, 192, 205, 184, 207, 180, 39, 162, 40, 255, 247};
                node.Properties.Add(node.Value);
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
            public static FbxNode BuildDefinitionsNode(bool doGlobal, bool doAnims, bool doModels)
            {
                int count = 0;
                if (doGlobal)
                    count++;
                if (doAnims)
                    count += 2;
                if (doModels)
                    count += 2;

                FbxNode node = new FbxNode().CreateNode("Definitions", null);
                node.Nodes.Add(new FbxNode().CreateNode("Version", 100));
                node.Nodes.Add(new FbxNode().CreateNode("Count", count));

                if(doGlobal)
                {
                    FbxNode global = new FbxNode().CreateNode("ObjectType", "GlobalSettings");
                    global.Nodes.Add(new FbxNode().CreateNode("Count", 1));
                    node.Nodes.Add(global);
                }

                if(doModels)
                {
                    FbxNode model = new FbxNode().CreateNode("ObjectType", "Model");
                    model.Nodes.Add(new FbxNode().CreateNode("Count", 1));

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
                    model.Nodes.Add(propTemplate);
                    node.Nodes.Add(model);

                    model = new FbxNode().CreateNode("ObjectType", "Geometry");
                    model.Nodes.Add(new FbxNode().CreateNode("Count", 1));

                    propTemplate = new FbxNode().CreateNode("PropertyTemplate", "FbxMesh");
                    properties70 = new FbxNode().CreateNode("Properties70", null);
                    properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "Color", "ColorRGB", "Color", "", 0.8, 0.8, 0.8 }));
                    properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "BBoxMin", "Vector3D", "Vector", "", 0, 0, 0 }));
                    properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "BBoxMax", "Vector3D", "Vector", "", 0, 0, 0 }));
                    properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "Primary Visibility", "bool", "", "", 1 }));
                    properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "Casts Shadows", "bool", "", "", 1 }));
                    properties70.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "Receive Shadows", "bool", "", "", 1 }));

                    propTemplate.Nodes.Add(properties70);
                    model.Nodes.Add(propTemplate);
                    node.Nodes.Add(model);
                }

                return node;
            }
        }

        public static void BuildEmptyFBX()
        {
            FbxDocument doc = new FbxDocument();
            doc.Version = FbxVersion.v7_4;
            doc.Nodes.Add(FbxPresetNodes.BuildHeaderExtensionNode());
            doc.Nodes.Add(FbxPresetNodes.BuildGlobalSettingsNode());
            doc.Nodes.Add(FbxPresetNodes.BuildDocumentsNode());
            doc.Nodes.Add(FbxPresetNodes.BuildDefinitionsNode(true, false, true));
            FbxIO.WriteAscii(doc, "file.fbx");
        }

        public static void BuildFBXFromModel(Model model)
        {
            FbxDocument doc = new FbxDocument();
            doc.Version = FbxVersion.v7_2;
            doc.Nodes.Add(FbxPresetNodes.BuildHeaderExtensionNode());
            doc.Nodes.Add(FbxPresetNodes.BuildGlobalSettingsNode());
            doc.Nodes.Add(FbxPresetNodes.BuildDocumentsNode());
            doc.Nodes.Add(FbxPresetNodes.BuildDefinitionsNode(true, false, true));
            doc.Nodes.Add(BuildObjectNode(model));
            doc.Nodes.Add(BuildConnections());
            FbxIO.WriteAscii(doc, "file.fbx");
        }

        private static FbxNode BuildObjectNode(Model model)
        {
            FbxNode node = new FbxNode().CreateNode("Objects", null);
            node.Nodes.Add(new FbxNode().CreateNode("Geometry", 6325820608));
            node["Geometry"].Properties.Add("Geometry::");
            node["Geometry"].Properties.Add("Mesh");
            FbxNode properties = new FbxNode().CreateNode("Properties70", null);
            properties.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "Color", "ColorRGB", "Color", "", 0.603921568627451, 0.603921568627451, 0.898039215686275 }));
            node["Geometry"].Nodes.Add(properties);

            double[] verts = new double[model.Lods[0].Vertices.Length*3];
            int verticesPos = 0;
            for(int i = 0;  i < model.Lods[0].Vertices.Length*3; i+=3)
            {
                verts[i] = model.Lods[0].Vertices[verticesPos].Position.X;
                verts[i+1] = model.Lods[0].Vertices[verticesPos].Position.Y;
                verts[i+2] = model.Lods[0].Vertices[verticesPos].Position.Z;
                verticesPos++;
            }

            List<int> triangles = new List<int>();
            for(int i = 0; i < model.Lods[0].Parts.Length; i++)
            {
                for(int x = 0; x < model.Lods[0].Parts[i].Indices.Length; x++)
                {
                    triangles.Add(model.Lods[0].Parts[i].Indices[x].S1);
                    triangles.Add(model.Lods[0].Parts[i].Indices[x].S2);
                    triangles.Add(~model.Lods[0].Parts[i].Indices[x].S3);
                }
            }

            FbxNode vertNode = new FbxNode().CreateNode("Vertices", verts);
            FbxNode polyVertIndex = new FbxNode().CreateNode("PolygonVertexIndex", triangles.ToArray());
            node["Geometry"].Nodes.Add(vertNode);
            node["Geometry"].Nodes.Add(polyVertIndex);
            node["Geometry"].Nodes.Add(new FbxNode().CreateNode("GeometryVersion", 124));

            node.Nodes.Add(new FbxNode().CreateNode("Model", 2064104752));
            node["Model"].Properties.Add("Model::"+model.FrameMesh.Name.String);
            node["Model"].Properties.Add("Model");
            node["Model"].Nodes.Add(new FbxNode().CreateNode("Version", 232));
            FbxNode properties70 = new FbxNode().CreateNode("Properties70", null);
            properties.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "PreRotation", "Vector3D", "Vector", "", -90, -0, 0 }));
            properties.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "RotationActive", "bool", "", "", 1 }));
            properties.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "InheritType", "enum", "", "", 1 }));
            properties.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "ScalingMax", "Vector3D", "Vector", "", 0, 0, 0 }));
            properties.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "DefaultAttributeIndex", "int", "Integer", "", 0 }));
            properties.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "mr displacement use global settings", "Bool", "", "AU", 1 }));
            properties.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "mr displacement view dependent", "Bool", "", "AU", 1 }));
            properties.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "mr displacement method", "Integer", "", "AU", 6, 6, 6 }));
            properties.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "mr displacement smoothing on", "Bool", "", "AU", 1 }));
            properties.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "mr displacement edge length", "Number", "", "AU", 2, 2, 2 }));

            properties.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "mr displacement max displace", "Number", "", "AU", 20, 20, 20 }));
            properties.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "mr displacement parametric subdivision level", "Integer", "", "AU", 5, 5, 5 }));
            properties.Nodes.Add(new FbxNode().CreatePropertyNode("P", new object[] { "MaxHandle", "int", "Integer", "UH", 2 }));
            node["Model"].Nodes.Add(properties);
            node["Model"].Nodes.Add(new FbxNode().CreateNode("Shading", 'T'));
            node["Model"].Nodes.Add(new FbxNode().CreateNode("Culling", "CullingOff"));
            return node;
        }

        private static FbxNode BuildConnections()
        {
            FbxNode node = new FbxNode().CreateNode("Connections", null);
            node.Nodes.Add(new FbxNode().CreateNode("C", null));
            node.Nodes[0].Properties.Add("OO");
            node.Nodes[0].Properties.Add(2064104752);
            node.Nodes[0].Properties.Add(0);
            node.Nodes.Add(new FbxNode().CreateNode("C", null));
            node.Nodes[1].Properties.Add("OO");
            node.Nodes[1].Properties.Add(6325820608);
            node.Nodes[1].Properties.Add(2064104752);
            return node;
        }
    }
}

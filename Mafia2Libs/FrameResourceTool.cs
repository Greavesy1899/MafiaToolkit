using Mafia2;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Mafia2Tool
{
    public partial class FrameResourceTool : Form
    {

        FrameResource frameResource;
        IndexBufferPool indexBufferPool;
        VertexBufferPool vertexBufferPool;

        private List<FrameObjectSingleMesh> mesh = new List<FrameObjectSingleMesh>();
        Dictionary<int, TreeNode> treeNodes = new Dictionary<int, TreeNode>();

        public FrameResourceTool()
        {
            InitializeComponent();
            MaterialsParse.ReadMatFile("default.mtl");

            ReadFrameResource();
        }

        public void ReadFrameResource()
        {

            using (BinaryReader reader = new BinaryReader(File.Open("FrameResource_0.bin", FileMode.Open)))
            {
                frameResource = new FrameResource();
                frameResource.ReadFromFile(reader);
                frameResource.DefineFrameBlockParents();

                for(int i = 0; i != frameResource.FrameBlocks.Count; i++)
                {
                    FrameResourceListBox.Items.Add(frameResource.FrameBlocks[i]);

                    if (frameResource.FrameBlocks[i].GetType() == typeof(FrameObjectSingleMesh))
                        mesh.Add((FrameObjectSingleMesh)frameResource.FrameBlocks[i]);
                }
            }
        }

        private void OnSelectedChanged(object sender, System.EventArgs e)
        {
            FrameResourceGrid.SelectedObject = FrameResourceListBox.SelectedItem;
        }

        private void LoadMaterialTool(object sender, System.EventArgs e)
        {
            MaterialTool tool = new MaterialTool();
            tool.ShowDialog();
        }

        private void OnClickLoad3D(object sender, System.EventArgs e)
        {
            DirectoryInfo dir = new DirectoryInfo(Application.StartupPath);
            FileInfo[] files = dir.GetFiles();

            List<FileInfo> indexFiles = new List<FileInfo>();
            List<FileInfo> vertexFiles = new List<FileInfo>();

            foreach (FileInfo file in files)
            {
                if (file.FullName.Contains("IndexBufferPool"))
                    indexFiles.Add(file);
                if (file.FullName.Contains("VertexBufferPool"))
                    vertexFiles.Add(file);
            }

            indexBufferPool = new IndexBufferPool(indexFiles);
            vertexBufferPool = new VertexBufferPool(vertexFiles);

            string[] fileNames = new string[mesh.Count];
            Vector3[] filePos = new Vector3[mesh.Count];
            float[] rotPos = new float[mesh.Count];

            for (int i = 0; i != mesh.Count; i++)
            {
                Model newModel = new Model((mesh[i]), vertexBufferPool, indexBufferPool, frameResource);
                
                for (int c = 0; c != newModel.Lods.Length; c++)
                {
                    Stopwatch watch = new Stopwatch();
                    watch.Start();
                   //newModel.ExportToOBJ(newModel.Lods[c], mesh[i].Name.Name + "_lod" + c);
                    newModel.ExportToEDM(newModel.Lods[c], mesh[i].Name.Name + "_lod" + c);

                    fileNames[i] = mesh[i].Name.Name + "_lod" + c;
                    filePos[i] = mesh[i].Matrix.Position;
                    rotPos[i] = mesh[i].Matrix.Rotation.Vector.X;

                    watch.Stop();
                    Debug.WriteLine("Mesh: {0} and time taken was {1}", mesh[i].Name.Name + "_lod" + c, watch.Elapsed);
                }
            }

            using (BinaryWriter writer = new BinaryWriter(File.Create("exported/frame.edd")))
            {
                writer.Write(mesh.Count);

                for (int i = 0; i != mesh.Count; i++)
                {
                    writer.Write(fileNames[i]);
                    writer.Write(filePos[i].X);
                    writer.Write(filePos[i].Y);
                    writer.Write(filePos[i].Z);
                    writer.Write(rotPos[i]);
                }
            }
        }
    }
}

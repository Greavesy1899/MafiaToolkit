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

        public FrameResourceTool()
        {
            InitializeComponent();
            MaterialsParse.ReadMatFile("default.mtl");

            FrameListView.Nodes.Add("Headers");
            FrameListView.Nodes.Add("Geometries");
            FrameListView.Nodes.Add("Materials");
            FrameListView.Nodes.Add("Blend Info");
            FrameListView.Nodes.Add("Skeletons");
            FrameListView.Nodes.Add("Skeleton Hierachies");
            ReadFrameResource();
        }

        public void ReadFrameResource()
        {

            using (BinaryReader reader = new BinaryReader(File.Open("FrameResource_0.bin", FileMode.Open)))
            {
                frameResource = new FrameResource();
                frameResource.ReadFromFile(reader);
                frameResource.DefineFrameBlockParents();

                foreach (object block in frameResource.FrameBlocks)
                {
                    FrameResourceListBox.Items.Add(block);
                    //if (block.GetType() == typeof(FrameHeaderScene))
                    //    FrameListView.Nodes[0].Nodes.Add("Header Block");

                    //if(block.GetType() == typeof(FrameGeometry))
                    //    FrameListView.Nodes[1].Nodes.Add("Geometry Block");

                    if (block.GetType() == typeof(FrameObjectSingleMesh))
                        mesh.Add((FrameObjectSingleMesh)block);
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
                }
            }
        }
    }
}

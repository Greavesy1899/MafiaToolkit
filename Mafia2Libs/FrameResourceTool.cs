using Mafia2;
using System.Collections.Generic;
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
            
            for (int i = 0; i != mesh.Count; i++)
            {
                Model newModel = new Model((mesh[i]), vertexBufferPool, indexBufferPool, frameResource);
                for (int c = 0; c != newModel.Lods.Length; c++)
                {
                    newModel.ExportToOBJ(newModel.Lods[c], mesh[i].Name.Name + "_lod" + c);
                }
            }

        }
    }
}

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

            using (BinaryReader reader = new BinaryReader(File.Open("IndexBufferPool_0.bin", FileMode.Open)))
            {
                indexBufferPool = new IndexBufferPool(reader);
            }
            using (BinaryReader reader = new BinaryReader(File.Open("VertexBufferPool_0.bin", FileMode.Open)))
            {
                vertexBufferPool = new VertexBufferPool(reader);
            }
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

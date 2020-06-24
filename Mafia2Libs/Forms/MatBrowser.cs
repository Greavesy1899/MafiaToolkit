using Gibbed.Squish;
using System;
using ResourceTypes.Materials;
using System.Windows.Forms;
using System.Linq;
using System.Drawing;
using System.IO;

namespace Mafia2Tool.Forms
{
    public partial class MatBrowser : Form
    {
        public MatBrowser()
        {
            InitializeComponent();
            Init();
            ShowDialog();
        }

        public bool ThumbnailCallback()
        {
            return false;
        }

        private void Init()
        {
            // Populate Material Libraries;
            for(int i = 0; i < MaterialsManager.MaterialLibraries.Count; i++)
            {
                var lib = MaterialsManager.MaterialLibraries.ElementAt(i).Value;
                ComboBox_Materials.Items.Add(lib.Name);
            }

            PopulateBrowser(0);
        }

        private void PopulateBrowser(int index)
        {
            var mtl = MaterialsManager.MaterialLibraries.ElementAt(index).Value;
            for (int x = 0; x < 20; x++)
            {
                var mat = mtl.Materials.ElementAt(x).Value;
                TextureEntry textEntry = new TextureEntry();
                textEntry.SetMaterialName(mat.MaterialName);
                textEntry.SetMaterialTexture(GetThumbnail(mat));
                FlowPanel_Materials.Controls.Add(textEntry);
            }
        }

        private Image LoadDDSSquish(string name)
        {
            Image.GetThumbnailImageAbort myCallback = new Image.GetThumbnailImageAbort(ThumbnailCallback);
            DdsFile dds = new DdsFile();

            name = File.Exists(name) == false ? "Resources/texture.dds" : name;

            using (var stream = File.Open(name, FileMode.Open))
            {
                try
                {
                    dds.Load(stream);
                }
                catch(Exception ex)
                {
                    return LoadDDSSquish("Resources/texture.dds").GetThumbnailImage(128, 120, myCallback, IntPtr.Zero);
                }
                
            }
            var thumbnail = dds.Image().GetThumbnailImage(128, 120, myCallback, IntPtr.Zero);
            dds = null;
            return thumbnail;
        }

        private Image GetThumbnail(Material mat)
        {
            Image thumbnail = null;
            if (mat != null)
            {
                if (mat.Samplers.ContainsKey("S000"))
                {
                    thumbnail = LoadDDSSquish(Path.Combine(SceneData.ScenePath, mat.Samplers["S000"].File));
                }
                else
                {
                    thumbnail = LoadDDSSquish("Resources/texture.dds");
                }
            }
            else
            {
                thumbnail = LoadDDSSquish("Resources/MissingMaterial.dds");
            }
            return thumbnail;
        }

        void UsersGrid_WasClicked(object sender, EventArgs e)
        {
            // Set IsSelected for all UCs in the FlowLayoutPanel to false. 
            foreach (Control c in FlowPanel_Materials.Controls)
            {
                if (c is TextureEntry)
                {
                    ((TextureEntry)c).IsSelected = false;
                }
            }
        }
    }
}

using System;
using System.Windows.Forms;
using ResourceTypes.FrameResource;
using Utils.Language;

namespace Forms.EditorControls
{
    public partial class ControlOptionFrameAdd : UserControl
    {
        public ControlOptionFrameAdd()
        {
            InitializeComponent();
            Localise();
            ComboBox_Type.SelectedIndex = 0;
            CheckBox_AddToNameTable.Checked = false;
        }

        private void Localise()
        {
            Group_General.Text = Language.GetString("$GENERAL");
            Label_Type.Text = Language.GetString("$FRADD_TYPE");
            Label_AddToNameTable.Text = Language.GetString("$FRADD_NAME_TABLE");
        }

        private FrameResourceObjectType GetTypeFromIndexToLookupTable()
        {
            int SelectedIndex = ComboBox_Type.SelectedIndex;
            FrameResourceObjectType SelectedType = FrameResourceObjectType.NULL;

            switch (SelectedIndex)
            {
                case 0:
                    SelectedType = FrameResourceObjectType.SingleMesh;
                    break;
                case 1:
                    SelectedType = FrameResourceObjectType.Frame;
                    break;
                case 2:
                    SelectedType = FrameResourceObjectType.Light;
                    break;
                case 3:
                    SelectedType = FrameResourceObjectType.Camera;
                    break;
                case 4:
                    SelectedType = FrameResourceObjectType.Component_U00000005;
                    break;
                case 5:
                    SelectedType = FrameResourceObjectType.Sector;
                    break;
                case 6:
                    SelectedType = FrameResourceObjectType.Dummy;
                    break;
                case 7:
                    SelectedType = FrameResourceObjectType.ParticleDeflector;
                    break;
                case 8:
                    SelectedType = FrameResourceObjectType.Area;
                    break;
                case 9:
                    SelectedType = FrameResourceObjectType.Target;
                    break;
                case 10:
                    SelectedType = FrameResourceObjectType.Model;
                    break;
                case 11:
                    SelectedType = FrameResourceObjectType.Collision;
                    break;
                case 12:
                    SelectedType = FrameResourceObjectType.Joint;
                    break;
                default:
                    SelectedType = FrameResourceObjectType.NULL;
                    Console.WriteLine("Unknown type selected");
                    break;
            }

            return SelectedType;
        }

        public FrameResourceObjectType GetSelectedType()
        {
            return GetTypeFromIndexToLookupTable();
        }

        public bool GetAddToNameTable()
        {
            return CheckBox_AddToNameTable.Checked;
        }
    }
}

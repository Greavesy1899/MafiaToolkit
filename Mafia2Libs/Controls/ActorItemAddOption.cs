using System.Windows.Forms;
using Utils.Language;
using ResourceTypes.Actors;

namespace Forms.EditorControls
{
    public partial class ActorItemAddOption : UserControl
    {
        public ActorItemAddOption()
        {
            InitializeComponent();
            Localise();

            TypeCombo.Items.Add(ActorTypes.Human);
            TypeCombo.Items.Add(ActorTypes.C_CrashObject);
            TypeCombo.Items.Add(ActorTypes.C_TrafficCar);
            TypeCombo.Items.Add(ActorTypes.C_TrafficHuman);
            TypeCombo.Items.Add(ActorTypes.C_TrafficTrain);
            TypeCombo.Items.Add(ActorTypes.ActionPoint);
            TypeCombo.Items.Add(ActorTypes.ActionPointScript);
            TypeCombo.Items.Add(ActorTypes.ActionPointSearch);
            TypeCombo.Items.Add(ActorTypes.C_Item);
            TypeCombo.Items.Add(ActorTypes.C_Door);
            TypeCombo.Items.Add(ActorTypes.Tree);
            TypeCombo.Items.Add(ActorTypes.C_Sound);
            TypeCombo.Items.Add(ActorTypes.StaticEntity);
            TypeCombo.Items.Add(ActorTypes.Garage);
            TypeCombo.Items.Add(ActorTypes.FrameWrapper);
            TypeCombo.Items.Add(ActorTypes.C_ActorDetector);
            TypeCombo.Items.Add(ActorTypes.Blocker);
            TypeCombo.Items.Add(ActorTypes.C_StaticWeapon);
            TypeCombo.Items.Add(ActorTypes.LightEntity);
            TypeCombo.Items.Add(ActorTypes.C_Cutscene);
            TypeCombo.Items.Add(ActorTypes.C_ScriptEntity);
            TypeCombo.Items.Add(ActorTypes.C_Pinup);
            TypeCombo.Items.Add(ActorTypes.C_Car);
            TypeCombo.Items.Add(ActorTypes.C_Train);
            TypeCombo.Items.Add(ActorTypes.JukeBox);
            TypeCombo.Items.Add(ActorTypes.PhysicsScene);
            TypeCombo.Items.Add(ActorTypes.Boat);
            TypeCombo.Items.Add(ActorTypes.Airplane);
            TypeCombo.Items.Add(ActorTypes.CleanEntity);
            TypeCombo.Items.Add(ActorTypes.Radio);
            TypeCombo.Items.Add(ActorTypes.Telephone);
            TypeCombo.SelectedIndex = 0;
        }

        private void Localise()
        {
            groupGeneral.Text = Language.GetString("$GENERAL");
            ActorTypeLabel.Text = Language.GetString("$ACTOR_TYPE");
            ActorDefinitionLabel.Text = Language.GetString("$ACTOR_DEFINITION_NAME");
        }

        public string GetDefinitionName()
        {
            return DefinitionBox.Text;
        }

        public ActorTypes GetSelectedType()
        {
            return (ActorTypes)TypeCombo.SelectedItem;
        }
    }
}

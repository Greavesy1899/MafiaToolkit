using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using Utils.Helpers.Reflection;

namespace ResourceTypes.M3.XBin
{
    public class GameMeleeTable : BaseTable
    {
        public class MeleeSettings
        {
            public float ReactionDelayMin { get; set; }
            public float ReactionDelayMax { get; set; }
            public float ReadySlowdownSpeed { get; set; }
            public float ReadyReactionDelay { get; set; }
            public float DefenseSlowdownSpeed { get; set; }
            public float DefenseReactionDelay { get; set; }
            public float CinematicRagdollTimer { get; set; }
            public float RagdollTimer { get; set; }
            public float PlaybackSpeed { get; set; }
            public uint AmountOfHit { get; set; }
            public bool Activated { get; set; }
            public bool DoNotDie { get; set; }
            public bool ForceCategoryMelee { get; set; }

            public override string ToString() => "Melee Settings";
        }

        public MeleeSettings Settings { get; set; }

        public GameMeleeTable()
        {
            Settings = new MeleeSettings();
        }

        public void ReadFromFile(BinaryReader reader)
        {
            Settings = new MeleeSettings
            {
                ReactionDelayMin = reader.ReadSingle(),
                ReactionDelayMax = reader.ReadSingle(),
                ReadySlowdownSpeed = reader.ReadSingle(),
                ReadyReactionDelay = reader.ReadSingle(),
                DefenseSlowdownSpeed = reader.ReadSingle(),
                DefenseReactionDelay = reader.ReadSingle(),
                CinematicRagdollTimer = reader.ReadSingle(),
                RagdollTimer = reader.ReadSingle(),
                PlaybackSpeed = reader.ReadSingle(),
                AmountOfHit = reader.ReadUInt32(),
                Activated = reader.ReadByte() != 0,
                DoNotDie = reader.ReadByte() != 0,
                ForceCategoryMelee = reader.ReadByte() != 0,
            };
            reader.ReadByte();
        }

        public void WriteToFile(XBinWriter writer)
        {
            writer.Write(Settings.ReactionDelayMin);
            writer.Write(Settings.ReactionDelayMax);
            writer.Write(Settings.ReadySlowdownSpeed);
            writer.Write(Settings.ReadyReactionDelay);
            writer.Write(Settings.DefenseSlowdownSpeed);
            writer.Write(Settings.DefenseReactionDelay);
            writer.Write(Settings.CinematicRagdollTimer);
            writer.Write(Settings.RagdollTimer);
            writer.Write(Settings.PlaybackSpeed);
            writer.Write(Settings.AmountOfHit);
            writer.Write((byte)(Settings.Activated ? 1 : 0));
            writer.Write((byte)(Settings.DoNotDie ? 1 : 0));
            writer.Write((byte)(Settings.ForceCategoryMelee ? 1 : 0));
            writer.Write((byte)0);
        }

        public void ReadFromXML(string file)
        {
            XElement Root = XElement.Load(file);
            GameMeleeTable Loaded = ReflectionHelpers.ConvertToPropertyFromXML<GameMeleeTable>(Root);
            Settings = Loaded.Settings;
        }

        public void WriteToXML(string file)
        {
            XElement RootElement = ReflectionHelpers.ConvertPropertyToXML(this);
            RootElement.Save(file, SaveOptions.None);
        }

        public TreeNode GetAsTreeNodes()
        {
            TreeNode Root = new TreeNode("Game Melee Settings");
            Root.Tag = Settings;
            return Root;
        }

        public void SetFromTreeNodes(TreeNode Root)
        {
            if (Root.Tag is MeleeSettings ms)
            {
                Settings = ms;
            }
        }
    }
}

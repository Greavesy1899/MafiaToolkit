using System;
using System.IO;
using System.Windows.Forms;
using ResourceTypes.M3.XBin.TableContainers.HealthSystem;

namespace ResourceTypes.M3.XBin.TableContainers
{
    public class TableContainer : BaseTable
    {
        public uint AIWeaponPtr { get; set; } // Not implemented in Toolkit.
        public uint AnimParticlesPtr { get; set; } // Not implemented in game.
        public uint AttackParamsPtr { get; set; } // Not implemented in game.
        public CarColoursTable CarColours { get; set; }
        public CarWindowTintTable CarWindowTints { get; set; }
        public uint CarInteriorColorsTableMPPtr { get; set; } // Not implemented in game.
        public uint CarGearboxesTableMPPtr { get; set; } // Not implemented in game.
        public CarMtrStuffTable CarMtrStuff { get; set; }
        public CarSkidmarksTable CarSkidmarks { get; set; }
        public CarTuningItemTable CarTuningItems { get; set; }
        public uint CarTuningModificatorsTableMPPtr { get; set; } // Not implemented in game.
        public uint CombinableCharactersTableMPPtr { get; set; } // Not implemented in game.
        public uint CrashObjectTablePtr { get; set; } // Not implemented in game.
        public uint CubeMapsTablePtr { get; set; } // Not implemented in game.
        public uint DamageMultiplierTablePtr { get; set; } // Not implemented in game.
        public uint FamilyAlbumExtrasTablePtr { get; set; } // Not implemented in game.
        public uint FamilyAlbumTablePtr { get; set; } // Not implemented in game.
        public HealthSystemTable HealthSystem { get; set; }
        public HumanWeaponImpactTable HumanWeaponImpacts { get; set; }
        public HumanDamageZonesTable HumanDamageZones { get; set; }
        public HumanMaterialsTable HumanMaterials { get; set; }
        public MaterialsPhysicsTable MaterialPhysics { get; set; }
        public MaterialsShotsTable MaterialShots { get; set; }

        public void ReadFromFile(BinaryReader reader)
        {
            // NB: Only suitable for M3 for now.
            AIWeaponPtr = reader.ReadUInt32();
            AnimParticlesPtr = reader.ReadUInt32();
            AttackParamsPtr = reader.ReadUInt32();

            long currentPosition = reader.BaseStream.Position + 4;
            XBinCoreUtils.GotoPtrWithOffset(reader);
            CarColours = new CarColoursTable();
            CarColours.ReadFromFile(reader);

            reader.BaseStream.Seek(currentPosition, SeekOrigin.Begin);
            currentPosition = reader.BaseStream.Position + 4;
            XBinCoreUtils.GotoPtrWithOffset(reader);
            CarWindowTints = new CarWindowTintTable();
            CarWindowTints.ReadFromFile(reader);

            reader.BaseStream.Seek(currentPosition, SeekOrigin.Begin);
            CarInteriorColorsTableMPPtr = reader.ReadUInt32();
            CarGearboxesTableMPPtr = reader.ReadUInt32();

            currentPosition = reader.BaseStream.Position + 4;
            XBinCoreUtils.GotoPtrWithOffset(reader);
            CarMtrStuff = new CarMtrStuffTable();
            CarMtrStuff.ReadFromFile(reader);

            reader.BaseStream.Seek(currentPosition, SeekOrigin.Begin);
            currentPosition = reader.BaseStream.Position + 4;
            XBinCoreUtils.GotoPtrWithOffset(reader);
            CarSkidmarks = new CarSkidmarksTable();
            CarSkidmarks.ReadFromFile(reader);

            reader.BaseStream.Seek(currentPosition, SeekOrigin.Begin);
            currentPosition = reader.BaseStream.Position + 4;
            XBinCoreUtils.GotoPtrWithOffset(reader);
            CarTuningItems = new CarTuningItemTable();
            CarTuningItems.ReadFromFile(reader);

            reader.BaseStream.Seek(currentPosition, SeekOrigin.Begin);
            CarTuningModificatorsTableMPPtr = reader.ReadUInt32();
            CombinableCharactersTableMPPtr = reader.ReadUInt32();
            CrashObjectTablePtr = reader.ReadUInt32();
            CubeMapsTablePtr = reader.ReadUInt32();
            DamageMultiplierTablePtr = reader.ReadUInt32();
            FamilyAlbumExtrasTablePtr = reader.ReadUInt32();
            FamilyAlbumTablePtr = reader.ReadUInt32();
            currentPosition = reader.BaseStream.Position;

            reader.BaseStream.Seek(currentPosition, SeekOrigin.Begin);
            currentPosition = reader.BaseStream.Position + 4;
            XBinCoreUtils.GotoPtrWithOffset(reader);
            HealthSystem = new HealthSystemTable();
            HealthSystem.ReadFromFile(reader);

            reader.BaseStream.Seek(currentPosition, SeekOrigin.Begin);
            currentPosition = reader.BaseStream.Position + 4;
            XBinCoreUtils.GotoPtrWithOffset(reader);
            HumanWeaponImpacts = new HumanWeaponImpactTable();
            HumanWeaponImpacts.ReadFromFile(reader);

            reader.BaseStream.Seek(currentPosition, SeekOrigin.Begin);
            currentPosition = reader.BaseStream.Position + 4;
            XBinCoreUtils.GotoPtrWithOffset(reader);
            HumanDamageZones = new HumanDamageZonesTable();
            HumanDamageZones.ReadFromFile(reader);

            reader.BaseStream.Seek(currentPosition, SeekOrigin.Begin);
            currentPosition = reader.BaseStream.Position + 4;
            XBinCoreUtils.GotoPtrWithOffset(reader);
            HumanMaterials = new HumanMaterialsTable();
            HumanMaterials.ReadFromFile(reader);

            reader.BaseStream.Seek(currentPosition, SeekOrigin.Begin);
            currentPosition = reader.BaseStream.Position + 4;
            XBinCoreUtils.GotoPtrWithOffset(reader);
            MaterialPhysics = new MaterialsPhysicsTable();
            MaterialPhysics.ReadFromFile(reader);

            reader.BaseStream.Seek(currentPosition, SeekOrigin.Begin);
            currentPosition = reader.BaseStream.Position + 4;
            XBinCoreUtils.GotoPtrWithOffset(reader);
            MaterialShots = new MaterialsShotsTable();
            MaterialShots.ReadFromFile(reader);

            // TODO: Everything in this function was always "temporary".
            // Maybe check the other table container files, see if they 
            // are good enough. Otherwise I need to create a new solution
        }

        public void WriteToFile(XBinWriter writer)
        {
            throw new NotImplementedException();
        }

        public void ReadFromXML(string file)
        {
            throw new NotImplementedException();
        }

        public void WriteToXML(string file)
        {
            throw new NotImplementedException();
        }

        public TreeNode GetAsTreeNodes()
        {
            TreeNode Root = new TreeNode();
            Root.Text = "Table Container";
            Root.Nodes.Add(CarColours.GetAsTreeNodes());
            Root.Nodes.Add(CarWindowTints.GetAsTreeNodes());
            Root.Nodes.Add(CarMtrStuff.GetAsTreeNodes());
            Root.Nodes.Add(CarSkidmarks.GetAsTreeNodes());
            Root.Nodes.Add(CarTuningItems.GetAsTreeNodes());
            Root.Nodes.Add(HealthSystem.GetAsTreeNodes());
            Root.Nodes.Add(HumanMaterials.GetAsTreeNodes());
            Root.Nodes.Add(MaterialPhysics.GetAsTreeNodes());
            Root.Nodes.Add(MaterialShots.GetAsTreeNodes());

            return Root;
        }

        public void SetFromTreeNodes(TreeNode Root)
        {
            throw new NotImplementedException();
        }
    }
}

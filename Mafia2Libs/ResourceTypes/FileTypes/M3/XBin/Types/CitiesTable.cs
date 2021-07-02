using ResourceTypes.XBin.Types;
using SharpDX;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using Utils.Helpers.Reflection;
using Utils.Settings;
using Utils.SharpDXExtensions;

namespace ResourceTypes.M3.XBin
{
    public class CitiesTableItem
    {
        public class CityRespawnPlace
        {
            public string StreamMapPart { get; set; }
            public XBinVector3 PosPlayer { get; set; }
            public XBinVector3 DirPlayer { get; set; }
            public ERespawnPlaceType RespawnType { get; set; }

            public CityRespawnPlace()
            {
                StreamMapPart = "";
                PosPlayer = new XBinVector3();
                DirPlayer = new XBinVector3();
            }
        }

        public class CityPolygon
        {
            public string Name { get; set; }
            public XBinHashName TextID { get; set; }
            public ulong Unk0 { get; set; } // TODO: Could include property - PoliceArrivalMultiplier?
            public ushort[] Indexes { get; set; }

            public CityPolygon()
            {
                Name = "";
                TextID = new XBinHashName();
                Indexes = new ushort[0];
            }
        }

        public uint ID { get; set; }
        public string Name { get; set; }
        public string MissionLine { get; set; }
        public string SDSPrefix { get; set; }
        public XBinHashName TextID { get; set; }
        public XBinHashName CarGarageType { get; set; }
        public XBinHashName BoatGarageType { get; set; }
        public string Map { get; set; }
        public string Unk01 { get; set; } // M1DE Only - Says "Lost Heaven" for all Cities.
        public CityRespawnPlace[] RespawnPlaces { get; set; }
        [Browsable(false), PropertyIgnoreByReflector]
        public uint RespawnPlaceOffset { get; set; }
        [Browsable(false), PropertyIgnoreByReflector]
        public uint RespawnPlaceCount { get; set; }
        public XBinVector2[] Points { get; set; }
        public CityPolygon[] Polygons { get; set; }

        public CitiesTableItem()
        {
            Name = "";
            MissionLine = "";
            SDSPrefix = "";
            TextID = new XBinHashName();
            CarGarageType = new XBinHashName();
            BoatGarageType = new XBinHashName();
            Map = "";
            RespawnPlaces = new CityRespawnPlace[0];
            Points = new XBinVector2[0];
            Polygons = new CityPolygon[0];
        }

        public override string ToString()
        {
            return string.Format("ID: {0} - {1}", ID, Name);
        }
    }

    public class CitiesTable : BaseTable
    {
        private uint unk0;
        private uint unk1;
        private CitiesTableItem[] cities;
        private GamesEnumerator gameVersion;

        public CitiesTableItem[] Cities {
            get { return cities; }
            set { cities = value; }
        }

        public CitiesTable()
        {
            cities = new CitiesTableItem[0];
            gameVersion = GameStorage.Instance.GetSelectedGame().GameType;
        }

        public void ReadFromFile(BinaryReader reader)
        {
            unk0 = reader.ReadUInt32();
            uint count0 = reader.ReadUInt32();
            uint count1 = reader.ReadUInt32();
            unk1 = reader.ReadUInt32();
            cities = new CitiesTableItem[count0];

            for (int i = 0; i < count1; i++)
            {
                CitiesTableItem item = new CitiesTableItem();

                item.RespawnPlaceOffset = reader.ReadUInt32();
                item.RespawnPlaceCount = reader.ReadUInt32();
                uint RespawnPlacesCount1 = reader.ReadUInt32();
                uint CityAreasOffset = reader.ReadUInt32(); // Maybe CityAreas?
              
                item.ID = reader.ReadUInt32();
                item.Name = XBinCoreUtils.ReadStringPtrWithOffset(reader);
                item.MissionLine = XBinCoreUtils.ReadStringPtrWithOffset(reader);
                item.SDSPrefix = XBinCoreUtils.ReadStringPtrWithOffset(reader);
                item.TextID.ReadFromFile(reader);
                item.CarGarageType.ReadFromFile(reader);
                item.BoatGarageType.ReadFromFile(reader);

                if (gameVersion == GamesEnumerator.MafiaI_DE)
                {
                    item.Unk01 = XBinCoreUtils.ReadStringPtrWithOffset(reader);
                }

                item.Map = XBinCoreUtils.ReadStringPtrWithOffset(reader);

                // Content doesn't exist in M1DE.. Unused?
                if (gameVersion == GamesEnumerator.MafiaIII)
                {
                    reader.ReadUInt32();
                }

                cities[i] = item;
            }

            foreach(var Item in Cities)
            {
                Item.RespawnPlaces = new CitiesTableItem.CityRespawnPlace[Item.RespawnPlaceCount];

                for(int i = 0; i < Item.RespawnPlaces.Length; i++)
                {
                    CitiesTableItem.CityRespawnPlace RespawnPlace = new CitiesTableItem.CityRespawnPlace();
                    RespawnPlace.PosPlayer.ReadFromFile(reader);
                    RespawnPlace.DirPlayer.ReadFromFile(reader);
                    RespawnPlace.RespawnType = (ERespawnPlaceType)reader.ReadInt32();
                    RespawnPlace.StreamMapPart = XBinCoreUtils.ReadStringPtrWithOffset(reader);
                    Item.RespawnPlaces[i] = RespawnPlace;
                }

                uint CityPointOffset = reader.ReadUInt32();
                uint CityPointCount0 = reader.ReadUInt32();
                uint CityPointCount1 = reader.ReadUInt32();
                uint CityPolygonOffset = reader.ReadUInt32();
                uint CityPolygonCount0 = reader.ReadUInt32();
                uint CityPolygonCount1 = reader.ReadUInt32();

                Item.Points = new XBinVector2[CityPointCount0];
                for(int x = 0; x < CityPointCount0; x++)
                {
                    Item.Points[x] = new XBinVector2();
                    Item.Points[x].ReadFromFile(reader);
                }

                Item.Polygons = new CitiesTableItem.CityPolygon[CityPolygonCount0];
                for (int x = 0; x < CityPolygonCount0; x++)
                {
                    uint PolygonPointOffset = reader.ReadUInt32();
                    uint PolygonPointCount0 = reader.ReadUInt32();
                    uint PolygonPointCount1 = reader.ReadUInt32();

                    CitiesTableItem.CityPolygon Polygon = new CitiesTableItem.CityPolygon();
                    Polygon.Name = XBinCoreUtils.ReadStringPtrWithOffset(reader);
                    Polygon.TextID.ReadFromFile(reader);
                    Polygon.Unk0 = reader.ReadUInt64();
                    Polygon.Indexes = new ushort[PolygonPointCount0];

                    for (int z = 0; z < PolygonPointCount0; z++)
                    {
                        Polygon.Indexes[z] = reader.ReadUInt16();
                    }

                    Item.Polygons[x] = Polygon;
                }
            }
        }

        public void WriteToFile(XBinWriter writer)
        {
            writer.Write(unk0);
            writer.Write(cities.Length);
            writer.Write(cities.Length);
            writer.Write(unk1);

            foreach(var Item in cities)
            {
                writer.PushObjectPtr(string.Format("RespawnPlace_{0}", Item.ID));
                writer.Write(Item.RespawnPlaces.Length);
                writer.Write(Item.RespawnPlaces.Length);
                writer.PushObjectPtr(string.Format("CityArea_{0}", Item.ID));

                writer.Write(Item.ID);
                writer.PushStringPtr(Item.Name);
                writer.PushStringPtr(Item.MissionLine);
                writer.PushStringPtr(Item.SDSPrefix);
                Item.TextID.WriteToFile(writer);
                Item.CarGarageType.WriteToFile(writer);
                Item.BoatGarageType.WriteToFile(writer);

                if (gameVersion == GamesEnumerator.MafiaI_DE)
                {
                    writer.PushStringPtr(Item.Unk01);
                }

                writer.PushStringPtr(Item.Map);

                // Content doesn't exist in M1DE.. Unused?
                if (gameVersion == GamesEnumerator.MafiaIII)
                {
                    writer.Write(0);
                }
            }

            foreach(var Item in Cities)
            {
                writer.FixUpObjectPtr(string.Format("RespawnPlace_{0}", Item.ID));
                foreach (var Respawn in Item.RespawnPlaces)
                {
                    Respawn.PosPlayer.WriteToFile(writer);
                    Respawn.DirPlayer.WriteToFile(writer);
                    writer.Write((int)Respawn.RespawnType);
                    writer.PushStringPtr(Respawn.StreamMapPart);
                }

                writer.FixUpObjectPtr(string.Format("CityArea_{0}", Item.ID));

                writer.PushObjectPtr("CityPointOffset");
                writer.Write(Item.Points.Length);
                writer.Write(Item.Points.Length);
                writer.PushObjectPtr("CityPolygonOffset");
                writer.Write(Item.Polygons.Length);
                writer.Write(Item.Polygons.Length);

                writer.FixUpObjectPtr("CityPointOffset");

                foreach(var Entry in Item.Points)
                {
                    Entry.WriteToFile(writer);
                }

                writer.FixUpObjectPtr("CityPolygonOffset");

                foreach (var Entry in Item.Polygons)
                {
                    writer.PushObjectPtr("PolygonIndexOffset");
                    writer.Write(Entry.Indexes.Length);
                    writer.Write(Entry.Indexes.Length);

                    writer.PushStringPtr(Entry.Name);
                    Entry.TextID.WriteToFile(writer);
                    writer.Write(Entry.Unk0);

                    writer.FixUpObjectPtr("PolygonIndexOffset");
                    foreach (var Index in Entry.Indexes)
                    {
                        writer.Write(Index);
                    }
                }
            }

            writer.FixUpStringPtrs();
        }

        public void ReadFromXML(string file)
        {          
            XElement Root = XElement.Load(file);
            CitiesTable TableInformation = ReflectionHelpers.ConvertToPropertyFromXML<CitiesTable>(Root);
            this.Cities = TableInformation.Cities;
        }

        public void WriteToXML(string file)
        {
            XElement RootElement = ReflectionHelpers.ConvertPropertyToXML(this);
            RootElement.Save(file, SaveOptions.None);
        }

        public TreeNode GetAsTreeNodes()
        {
            TreeNode Root = new TreeNode();
            Root.Text = "Cities Table";

            foreach (var Item in cities)
            {
                TreeNode ChildNode = new TreeNode();
                ChildNode.Tag = Item;
                ChildNode.Text = Item.ToString();
                Root.Nodes.Add(ChildNode);
            }
            
            return Root;
        }

        public void SetFromTreeNodes(TreeNode Root)
        {
            Cities = new CitiesTableItem[Root.Nodes.Count];

            for (int i = 0; i < Cities.Length; i++)
            {
                TreeNode ChildNode = Root.Nodes[i];
                CitiesTableItem Entry = (CitiesTableItem)ChildNode.Tag;
                Cities[i] = Entry;
            }
        }
    }
}

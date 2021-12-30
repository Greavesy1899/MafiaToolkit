using System;
using System.Globalization;
using System.IO;
using System.Numerics;
using System.Xml;

namespace ResourceTypes.Navigation.Traffic
{
    static class XmlNodeExtensions
    {
        public static string GetAttrValue(this XmlNode node, string name)
        {
            return node.Attributes.GetNamedItem(name).Value;
        }
        
        public static string GetFieldValue(this XmlNode node, string name)
        {
            return node.SelectSingleNode(name).InnerText;
        }
        
        public static void AddAttrWithValue(this XmlElement node, XmlDocument xml, string name, string value)
        {
            var attribute = xml.CreateAttribute(name);
            attribute.Value = value;
            node.Attributes.Append(attribute);
        }
        
        public static void AddFieldWithValue(this XmlNode node, XmlDocument xml, string name, string value)
        {
            var fieldNode = xml.CreateElement(name);
            fieldNode.InnerText = value;
            node.AppendChild(fieldNode);
        }
        
        public static void AddFieldWithValue(this XmlNode node, XmlDocument xml, string name, int value)
        {
            var fieldNode = xml.CreateElement(name);
            fieldNode.InnerText = value.ToString();
            node.AppendChild(fieldNode);
        }
        
        public static void AddFieldWithValue(this XmlNode node, XmlDocument xml, string name, float value)
        {
            var fieldNode = xml.CreateElement(name);
            fieldNode.InnerText = value.ToString(CultureInfo.InvariantCulture);
            node.AppendChild(fieldNode);
        }
        
        public static string ToStrMaxAsNegOne(this ushort value)
        {
            return value == ushort.MaxValue ? "-1" : value.ToString();
        }
    }
    
    public class RoadmapXmlSerializer
    {
        public void Serialize(IRoadmap roadmap, string outFilePath)
        {
            using (FileStream fileStream = File.Open(outFilePath, FileMode.Create))
            {
                XmlDocument xml = new XmlDocument();

                XmlElement root = xml.CreateElement("roadmap");
                xml.AppendChild(root);

                var splinesNode = xml.CreateElement("roadSplines");
                for (var i = 0; i < roadmap.Splines.Count; i++)
                {
                    var splineNode = xml.CreateElement("roadSpline");
                    var indexAttribute = xml.CreateAttribute("index");
                    indexAttribute.Value = i.ToString();
                    splineNode.Attributes.Append(indexAttribute);
                    WriteSpline(roadmap.Splines[i], xml, splineNode);
                    splinesNode.AppendChild(splineNode);
                }
                root.AppendChild(splinesNode);

                var roadsNode = xml.CreateElement("roads");
                for (var i = 0; i < roadmap.Roads.Count; i++)
                {
                    var roadNode = xml.CreateElement("road");
                    var indexAttribute = xml.CreateAttribute("index");
                    indexAttribute.Value = i.ToString();
                    roadNode.Attributes.Append(indexAttribute);
                    var fromCrossroad = roadmap.RoadToCrossroadMapping[i * 2];
                    var toCrossroad = roadmap.RoadToCrossroadMapping[i * 2 + 1];
                    WriteRoad(roadmap.Roads[i], fromCrossroad, toCrossroad, xml, roadNode);
                    roadsNode.AppendChild(roadNode);
                }
                root.AppendChild(roadsNode);

                var crossroadsNode = xml.CreateElement("crossroads");
                for (var i = 0; i < roadmap.Crossroads.Count; i++)
                {
                    var crossroadNode = xml.CreateElement("crossroad");
                    var indexAttribute = xml.CreateAttribute("index");
                    indexAttribute.Value = i.ToString();
                    crossroadNode.Attributes.Append(indexAttribute);
                    WriteCrossroad(roadmap.Crossroads[i], xml, crossroadNode);
                    crossroadsNode.AppendChild(crossroadNode);
                }
                root.AppendChild(crossroadsNode);
               
                var roadGraphNode = xml.CreateElement("roadGraph");
                for (var i = 0; i < roadmap.CostMap.Count; i++)
                {
                    var roadGraphEdgeNode = xml.CreateElement("roadGraphEdge");
                    var costMapping = roadmap.CostMap[i];
                    roadGraphEdgeNode.AddAttrWithValue(xml, "index", i.ToString());
                    roadGraphEdgeNode.AddFieldWithValue(xml, "edgeType", costMapping.RoadGraphEdgeType.ToString());
                    roadGraphEdgeNode.AddFieldWithValue(xml, "cost", costMapping.Cost.ToString());

                    if (costMapping.RoadGraphEdgeType == RoadGraphEdgeType.Road)
                    {
                        roadGraphEdgeNode.AddFieldWithValue(xml, "roadIndex", costMapping.RoadGraphEdgeLink.ToString());
                        var roadIndex = roadmap.GetRoadIndexForRoadGraphEdge(i);
                        if (roadIndex != costMapping.RoadGraphEdgeLink)
                        {
                            throw new InvalidDataException($"Unexpected road index mismatch: {costMapping.RoadGraphEdgeLink} != {roadIndex}");
                        }
                    }
                    else if (costMapping.RoadGraphEdgeType == RoadGraphEdgeType.CrossroadJunction)
                    {
                        roadGraphEdgeNode.AddFieldWithValue(xml, "crossroadIndex", costMapping.RoadGraphEdgeLink.ToString());
                        roadGraphEdgeNode.AddFieldWithValue(xml, "targetRoadIndex", roadmap.GetRoadIndexForRoadGraphEdge(i).ToString());
                    }

                    roadGraphNode.AppendChild(roadGraphEdgeNode);
                }
                root.AppendChild(roadGraphNode);

                xml.Save(fileStream);
            }
        }

        // private void WriteXmlField(string name, string value, XmlDocument xml, XmlNode node)
        // {
        //     var fieldNode = xml.CreateElement(name);
        //     fieldNode.InnerText = value;
        //     node.AppendChild(fieldNode);
        // }

        // private string UshortToStrWithMaxValueNegativeOne(ushort value)
        // {
        //     return value == ushort.MaxValue ? "-1" : value.ToString();
        // }

        private ushort StrWithMaxValueNegativeOneToUshort(string value)
        {
            return value == "-1" ? ushort.MaxValue : ushort.Parse(value);
        }

        #region Serialization

        private void WritePoint(Vector3 point, XmlDocument xml, XmlElement node)
        {
            var attributeX = xml.CreateAttribute("x");
            attributeX.Value = point.X.ToString(CultureInfo.InvariantCulture);
            var attributeY = xml.CreateAttribute("y");
            attributeY.Value = point.Y.ToString(CultureInfo.InvariantCulture);
            var attributeZ = xml.CreateAttribute("z");
            attributeZ.Value = point.Z.ToString(CultureInfo.InvariantCulture);
            node.Attributes.Append(attributeX);
            node.Attributes.Append(attributeY);
            node.Attributes.Append(attributeZ);
        }
        
        private void WriteSpline(IRoadSpline spline, XmlDocument xml, XmlNode node)
        {
            var pointsNode = xml.CreateElement("points");
            for (var i = 0; i < spline.Points.Count; i++)
            {
                var point = spline.Points[i];
                var pointNode = xml.CreateElement("point");
                WritePoint(point, xml, pointNode);
                pointsNode.AppendChild(pointNode);
            }
            
            var lengthNode = xml.CreateElement("length");
            lengthNode.InnerText = spline.Length.ToString(CultureInfo.InvariantCulture);

            node.AppendChild(pointsNode);
            node.AppendChild(lengthNode);
        }

        private void WriteRangeFlag(IRangeFlag flag, XmlDocument xml, XmlNode node)
        {
            node.AddFieldWithValue(xml, "type", flag.RangeFlagType.ToString());
            switch (flag.RangeFlagType)
            {
                case RangeFlagType.Crosswalk:
                case RangeFlagType.Tunnel:
                    node.AddFieldWithValue(xml, "index", flag.Index);
                    node.AddFieldWithValue(xml, "unkn4", flag.Unkn4);
                    goto case RangeFlagType.RailroadCrossing;
                case RangeFlagType.RailroadCrossing:
                case RangeFlagType.Parking:
                case RangeFlagType.BusStop:
                case RangeFlagType.TrainStation:
                    node.AddFieldWithValue(xml, "from", flag.From);
                    node.AddFieldWithValue(xml, "distance", flag.Distance);
                    node.AddFieldWithValue(xml, "unkn5", flag.Unkn5);
                    break;
            }
        }
        
        private void WriteLane(ILaneDefinition lane, XmlDocument xml, XmlNode node)
        {
            var widthNode = xml.CreateElement("width");
            widthNode.InnerText = lane.Width.ToString(CultureInfo.InvariantCulture);
            node.AppendChild(widthNode);
            
            var typeNode = xml.CreateElement("type");
            typeNode.InnerText = lane.LaneType.ToString();
            node.AppendChild(typeNode);
            
            var flagsNode = xml.CreateElement("flags");
            flagsNode.InnerText = lane.LaneFlags.ToString();
            node.AppendChild(flagsNode);
            
            var centerOffsetNode = xml.CreateElement("centerOffset");
            centerOffsetNode.InnerText = lane.CenterOffset.ToString();
            node.AppendChild(centerOffsetNode);
            
            var rangeFlagsNode = xml.CreateElement("rangeFlags");
            node.AppendChild(rangeFlagsNode);
            for (var i = 0; i < lane.RangeFlags.Count; i++)
            {
                var rangeFlagNode = xml.CreateElement("rangeFlag");
                var indexAttribute = xml.CreateAttribute("index");
                indexAttribute.Value = i.ToString();
                rangeFlagNode.Attributes.Append(indexAttribute);
                WriteRangeFlag(lane.RangeFlags[i], xml, rangeFlagNode);
                rangeFlagsNode.AppendChild(rangeFlagNode);
            }
        }

        private void WriteRoad(IRoadDefinition road, ushort fromCrossroad, ushort toCrossroad, XmlDocument xml, XmlElement node)
        {
            node.AddAttrWithValue(xml, "fromCrossroad", fromCrossroad.ToString());
            node.AddAttrWithValue(xml, "toCrossroad", toCrossroad.ToString());
            
            node.AddFieldWithValue(xml, "roadGraphEdgeIndex", road.RoadGraphEdgeIndex.ToStrMaxAsNegOne());
            node.AddFieldWithValue(xml, "oppositeRoadGraphEdgeIndex", road.OppositeRoadGraphEdgeIndex.ToStrMaxAsNegOne());
            
            node.AddFieldWithValue(xml, "oppositeLanesCount", road.OppositeLanesCount);
            node.AddFieldWithValue(xml, "forwardLanesCount", road.ForwardLanesCount);
            
            var lanesNode = xml.CreateElement("lanes");
            node.AppendChild(lanesNode);
            for (var i = 0; i < road.Lanes.Count; i++)
            {
                var laneNode = xml.CreateElement("lane");
                laneNode.AddAttrWithValue(xml, "index", i.ToString());
                WriteLane(road.Lanes[i], xml, laneNode);
                lanesNode.AppendChild(laneNode);
            }
            
            var rangeFlagsNode = xml.CreateElement("rangeFlags");
            node.AppendChild(rangeFlagsNode);
            for (var i = 0; i < road.RangeFlags.Count; i++)
            {
                var rangeFlagNode = xml.CreateElement("rangeFlag");
                rangeFlagNode.AddAttrWithValue(xml, "index", i.ToString());
                WriteRangeFlag(road.RangeFlags[i], xml, rangeFlagNode);
                rangeFlagsNode.AppendChild(rangeFlagNode);
            }
 
            node.AddFieldWithValue(xml, "maxSpawnedCars", road.MaxSpawnedCars);
            node.AddFieldWithValue(xml, "direction", road.Direction.ToString());
            node.AddFieldWithValue(xml, "splineIndex", road.RoadSplineIndex);
            node.AddFieldWithValue(xml, "type", road.RoadType.ToString());
        }

        private void WriteJunction(IRoadJunction junction, XmlDocument xml, XmlNode node)
        {
            node.AddFieldWithValue(xml, "fromRoadGraphEdge", junction.FromRoadGraphEdgeIndex);
            node.AddFieldWithValue(xml, "fromLane", junction.FromLaneIndex);
            node.AddFieldWithValue(xml, "toRoadGraphEdge", junction.ToRoadGraphEdgeIndex);
            node.AddFieldWithValue(xml, "toLane", junction.ToLaneIndex);
            node.AddFieldWithValue(xml, "unkn6", junction.Unkn6);
            node.AddFieldWithValue(xml, "unkn8", junction.Unkn8);
            
            var splineNode = xml.CreateElement("spline");
            WriteSpline(junction.Spline, xml, splineNode);
            node.AppendChild(splineNode);
        }

        private void WriteTrafficLightSemaphore(ITrafficLightSemaphore semaphore, XmlDocument xml, XmlNode node)
        {
            node.AddFieldWithValue(xml, "unkn0_0", semaphore.Unkn0_0);
            node.AddFieldWithValue(xml, "unkn0_4", semaphore.Unkn0_4);
            node.AddFieldWithValue(xml, "unkn0_8", semaphore.Unkn0_8);
            node.AddFieldWithValue(xml, "unkn0_12", semaphore.Unkn0_12);
            node.AddFieldWithValue(xml, "unkn2", semaphore.Unkn2);
            
            var managedRoadsNode = xml.CreateElement("managedRoads");
            node.AppendChild(managedRoadsNode);
            foreach (var value in semaphore.ManagedRoads)
            {
                managedRoadsNode.AddFieldWithValue(xml, "value", value.ToStrMaxAsNegOne());
            }
        }

        private void WriteCrossroad(ICrossroad crossroad, XmlDocument xml, XmlNode node)
        {
            var pivotPointNode = xml.CreateElement("pivotPoint");
            WritePoint(crossroad.PivotPoint, xml, pivotPointNode);
            node.AppendChild(pivotPointNode);
            
            node.AddFieldWithValue(xml, "index", crossroad.Index);
            
            var junctionsNode = xml.CreateElement("roadJunctions");
            node.AppendChild(junctionsNode);
            for (var i = 0; i < crossroad.Junctions.Count; i++)
            {
                var junctionNode = xml.CreateElement("roadJunction");
                junctionNode.AddAttrWithValue(xml, "index", i.ToString());
                WriteJunction(crossroad.Junctions[i], xml, junctionNode);
                junctionsNode.AppendChild(junctionNode);
            }
            
            var boundsNode = xml.CreateElement("bounds");
            node.AppendChild(boundsNode);
            for (var i = 0; i < crossroad.Bounds.Count; i++)
            {
                var junctionNode = xml.CreateElement("point");
                WritePoint(crossroad.Bounds[i], xml, junctionNode);
                boundsNode.AppendChild(junctionNode);
            }
            
            var trafficLightSemaphoresNode = xml.CreateElement("trafficLightSemaphores");
            node.AppendChild(trafficLightSemaphoresNode);
            for (var i = 0; i < crossroad.TrafficLightSemaphores.Count; i++)
            {
                var semaphoreNode = xml.CreateElement("semaphore");
                semaphoreNode.AddAttrWithValue(xml, "index", i.ToString());
                WriteTrafficLightSemaphore(crossroad.TrafficLightSemaphores[i], xml, semaphoreNode);
                trafficLightSemaphoresNode.AppendChild(semaphoreNode);
            }
        }

        #endregion

        public IRoadmap Deserialize(IRoadmapFactory roadmapFactory, string filePath)
        {
            IRoadmap roadmap = roadmapFactory.Roadmap();

            using (FileStream fileStream = File.Open(filePath, FileMode.Open))
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(fileStream);
                XmlElement root = xml.DocumentElement;

                var roadSplinesNode = root.SelectSingleNode("roadSplines");
                foreach (XmlNode splineNode in roadSplinesNode.ChildNodes)
                {
                    if (splineNode.Name == "roadSpline")
                    {
                        IRoadSpline spline = roadmapFactory.Spline();
                        ReadSpline(spline, splineNode);
                        roadmap.Splines.Add(spline);
                    }
                }
                
                var roadNodes = root.SelectNodes("roads/road");
                foreach (XmlNode roadNode in roadNodes)
                {
                        IRoadDefinition road = roadmapFactory.Road();
                        ReadRoad(road, roadmapFactory, roadNode);
                        roadmap.Roads.Add(road);
                        var fromCrossroad = ushort.Parse(roadNode.GetAttrValue("fromCrossroad"));
                        var toCrossroad = ushort.Parse(roadNode.GetAttrValue("toCrossroad"));
                        roadmap.RoadToCrossroadMapping.Add(fromCrossroad);
                        roadmap.RoadToCrossroadMapping.Add(toCrossroad);
                }
                
                var crossroadNodes = root.SelectNodes("crossroads/crossroad");
                foreach (XmlNode crossroadNode in crossroadNodes)
                {
                    ICrossroad crossroad = roadmapFactory.Crossroad();
                    ReadCrossroad(crossroad, roadmapFactory, crossroadNode);
                    roadmap.Crossroads.Add(crossroad);
                }
                
                var roadGraphEdgeNodes = root.SelectNodes("roadGraph/roadGraphEdge");
                foreach (XmlNode edgeNode in roadGraphEdgeNodes)
                {
                    ICostMapEntry costMapEntry = roadmapFactory.CostMapEntry();
                    roadmap.CostMap.Add(costMapEntry);
                    
                    var type = (RoadGraphEdgeType) Enum.Parse(typeof(RoadGraphEdgeType), edgeNode.GetFieldValue("edgeType"));
                    var cost = ushort.Parse(edgeNode.GetFieldValue("cost"));
                    costMapEntry.RoadGraphEdgeType = type;
                    costMapEntry.Cost = cost;

                    if (type == RoadGraphEdgeType.Road)
                    {
                        var roadIndex = ushort.Parse(edgeNode.GetFieldValue("roadIndex"));
                        costMapEntry.RoadGraphEdgeLink = roadIndex;
                        
                        roadmap.AddRoadEdge(roadIndex);
                    }
                    else if (type == RoadGraphEdgeType.CrossroadJunction)
                    {
                        var crossroadIndex = ushort.Parse(edgeNode.GetFieldValue("crossroadIndex"));
                        var targetRoadIndex = ushort.Parse(edgeNode.GetFieldValue("targetRoadIndex"));
                        
                        costMapEntry.RoadGraphEdgeLink = crossroadIndex;
                        
                        roadmap.AddCrossroadJunctionEdge(targetRoadIndex);
                    }
                }
            }

            return roadmap;
        }

        private Vector3 ReadPoint(XmlNode node)
        {
            Vector3 point = new Vector3();
            point.X = float.Parse(node.GetAttrValue("x"));
            point.Y = float.Parse(node.GetAttrValue("y"));
            point.Z = float.Parse(node.GetAttrValue("z"));
            return point;
        }

        private void ReadSpline(IRoadSpline spline, XmlNode splineNode)
        {
            var pointsNodes = splineNode.SelectNodes("points/point");
            foreach (XmlNode pointNode in pointsNodes)
            {
                Vector3 point = ReadPoint(pointNode);
                spline.Points.Add(point);
            }

            spline.Length = float.Parse(splineNode.GetFieldValue("length"));
        }
        
        private void ReadRoad(IRoadDefinition road, IRoadmapFactory roadmapFactory, XmlNode roadNode)
        {
            var roadGraphEdgeIndex = StrWithMaxValueNegativeOneToUshort(roadNode.GetFieldValue("roadGraphEdgeIndex"));
            var oppositeRoadGraphEdgeIndex = StrWithMaxValueNegativeOneToUshort(roadNode.GetFieldValue("oppositeRoadGraphEdgeIndex"));
            var oppositeLanesCount = byte.Parse(roadNode.GetFieldValue("oppositeLanesCount"));
            var forwardLanesCount = byte.Parse(roadNode.GetFieldValue("forwardLanesCount"));
            var maxSpawnedCars = byte.Parse(roadNode.GetFieldValue("maxSpawnedCars"));
            var direction = (RoadDirection) Enum.Parse(typeof(RoadDirection), roadNode.GetFieldValue("direction"));
            var splineIndex = ushort.Parse(roadNode.GetFieldValue("splineIndex"));
            var type = (RoadType) Enum.Parse(typeof(RoadType), roadNode.GetFieldValue("type"));

            road.RoadGraphEdgeIndex = roadGraphEdgeIndex;
            road.OppositeRoadGraphEdgeIndex = oppositeRoadGraphEdgeIndex;
            road.OppositeLanesCount = oppositeLanesCount;
            road.ForwardLanesCount = forwardLanesCount;
            road.MaxSpawnedCars = maxSpawnedCars;
            road.Direction = direction;
            road.RoadSplineIndex = splineIndex;
            road.RoadType = type;
            
            var laneNodes = roadNode.SelectNodes("lanes/lane");
            foreach (XmlNode laneNode in laneNodes)
            {
                ILaneDefinition lane = roadmapFactory.Lane();
                ReadLane(lane, roadmapFactory, laneNode);
                road.Lanes.Add(lane);
            }
            
            var rangeFlagNodes = roadNode.SelectNodes("rangeFlags/rangeFlag");
            foreach (XmlNode rangeFlagNode in rangeFlagNodes)
            {
                IRangeFlag rangeFlag = roadmapFactory.RangeFlag();
                ReadRangeFlag(rangeFlag, rangeFlagNode);
                road.RangeFlags.Add(rangeFlag);
            }
        }

        private void ReadLane(ILaneDefinition lane, IRoadmapFactory roadmapFactory, XmlNode laneNode)
        {
            var width = float.Parse(laneNode.GetFieldValue("width"));
            var type = (LaneType) Enum.Parse(typeof(LaneType), laneNode.GetFieldValue("type"));
            var flags = (LaneFlags) Enum.Parse(typeof(LaneFlags), laneNode.GetFieldValue("flags"));
            var centerOffset = ushort.Parse(laneNode.GetFieldValue("centerOffset"));

            lane.Width = width;
            lane.LaneType = type;
            lane.LaneFlags = flags;
            lane.CenterOffset = centerOffset;
            
            var rangeFlagNodes = laneNode.SelectNodes("rangeFlags/rangeFlag");
            foreach (XmlNode rangeFlagNode in rangeFlagNodes)
            {
                IRangeFlag rangeFlag = roadmapFactory.RangeFlag();
                ReadRangeFlag(rangeFlag, rangeFlagNode);
                lane.RangeFlags.Add(rangeFlag);
            }
        }

        private void ReadRangeFlag(IRangeFlag rangeFlag, XmlNode rangeFlagNode)
        {
            var type = (RangeFlagType) Enum.Parse(typeof(RangeFlagType), rangeFlagNode.GetFieldValue("type"));
            rangeFlag.RangeFlagType = type;

            switch (type)
            {
                case RangeFlagType.Crosswalk:
                case RangeFlagType.Tunnel:
                    var index = byte.Parse(rangeFlagNode.GetFieldValue("index"));
                    var unkn4 = ushort.Parse(rangeFlagNode.GetFieldValue("unkn4"));
                    rangeFlag.Index = index;
                    rangeFlag.Unkn4 = unkn4;
                    goto case RangeFlagType.RailroadCrossing;
                case RangeFlagType.RailroadCrossing:
                case RangeFlagType.Parking:
                case RangeFlagType.BusStop:
                case RangeFlagType.TrainStation:
                    var from = float.Parse(rangeFlagNode.GetFieldValue("from"));
                    var distance = float.Parse(rangeFlagNode.GetFieldValue("distance"));
                    var unkn5 = float.Parse(rangeFlagNode.GetFieldValue("unkn5"));
                    rangeFlag.From = from;
                    rangeFlag.Distance = distance;
                    rangeFlag.Unkn5 = unkn5;
                    break;
            }
        }

        private void ReadCrossroad(ICrossroad crossroad, IRoadmapFactory roadmapFactory, XmlNode crossroadNode)
        {
            var pivotPoint = ReadPoint(crossroadNode.SelectSingleNode("pivotPoint"));
            var index = ushort.Parse(crossroadNode.GetFieldValue("index"));
            crossroad.PivotPoint = pivotPoint;
            crossroad.Index = index;

            
            var junctionNodes = crossroadNode.SelectNodes("roadJunctions/roadJunction");
            foreach (XmlNode junctionNode in junctionNodes)
            {
                IRoadJunction junction = roadmapFactory.Junction();
                ReadRoadJunction(junction, roadmapFactory, junctionNode);
                crossroad.Junctions.Add(junction);
            }
            
            var boundNodes = crossroadNode.SelectNodes("bounds/point");
            foreach (XmlNode pointNode in boundNodes)
            {
                crossroad.Bounds.Add(ReadPoint(pointNode));
            }
            
            var semaphoreNodes = crossroadNode.SelectNodes("trafficLightSemaphores/semaphore");
            foreach (XmlNode semaphoreNode in semaphoreNodes)
            {
                ITrafficLightSemaphore semaphore = roadmapFactory.TrafficLightSemaphore();
                ReadTrafficLightSemaphore(semaphore, semaphoreNode);
                crossroad.TrafficLightSemaphores.Add(semaphore);
            }
        }

        private void ReadRoadJunction(IRoadJunction junction, IRoadmapFactory roadmapFactory, XmlNode junctionNode)
        {
            var from = ushort.Parse(junctionNode.GetFieldValue("fromRoadGraphEdge"));
            var fromLane = byte.Parse(junctionNode.GetFieldValue("fromLane"));
            var to = ushort.Parse(junctionNode.GetFieldValue("toRoadGraphEdge"));
            var toLane = byte.Parse(junctionNode.GetFieldValue("toLane"));
            var unkn6 = byte.Parse(junctionNode.GetFieldValue("unkn6"));
            var unkn8 = ushort.Parse(junctionNode.GetFieldValue("unkn8"));

            var spline = roadmapFactory.Spline();
            ReadSpline(spline, junctionNode.SelectSingleNode("spline"));

            junction.FromRoadGraphEdgeIndex = from;
            junction.FromLaneIndex = fromLane;
            junction.ToRoadGraphEdgeIndex = to;
            junction.ToLaneIndex = toLane;
            junction.Unkn4 = 255; // always 255
            junction.Unkn6 = unkn6;
            junction.Unkn8 = unkn8;
            junction.Spline = spline;
        }

        private void ReadTrafficLightSemaphore(ITrafficLightSemaphore semaphore, XmlNode semaphoreNode)
        {
            var unkn0_0 = byte.Parse(semaphoreNode.GetFieldValue("unkn0_0"));
            var unkn0_4 = byte.Parse(semaphoreNode.GetFieldValue("unkn0_4"));
            var unkn0_8 = byte.Parse(semaphoreNode.GetFieldValue("unkn0_8"));
            var unkn0_12 = byte.Parse(semaphoreNode.GetFieldValue("unkn0_12"));
            var unkn2 = ushort.Parse(semaphoreNode.GetFieldValue("unkn2"));
            semaphore.Unkn0_0 = unkn0_0;
            semaphore.Unkn0_4 = unkn0_4;
            semaphore.Unkn0_8 = unkn0_8;
            semaphore.Unkn0_12 = unkn0_12;
            semaphore.Unkn2 = unkn2;
            
            var managedRoadsValuesNodes = semaphoreNode.SelectNodes("managedRoads/value");
            foreach (XmlNode valueNode in managedRoadsValuesNodes)
            {
                var valueStr = valueNode.InnerText;
                var value = StrWithMaxValueNegativeOneToUshort(valueStr);
                semaphore.ManagedRoads.Add(value);
            }
        }
    }
}
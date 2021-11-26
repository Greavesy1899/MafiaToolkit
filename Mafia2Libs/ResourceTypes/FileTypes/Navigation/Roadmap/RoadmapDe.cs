using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using Gibbed.IO;


namespace ResourceTypes.Navigation.Traffic
{
    public class RangeFlagDe : IRangeFlag
    {
        public float From { get; set; }
        public float Distance { get; set; }
        private RangeFlagType _rangeFlagType;
        public RangeFlagType RangeFlagType
        {
            get { return _rangeFlagType; }
            set { _rangeFlagType = value; SetDefaultValues(); }
        }
        public byte Unkn3 { get; set; }
        
        private ushort _indexAndUnkn4;
        public byte Index
        {
            get { return (byte) (_indexAndUnkn4 & 0x7); }
            set { _indexAndUnkn4 = (byte) ((byte) (_indexAndUnkn4 & 0xFFF8) | (byte) (value & 0x7)); }
        }
        public ushort Unkn4
        {
            get { return (ushort) (_indexAndUnkn4 >> 3); }
            set { _indexAndUnkn4 = (ushort) ((ushort) (_indexAndUnkn4 & 0x7) | (ushort) ((ushort) (value & 0x1FFF) << 3)); }
        }
        public float Unkn5 { get; set; }
        
        public void Read(Stream input, Endian endian = Endian.Little)
        {
            From = input.ReadValueF32(endian);
            Distance = input.ReadValueF32(endian);
            _rangeFlagType = (RangeFlagType)input.ReadValueU8();
            Unkn3 = input.ReadValueU8();
            Unkn5 = input.ReadValueF32(endian);
            _indexAndUnkn4 = input.ReadValueU16(endian);
        }

        public void Write(Stream output, Endian endian = Endian.Little)
        {
            output.WriteValueF32(From, endian);
            output.WriteValueF32(Distance, endian);
            output.WriteValueU8((byte) _rangeFlagType);
            output.WriteValueU8(Unkn3);
            output.WriteValueF32(Unkn5, endian);
            output.WriteValueU16(_indexAndUnkn4, endian);
        }

        private void SetDefaultValues()
        {
            switch (_rangeFlagType)
            {
                case RangeFlagType.RailroadCrossing:
                case RangeFlagType.Parking:
                case RangeFlagType.BusStop:
                case RangeFlagType.TrainStation:
                    Unkn3 = 0xFA;
                    _indexAndUnkn4 = 0xFAFA;
                    break;
                case RangeFlagType.Crosswalk:
                case RangeFlagType.Tunnel:
                    Unkn3 = 0xFA;
                    break;
            }
        }
    }

    public class LaneDefinitionDe : ILaneDefinition
    {
        public float Width { get; set; }
        private byte _laneTypeAndFlags;
        public LaneType LaneType
        {
            get { return (LaneType)(_laneTypeAndFlags & 0xF); }
            set { _laneTypeAndFlags = (byte) ((byte) (_laneTypeAndFlags & 0xF0) | (byte) ((byte)value & 0xF)); }
        }
        public LaneFlags LaneFlags
        {
            get { return (LaneFlags)((_laneTypeAndFlags & 0xF0) >> 4); }
            set { _laneTypeAndFlags = (byte) ((byte) (_laneTypeAndFlags & 0x0F) | (byte) (((byte)value & 0xF) << 4)); }
        }
        public ushort CenterOffset { get; set; }
        public List<IRangeFlag> RangeFlags { get; } = new List<IRangeFlag>();
        
        public void Read(Stream input, Endian endian = Endian.Little)
        {
            Width = input.ReadValueF32(endian);
            CenterOffset = input.ReadValueU16(endian);
            _laneTypeAndFlags = input.ReadValueU8();
            ushort rangeFlagsCount = input.ReadValueU16(endian);
            for (int i = 0; i < rangeFlagsCount; i++)
            {
                var rangeFlag = new RangeFlagDe();
                rangeFlag.Read(input, endian);
                RangeFlags.Add(rangeFlag);
            }
        }

        public void Write(Stream output, Endian endian = Endian.Little)
        {
            output.WriteValueF32(Width, endian);
            output.WriteValueU16(CenterOffset, endian);
            output.WriteValueU8(_laneTypeAndFlags);
            output.WriteValueU16((ushort) RangeFlags.Count, endian);
            foreach (var rangeFlag in RangeFlags)
            {
                rangeFlag.Write(output, endian);
            }
        }
    }
    
    public class RoadDefinitionDe : IRoadDefinition
    {
        public ushort RoadGraphEdgeIndex { get; set; }
        public ushort OppositeRoadGraphEdgeIndex { get; set; }
        public List<ILaneDefinition> Lanes { get; } = new List<ILaneDefinition>();
        public List<IRangeFlag> RangeFlags { get; } = new List<IRangeFlag>();
        public byte OppositeLanesCount { get; set; }
        public byte ForwardLanesCount { get; set; }
        public byte MaxSpawnedCars { get; set; }
        public RoadDirection Direction { get; set; }
        public ushort RoadSplineIndex { get; set; }
        public RoadType RoadType { get; set; }
        
        public void Read(Stream input, Endian endian = Endian.Little)
        {
            RoadGraphEdgeIndex = input.ReadValueU16(endian);
            OppositeRoadGraphEdgeIndex = input.ReadValueU16(endian);
            OppositeLanesCount = input.ReadValueU8();
            ForwardLanesCount = input.ReadValueU8();
            RoadType = (RoadType) input.ReadValueU8();
            
            byte lanesCount = input.ReadValueU8();
            for (int i = 0; i < lanesCount; i++)
            {
                var lane = new LaneDefinitionDe();
                lane.Read(input, endian);
                Lanes.Add(lane);
            }
            
            ushort rangeFlagsCount = input.ReadValueU16(endian);
            for (int i = 0; i < rangeFlagsCount; i++)
            {
                var rangeFlag = new RangeFlagDe();
                rangeFlag.Read(input, endian);
                RangeFlags.Add(rangeFlag);
            }
            
            RoadSplineIndex = input.ReadValueU16(endian);
            MaxSpawnedCars = input.ReadValueU8();
            Direction = (RoadDirection) input.ReadValueU8();
        }

        public void Write(Stream output, Endian endian = Endian.Little)
        {
            output.WriteValueU16(RoadGraphEdgeIndex, endian);
            output.WriteValueU16(OppositeRoadGraphEdgeIndex, endian);
            output.WriteValueU8(OppositeLanesCount);
            output.WriteValueU8(ForwardLanesCount);
            output.WriteValueU8((byte) RoadType);

            output.WriteValueU8((byte) Lanes.Count);
            foreach (var lane in Lanes)
            {
                lane.Write(output, endian);
            }
            
            output.WriteValueU16((ushort) RangeFlags.Count, endian);
            foreach (var rangeFlag in RangeFlags)
            {
                rangeFlag.Write(output, endian);
            }
            
            output.WriteValueU16(RoadSplineIndex, endian);
            output.WriteValueU8(MaxSpawnedCars);
            output.WriteValueU8((byte) Direction);
        }
    }
    
    public class RoadSplineDe : IRoadSpline
    {
        public List<Vector3> Points { get; } = new List<Vector3>();
        public float Length { get; set; }
        
        public void Read(Stream input, Endian endian = Endian.Little)
        {
            ushort pointsCount = input.ReadValueU16(endian);
            Length = input.ReadValueF32(endian);
            for (int i = 0; i < pointsCount; i++)
            {
                Vector3 point = Vector3.Zero;
                point.X = input.ReadValueF32(endian);
                point.Y = input.ReadValueF32(endian);
                point.Z = input.ReadValueF32(endian);
                Points.Add(point);
            }
        }

        public void Write(Stream output, Endian endian = Endian.Little)
        {
            output.WriteValueU16((ushort) Points.Count, endian);
            output.WriteValueF32(Length, endian);
            foreach (var point in Points)
            {
                output.WriteValueF32(point.X, endian);
                output.WriteValueF32(point.Y, endian);
                output.WriteValueF32(point.Z, endian);
            }
        }

        public void CalculateLength()
        {
            float totalLength = 0.0f;
            for (var i = 0; i < Points.Count - 1; i++)
            {
                totalLength += Vector3.Distance(Points[i], Points[i + 1]);
            }

            Length = totalLength;
        }
    }
    
    public class RoadJunctionDe : IRoadJunction
    {
        public ushort FromRoadGraphEdgeIndex { get; set; }
        public byte FromLaneIndex { get; set; }
        public ushort ToRoadGraphEdgeIndex { get; set; }
        public byte ToLaneIndex { get; set; }
        public byte Unkn4 { get; set; }
        public byte Unkn6 { get; set; }
        public ushort Unkn8 { get; set; }
        public IRoadSpline Spline { get; set; } = new RoadSplineDe();
        
        public void Read(Stream input, Endian endian = Endian.Little)
        {
            FromRoadGraphEdgeIndex = input.ReadValueU16(endian);
            FromLaneIndex = input.ReadValueU8();
            ToRoadGraphEdgeIndex = input.ReadValueU16(endian);
            ToLaneIndex = input.ReadValueU8();
            Unkn4 = input.ReadValueU8();
            Unkn6 = input.ReadValueU8();
            Unkn8 = input.ReadValueU16(endian);
            Spline.Read(input, endian);
        }

        public void Write(Stream output, Endian endian = Endian.Little)
        {
            output.WriteValueU16(FromRoadGraphEdgeIndex, endian);
            output.WriteValueU8(FromLaneIndex);
            output.WriteValueU16(ToRoadGraphEdgeIndex, endian);
            output.WriteValueU8(ToLaneIndex);
            output.WriteValueU8(Unkn4);
            output.WriteValueU8(Unkn6);
            output.WriteValueU16(Unkn8, endian);
            Spline.Write(output, endian);
        }
    }
    
    public class TrafficLightSemaphoreDe : ITrafficLightSemaphore
    {
        private ushort _unkn0;
        public byte Unkn0_0
        {
            get { return (byte)(_unkn0 & 0xF); }
            set { _unkn0 = (ushort) ((ushort) (_unkn0 & 0xFFF0) | (ushort) (value & 0xF)); }
        }
        public byte Unkn0_4
        {
            get { return (byte)((_unkn0 & 0xF0) >> 4); }
            set { _unkn0 = (ushort) ((ushort) (_unkn0 & 0xFF0F) | (ushort) ((value & 0xF) << 4)); }
        }
        public byte Unkn0_8
        {
            get { return (byte)((_unkn0 & 0xF00) >> 8); }
            set { _unkn0 = (ushort) ((ushort) (_unkn0 & 0xF0FF) | (ushort) ((value & 0xF) << 8)); }
        }
        public byte Unkn0_12
        {
            get { return (byte)((_unkn0 & 0xF000) >> 12); }
            set { _unkn0 = (ushort) ((ushort) (_unkn0 & 0x0FFF) | (ushort) ((value & 0xF) << 12)); }
        }
        public ushort Unkn2 { get; set; }
        public List<ushort> ManagedRoads { get; } = new List<ushort>();
        
        public void Read(Stream input, Endian endian = Endian.Little)
        {
            _unkn0 = input.ReadValueU16(endian);
            Unkn2 = input.ReadValueU16(endian);
            ushort unkn3Count = input.ReadValueU16(endian);
            for (int i = 0; i < unkn3Count; i++)
            {
                ManagedRoads.Add(input.ReadValueU16(endian));
            }
        }

        public void Write(Stream output, Endian endian = Endian.Little)
        {
            output.WriteValueU16(_unkn0, endian);
            output.WriteValueU16(Unkn2, endian);
            output.WriteValueU16((ushort) ManagedRoads.Count, endian);
            foreach (var index in ManagedRoads)
            {
                output.WriteValueU16(index, endian);
            }
        }
    }
    
    public class CrossroadDe : ICrossroad
    {
        public ushort Index { get; set; }
        public Vector3 PivotPoint { get; set; } = Vector3.Zero;
        public List<IRoadJunction> Junctions { get; } = new List<IRoadJunction>();
        public List<Vector3> Bounds { get; } = new List<Vector3>();
        public List<ITrafficLightSemaphore> TrafficLightSemaphores { get; } = new List<ITrafficLightSemaphore>();
        
        public void Read(Stream input, Endian endian = Endian.Little)
        {
            Index = input.ReadValueU16(endian);

            byte junctionsCount = input.ReadValueU8();
            for (int i = 0; i < junctionsCount; i++)
            {
                var junction = new RoadJunctionDe();
                junction.Read(input, endian);
                Junctions.Add(junction);
            }
            
            ushort boundsCount = input.ReadValueU16(endian);
            for (int i = 0; i < boundsCount; i++)
            {
                Bounds.Add(new Vector3(
                    input.ReadValueF32(endian),
                    input.ReadValueF32(endian),
                    input.ReadValueF32(endian)
                ));
            }
            
            PivotPoint = new Vector3(
                input.ReadValueF32(endian),
                input.ReadValueF32(endian),
                input.ReadValueF32(endian)
            );
            
            ushort trafficLightSemaphoresCount = input.ReadValueU16(endian);
            
            ushort unkn4 = input.ReadValueU16(endian);
            if (unkn4 != 0)
            {
                throw new IOException($"Unexpected non-zero unkn4 value {unkn4}");
            }
            
            for (int i = 0; i < trafficLightSemaphoresCount; i++)
            {
                var semaphore = new TrafficLightSemaphoreDe();
                semaphore.Read(input, endian);
                TrafficLightSemaphores.Add(semaphore);
            }
        }

        public void Write(Stream output, Endian endian = Endian.Little)
        {
            output.WriteValueU16(Index, endian);
            
            output.WriteValueU8((byte) Junctions.Count);
            foreach (var junction in Junctions)
            {
                junction.Write(output, endian);
            }
            
            output.WriteValueU16((ushort) Bounds.Count, endian);
            foreach (var point in Bounds)
            {
                output.WriteValueF32(point.X, endian);
                output.WriteValueF32(point.Y, endian);
                output.WriteValueF32(point.Z, endian);
            }
            
            output.WriteValueF32(PivotPoint.X, endian);
            output.WriteValueF32(PivotPoint.Y, endian);
            output.WriteValueF32(PivotPoint.Z, endian);
            
            output.WriteValueU16((ushort) TrafficLightSemaphores.Count, endian);
            output.WriteValueU16(0);
            foreach (var semaphore in TrafficLightSemaphores)
            {
                semaphore.Write(output, endian);
            }
        }
    }
    
    public class CostMapEntryDe : ICostMapEntry
    {
        public RoadGraphEdgeType RoadGraphEdgeType { get; set; }
        public ushort RoadGraphEdgeLink { get; set; }
        public ushort Cost { get; set; }
        
        public void Read(Stream input, Endian endian = Endian.Little)
        {
            RoadGraphEdgeType = (RoadGraphEdgeType) input.ReadValueU32(endian);
            RoadGraphEdgeLink =  (ushort) input.ReadValueU32(endian);
            Cost = input.ReadValueU16(endian);
        }

        public void Write(Stream output, Endian endian = Endian.Little)
        {
            output.WriteValueU32((uint) RoadGraphEdgeType);
            output.WriteValueU32(RoadGraphEdgeLink);
            output.WriteValueU16(Cost);
        }
    }

    public class RoadmapDe : IRoadmap
    {
        public ushort RoadGraphRoadToJunctionEdgeMappingCount { get; set; }
        public ushort RoadGraphEdgeCount { get; set; }
        public List<Tuple<ushort, ushort>> RoadGraphEdges { get; } = new List<Tuple<ushort, ushort>>();
        public List<IRoadDefinition> Roads { get; } = new List<IRoadDefinition>();
        public List<ICostMapEntry> CostMap { get; } = new List<ICostMapEntry>();
        public List<IRoadSpline> Splines { get; } = new List<IRoadSpline>();
        public List<ushort> RoadToCrossroadMapping { get; } = new List<ushort>();
        public List<ICrossroad> Crossroads { get; } = new List<ICrossroad>();
        
        public void Read(Stream input, Endian endian = Endian.Little)
        {
            RoadGraphRoadToJunctionEdgeMappingCount = input.ReadValueU16(endian);
            RoadGraphEdgeCount = input.ReadValueU16(endian);
            
            for (int i = 0; i < RoadGraphEdgeCount; i++)
            {
                RoadGraphEdges.Add(new Tuple<ushort, ushort>(input.ReadValueU16(endian), input.ReadValueU16(endian)));
            }
            
            ushort roadCount = input.ReadValueU16(endian);
            for (int i = 0; i < roadCount; i++)
            {
                var road = new RoadDefinitionDe();
                road.Read(input, endian);
                Roads.Add(road);
            }

            for (int i = 0; i < RoadGraphEdgeCount; i++)
            {
                var costMapping = new CostMapEntryDe();
                costMapping.Read(input, endian);
                CostMap.Add(costMapping);
            }

            byte magic0 = input.ReadValueU8();
            byte magic1 = input.ReadValueU8();
            byte magic2 = input.ReadValueU8();
            byte magic3 = input.ReadValueU8();
            if (magic0 != 0x11 && magic1 != 0x11 && magic2 != 0x11 && magic3 != 0)
            {
                throw new IOException($"Unexpected magic values ({magic0}, {magic1}, {magic2}, {magic3})");
            }

            ushort splineCount = input.ReadValueU16(endian);
            for (int i = 0; i < splineCount; i++)
            {
                var spline = new RoadSplineDe();
                spline.Read(input, endian);
                Splines.Add(spline);
            }

            for (int i = 0; i < roadCount * 2; i++)
            {
                RoadToCrossroadMapping.Add(input.ReadValueU16(endian));
            }
            
            ushort crossroadCount = input.ReadValueU16(endian);
            for (int i = 0; i < crossroadCount; i++)
            {
                var crossroad = new CrossroadDe();
                crossroad.Read(input, endian);
                Crossroads.Add(crossroad);
            }
        }

        public void Write(Stream output, Endian endian = Endian.Little)
        {
            output.WriteValueU16(RoadGraphRoadToJunctionEdgeMappingCount, endian);
            output.WriteValueU16(RoadGraphEdgeCount, endian);
            foreach (var val in RoadGraphEdges)
            {
                output.WriteValueU16(val.Item1, endian);
                output.WriteValueU16(val.Item2, endian);
            }
            
            output.WriteValueU16((ushort) Roads.Count, endian);
            foreach (var road in Roads)
            {
                road.Write(output, endian);
            }
            
            foreach (var costMapping in CostMap)
            {
                costMapping.Write(output, endian);
            }
            
            output.WriteValueU8(0x11);
            output.WriteValueU8(0x11);
            output.WriteValueU8(0x11);
            output.WriteValueU8(0);
            
            output.WriteValueU16((ushort)Splines.Count, endian);
            foreach (var spline in Splines)
            {
                spline.Write(output, endian);
            }
            
            foreach (var val in RoadToCrossroadMapping)
            {
                output.WriteValueU16(val, endian);
            }
            
            output.WriteValueU16((ushort) Crossroads.Count, endian);
            foreach (var crossroad in Crossroads)
            {
                crossroad.Write(output, endian);
            }
        }

        public ushort GetRoadIndexForRoadGraphEdge(int edgeIndex)
        {
            var edgeType = CostMap[edgeIndex].RoadGraphEdgeType;
            switch (edgeType)
            {
                case RoadGraphEdgeType.CrossroadJunction:
                    return (ushort)(RoadGraphEdges[edgeIndex].Item2 / 2);
                case RoadGraphEdgeType.Road:
                    return (ushort)((RoadGraphEdges[edgeIndex].Item2 - 1) / 2);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void AddRoadEdge(ushort roadIndex)
        {
            RoadGraphEdges.Add(new Tuple<ushort, ushort>((ushort) (roadIndex * 2), (ushort) (roadIndex * 2 + 1)));
            RoadGraphRoadToJunctionEdgeMappingCount = (ushort) (GetLastRoadEdgeIndex() + 1);
            RoadGraphEdgeCount = (ushort) (RoadGraphEdges.Count + 1);
        }

        public void AddCrossroadJunctionEdge(ushort targetRoadIndex)
        {
            RoadGraphEdges.Add(new Tuple<ushort, ushort>(GetLastRoadEdgeIndex(), (ushort) (targetRoadIndex * 2)));
        }

        private ushort GetLastRoadEdgeIndex()
        {
            for (int i = CostMap.Count - 1; i >= 0; i--)
            {
                if (CostMap[i].RoadGraphEdgeType == RoadGraphEdgeType.Road)
                {
                    return RoadGraphEdges[i].Item2;
                }
            }

            return 0;
        }
    }
}
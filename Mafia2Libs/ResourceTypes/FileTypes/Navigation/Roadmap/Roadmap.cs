using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using Gibbed.IO;

namespace ResourceTypes.Navigation.Traffic
{
    public interface IRoadmapSerializable
    {
        void Read(Stream input, Endian endian = Endian.Little);
        void Write(Stream output, Endian endian = Endian.Little);
    }
    
    public enum RangeFlagType : byte
    {
        RailroadCrossing = 1, // or alternate name LevelCrossing? // Lanes only
        Parking = 2, // Lanes only
        BusStop = 3, // Lanes only
        Crosswalk = 4, // Roads only
        Tunnel = 5, // Roads only
        TrainStation = 6 // Lanes only // Used in paths with type RoadType.Train 
    }

    public interface IRangeFlag : IRoadmapSerializable
    {
       float From { get; set; }
       float Distance { get; set; }
       RangeFlagType RangeFlagType { get; set; }
       byte Unkn3 { get; set; }
       byte Index { get; set; }
       // some index
       ushort Unkn4 { get; set; }
       float Unkn5 { get; set; }
    }
    
    public enum LaneType: byte // 4 bits
    {
        MainRoad = 0,
        Byroad = 1,
        ExclImpassable = 2,
        EmptyRoad = 3, // disabled?
        Parking = 4
    }

    [Flags]
    public enum LaneFlags : byte // 4 bits
    {
        Bus = 1,
        Truck = 2,
        Highway = 4
    }
    
    public interface ILaneDefinition : IRoadmapSerializable
    {
        float Width { get; set; }
        LaneType LaneType { get; set; }
        LaneFlags LaneFlags { get; set; }
        /// <summary>
        /// Offset from the spline to the center of this lane, value is multiplied by 100.0f
        /// </summary>
        ushort CenterOffset { get; set; } // is divided by 100.0 in "follw_spline(...)" method
        List<IRangeFlag> RangeFlags { get; }
    }
    
    public enum RoadDirection : byte // 4 bits
    { 
        Towards = 0,
        Backwards = 1
    }
    
    public enum RoadType : byte // 4 bits
    {
        Road = 0,
        Train = 1,
        Boat = 4,
        EmptyRoad = 6,
        UnknRoadType8 = 8,
        UnknRoadType9 = 9
    }
    
    public interface IRoadDefinition : IRoadmapSerializable
    {
        ushort RoadGraphEdgeIndex { get; set; } 
        ushort OppositeRoadGraphEdgeIndex { get; set; } 
        List<ILaneDefinition> Lanes { get; } 
        List<IRangeFlag> RangeFlags { get; }
        byte OppositeLanesCount { get; set; }
        byte ForwardLanesCount { get; set; }
        byte MaxSpawnedCars { get; set; }
        RoadDirection Direction { get; set; }
        ushort RoadSplineIndex { get; set; }
        RoadType RoadType { get; set; }
    }
    
    public interface IRoadSpline : IRoadmapSerializable
    {
        List<Vector3> Points { get; }
        float Length { get; set; }

        void CalculateLength();
    }
    
    public interface IRoadJunction : IRoadmapSerializable
    {
        ushort FromRoadGraphEdgeIndex { get; set; } // 0..2742
        byte FromLaneIndex { get; set; } // 0, 1 or 2
        ushort ToRoadGraphEdgeIndex { get; set; } // 0..2742
        byte ToLaneIndex { get; set; } // 0, 1 or 2
        byte Unkn4 { get; set; } // always 255 (or -1)
        byte Unkn6 { get; set; } // 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 or 10
        ushort Unkn8 { get; set; }
        IRoadSpline Spline { get; set; }
    }

    public interface ITrafficLightSemaphore : IRoadmapSerializable
    {
        byte Unkn0_0 { get; set; }
        byte Unkn0_4 { get; set; }
        byte Unkn0_8 { get; set; }
        byte Unkn0_12 { get; set; }
        ushort Unkn2 { get; set; }
        List<ushort> ManagedRoads { get; }
    }
    
    public interface ICrossroad : IRoadmapSerializable
    {
        ushort Index { get; set; }
        Vector3 PivotPoint { get; set; }
        List<IRoadJunction> Junctions { get; } // max possible count is 16
        List<Vector3> Bounds { get; }
        List<ITrafficLightSemaphore> TrafficLightSemaphores { get; }
    }

    public enum RoadGraphEdgeType : byte // 4 bits
    { 
        // road or junction/crossroad
        CrossroadJunction = 0,
        Road = 1
    }
    
    public interface ICostMapEntry : IRoadmapSerializable
    {
        RoadGraphEdgeType RoadGraphEdgeType { get; set; }
        
        /// <summary>
        /// Road or Crossroad index
        /// </summary>
        ushort RoadGraphEdgeLink { get; set; }
        ushort Cost { get; set; }
    }

    public interface IRoadmap : IRoadmapSerializable
    {
        List<IRoadSpline> Splines { get; }
        List<IRoadDefinition> Roads { get; }
        List<ICrossroad> Crossroads { get; }
        List<ushort> RoadToCrossroadMapping { get; }
        List<ICostMapEntry> CostMap { get; }

        ushort GetRoadIndexForRoadGraphEdge(int edgeIndex);
        void AddRoadEdge(ushort roadIndex);
        void AddCrossroadJunctionEdge(ushort targetRoadIndex);
        ushort CalculateRoadCost(IRoadDefinition road);
    }

}
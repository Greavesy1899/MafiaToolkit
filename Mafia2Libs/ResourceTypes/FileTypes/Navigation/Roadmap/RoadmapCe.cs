using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using Gibbed.IO;

namespace ResourceTypes.Navigation.Traffic
{
    public interface IRoadmapSerializableCe : IRoadmapSerializable
    {
        void ReadData(Stream input, Endian endian = Endian.Little);
        
        void WriteData(Stream output, Endian endian = Endian.Little);

        uint TypeSize();
    }
    
    abstract class SerializedListBase<T> : IRoadmapSerializableCe
    {
        private uint _offset;

        private uint Offset
        {
            get { return _offset & 0x7FFFFFFF; }
            set { _offset = value > 0 ? value | 0x80000000 : value; }
        } 
        public List<T> Items { get; } = new List<T>();
        public ushort SerializedCount { get; private set; }
        
        public void Read(Stream input, Endian endian = Endian.Little)
        {
            _offset = input.ReadValueU32(endian);
            SerializedCount = input.ReadValueU16(endian);
            // note: most likely one of the values is the actual count and another one is the capacity, 
            // and before the serialization the original list is just TrimExcess-ed, that is why the values are the same
            ushort anotherCount = input.ReadValueU16(endian);
            if (SerializedCount != anotherCount)
            {
                throw new IOException($"Unexpected sizes mismatch: {SerializedCount} != {anotherCount}");
            }
        }

        public void ReadData(Stream input, Endian endian = Endian.Little)
        {
            if (Offset != 0 && SerializedCount > 0)
            {
                long position = input.Position;
                input.Position = Offset - 4;

                for (int i = 0; i < SerializedCount; i++)
                {
                    var item = NewInstance(); 
                    ReadItemInternal(ref item, input, endian);
                    Items.Add(item);
                }
                foreach (var item in Items)
                {
                    ReadItemDataInternal(item, input, endian);
                }

                input.Position = position;
            }
        }

        public void Write(Stream output, Endian endian = Endian.Little)
        {
            output.WriteValueU32(_offset, endian);
            output.WriteValueU16((ushort) Items.Count, endian);
            output.WriteValueU16((ushort) Items.Count, endian);
        }

        public void WriteData(Stream output, Endian endian = Endian.Little)
        {
            if (Items.Count == 0)
            {
                Offset = 0;
            }
            else 
            {
                Offset = (uint) (output.Position + 4);
                
                long startOffset = output.Position;

                output.Position += TypeSize();
                
                foreach (var item in Items)
                {
                    WriteItemDataInternal(item, output, endian);
                }

                long endOffset = output.Position;

                output.Position = startOffset;
                foreach (var item in Items)
                {
                    WriteItemInternal(item, output, endian);
                }
                
                output.Position = endOffset;
            }
        }

        public uint TypeSize()
        {
            if (Items.Count > 0)
            {
                return (uint) (Items.Count * GetItemSize(Items[0]));
            }
            return 0;
        }

        protected abstract T NewInstance();
        protected abstract void ReadItemInternal(ref T item, Stream input, Endian endian);
        protected abstract void ReadItemDataInternal(T item, Stream input, Endian endian);

        protected abstract void WriteItemInternal(T item, Stream output, Endian endian);
        protected abstract void WriteItemDataInternal(T item, Stream output, Endian endian);
        protected abstract uint GetItemSize(T item);
    }

    class SerializedVector3List : SerializedListBase<Vector3>
    {
        protected override Vector3 NewInstance()
        {
            return Vector3.Zero;
        }

        protected override void ReadItemInternal(ref Vector3 item, Stream input, Endian endian)
        {
            item.X = input.ReadValueF32(endian);
            item.Y = input.ReadValueF32(endian);
            item.Z = input.ReadValueF32(endian);
        }

        protected override void ReadItemDataInternal(Vector3 item, Stream input, Endian endian)
        {
            // do nothing
        }

        protected override void WriteItemInternal(Vector3 item, Stream output, Endian endian)
        {
            output.WriteValueF32(item.X, endian);
            output.WriteValueF32(item.Y, endian);
            output.WriteValueF32(item.Z, endian);
        }

        protected override void WriteItemDataInternal(Vector3 item, Stream output, Endian endian)
        {
            // do nothing
        }

        protected override uint GetItemSize(Vector3 item)
        {
            return 12; //sizeof(Vector3);
        }
    }
    
    class SerializedUint16List : SerializedListBase<ushort>
    {
        protected override ushort NewInstance()
        {
            return 0;
        }

        protected override void ReadItemInternal(ref ushort item, Stream input, Endian endian)
        {
            item = input.ReadValueU16(endian);
        }

        protected override void ReadItemDataInternal(ushort item, Stream input, Endian endian)
        {
            // do nothing;
        }

        protected override void WriteItemInternal(ushort item, Stream output, Endian endian)
        {
            output.WriteValueU16(item, endian);
        }

        protected override void WriteItemDataInternal(ushort item, Stream output, Endian endian)
        {
            // do nothing
        }

        protected override uint GetItemSize(ushort item)
        {
            return sizeof(ushort);
        }
    }

    class SerializedList<T, R> : SerializedListBase<T> where R: T, IRoadmapSerializableCe, new()
    {
        protected override T NewInstance()
        {
            return new R();
        }

        protected override void ReadItemInternal(ref T item, Stream input, Endian endian)
        {
            ((IRoadmapSerializableCe)item).Read(input, endian);
        }

        protected override void ReadItemDataInternal(T item, Stream input, Endian endian)
        {
            ((IRoadmapSerializableCe)item).ReadData(input, endian);
        }

        protected override void WriteItemInternal(T item, Stream output, Endian endian)
        {
            ((IRoadmapSerializableCe)item).Write(output, endian);
        }

        protected override void WriteItemDataInternal(T item, Stream output, Endian endian)
        {
            ((IRoadmapSerializableCe)item).WriteData(output, endian);
        }

        protected override uint GetItemSize(T item)
        {
            return ((IRoadmapSerializableCe)item).TypeSize();
        }
    }
    
    public class RangeFlagCe : IRangeFlag, IRoadmapSerializableCe
    {
        public float From { get; set; }
        public float Distance { get; set; }
        public RangeFlagType RangeFlagType { get; set; }
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
            RangeFlagType = (RangeFlagType)input.ReadValueU8();
            Unkn3 = input.ReadValueU8();
            _indexAndUnkn4 = input.ReadValueU16(endian);
            Unkn5 = input.ReadValueF32(endian);
        }

        public void Write(Stream output, Endian endian = Endian.Little)
        {
            output.WriteValueF32(From, endian);
            output.WriteValueF32(Distance, endian);
            output.WriteValueU8((byte) RangeFlagType);
            output.WriteValueU8(Unkn3);
            output.WriteValueU16(_indexAndUnkn4, endian);
            output.WriteValueF32(Unkn5, endian);
        }

        public void ReadData(Stream input, Endian endian = Endian.Little)
        {
            // do nothing
        }

        public void WriteData(Stream output, Endian endian = Endian.Little)
        {
            // do nothing
        }

        public uint TypeSize()
        {
            return 16;
        }
    }
    
    public class LaneDefinitionCe : ILaneDefinition, IRoadmapSerializableCe
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
        private readonly SerializedList<IRangeFlag, RangeFlagCe> _rangeFlags = new SerializedList<IRangeFlag, RangeFlagCe>();
        public List<IRangeFlag> RangeFlags => _rangeFlags.Items;

        public void Read(Stream input, Endian endian = Endian.Little)
        {
            Width = input.ReadValueF32(endian);
            _laneTypeAndFlags = input.ReadValueU8();
            byte someZero = input.ReadValueU8();
            if (someZero != 0)
            {
                throw new IOException($"Unexpected non-zero someZero value {someZero}");
            }
            CenterOffset = input.ReadValueU16(endian);
            _rangeFlags.Read(input, endian);
        }

        public void Write(Stream output, Endian endian = Endian.Little)
        {
            output.WriteValueF32(Width, endian);
            output.WriteValueU8(_laneTypeAndFlags);
            output.WriteValueU8(0);
            output.WriteValueU16(CenterOffset, endian);
            _rangeFlags.Write(output, endian);
        }

        public void ReadData(Stream input, Endian endian = Endian.Little)
        {
            _rangeFlags.ReadData(input, endian);
        }

        public void WriteData(Stream output, Endian endian = Endian.Little)
        {
            _rangeFlags.WriteData(output, endian);
        }

        public uint TypeSize()
        {
            return 16;
        }
    }
    
    public class RoadDefinitionCe : IRoadDefinition, IRoadmapSerializableCe
    {
        public ushort RoadGraphEdgeIndex { get; set; }
        public ushort OppositeRoadGraphEdgeIndex { get; set; }
        
        private readonly SerializedList<ILaneDefinition, LaneDefinitionCe> _lanes = new SerializedList<ILaneDefinition, LaneDefinitionCe>();
        public List<ILaneDefinition> Lanes => _lanes.Items;
        
        private readonly SerializedList<IRangeFlag, RangeFlagCe> _rangeFlags = new SerializedList<IRangeFlag, RangeFlagCe>();
        public List<IRangeFlag> RangeFlags => _rangeFlags.Items;
        
        private byte _oppositeAndForwardLanesCount; // 4+4
        public byte OppositeLanesCount
        {
            get { return (byte)(_oppositeAndForwardLanesCount & 0xF); }
            set { _oppositeAndForwardLanesCount = (byte) ((byte) (_oppositeAndForwardLanesCount & 0xF0) | (byte) (value & 0xF)); }
        }
        public byte ForwardLanesCount
        {
            get { return (byte)((_oppositeAndForwardLanesCount & 0xF0) >> 4); }
            set { _oppositeAndForwardLanesCount = (byte) ((byte) (_oppositeAndForwardLanesCount & 0x0F) | (byte) ((value & 0xF) << 4)); }
        }
        
        private byte _maxSpawnedCarsAndDirection; // 4+4
        public byte MaxSpawnedCars
        {
            get { return (byte)(_maxSpawnedCarsAndDirection & 0xF); }
            set { _maxSpawnedCarsAndDirection = (byte) ((byte) (_maxSpawnedCarsAndDirection & 0xF0) | (byte) (value & 0xF)); }
        }
        public RoadDirection Direction
        {
            get { return (RoadDirection)((_maxSpawnedCarsAndDirection & 0xF0) >> 4); }
            set { _maxSpawnedCarsAndDirection = (byte) ((byte) (_maxSpawnedCarsAndDirection & 0x0F) | (byte) (((byte)value & 0xF) << 4)); }
        }
        
        private ushort _splineIndexAndRoadType; // 12+4
        public ushort RoadSplineIndex
        {
            get { return (ushort)(_splineIndexAndRoadType & 0xFFF); }
            set { _splineIndexAndRoadType = (ushort) ((ushort) (_splineIndexAndRoadType & 0xF000) | (ushort) (value & 0xFFF)); }
        }
        public RoadType RoadType
        {
            get { return (RoadType)((_splineIndexAndRoadType & 0xF000) >> 12); }
            set { _splineIndexAndRoadType = (ushort) ((ushort) (_splineIndexAndRoadType & 0x0FFF) | (ushort) (((ushort)value & 0xF) << 12)); }
        }

        public void Read(Stream input, Endian endian = Endian.Little)
        {
            RoadGraphEdgeIndex = input.ReadValueU16(endian);
            OppositeRoadGraphEdgeIndex = input.ReadValueU16(endian);
            _lanes.Read(input, endian);
            _rangeFlags.Read(input, endian);
            _oppositeAndForwardLanesCount = input.ReadValueU8();
            _maxSpawnedCarsAndDirection = input.ReadValueU8();
            if (endian == Endian.Big)
            {
                _oppositeAndForwardLanesCount = (byte) (((_oppositeAndForwardLanesCount & 0xF) << 4) |
                                                       ((_oppositeAndForwardLanesCount & 0xF0) >> 4));
                _maxSpawnedCarsAndDirection = (byte) (((_maxSpawnedCarsAndDirection & 0xF) << 4) |
                                                     ((_maxSpawnedCarsAndDirection & 0xF0) >> 4));
            }
            _splineIndexAndRoadType = input.ReadValueU16(endian);
        }

        public void Write(Stream output, Endian endian = Endian.Little)
        {
            output.WriteValueU16(RoadGraphEdgeIndex, endian);
            output.WriteValueU16(OppositeRoadGraphEdgeIndex, endian);
            _lanes.Write(output, endian);
            _rangeFlags.Write(output, endian);

            byte oppositeAndForwardLanesCountVal = _oppositeAndForwardLanesCount;
            byte maxSpawnedCarsAndDirectionVal = _maxSpawnedCarsAndDirection;
            if (endian == Endian.Big)
            {
                oppositeAndForwardLanesCountVal = (byte) (((oppositeAndForwardLanesCountVal & 0xF) << 4) |
                                                          ((oppositeAndForwardLanesCountVal & 0xF0) >> 4));
                maxSpawnedCarsAndDirectionVal = (byte) (((maxSpawnedCarsAndDirectionVal & 0xF) << 4) |
                                                        ((maxSpawnedCarsAndDirectionVal & 0xF0) >> 4));
            }
            
            output.WriteValueU8(oppositeAndForwardLanesCountVal);
            output.WriteValueU8(maxSpawnedCarsAndDirectionVal);
            
            output.WriteValueU16(_splineIndexAndRoadType, endian);
        }

        public void ReadData(Stream input, Endian endian = Endian.Little)
        {
            _lanes.ReadData(input, endian);
            _rangeFlags.ReadData(input, endian);
        }

        public void WriteData(Stream output, Endian endian = Endian.Little)
        {
            _lanes.WriteData(output, endian);
            _rangeFlags.WriteData(output, endian);
        }

        public uint TypeSize()
        {
            return 24;
        }
    }
    
    public class RoadSplineCe : IRoadSpline, IRoadmapSerializableCe
    {
        private readonly SerializedVector3List _points = new SerializedVector3List();
        public List<Vector3> Points => _points.Items;
        
        public float Length { get; set; }
        public void Read(Stream input, Endian endian = Endian.Little)
        {
            _points.Read(input, endian);
            Length = input.ReadValueF32(endian);
        }

        public void Write(Stream output, Endian endian = Endian.Little)
        {
            _points.Write(output, endian);
            output.WriteValueF32(Length, endian);
        }

        public void ReadData(Stream input, Endian endian = Endian.Little)
        {
            _points.ReadData(input, endian);
        }

        public void WriteData(Stream output, Endian endian = Endian.Little)
        {
            _points.WriteData(output, endian);
        }

        public uint TypeSize()
        {
            return 12;
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

    public class RoadJunctionCe : IRoadJunction, IRoadmapSerializableCe
    {
        public ushort FromRoadGraphEdgeIndex { get; set; }
        public byte FromLaneIndex { get; set; }
        public ushort ToRoadGraphEdgeIndex { get; set; }
        public byte ToLaneIndex { get; set; }
        public byte Unkn4 { get; set; }
        public byte Unkn6 { get; set; }
        public ushort Unkn8 { get; set; }
        public IRoadSpline Spline { get; set; } = new RoadSplineCe();
        
        public void Read(Stream input, Endian endian = Endian.Little)
        {
            FromRoadGraphEdgeIndex = input.ReadValueU16(endian);
            FromLaneIndex = input.ReadValueU8();
            ToLaneIndex = input.ReadValueU8();
            ToRoadGraphEdgeIndex = input.ReadValueU16(endian);
            Unkn4 = input.ReadValueU8();
            byte someZero = input.ReadValueU8();
            if (someZero != 0)
            {
                throw new IOException($"Unexpected non-zero someZero value {someZero}");
            }
            Unkn6 = input.ReadValueU8();
            byte someZero2 = input.ReadValueU8();
            if (someZero2 != 0)
            {
                throw new IOException($"Unexpected non-zero someZero2 value {someZero2}");
            }
            Unkn8 = input.ReadValueU16(endian);
            Spline.Read(input, endian);
        }

        public void Write(Stream output, Endian endian = Endian.Little)
        {
            output.WriteValueU16(FromRoadGraphEdgeIndex, endian);
            output.WriteValueU8(FromLaneIndex);
            output.WriteValueU8(ToLaneIndex);
            output.WriteValueU16(ToRoadGraphEdgeIndex, endian);
            output.WriteValueU8(Unkn4);
            output.WriteValueU8(0);
            output.WriteValueU8(Unkn6);
            output.WriteValueU8(0);
            output.WriteValueU16(Unkn8, endian);
            Spline.Write(output, endian);
        }

        public void ReadData(Stream input, Endian endian = Endian.Little)
        {
            ((RoadSplineCe)Spline).ReadData(input, endian);
        }

        public void WriteData(Stream output, Endian endian = Endian.Little)
        {
            ((RoadSplineCe)Spline).WriteData(output, endian);
        }

        public uint TypeSize()
        {
            return 24;
        }
    }
    
    public class TrafficLightSemaphoreCe : ITrafficLightSemaphore, IRoadmapSerializableCe
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
        
        private readonly SerializedUint16List _managedRoads = new SerializedUint16List();
        public List<ushort> ManagedRoads => _managedRoads.Items;
        
        public void Read(Stream input, Endian endian = Endian.Little)
        {
            _unkn0 = input.ReadValueU16(endian);
            Unkn2 = input.ReadValueU16(endian);
            _managedRoads.Read(input, endian);
        }

        public void Write(Stream output, Endian endian = Endian.Little)
        {
            output.WriteValueU16(_unkn0, endian);
            output.WriteValueU16(Unkn2, endian);
            _managedRoads.Write(output, endian);
        }

        public void ReadData(Stream input, Endian endian = Endian.Little)
        {
            _managedRoads.ReadData(input, endian);
        }

        public void WriteData(Stream output, Endian endian = Endian.Little)
        {
            _managedRoads.WriteData(output, endian);
        }

        public uint TypeSize()
        {
            return 12;
        }
    }

    public class CrossroadCe : ICrossroad, IRoadmapSerializableCe
    {
        public ushort Index { get; set; }
        public Vector3 PivotPoint { get; set; }
        
        private readonly SerializedList<IRoadJunction, RoadJunctionCe> _junctions = new SerializedList<IRoadJunction, RoadJunctionCe>();
        public List<IRoadJunction> Junctions => _junctions.Items;
        
        private readonly SerializedVector3List _bounds = new SerializedVector3List();
        public List<Vector3> Bounds => _bounds.Items;
        
        private readonly SerializedList<ITrafficLightSemaphore, TrafficLightSemaphoreCe> _trafficLightSemaphores = new SerializedList<ITrafficLightSemaphore, TrafficLightSemaphoreCe>();
        public List<ITrafficLightSemaphore> TrafficLightSemaphores => _trafficLightSemaphores.Items;
        public void Read(Stream input, Endian endian = Endian.Little)
        {
            PivotPoint = new Vector3
            {
                X = input.ReadValueF32(endian),
                Y = input.ReadValueF32(endian),
                Z = input.ReadValueF32(endian)
            };
            _junctions.Read(input, endian);
            _bounds.Read(input, endian);
            Index = input.ReadValueU16(endian);
            ushort someZero = input.ReadValueU16(endian);
            if (someZero != 0)
            {
                throw new IOException($"Unexpected non-zero value {someZero}");
            }
            _trafficLightSemaphores.Read(input, endian);
        }

        public void Write(Stream output, Endian endian = Endian.Little)
        {
            output.WriteValueF32(PivotPoint.X, endian);
            output.WriteValueF32(PivotPoint.Y, endian);
            output.WriteValueF32(PivotPoint.Z, endian);
            _junctions.Write(output, endian);
            _bounds.Write(output, endian);
            output.WriteValueU16(Index, endian);
            output.WriteValueU16(0);
            _trafficLightSemaphores.Write(output, endian);
        }

        public void ReadData(Stream input, Endian endian = Endian.Little)
        {
            _junctions.ReadData(input, endian);
            _bounds.ReadData(input, endian);
            _trafficLightSemaphores.ReadData(input, endian);
        }

        public void WriteData(Stream output, Endian endian = Endian.Little)
        {
            _junctions.WriteData(output, endian);
            _bounds.WriteData(output, endian);
            _trafficLightSemaphores.WriteData(output, endian);
        }

        public uint TypeSize()
        {
            return 40;
        }
    }
    
    public class CostMapEntryCe : ICostMapEntry, IRoadmapSerializableCe
    {
        private ushort _roadGraphEdgeTypeAndLink;
        public RoadGraphEdgeType RoadGraphEdgeType
        {
            get { return (RoadGraphEdgeType)(byte)(_roadGraphEdgeTypeAndLink & 1); }
            set { _roadGraphEdgeTypeAndLink = (ushort) ((ushort) (_roadGraphEdgeTypeAndLink & 0xFFFE) | (ushort) ((byte)value & 0x1)); }
        }
        public ushort RoadGraphEdgeLink 
        {
            get { return (ushort)(_roadGraphEdgeTypeAndLink >> 1); }
            set { _roadGraphEdgeTypeAndLink = (ushort) ((ushort) (_roadGraphEdgeTypeAndLink & 0x1) | (ushort) ((value & 0x7FFF) << 1)); }
        }
        public ushort Cost { get; set; }
        public void Read(Stream input, Endian endian = Endian.Little)
        {
            _roadGraphEdgeTypeAndLink = input.ReadValueU16(endian);
            if (endian == Endian.Big)
            {
                _roadGraphEdgeTypeAndLink =
                    (ushort) (((_roadGraphEdgeTypeAndLink & 0x8000) >> 15) | ((_roadGraphEdgeTypeAndLink & 0x7FFF) << 1));
            }
            Cost = input.ReadValueU16(endian);
        }

        public void Write(Stream output, Endian endian = Endian.Little)
        {
            ushort connectionTypeAndIndexVal = _roadGraphEdgeTypeAndLink;
            if (endian == Endian.Big)
            {
                connectionTypeAndIndexVal =
                    (ushort) (((connectionTypeAndIndexVal & 1) << 15) | ((connectionTypeAndIndexVal & 0xFFFE) >> 1));
            }
            output.WriteValueU16(connectionTypeAndIndexVal, endian);
            output.WriteValueU16(Cost, endian);
        }

        public void ReadData(Stream input, Endian endian = Endian.Little)
        {
            // do nothing
        }

        public void WriteData(Stream output, Endian endian = Endian.Little)
        {
            // do nothing
        }

        public uint TypeSize()
        {
            return 4;
        }
    }

    public class RoadmapCe: IRoadmap, IRoadmapSerializableCe
    {
        private readonly SerializedList<IRoadSpline, RoadSplineCe> _roadSplines = new SerializedList<IRoadSpline, RoadSplineCe>();
        public List<IRoadSpline> Splines => _roadSplines.Items;
        
        private readonly SerializedList<IRoadDefinition, RoadDefinitionCe> _roads = new SerializedList<IRoadDefinition, RoadDefinitionCe>();
        public List<IRoadDefinition> Roads => _roads.Items;
        
        private readonly SerializedList<ICrossroad, CrossroadCe> _crossroads = new SerializedList<ICrossroad, CrossroadCe>();
        public List<ICrossroad> Crossroads => _crossroads.Items;
        
        /// <summary>
        /// Contains pairs of "from" crossroad index and "to" crossroad index for each road definition.
        /// Count must be Roads.Count * 2 
        /// </summary>
        private readonly SerializedUint16List _roadToCrossroadMapping = new SerializedUint16List(); // values 0..356

        public List<ushort> RoadToCrossroadMapping => _roadToCrossroadMapping.Items;
        
        private readonly SerializedList<ICostMapEntry, CostMapEntryCe> _costMap = new SerializedList<ICostMapEntry, CostMapEntryCe>();
        public List<ICostMapEntry> CostMap => _costMap.Items;

        private readonly SerializedUint16List _roadGraphRoadToJunctionEdgeMapping = new SerializedUint16List(); // 0..2744
        public List<ushort> RoadGraphRoadToJunctionEdgeMapping => _roadGraphRoadToJunctionEdgeMapping.Items; // 0..2744
        
        private readonly SerializedUint16List _roadGraphEdges = new SerializedUint16List(); // 0, 2..1853
        public List<ushort> RoadGraphEdges => _roadGraphEdges.Items; // 0, 2..1853

        public ushort RoadGraphRoadToJunctionEdgeMappingCount { get; set; }
        
        public void Read(Stream input, Endian endian = Endian.Little)
        {
            _roadSplines.Read(input, endian);
            _roads.Read(input, endian);
            _crossroads.Read(input, endian);
            _roadToCrossroadMapping.Read(input, endian);
            _costMap.Read(input, endian);
            _roadGraphRoadToJunctionEdgeMapping.Read(input, endian);
            _roadGraphEdges.Read(input, endian);
            RoadGraphRoadToJunctionEdgeMappingCount = input.ReadValueU16(endian);
            ushort someZero = input.ReadValueU16(endian);
            if (someZero != 0)
            {
                throw new IOException($"Unexpected non-zero someZero value {someZero}");
            }
                
            ReadData(input, endian);
        }

        public void Write(Stream output, Endian endian = Endian.Little)
        {
            long startOffset = output.Position;
            output.Position += TypeSize();
            WriteData(output, endian);
            output.Position = startOffset;

            _roadSplines.Write(output, endian);
            _roads.Write(output, endian);
            _crossroads.Write(output, endian);
            _roadToCrossroadMapping.Write(output, endian);
            _costMap.Write(output, endian);
            _roadGraphRoadToJunctionEdgeMapping.Write(output, endian);
            _roadGraphEdges.Write(output, endian);
            output.WriteValueU16(RoadGraphRoadToJunctionEdgeMappingCount, endian);
            output.WriteValueU16(0);
        }

        public void ReadData(Stream input, Endian endian = Endian.Little)
        {
            _roadSplines.ReadData(input, endian);
            _roads.ReadData(input, endian);
            _crossroads.ReadData(input, endian);
            _roadToCrossroadMapping.ReadData(input, endian);
            _costMap.ReadData(input, endian);
            _roadGraphRoadToJunctionEdgeMapping.ReadData(input, endian);
            _roadGraphEdges.ReadData(input, endian);
        }

        public void WriteData(Stream output, Endian endian = Endian.Little)
        {
            _roadSplines.WriteData(output, endian);
            _roads.WriteData(output, endian);
            _crossroads.WriteData(output, endian);
            _roadToCrossroadMapping.WriteData(output, endian);
            _costMap.WriteData(output, endian);
            _roadGraphRoadToJunctionEdgeMapping.WriteData(output, endian);
            _roadGraphEdges.WriteData(output, endian);
        }

        public uint TypeSize()
        {
            return 60;
        }
        
        public ushort GetRoadIndexForRoadGraphEdge(int edgeIndex)
        {
            var edgeType = CostMap[edgeIndex].RoadGraphEdgeType;
            switch (edgeType)
            {
                case RoadGraphEdgeType.CrossroadJunction:
                    return (ushort)(RoadGraphEdges[edgeIndex] / 2);
                case RoadGraphEdgeType.Road:
                    return (ushort)((RoadGraphEdges[edgeIndex] - 1) / 2);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void AddRoadEdge(ushort roadIndex)
        {
            RoadGraphEdges.Add((ushort) (roadIndex * 2 + 1));
            
            EnsureMappingInitialized();
            RoadGraphRoadToJunctionEdgeMapping[roadIndex * 2] = (ushort) (CostMap.Count - 1);
            RoadGraphRoadToJunctionEdgeMapping[roadIndex * 2 + 1] = (ushort) (CostMap.Count);
            
            RoadGraphRoadToJunctionEdgeMapping[RoadGraphRoadToJunctionEdgeMapping.Count - 1] = (ushort) (CostMap.Count + 1);
        }

        private void EnsureMappingInitialized()
        {
            if (RoadGraphRoadToJunctionEdgeMapping.Count == 0)
            {
                for (var i = 0; i < Roads.Count; i++)
                {
                    RoadGraphRoadToJunctionEdgeMapping.Add(0);
                    RoadGraphRoadToJunctionEdgeMapping.Add(0);
                }

                RoadGraphRoadToJunctionEdgeMappingCount = (ushort) RoadGraphRoadToJunctionEdgeMapping.Count;
                RoadGraphRoadToJunctionEdgeMapping.Add(0);
            }
        }

        public void AddCrossroadJunctionEdge(ushort targetRoadIndex)
        {
            RoadGraphEdges.Add((ushort) (targetRoadIndex * 2));
        }
    }
}
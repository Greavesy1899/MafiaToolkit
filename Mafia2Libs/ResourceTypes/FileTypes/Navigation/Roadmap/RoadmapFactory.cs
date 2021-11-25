namespace ResourceTypes.Navigation.Traffic
{
    public interface IRoadmapFactory
    {
        IRangeFlag RangeFlag();
        ILaneDefinition Lane();
        IRoadDefinition Road();
        IRoadSpline Spline();
        IRoadJunction Junction();
        ITrafficLightSemaphore TrafficLightSemaphore();
        ICrossroad Crossroad();
        ICostMapEntry CostMapEntry();
        IRoadmap Roadmap();
    }

    public class RoadmapFactoryCe : IRoadmapFactory
    {
        public IRangeFlag RangeFlag()
        {
            return new RangeFlagCe();
        }

        public ILaneDefinition Lane()
        {
            return new LaneDefinitionCe();
        }

        public IRoadDefinition Road()
        {
            return new RoadDefinitionCe();
        }

        public IRoadSpline Spline()
        {
            return new RoadSplineCe();
        }

        public IRoadJunction Junction()
        {
            return new RoadJunctionCe();
        }

        public ITrafficLightSemaphore TrafficLightSemaphore()
        {
            return new TrafficLightSemaphoreCe();
        }

        public ICrossroad Crossroad()
        {
            return new CrossroadCe();
        }

        public ICostMapEntry CostMapEntry()
        {
            return new CostMapEntryCe();
        }

        public IRoadmap Roadmap()
        {
            return new RoadmapCe();
        }
    }
    
    public class RoadmapFactoryDe : IRoadmapFactory
    {
        public IRangeFlag RangeFlag()
        {
            return new RangeFlagDe();
        }

        public ILaneDefinition Lane()
        {
            return new LaneDefinitionDe();
        }

        public IRoadDefinition Road()
        {
            return new RoadDefinitionDe();
        }

        public IRoadSpline Spline()
        {
            return new RoadSplineDe();
        }

        public IRoadJunction Junction()
        {
            return new RoadJunctionDe();
        }

        public ITrafficLightSemaphore TrafficLightSemaphore()
        {
            return new TrafficLightSemaphoreDe();
        }

        public ICrossroad Crossroad()
        {
            return new CrossroadDe();
        }

        public ICostMapEntry CostMapEntry()
        {
            return new CostMapEntryDe();
        }

        public IRoadmap Roadmap()
        {
            return new RoadmapDe();
        }
    }
}
namespace ResourceTypes.FrameResource
{
    public class FrameObjectPoint : FrameObjectJoint
    {
        public FrameObjectPoint(FrameResource OwningResource) : base(OwningResource) { }

        public FrameObjectPoint(FrameObjectPoint other) : base(other) { }
    }
}

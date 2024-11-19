using Utils.Logging;

namespace ResourceTypes.ModelHelpers.ModelExporter
{
    public abstract class IValidator
    {
        private MT_ValidationTracker OurTrackerObject;

        public IValidator() { }

        public bool ValidateObject(MT_ValidationTracker TrackerObject)
        {
            // Temporarily set our reference
            OurTrackerObject = TrackerObject;

            TrackerObject.Setup(this);
            bool bResult = InternalValidate(TrackerObject);
            TrackerObject.PopObject(this);

            OurTrackerObject = null;

            return bResult;
        }

        public void AddMessage(MT_MessageType MessageType, string Format, params object[] Args)
        {
            ToolkitAssert.Ensure(OurTrackerObject != null, "TrackerObject shouldn't be invalid.");
            OurTrackerObject.AddMessage(this, MessageType, Format, Args);
        }

        public void AddMessage(MT_MessageType MessageType, string Text)
        {
            ToolkitAssert.Ensure(OurTrackerObject != null, "TrackerObject shouldn't be invalid.");
            OurTrackerObject.AddMessage(this, MessageType, Text);
        }

        protected abstract bool InternalValidate(MT_ValidationTracker TrackerObject);

    }
}

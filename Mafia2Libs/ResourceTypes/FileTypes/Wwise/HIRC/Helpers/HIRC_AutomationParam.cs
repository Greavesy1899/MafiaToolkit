using System.ComponentModel;

namespace ResourceTypes.Wwise.Helpers
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class AutomationParam
    {
        public float xRange { get; set; }
        public float yRange { get; set; }
        public float zRange { get; set; }

        public AutomationParam(float fxRange, float fyRange, float fzRange)
        {
            xRange = fxRange;
            yRange = fyRange;
            zRange = fzRange;
        }

        public AutomationParam()
        {
            xRange = 0;
            yRange = 0;
            zRange = 0;
        }
    }
}

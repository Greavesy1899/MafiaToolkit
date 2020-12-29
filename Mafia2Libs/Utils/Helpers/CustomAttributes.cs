using System.ComponentModel;

namespace Utils.Helpers
{
    // Custom attribute which supports Toolkit localisation.
    public class LocalisedDescriptionAttribute : DescriptionAttribute
    {
        public LocalisedDescriptionAttribute() : base()
        {
        }

        public LocalisedDescriptionAttribute(string description)
        {
            DescriptionValue = Language.Language.GetString(description);
        }
    }
}

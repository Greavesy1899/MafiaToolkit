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

    // Custom attribute which supports Toolkit localisation.
    public class LocalisedCategoryAttribute : CategoryAttribute
    {
        public LocalisedCategoryAttribute(string category) : base(category) { }

        protected override string GetLocalizedString(string value)
        {
            return Language.Language.GetString(value);
        }
    }
}

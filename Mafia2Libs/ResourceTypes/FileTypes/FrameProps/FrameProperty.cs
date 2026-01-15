using System.ComponentModel;
using Gibbed.Illusion.FileFormats.Hashing;
using Utils.Helpers.Reflection;

namespace ResourceTypes.FrameProps
{
    /// <summary>
    /// Represents a single property within a frame entry
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [PropertyClassAllowReflection]
    public class FrameProperty
    {
        private ulong propertyNameHash;
        private string propertyName;

        /// <summary>
        /// FNV64 hash of the property name
        /// </summary>
        [Description("FNV64 hash of the property name")]
        [ReadOnly(true)]
        public ulong PropertyNameHash
        {
            get => propertyNameHash;
            set
            {
                propertyNameHash = value;
                propertyName = null; // Clear cached name when hash changes
            }
        }

        /// <summary>
        /// The property name (if known). Setting this will update the hash.
        /// </summary>
        [Description("The property name. Leave empty if unknown.")]
        public string PropertyName
        {
            get => propertyName ?? $"0x{propertyNameHash:X16}";
            set
            {
                if (!string.IsNullOrEmpty(value) && !value.StartsWith("0x"))
                {
                    propertyName = value;
                    propertyNameHash = FNV64.Hash(value);
                }
            }
        }

        /// <summary>
        /// The property value (semicolon-separated if multiple parts)
        /// </summary>
        [Description("The property value. Multiple values are separated by semicolons.")]
        public string Value { get; set; } = string.Empty;

        /// <summary>
        /// Get the value parts as an array
        /// </summary>
        [Browsable(false)]
        public string[] ValueParts => Value?.Split(';') ?? System.Array.Empty<string>();

        public override string ToString()
        {
            return $"{PropertyName} = {Value}";
        }
    }
}

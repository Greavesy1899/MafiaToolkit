using System;
using System.ComponentModel;
using System.Linq;
using Gibbed.Illusion.FileFormats.Hashing;
using Utils.Helpers.Reflection;

namespace ResourceTypes.FrameProps
{
    /// <summary>
    /// Represents a frame entry with its associated properties
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [PropertyClassAllowReflection]
    public class FramePropsEntry
    {
        private ulong frameNameHash;
        private string frameName;

        /// <summary>
        /// FNV64 hash of the frame name
        /// </summary>
        [Description("FNV64 hash of the frame name")]
        [ReadOnly(true)]
        public ulong FrameNameHash
        {
            get => frameNameHash;
            set
            {
                frameNameHash = value;
                frameName = null; // Clear cached name when hash changes
            }
        }

        /// <summary>
        /// The frame name (if known). Setting this will update the hash.
        /// </summary>
        [Description("The frame name. Leave empty if unknown.")]
        public string FrameName
        {
            get => frameName ?? $"0x{frameNameHash:X16}";
            set
            {
                if (!string.IsNullOrEmpty(value) && !value.StartsWith("0x"))
                {
                    frameName = value;
                    frameNameHash = FNV64.Hash(value);
                }
            }
        }

        /// <summary>
        /// Properties associated with this frame
        /// </summary>
        [Description("Properties associated with this frame")]
        public FrameProperty[] Properties { get; set; } = Array.Empty<FrameProperty>();

        /// <summary>
        /// Get a property value by name hash
        /// </summary>
        public string GetPropertyValue(ulong propertyNameHash)
        {
            var prop = Properties.FirstOrDefault(p => p.PropertyNameHash == propertyNameHash);
            return prop?.Value;
        }

        /// <summary>
        /// Get a property value by name (computes FNV64 hash)
        /// </summary>
        public string GetPropertyValue(string propertyName)
        {
            ulong hash = FNV64.Hash(propertyName);
            return GetPropertyValue(hash);
        }

        /// <summary>
        /// Set a property value by name
        /// </summary>
        public void SetPropertyValue(string propertyName, string value)
        {
            ulong hash = FNV64.Hash(propertyName);
            var prop = Properties.FirstOrDefault(p => p.PropertyNameHash == hash);

            if (prop != null)
            {
                prop.Value = value;
            }
            else
            {
                // Add new property
                var newProps = new FrameProperty[Properties.Length + 1];
                Array.Copy(Properties, newProps, Properties.Length);
                newProps[Properties.Length] = new FrameProperty
                {
                    PropertyNameHash = hash,
                    PropertyName = propertyName,
                    Value = value
                };
                Properties = newProps;
            }
        }

        public override string ToString()
        {
            return $"{FrameName} ({Properties.Length} properties)";
        }
    }
}

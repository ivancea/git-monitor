using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GitMonitor.Objects.Changes
{
    /// <summary>
    /// The changed object type.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ChangeObjectType
    {
        /// <summary>
        /// A branch.
        /// </summary>
        Branch,

        /// <summary>
        /// A tag.
        /// </summary>
        Tag,

        /// <summary>
        /// A commit.
        /// </summary>
        Commit,
    }
}
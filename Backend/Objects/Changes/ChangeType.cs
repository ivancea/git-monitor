using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GitMonitor.Objects.Changes
{
    /// <summary>
    /// The type of a change.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ChangeType
    {
        /// <summary>
        /// An object creation.
        /// </summary>
        Created,

        /// <summary>
        /// An object update.
        /// </summary>
        Updated,

        /// <summary>
        /// An object deletion.
        /// </summary>
        Deleted,
    }
}
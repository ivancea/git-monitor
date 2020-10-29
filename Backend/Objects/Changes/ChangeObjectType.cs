using System.Text.Json.Serialization;

namespace GitMonitor.Objects.Changes
{
    /// <summary>
    /// The changed object type.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
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
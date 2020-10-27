namespace GitMonitor.Services.Changes
{
    /// <summary>
    /// The type of a change.
    /// </summary>
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
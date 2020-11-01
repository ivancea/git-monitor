using System.Collections.Generic;
using GitMonitor.Objects.Changes;

namespace GitMonitor.Objects
{
    /// <summary>
    /// The notification with the changes of the repositories.
    /// </summary>
    public class ChangesNotification
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChangesNotification"/> class.
        /// </summary>
        /// <param name="changes">The notification changes.</param>
        /// <param name="errors">The error occurred during changes retrieval, if any.</param>
        public ChangesNotification(Dictionary<string, List<Change>> changes, Dictionary<string, string> errors)
        {
            Changes = changes;
            Errors = errors;
        }

        /// <summary>
        /// Gets the changes.
        /// </summary>
        /// <value>The changes.</value>
        public Dictionary<string, List<Change>> Changes { get; }

        /// <summary>
        /// Gets the errors occurred during changes retrieval in every repository.
        /// </summary>
        /// <value>The errors occurred during changes retrieval in every repository.</value>
        public Dictionary<string, string> Errors { get; }
    }
}
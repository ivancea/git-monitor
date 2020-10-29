using LibGit2Sharp;

namespace GitMonitor.Objects.Changes
{
    /// <summary>
    /// Object that represents a tag change in a git repository.
    /// </summary>
    public class TagChange : Change
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TagChange"/> class.
        /// </summary>
        /// <param name="type">The type of the change.</param>
        /// <param name="tag">The changed tag.</param>
        public TagChange(ChangeType type, Tag tag)
            : base(ChangeObjectType.Tag, type, tag.FriendlyName)
        {
            TargetCommit = tag.PeeledTarget.Sha;

            if (tag.Annotation is not null)
            {
                User = new ChangeUser(tag.Annotation.Tagger);
                Message = tag.Annotation.Message;
            }
        }

        /// <summary>
        /// Gets the target commit hash.
        /// </summary>
        /// <value>The target commit hash.</value>
        public string TargetCommit { get; }

        /// <summary>
        /// Gets the annotated tag message.
        /// </summary>
        /// <value>The annotated tag message.</value>
        public string? Message { get; }
    }
}
using LibGit2Sharp;

namespace GitMonitor.Objects.Changes
{
    /// <summary>
    /// Object that represents a commit change in a git repository.
    /// </summary>
    public class CommitChange : Change
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommitChange"/> class.
        /// </summary>
        /// <param name="type">The type of the change.</param>
        /// <param name="commit">The changed commit.</param>
        public CommitChange(ChangeType type, Commit commit)
            : base(ChangeObjectType.Commit, type, commit.MessageShort)
        {
            User = new ChangeUser(commit.Committer);
            Hash = commit.Sha;
            Message = commit.Message;
        }

        /// <summary>
        /// Gets the commit hash.
        /// </summary>
        /// <value>The commit hash.</value>
        public string Hash { get; }

        /// <summary>
        /// Gets the commit message.
        /// </summary>
        /// <value>The commit message.</value>
        public string Message { get; }
    }
}
using LibGit2Sharp;

namespace GitMonitor.Services.Changes
{
    /// <summary>
    /// Object that represents a branch change in a git repository.
    /// </summary>
    public class BranchChange : Change
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BranchChange"/> class.
        /// </summary>
        /// <param name="type">The type of the change.</param>
        /// <param name="branch">The changed branch.</param>
        public BranchChange(ChangeType type, Branch branch)
            : base(ChangeObjectType.Branch, type, branch.FriendlyName)
        {
            TargetCommit = branch.Tip.Sha;
        }

        /// <summary>
        /// Gets the target commit hash.
        /// </summary>
        /// <value>The target commit hash.</value>
        public string TargetCommit { get; }
    }
}
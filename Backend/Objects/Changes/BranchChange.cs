using LibGit2Sharp;

namespace GitMonitor.Objects.Changes
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
            : base(ChangeObjectType.Branch, type, MakeBranchName(branch))
        {
            TargetCommit = branch.Tip.Sha;
        }

        /// <summary>
        /// Gets the target commit hash.
        /// </summary>
        /// <value>The target commit hash.</value>
        public string TargetCommit { get; }

        private static string MakeBranchName(Branch branch)
        {
            return branch.IsRemote
                ? branch.FriendlyName.Remove(0, branch.RemoteName.Length + 1)
                : branch.FriendlyName;
        }
    }
}
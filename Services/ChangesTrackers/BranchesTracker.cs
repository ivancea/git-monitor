using System;
using System.Collections.Generic;
using System.Linq;
using GitMonitor.Objects.Changes;
using LibGit2Sharp;

namespace GitMonitor.Services.ChangesTrackers
{
    /// <summary>
    /// Class that tracks branch changes in a git repository.<br/>
    /// It's intended use is to be put in the using block where the repository will be updated.
    /// </summary>
    public class BranchesTracker : AbstractChangesTracker
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BranchesTracker"/> class.
        /// </summary>
        /// <param name="repository">The git repository.</param>
        /// <param name="changes">The list of changes to fill.</param>
        public BranchesTracker(Repository repository, List<Change> changes)
            : base(repository, changes)
        {
            OldBranches = repository.Branches
                .Where(b => b.IsRemote)
                .ToDictionary(b => b.CanonicalName);
        }

        private Dictionary<string, Branch> OldBranches { get; }

        /// <inheritdoc/>
        public override void Dispose()
        {
            GC.SuppressFinalize(this);

            var branches = Repository.Branches
                .Where(b => b.IsRemote)
                .ToDictionary(b => b.CanonicalName);

            foreach (var branch in branches.Values)
            {
                if (OldBranches.TryGetValue(branch.CanonicalName, out var oldBranch))
                {
                    if (oldBranch.Tip.Sha != branch.Tip.Sha)
                    {
                        Changes.Add(new BranchChange(ChangeType.Updated, branch));
                    }
                }
                else
                {
                    Changes.Add(new BranchChange(ChangeType.Created, branch));
                }
            }

            foreach (var deletedBranch in OldBranches.Values.Where(b => !branches.ContainsKey(b.CanonicalName)))
            {
                Changes.Add(new BranchChange(ChangeType.Deleted, deletedBranch));
            }
        }
    }
}
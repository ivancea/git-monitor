using System;
using System.Collections.Generic;
using System.Linq;
using GitMonitor.Objects.Changes;
using LibGit2Sharp;

namespace GitMonitor.Services.ChangesTrackers
{
    /// <summary>
    /// Class that tracks commit changes in a git repository.<br/>
    /// It's intended use is to be put in the using block where the repository will be updated.
    /// </summary>
    public class CommitsTracker : AbstractChangesTracker
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommitsTracker"/> class.
        /// </summary>
        /// <param name="repository">The git repository.</param>
        /// <param name="changes">The list of changes to fill.</param>
        public CommitsTracker(IRepository repository, List<Change> changes)
            : base(repository, changes)
        {
            OldCommits = repository.Commits
                .QueryBy(new CommitFilter() { IncludeReachableFrom = Repository.Refs })
                .ToDictionary(c => c.Sha);
        }

        private Dictionary<string, Commit> OldCommits { get; }

        /// <inheritdoc/>
        public override void Dispose()
        {
            GC.SuppressFinalize(this);

            var commits = Repository.Commits
                .QueryBy(new CommitFilter() { IncludeReachableFrom = Repository.Refs })
                .ToDictionary(c => c.Sha);

            foreach (var commit in commits.Values.Where(c => !OldCommits.ContainsKey(c.Sha)))
            {
                Changes.Add(new CommitChange(ChangeType.Created, commit));
            }

            foreach (var deletedCommit in OldCommits.Values.Where(b => !commits.ContainsKey(b.Sha)))
            {
                Changes.Add(new CommitChange(ChangeType.Deleted, deletedCommit));
            }
        }
    }
}
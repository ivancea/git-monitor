using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GitMonitor.Objects.Changes;
using LibGit2Sharp;

namespace GitMonitor.Services.ChangesTrackers
{
    /// <summary>
    /// Class that tracks tag changes in a git repository.<br/>
    /// It's intended use is to be put in the using block where the repository will be updated.
    /// </summary>
    public class TagsTracker : AbstractChangesTracker
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TagsTracker"/> class.
        /// </summary>
        /// <param name="repository">The git repository.</param>
        /// <param name="changes">The list of changes to fill.</param>
        public TagsTracker(IRepository repository, List<Change> changes)
            : base(repository, changes)
        {
            OldTags = repository.Tags
                .ToDictionary(t => t.CanonicalName);
        }

        private Dictionary<string, Tag> OldTags { get; }

        /// <inheritdoc/>
        public override void Dispose()
        {
            GC.SuppressFinalize(this);

            var tags = Repository.Tags
                .ToDictionary(r => r.CanonicalName);

            foreach (var tag in tags.Values)
            {
                if (OldTags.TryGetValue(tag.CanonicalName, out var oldTag))
                {
                    if (oldTag.Target.Sha != tag.Target.Sha)
                    {
                        Changes.Add(new TagChange(ChangeType.Updated, tag));
                    }
                }
                else
                {
                    Changes.Add(new TagChange(ChangeType.Created, tag));
                }
            }

            foreach (var deletedTag in OldTags.Values.Where(t => !tags.ContainsKey(t.CanonicalName)))
            {
                Changes.Add(new TagChange(ChangeType.Deleted, deletedTag));
            }
        }
    }
}
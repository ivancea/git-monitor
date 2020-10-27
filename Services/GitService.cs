using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GitMonitor.Configurations;
using GitMonitor.Objects;
using LibGit2Sharp;
using Microsoft.Extensions.Options;

namespace GitMonitor.Services
{
    /// <summary>
    /// Service to manage the internal git repositories of the application.
    /// </summary>
    public class GitService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GitService"/> class.
        /// </summary>
        /// <param name="applicationOptions">The application configuration.</param>
        public GitService(IOptions<ApplicationOptions> applicationOptions)
        {
            ApplicationOptions = applicationOptions.Value;
        }

        private ApplicationOptions ApplicationOptions { get; set; }

        /// <summary>
        /// Clones and initializes a repository.
        /// </summary>
        /// <param name="repositoryDescriptor">The repository to clone and initialize.</param>
        public void InitializeRepository(RepositoryDescriptor repositoryDescriptor)
        {
            string clonePath = Path.Combine(ApplicationOptions.RepositoryClonesPath ?? string.Empty, repositoryDescriptor.Name);

            if (Repository.IsValid(clonePath))
            {
                var repository = new Repository(clonePath);

                repository.Network.Remotes.Update("origin", r => r.Url = repositoryDescriptor.Uri.ToString());
                repository.Network.Fetch("origin", new[] { "+refs/heads/*:refs/remotes/origin/*" });
            }
            else
            {
                CloneOptions cloneOptions = new CloneOptions
                {
                    IsBare = true,
                };

                Repository.Clone(
                    repositoryDescriptor.Uri.ToString(),
                    clonePath,
                    cloneOptions);
            }
        }

        /// <summary>
        /// Fetches from the remote repository and find the changes.
        /// </summary>
        /// <param name="repositoryDescriptor">The repository to fetch.</param>
        /// <returns>A list of changes on the repository.</returns>
        public List<string> FetchChanges(RepositoryDescriptor repositoryDescriptor)
        {
            string path = Path.Combine(ApplicationOptions.RepositoryClonesPath ?? string.Empty, repositoryDescriptor.Name);

            var repository = new Repository(path);

            var oldBranches = repository.Branches
                .Where(b => b.IsRemote)
                .ToDictionary(r => r.CanonicalName);

            repository.Network.Fetch("origin", new[] { "+refs/heads/*:refs/remotes/origin/*" }, new FetchOptions { Prune = true });

            var changes = new List<string>();

            var branches = repository.Branches
                .Where(b => b.IsRemote)
                .ToDictionary(r => r.CanonicalName);

            foreach (var branch in branches.Values)
            {
                if (oldBranches.TryGetValue(branch.CanonicalName, out var oldBranch))
                {
                    if (oldBranch.Tip.Sha != branch.Tip.Sha)
                    {
                        changes.Add($"Updated branch '{branch.FriendlyName}'. Now pointing to {branch.Tip.Sha}");
                    }
                }
                else
                {
                    changes.Add($"Created branch '{branch.FriendlyName}'. Pointing to {branch.Tip.Sha}");
                }
            }

            foreach (var deletedBranch in oldBranches.Values.Where(b => !branches.ContainsKey(b.CanonicalName)))
            {
                changes.Add($"Deleted branch '{deletedBranch.FriendlyName}'");
            }

            return changes;
        }
    }
}
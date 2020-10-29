using System.Collections.Generic;
using System.IO;
using GitMonitor.Configurations;
using GitMonitor.Objects;
using GitMonitor.Objects.Changes;
using GitMonitor.Services.ChangesTrackers;
using LibGit2Sharp;
using LibGit2Sharp.Handlers;
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

                repository.Network.Remotes.Update("origin", r => r.Url = repositoryDescriptor.Url.ToString());
                Update(repository, repositoryDescriptor);
            }
            else
            {
                CloneOptions cloneOptions = new CloneOptions
                {
                    IsBare = true,
                    FetchOptions = MakeFetchOptions(repositoryDescriptor),
                    CredentialsProvider = MakeCredentialsProvider(repositoryDescriptor),
                };

                Update(
                    new Repository(
                        Repository.Clone(
                            repositoryDescriptor.Url.ToString(),
                            clonePath,
                            cloneOptions)),
                    repositoryDescriptor);
            }
        }

        /// <summary>
        /// Fetches from the remote repository and find the changes.
        /// </summary>
        /// <param name="repositoryDescriptor">The repository to fetch.</param>
        /// <returns>A list of changes on the repository.</returns>
        public List<Change> FetchChanges(RepositoryDescriptor repositoryDescriptor)
        {
            string path = Path.Combine(ApplicationOptions.RepositoryClonesPath ?? string.Empty, repositoryDescriptor.Name);

            var repository = new Repository(path);

            var changes = new List<Change>();

            using (new CommitsTracker(repository, changes))
            using (new BranchesTracker(repository, changes))
            using (new TagsTracker(repository, changes))
            {
                Update(repository, repositoryDescriptor);
            }

            return changes;
        }

        private void Update(Repository repository, RepositoryDescriptor repositoryDescriptor)
        {
            foreach (var tag in repository.Tags)
            {
                repository.Tags.Remove(tag);
            }

            repository.Network.Fetch(
                "origin",
                new[] { "+refs/heads/*:refs/remotes/origin/*" },
                MakeFetchOptions(repositoryDescriptor));
        }

        private FetchOptions MakeFetchOptions(RepositoryDescriptor repositoryDescriptor)
        {
            var fetchOptions = new FetchOptions
            {
                TagFetchMode = TagFetchMode.All,
                Prune = true,
                CredentialsProvider = MakeCredentialsProvider(repositoryDescriptor),
            };

            return fetchOptions;
        }

        private CredentialsHandler? MakeCredentialsProvider(RepositoryDescriptor repositoryDescriptor)
        {
            if (repositoryDescriptor.Credentials != null)
            {
                return (url, user, cred) => new UsernamePasswordCredentials
                {
                    Username = repositoryDescriptor.Credentials.Username,
                    Password = repositoryDescriptor.Credentials.Password,
                };
            }

            return null;
        }
    }
}
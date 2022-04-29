using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GitMonitor.Hubs;
using GitMonitor.Objects;
using GitMonitor.Objects.Changes;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GitMonitor.Services
{
    /// <summary>
    /// Hosted service that will check for changes in the repositories and notify users.
    /// </summary>
    public class NotificationsService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationsService"/> class.
        /// </summary>
        /// <param name="logger">The service logger.</param>
        /// <param name="gitService">The service that handles git repositories.</param>
        /// <param name="repositoryChangesHub">The repository changes hub to send changes notifications.</param>
        public NotificationsService(ILogger<NotificationsService> logger, GitService gitService, IHubContext<RepositoryChangesHub> repositoryChangesHub)
        {
            Logger = logger;
            GitService = gitService;
            RepositoryChangesHub = repositoryChangesHub;
        }

        private ILogger Logger { get; }

        private GitService GitService { get; }

        private IHubContext<RepositoryChangesHub> RepositoryChangesHub { get; }

        private Timer? Timer { get; set; }

        private IReadOnlyCollection<RepositoryDescriptor>? Repositories { get; set; }

        /// <summary>
        /// Configure the repositories notifications.
        /// <br/>
        /// Enables calls to <see cref="RefreshRepositoriesAsync"/> and activates a Timer that automatically calls it.
        /// </summary>
        /// <param name="refreshInterval">The repositories refresh interval in minutes.</param>
        /// <param name="repositories">The repositories to monitor.</param>
        public void ConfigureRepositories(int refreshInterval, IEnumerable<RepositoryDescriptor> repositories)
        {
            if (Timer is not null)
            {
                Timer.Dispose();
            }

            Logger.LogInformation($"Refreshing repositories every {refreshInterval} minutes");

            Repositories = repositories.ToList();

            Timer = new Timer(
                async s => await RefreshRepositoriesAsync(),
                null,
                TimeSpan.FromMinutes(refreshInterval),
                TimeSpan.FromMinutes(refreshInterval));
        }

        /// <summary>
        /// Refreshes repositories, fetching, generating and sending changes to the clients.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task RefreshRepositoriesAsync()
        {
            if (Repositories is null)
            {
                throw new InvalidOperationException("ConfigureRepositories must be called first");
            }

            Logger.LogDebug($"Refreshing repositories");

            var changes = new Dictionary<string, List<Change>>();
            var errors = new Dictionary<string, string>();

            foreach (var repository in Repositories)
            {
                try
                {
                    var repositoryChanges = GitService.FetchChanges(repository);

                    if (repositoryChanges.Count > 0)
                    {
                        changes[repository.Name] = repositoryChanges;
                    }
                }
                catch (Exception exc)
                {
                    errors[repository.Name] = exc.Message;
                }
            }

            var notification = new ChangesNotification(changes, errors);
            var serializedChanges = JsonConvert.SerializeObject(notification, Formatting.Indented, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
            });

            await RepositoryChangesHub.Clients.All.SendAsync("changes", serializedChanges);
            Logger.LogDebug($"Changes: {serializedChanges}");
        }
    }
}
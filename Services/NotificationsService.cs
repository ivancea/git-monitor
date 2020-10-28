using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GitMonitor.Hubs;
using GitMonitor.Objects;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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

        private ILogger Logger { get; set; }

        private GitService GitService { get; set; }

        private IHubContext<RepositoryChangesHub> RepositoryChangesHub { get; set; }

        private Timer? Timer { get; set; }

        /// <summary>
        /// Configure the repositories notifications.
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

            Timer = new Timer(
                async s => await RefreshRepositoriesAsync(repositories),
                null,
                TimeSpan.FromMinutes(refreshInterval),
                TimeSpan.FromMinutes(refreshInterval));
        }

        private async Task RefreshRepositoriesAsync(IEnumerable<RepositoryDescriptor> repositories)
        {
            Logger.LogInformation($"Refreshing repositories");

            var changes = repositories
                .Select(r => new
                {
                    descriptor = r,
                    changes = GitService.FetchChanges(r),
                })
                .Where(r => r.changes.Count > 0)
                .ToDictionary(
                    r => r.descriptor.Name,
                    r => r.changes);

            if (changes.Count > 0)
            {
                await RepositoryChangesHub.Clients.All.SendAsync("changes", changes);
            }

            Logger.LogInformation($"Changes: {JsonConvert.SerializeObject(changes, Formatting.Indented)}");
        }
    }
}
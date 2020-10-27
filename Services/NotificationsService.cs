using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using GitMonitor.Objects;
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
        /// <param name="gitService">The service that handles git repositories.</param>
        public NotificationsService(ILogger<NotificationsService> logger, GitService gitService)
        {
            Logger = logger;
            GitService = gitService;
        }

        private ILogger Logger { get; set; }

        private GitService GitService { get; set; }

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
                s => RefreshRepositories(repositories),
                null,
                TimeSpan.FromMinutes(refreshInterval),
                TimeSpan.FromMinutes(refreshInterval));
        }

        private void RefreshRepositories(IEnumerable<RepositoryDescriptor> repositories)
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

            Logger.LogInformation($"Changes: {JsonConvert.SerializeObject(changes, Formatting.Indented)}");
        }
    }
}
using System;
using System.Linq;
using GitMonitor.Objects;
using GitMonitor.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace GitMonitor
{
    /// <summary>
    /// Startup filter that initializes the services with the user provided configuration.
    /// </summary>
    public class StartupConfiguration : IStartupFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StartupConfiguration"/> class.
        /// </summary>
        /// <param name="yamlConfigurationService">The configuration service to read user configuration.</param>
        /// <param name="gitService">The git service to configure and check repositories.</param>
        /// <param name="notificationsService">The notifications service to configure with the repositories.</param>
        public StartupConfiguration(YamlConfigurationService yamlConfigurationService, GitService gitService, NotificationsService notificationsService)
        {
            YamlConfigurationService = yamlConfigurationService;
            GitService = gitService;
            NotificationsService = notificationsService;
        }

        private YamlConfigurationService YamlConfigurationService { get; set; }

        private GitService GitService { get; set; }

        private NotificationsService NotificationsService { get; set; }

        /// <inheritdoc/>
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            try
            {
                YamlConfigurationService.LoadConfiguration();

                var repositories = YamlConfigurationService.Repositories ?? Enumerable.Empty<RepositoryDescriptor>();

                foreach (var repository in repositories)
                {
                    GitService.InitializeRepository(repository);
                }

                NotificationsService.ConfigureRepositories(YamlConfigurationService.YamlConfiguration?.RefreshInterval ?? 5, repositories);
            }
            catch (Exception exc)
            {
                throw new ApplicationException("Error loading configuration: " + exc.Message +
                    (exc.InnerException is null
                        ? string.Empty
                        : $" ({exc.InnerException.Message})"));
            }

            return next;
        }
    }
}
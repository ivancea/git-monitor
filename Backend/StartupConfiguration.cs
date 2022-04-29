using System;
using System.Linq;
using GitMonitor.Configurations;
using GitMonitor.Objects;
using GitMonitor.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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
        /// <param name="logger">The service logger.</param>
        /// <param name="applicationOptions">The application configuration to validate.</param>
        /// <param name="yamlConfigurationService">The configuration service to read user configuration.</param>
        /// <param name="gitService">The git service to configure and check repositories.</param>
        /// <param name="notificationsService">The notifications service to configure with the repositories.</param>
        public StartupConfiguration(ILogger<StartupConfiguration> logger, IOptions<ApplicationOptions> applicationOptions, YamlConfigurationService yamlConfigurationService, GitService gitService, NotificationsService notificationsService)
        {
            Logger = logger;
            ApplicationOptions = applicationOptions.Value;
            YamlConfigurationService = yamlConfigurationService;
            GitService = gitService;
            NotificationsService = notificationsService;
        }

        private ILogger<StartupConfiguration> Logger { get; }

        private ApplicationOptions ApplicationOptions { get; }

        private YamlConfigurationService YamlConfigurationService { get; }

        private GitService GitService { get; }

        private NotificationsService NotificationsService { get; }

        /// <inheritdoc/>
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            if (ApplicationOptions.Username != null || ApplicationOptions.Password != null)
            {
                if (ApplicationOptions.Username == null)
                {
                    throw new ApplicationException("Error in configuration: Password set without Username");
                }
                else if (ApplicationOptions.Password == null)
                {
                    throw new ApplicationException("Error in configuration: Username set without Password");
                }
            }

            try
            {
                YamlConfigurationService.LoadConfiguration();

                var repositories = YamlConfigurationService.Repositories ?? Enumerable.Empty<RepositoryDescriptor>();

                foreach (var repository in repositories)
                {
                    Logger.LogInformation("Initializing repository '{repository}'", repository.Name);
                    GitService.InitializeRepository(repository);
                }

                NotificationsService.ConfigureRepositories(YamlConfigurationService.YamlConfiguration?.RefreshInterval ?? 5, repositories);
            }
            catch (Exception exc)
            {
                throw new ApplicationException("Error loading configuration", exc);
            }

            return next;
        }
    }
}
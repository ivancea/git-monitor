using GitMonitor.Configurations;
using GitMonitor.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GitMonitor
{
    /// <summary>
    /// Extension to inject services into the application.
    /// </summary>
    public static class ApplicationInjectionExtensions
    {
        /// <summary>
        /// Injects dependencies into the application.
        /// </summary>
        /// <param name="services">The application service collection.</param>
        /// <param name="configuration">The application configuration.</param>
        public static void AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IStartupFilter, StartupConfiguration>();

            services.AddSingleton<YamlConfigurationService>();
            services.AddSingleton<GitService>();
            services.AddSingleton<NotificationsService>();

            services
                .AddOptions<ApplicationOptions>()
                .Bind(configuration.GetSection(ApplicationOptions.Position))
                .ValidateDataAnnotations();
        }
    }
}
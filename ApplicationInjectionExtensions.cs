using GitMonitor.Configurations;
using GitMonitor.Services;
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
        /// Injects services into the application.
        /// </summary>
        /// <param name="services">The application service collection.</param>
        public static void AddServices(this IServiceCollection services)
        {
            services.AddSingleton<YamlConfigurationService>();
            services.AddSingleton<GitService>();
        }

        /// <summary>
        /// Injects configurations into the application.
        /// </summary>
        /// <param name="services">The application service collection.</param>
        /// <param name="configuration">The application configuration.</param>
        public static void AddConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddOptions<ApplicationOptions>()
                .Bind(configuration.GetSection(ApplicationOptions.Position))
                .ValidateDataAnnotations();
        }
    }
}
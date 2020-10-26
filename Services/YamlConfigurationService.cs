using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GitMonitor.Configurations;
using GitMonitor.Exceptions;
using GitMonitor.Objects;
using GitMonitor.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace GitMonitor
{
    /// <summary>
    /// Class with an extension method to load the Application YAML configuration.
    /// </summary>
    public class YamlConfigurationService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="YamlConfigurationService"/> class.
        /// </summary>
        /// <param name="applicationConfiguration">The application configuration.</param>
        /// <param name="gitService">The service that manages git repositories.</param>
        public YamlConfigurationService(IOptions<ApplicationOptions> applicationConfiguration, GitService gitService)
        {
            ApplicationConfiguration = applicationConfiguration.Value;
            GitService = gitService;

            Deserializer = new DeserializerBuilder()
                .WithNamingConvention(HyphenatedNamingConvention.Instance)
                .Build();
        }

        private ApplicationOptions ApplicationConfiguration { get; }

        private GitService GitService { get; }

        private IDeserializer Deserializer { get; }

        /// <summary>
        /// Reads the YAML configuration and initialices services with it.
        /// This method should be called just once.
        /// </summary>
        public void ConfigureApplication()
        {
            try
            {
                string configPath = ApplicationConfiguration.ConfigPath ?? string.Empty;
                string configFilePath = Path.GetFullPath(Path.Combine(configPath, "config.yml"));

                Console.WriteLine($"Using configuration file '{configFilePath}'");

                if (!File.Exists(configFilePath))
                {
                    throw new YamlConfigurationValidationException("Configuration file doesn't exists");
                }

                var file = File.OpenText(configFilePath);

                var yamlConfiguration = Deserializer.Deserialize<YamlConfiguration?>(file);

                if (yamlConfiguration?.Repositories is null)
                {
                    throw new YamlConfigurationValidationException("No repositories found in configuration");
                }

                foreach (var entry in yamlConfiguration.Repositories)
                {
                }

                var repositories = yamlConfiguration.Repositories
                    .Select(entry =>
                    {
                        if (entry.Value.Uri is null)
                        {
                            throw new YamlConfigurationValidationException($"Repository '{entry.Key}' without uri");
                        }

                        return new RepositoryDescriptor(entry.Key, entry.Value.Uri);
                    })
                    .ToList();

                repositories.ForEach(GitService.InitializeRepository);
            }
            catch (FileNotFoundException exc)
            {
                throw new YamlConfigurationValidationException("Configuration file not found", exc);
            }
            catch (Exception exc)
            {
                throw new YamlConfigurationValidationException("Error loading configuration", exc);
            }
        }
    }
}
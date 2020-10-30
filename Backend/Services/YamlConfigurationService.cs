using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GitMonitor.Configurations;
using GitMonitor.Exceptions;
using GitMonitor.Objects;
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
        public YamlConfigurationService(IOptions<ApplicationOptions> applicationConfiguration)
        {
            ApplicationConfiguration = applicationConfiguration.Value;

            Deserializer = new DeserializerBuilder()
                .WithNamingConvention(HyphenatedNamingConvention.Instance)
                .Build();
        }

        /// <summary>
        /// Gets or sets the YAML user configuration.
        /// </summary>
        /// <value>The YAML user configuration.</value>
        public YamlConfiguration? YamlConfiguration { get; set; }

        /// <summary>
        /// Gets or sets the configured repositories.
        /// </summary>
        /// <value>The configured repositories.</value>
        public List<RepositoryDescriptor>? Repositories { get; set; }

        private ApplicationOptions ApplicationConfiguration { get; }

        private IDeserializer Deserializer { get; }

        /// <summary>
        /// Reads the YAML configuration and validates it it.
        /// </summary>
        public void LoadConfiguration()
        {
            try
            {
                string configPath = ApplicationConfiguration.ConfigPath ?? string.Empty;

                Console.WriteLine($"Using configuration file '{configPath}'");

                if (!File.Exists(configPath))
                {
                    throw new YamlConfigurationValidationException("Configuration file doesn't exists");
                }

                var file = File.OpenText(configPath);

                var yamlConfiguration = Deserializer.Deserialize<YamlConfiguration?>(file);

                if (yamlConfiguration?.Repositories is null)
                {
                    throw new YamlConfigurationValidationException("No repositories found in configuration");
                }

                YamlConfiguration = yamlConfiguration;

                Repositories = yamlConfiguration.Repositories
                    .Select(entry =>
                    {
                        if (entry.Value.Url is null)
                        {
                            throw new YamlConfigurationValidationException($"Repository '{entry.Key}' without uri");
                        }

                        RepositoryCredentials? credentials = null;

                        if (entry.Value.Username != null || entry.Value.Password != null)
                        {
                            if (entry.Value.Username == null)
                            {
                                throw new YamlConfigurationValidationException($"Repository '{entry.Key}' with username but without password");
                            }

                            if (entry.Value.Password == null)
                            {
                                throw new YamlConfigurationValidationException($"Repository '{entry.Key}' with password but without username");
                            }

                            credentials = new RepositoryCredentials(entry.Value.Username, entry.Value.Password);
                        }

                        return new RepositoryDescriptor(entry.Key, entry.Value.Url, credentials, entry.Value.Config);
                    })
                    .ToList();
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
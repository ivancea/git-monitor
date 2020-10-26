using System;
using Microsoft.Extensions.Hosting;

namespace GitMonitor
{
    /// <summary>
    /// Helper class that validates the YAML configuration and the repositories.
    /// </summary>
    public static class ConfigurationValidation
    {
        /// <summary>
        /// Validates the configurations and initializes the host.
        /// </summary>
        /// <param name="host">The host to initialize with the configurations.</param>
        /// <returns>False if there was an error, true otherwise.</returns>
        public static bool Validate(IHost host)
        {
            var service = (YamlConfigurationService?)host.Services.GetService(typeof(YamlConfigurationService));

            if (service is null)
            {
                Console.WriteLine("Fatal error: Unable to load validation class");

                return false;
            }

            try
            {
                service.ConfigureApplication();
            }
            catch (Exception exc)
            {
                Console.WriteLine(
                    "Error loading configuration: " + exc.Message +
                    (exc.InnerException is null
                        ? string.Empty
                        : $" ({exc.InnerException.Message})"));

                return false;
            }

            return true;
        }
    }
}
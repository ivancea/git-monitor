using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace GitMonitor
{
    /// <summary>
    /// Program entry point.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Program entry point.
        /// </summary>
        /// <param name="args">Program arguments.</param>
        /// <returns>1 if there ir an error reading the configuration.</returns>
        public static int Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            if (!ConfigurationValidation.Validate(host))
            {
                return 1;
            }

            host.Run();

            return 0;
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

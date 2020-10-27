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
            try
            {
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception exc)
            {
                Console.WriteLine($"Error: {exc}");

                return 1;
            }

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

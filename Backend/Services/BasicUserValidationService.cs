using System.Threading.Tasks;
using AspNetCore.Authentication.Basic;
using GitMonitor.Configurations;
using Microsoft.Extensions.Options;

namespace GitMonitor.Services
{
    /// <summary>
    /// Basic authentication validation service.
    /// </summary>
    public class BasicUserValidationService : IBasicUserValidationService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BasicUserValidationService"/> class.
        /// </summary>
        /// <param name="logger">The service logger.</param>
        /// <param name="applicationOptions">The application configuration to check the username and password.</param>
        public BasicUserValidationService(IOptions<ApplicationOptions> applicationOptions)
        {
            ApplicationOptions = applicationOptions.Value;
        }

        private ApplicationOptions ApplicationOptions { get; }

        /// <inheritdoc/>
        public Task<bool> IsValidAsync(string username, string password)
        {
            return Task.FromResult(
                ApplicationOptions.Username == null ||
                (
                    username == ApplicationOptions.Username &&
                    password == ApplicationOptions.Password));
        }
    }
}
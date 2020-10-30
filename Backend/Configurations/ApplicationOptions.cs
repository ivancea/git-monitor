using System.ComponentModel.DataAnnotations;

namespace GitMonitor.Configurations
{
    /// <summary>
    /// Options class with application level configurations.
    /// </summary>
    public class ApplicationOptions
    {
        /// <summary>
        /// Configuration key.
        /// </summary>
        public const string Position = "Application";

        /// <summary>
        /// Gets or sets the configuration path.
        /// </summary>
        /// <value>The configuration path.</value>
        [Required]
        public string? ConfigPath { get; set; }

        /// <summary>
        /// Gets or sets the path of the repository clones.
        /// </summary>
        /// <value>The path of the repository clones.</value>
        [Required]
        public string? RepositoryClonesPath { get; set; }

        /// <summary>
        /// Gets or sets the authentication username.
        /// </summary>
        /// <value>The authentication username.</value>
        public string? Username { get; set; }

        /// <summary>
        /// Gets or sets the authentication password.
        /// </summary>
        /// <value>The authentication password.</value>
        public string? Password { get; set; }
    }
}
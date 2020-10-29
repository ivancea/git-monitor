using System;
using System.Collections.Generic;

namespace GitMonitor.Configurations
{
    /// <summary>
    /// Root node of the YAML application configuration.
    /// </summary>
    public class YamlConfiguration
    {
        /// <summary>
        /// Gets or sets the refresh interval in minutes.
        /// </summary>
        /// <value>The refresh interval in minutes.</value>
        public int? RefreshInterval { get; set; }

        /// <summary>
        /// Gets or sets repositories to monitor, identified by a name.
        /// </summary>
        /// <value>Repositories to monitor, identified by a name.</value>
        public Dictionary<string, Repository>? Repositories { get; set; }

        /// <summary>
        /// Repository of the YAML application configuration.
        /// </summary>
        public class Repository
        {
            /// <summary>
            /// Gets or sets repository URL.
            /// </summary>
            /// <value>Repository URL.</value>
            public Uri? Url { get; set; }

            /// <summary>
            /// Gets or sets the remote repository username.
            /// </summary>
            /// <value>The remote repository username.</value>
            public string? Username { get; set; }

            /// <summary>
            /// Gets or sets the remote repository password.
            /// </summary>
            /// <value>The remote repository password.</value>
            public string? Password { get; set; }

            /// <summary>
            /// Gets or sets the repository configuration.
            /// </summary>
            /// <value>The repository configuration.</value>
            public Dictionary<string, string> Config { get; set; } = new Dictionary<string, string>();
        }
    }
}
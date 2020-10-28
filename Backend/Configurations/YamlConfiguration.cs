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
            /// Gets or sets repository URI.
            /// </summary>
            /// <value>Repository URI.</value>
            public Uri? Uri { get; set; }
        }
    }
}
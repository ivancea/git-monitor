using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace GitMonitor.Objects
{
    /// <summary>
    /// Class with all the data of a repository.
    /// </summary>
    public class RepositoryDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryDescriptor"/> class.
        /// </summary>
        /// <param name="name">The name of the repository.</param>
        /// <param name="url">The URL of the repository.</param>
        /// <param name="credentials">The remote repository credentials.</param>
        /// <param name="config">The repository configuration.</param>
        public RepositoryDescriptor(string name, Uri url, RepositoryCredentials? credentials, Dictionary<string, string> config)
        {
            Name = name;
            Url = url;
            Credentials = credentials;
            Config = config.ToImmutableDictionary();
        }

        /// <summary>
        /// Gets the name of the repository, that will be used as an identifier.
        /// </summary>
        /// <value>The name of the repository, that will be used as an identifier.</value>
        public string Name { get; }

        /// <summary>
        /// Gets the URL of the repository.
        /// </summary>
        /// <value>The URL of the repository.</value>
        public Uri Url { get; }

        /// <summary>
        /// Gets the remote repository credentials.
        /// </summary>
        /// <value>The remote repository credentials.</value>
        public RepositoryCredentials? Credentials { get; }

        /// <summary>
        /// Gets the repository configuration.
        /// </summary>
        /// <value>The repository configuration.</value>
        public IReadOnlyDictionary<string, string> Config { get; } = new Dictionary<string, string>();
    }
}
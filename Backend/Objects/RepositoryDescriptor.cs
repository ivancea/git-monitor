using System;

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
        public RepositoryDescriptor(string name, Uri url, RepositoryCredentials? credentials)
        {
            Name = name;
            Url = url;
            Credentials = credentials;
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
    }
}
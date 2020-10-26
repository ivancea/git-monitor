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
        /// <param name="uri">The URI of the repository.</param>
        public RepositoryDescriptor(string name, Uri uri)
        {
            Name = name;
            Uri = uri;
        }

        /// <summary>
        /// Gets or sets the name of the repository, that will be used as an identifier.
        /// </summary>
        /// <value>The name of the repository, that will be used as an identifier.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the uri of the repository.
        /// </summary>
        /// <value>The uri of the repository.</value>
        public Uri Uri { get; set; }
    }
}
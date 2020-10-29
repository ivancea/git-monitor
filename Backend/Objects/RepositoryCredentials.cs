namespace GitMonitor.Objects
{
    /// <summary>
    /// Class with the credentials of a remote repository.
    /// </summary>
    public class RepositoryCredentials
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryCredentials"/> class.
        /// </summary>
        /// <param name="username">The remote repository username.</param>
        /// <param name="password">The remote repository password.</param>
        public RepositoryCredentials(string username, string password)
        {
            Username = username;
            Password = password;
        }

        /// <summary>
        /// Gets the remote repository username.
        /// </summary>
        /// <value>The remote repository username.</value>
        public string Username { get; }

        /// <summary>
        /// Gets the remote repository password.
        /// </summary>
        /// <value>The remote repository password.</value>
        public string Password { get; }
    }
}
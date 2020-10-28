using LibGit2Sharp;

namespace GitMonitor.Objects.Changes
{
    /// <summary>
    /// A change user information.
    /// </summary>
    public class ChangeUser
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeUser"/> class.
        /// </summary>
        /// <param name="signature">The changed object signature.</param>
        public ChangeUser(Signature signature)
        {
            Name = signature.Name;
            Email = signature.Email;
        }

        /// <summary>
        /// Gets the user name.
        /// </summary>
        /// <value>The user name.</value>
        public string Name { get; }

        /// <summary>
        /// Gets the user email.
        /// </summary>
        /// <value>The user email.</value>
        public string Email { get; }
    }
}
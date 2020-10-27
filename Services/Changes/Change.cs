namespace GitMonitor.Services.Changes
{
    /// <summary>
    /// Base object that represents a change in a git repository.
    /// </summary>
    public abstract class Change
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Change"/> class.
        /// </summary>
        /// <param name="objectType">The changed object type.</param>
        /// <param name="type">The type of the change.</param>
        /// <param name="objectName">The name of the changed object.</param>
        protected Change(ChangeObjectType objectType, ChangeType type, string objectName)
        {
            ObjectType = objectType;
            Type = type;
            ObjectName = objectName;
        }

        /// <summary>
        /// Gets the changed object type.
        /// </summary>
        /// <value>The changed object type.</value>
        public ChangeObjectType ObjectType { get; }

        /// <summary>
        /// Gets the type of the change.
        /// </summary>
        /// <value>The type of the change.</value>
        public ChangeType Type { get; }

        /// <summary>
        /// Gets the name of the changed object.
        /// </summary>
        /// <value>The name of the changed object.</value>
        public string ObjectName { get; }
    }
}
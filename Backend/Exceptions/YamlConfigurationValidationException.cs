using System;

namespace GitMonitor.Exceptions
{
    /// <summary>
    /// Generic exception thrown on YAML configuration validation errors.
    /// </summary>
    public class YamlConfigurationValidationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="YamlConfigurationValidationException"/> class.
        /// </summary>
        /// <param name="message">The validation error message.</param>
        public YamlConfigurationValidationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="YamlConfigurationValidationException"/> class.
        /// </summary>
        /// <param name="message">The validation error message.</param>
        /// <param name="exc">The original exception.</param>
        public YamlConfigurationValidationException(string message, Exception exc)
            : base(message, exc)
        {
        }
    }
}
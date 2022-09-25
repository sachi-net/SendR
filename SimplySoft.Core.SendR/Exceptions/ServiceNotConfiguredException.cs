using System;

namespace SimplySoft.Core.SendR.Exceptions
{
    /// <summary>
    /// The exception that is thrown when tries to use a SendR service before it is registered as a dependency.
    /// </summary>
    public class ServiceNotConfiguredException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceNotConfiguredException"/> class.
        /// </summary>
        public ServiceNotConfiguredException() : base()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceNotConfiguredException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public ServiceNotConfiguredException(string message) : base(message)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateTemplateException"/> class with a specified
        /// error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="inner">The exception that is the cause of the current exception.</param>
        public ServiceNotConfiguredException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}

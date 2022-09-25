using System;

namespace SimplySoft.Core.SendR.Exceptions
{
    /// <summary>
    /// The exception that is thrown when the template is not found in template collection.
    /// </summary>
    public class TemplateNotDefinedException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateNotDefinedException"/> class.
        /// </summary>
        public TemplateNotDefinedException() : base()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateNotDefinedException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public TemplateNotDefinedException(string message) : base(message)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateNotDefinedException"/> class with a specified
        /// error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="inner">The exception that is the cause of the current exception.</param>
        public TemplateNotDefinedException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}

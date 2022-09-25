using System;

namespace SimplySoft.Core.SendR.Exceptions
{
    /// <summary>
    /// The exception that is thrown when there are duplicate templates found in the same template collection.
    /// </summary>
    public class DuplicateTemplateException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateTemplateException"/> class.
        /// </summary>
        public DuplicateTemplateException() : base()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateTemplateException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public DuplicateTemplateException(string message) : base(message)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateTemplateException"/> class with a specified
        /// error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="inner">The exception that is the cause of the current exception.</param>
        public DuplicateTemplateException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}

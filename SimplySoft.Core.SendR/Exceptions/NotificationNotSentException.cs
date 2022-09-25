﻿using System;

namespace SimplySoft.Core.SendR.Exceptions
{
    /// <summary>
    /// The exception that is thrown when the SendR notification is failed in sending.
    /// </summary>
    public class NotificationNotSentException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationNotSentException"/> class.
        /// </summary>
        public NotificationNotSentException() : base()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationNotSentException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public NotificationNotSentException(string message) : base(message)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationNotSentException"/> class with a specified
        /// error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="inner">The exception that is the cause of the current exception.</param>
        public NotificationNotSentException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}

using System;

namespace Reach.Exceptions
{
    /// <summary>
    /// Base ReachException
    /// </summary>
    public class ReachException : Exception
    {
        /// <summary>
        /// Create an empty ReachException
        /// </summary>
        public ReachException() {}

        /// <summary>
        /// Create a ReachException from an error message
        /// </summary>
        /// <param name="message">Error message</param>
        public ReachException (string message) : base(message) {}

        /// <summary>
        /// Create a ReachException from message and another exception
        /// </summary>
        /// <param name="message">Error message</param>
        /// <param name="exception">Original Exception</param>
        public ReachException(string message, Exception exception) : base(message, exception) {}
    }

    /// <summary>
    /// Exception related to connection errors
    /// </summary>
    public class ApiConnectionException : ReachException
    {
        /// <summary>
        /// Create an ApiConnectionException from a message
        /// </summary>
        /// <param name="message">Error message</param>
        public ApiConnectionException(string message) : base(message) {}

        /// <summary>
        /// Create an ApiConnectionException from a message and another Exception
        /// </summary>
        /// <param name="message">Error message</param>
        /// <param name="exception">Original Exception</param>
        public ApiConnectionException(string message, Exception exception) : base(message, exception) {}
    }

    /// <summary>
    /// Exception related to Authentication Errors
    /// </summary>
    public class AuthenticationException : ReachException
    {
        /// <summary>
        /// Create AuthenticationException from an error messsage
        /// </summary>
        /// <param name="message">Error message</param>
        public AuthenticationException(string message) : base(message) {}
    }
}


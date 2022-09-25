using SimplySoft.Core.SendR.Email.Models;

namespace SimplySoft.Core.SendR.Enumerators
{
    /// <summary>
    /// Fallback action type to handle a failure when performing SendR transaction.
    /// </summary>
    public enum FallbackActionMode
    {
        /// <summary>
        /// Ignore the underlying failure and try to proceed forward.
        /// </summary>
        Ignore,

        /// <summary>
        /// Throw an exception on a failure of SendR transaction.
        /// </summary>
        ThrowException,

        /// <summary>
        /// Invoke the provided action on a failure of SendR transaction. This requires to setup a
        /// fallback action into <see cref="EmailOptions.SendingFailedFallback"/> action delegate.
        /// </summary>
        ExecuteAction
    }
}

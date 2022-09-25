using System.Threading.Tasks;

namespace SimplySoft.Core.SendR
{
    /// <summary>
    /// Defines SendR notification actions that other built-in or custom notifications classes implement to work 
    /// with <see cref="INotificationCollection"/>.
    /// </summary>
    public interface INotificationSendable
    {
        /// <summary>
        /// Add this <see cref="INotificationSendable"/> to the provided <see cref="INotificationCollection"/>.
        /// </summary>
        /// <param name="notifications"><see cref="INotificationCollection"/> in which this <see cref="INotificationSendable"/> 
        /// is added into.</param>
        void AddTo(INotificationCollection notifications);

        /// <summary>
        /// Send this <see cref="INotificationSendable"/> to the specified recipients.
        /// </summary>
        void Send();

        /// <summary>
        /// Send this <see cref="INotificationSendable"/> to the specified recipients asynchronoulsy.
        /// </summary>
        /// <returns>Awaitable <see cref="Task"/> as the result of this operation.</returns>
        Task SendAsync();
    }
}

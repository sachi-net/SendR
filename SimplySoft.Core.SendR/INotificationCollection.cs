using System.Threading.Tasks;

namespace SimplySoft.Core.SendR
{
    /// <summary>
    /// Define SendR notification collection which allows to perform batch-tasks against multiple notification definitions.
    /// </summary>
    public interface INotificationCollection
    {
        /// <summary>
        /// Add the provided <see cref="INotificationSendable"/> into this <see cref="INotificationCollection"/>.
        /// </summary>
        /// <param name="notification">The <see cref="INotificationSendable"/> to be added into the collection.</param>
        void Add(INotificationSendable notification);

        /// <summary>
        /// Send all notifications saved in the current collection to their specified recipients.
        /// </summary>
        void SendAll();

        /// <summary>
        /// Send all notifications added to the current collection to their specified recipients asynchronously.
        /// </summary>
        /// <returns>Awaitable <see cref="Task"/> as the result of this operation.</returns>
        Task SendAllAsync();
    }
}
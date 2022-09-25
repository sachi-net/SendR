using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimplySoft.Core.SendR
{
    /// <summary>
    /// Define SendR notification collection which allows to perform batch-tasks against multiple notification definitions.
    /// </summary>
    public class NotificationCollection : INotificationCollection
    {
        private List<INotificationSendable> _notifications = new List<INotificationSendable>();

        /// <summary>
        /// Add the provided <see cref="INotificationSendable"/> into this <see cref="NotificationCollection"/>.
        /// </summary>
        /// <param name="notification">The <see cref="INotificationSendable"/> to be added into the collection.</param>
        public void Add(INotificationSendable notification)
        {
            _notifications.Add(notification);
        }

        /// <summary>
        /// Send all notifications saved in the current collection to their specified recipients.
        /// </summary>
        /// <remarks>The notification collection will be cleared after this method call.</remarks>
        public void SendAll()
        {
            try
            {
                if (_notifications.Count > 0)
                {
                    foreach (var notification in _notifications)
                        notification.Send();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally 
            { 
                _notifications.Clear(); 
            }
        }

        /// <summary>
        /// Send all notifications added to the current collection to their specified recipients asynchronously.
        /// </summary>
        /// <returns>Awaitable <see cref="Task"/> as the result of this operation.</returns>
        /// <remarks>The notification collection will be cleared after this method call.</remarks>
        public async Task SendAllAsync()
        {
            try
            {
                if (_notifications.Count > 0)
                {
                    foreach (var notification in _notifications)
                        await Task.Run(notification.SendAsync);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally 
            {
                _notifications.Clear();
            }
        }
    }
}

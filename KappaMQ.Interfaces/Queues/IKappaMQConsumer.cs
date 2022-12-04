using KappaMQ.Classes.Messages;

namespace KappaMQ.Interfaces
{
    public interface IKappaMQConsumer
    {
        /// <summary>
        /// Subscribe to queue
        /// </summary>
        /// <param name="queueName"></param>
        void Subscribe(string queueName);
        /// <summary>
        /// Unsubscribe from queue
        /// </summary>
        /// <param name="queueName"></param>
        void Unsubscribe(string queueName);
        /// <summary>
        /// Message was processed, delete message
        /// </summary>
        /// <param name="messageGuid"></param>
        void Approve(Guid messageGuid);
        /// <summary>
        /// Message wasn't processed, return it to queue
        /// </summary>
        /// <param name="messageGuid"></param>
        void Decline(Guid messageGuid);
        /// <summary>
        /// Get next message from queue
        /// </summary>
        /// <param name="queueName"></param>
        /// <returns></returns>
        MQMessage NextMessage(string queueName);
    }
}

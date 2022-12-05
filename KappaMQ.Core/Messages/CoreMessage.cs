using KappaMQ.Classes.Messages;

namespace KappaMQ.Core.Messages
{
    internal class CoreMessage : MQMessage
    {
        /// <summary>
        /// Time when message was leased
        /// </summary>
        public DateTime LeaseTime { get; set; } = DateTime.MinValue;
    }
}

using KappaMQ.Classes.Messages;

namespace KappaMQ.Core.Queues
{
    internal class Subscriber
    {
        public Guid Id { get; set; }
        public Action<MQMessage> Action { get; set; }
        public SubscriberState State { get; set; }
        public Guid MessageId { get; set; }
    }
}

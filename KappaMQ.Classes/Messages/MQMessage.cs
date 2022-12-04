namespace KappaMQ.Classes.Messages
{
    public class MQMessage
    {
        public Guid Id { get; set; }
        public DateTime CreateDate { get; set; }
        public MessagePersistence Persistence { get; set; }
        public string Queue { get; set; }
        public string Body { get; set; }
    }
}

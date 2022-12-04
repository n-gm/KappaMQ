using KappaMQ.Classes.Messages;
using KappaMQ.Core.Messages;
using KappaMQ.Core.Settings;
using System.Text.Json;

namespace KappaMQ.Core.Queues
{
    internal class Queue
    {
        private List<CoreMessage> _messages;
        private List<Subscriber> _subscribers;
        private MQSettings _settings;
        public string Name { get; set; }

        public Queue(string queueName, MQSettings settings)
        {
            Name = queueName;
            _settings = settings;
            _messages = new List<CoreMessage>();
            _subscribers = new List<Subscriber>();
        }

        public Guid Subscribe(Action<MQMessage> action)
        {
            var subscriber = new Subscriber
            {
                Id = Guid.NewGuid(),
                Action = action,
                MessageId = Guid.Empty,
                State = SubscriberState.Active
            };
            lock (_subscribers)
            {
                _subscribers.Add(subscriber);
            }
            return subscriber.Id;
        }

        public void Unsubscribe(Guid id)
        {
            lock (_subscribers)
            {
                for(int i = 0; i < _subscribers.Count(); i++)
                {
                    if (_subscribers[i].Id == id)
                    {
                        _subscribers.RemoveAt(i);
                    }
                }
            }
        }

        public void Approve(Guid messageGuid)
        {
            lock (_messages)
            {
                foreach (CoreMessage message in _messages)
                {
                    if (message.Id == messageGuid)
                    {
                        _messages.Remove(message);
                        return;
                    }
                }
            }
        }

        public void Decline(Guid messageGuid)
        {
            lock (_messages)
            {
                foreach (CoreMessage message in _messages)
                {
                    if (message.Id == messageGuid)
                    {
                        message.State = MessageState.Available;
                        message.LeaseTime = DateTime.MinValue;
                        return;
                    }
                }
            }
        }

        public MQMessage? NextMessage()
        {
            lock (_messages) {
                foreach (CoreMessage message in _messages)
                {
                    if (message.State == MessageState.Available)
                    {
                        message.State = MessageState.Leased;
                        message.LeaseTime = DateTime.Now;
                        return message;
                    } else if (message.LeaseTime.AddSeconds(_settings.LeaseTime) >= DateTime.Now)
                    {
                        message.LeaseTime = DateTime.Now;
                        return message;
                    }
                }
            }
            return null;
        }

        public void Produce(string body)
        {
            var message = new CoreMessage
            {
                Body = body,
                CreateDate = DateTime.Now,
                Id = Guid.NewGuid(),
                LeaseTime = DateTime.MinValue,
                Queue = Name,
                State = MessageState.Available
            };
            lock (_messages)
            {
                _messages.Add(message);
            }
        }

        public void Produce<T>(T body) where T : class
        {
            var value = JsonSerializer.Serialize<T>(body);
            Produce(value);
        }
    }
}

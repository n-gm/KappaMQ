using KappaMQ.Classes.Messages;
using KappaMQ.Core.Messages;
using KappaMQ.Core.Settings;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace KappaMQ.Core.Queues
{
    internal class Queue
    {
        private ConcurrentQueue<CoreMessage> _messages;
        private List<CoreMessage> _leasedMessages;
        private MQSettings _settings;

        public string Name { get; private set; }

        public Queue(string name, MQSettings settings)
        {
            _messages = new();
            _leasedMessages = new();
            _settings = settings;
            Name = name;
        }

        public async Task<MQMessage> ConsumeAsync(CancellationToken token = default)
        {
            CoreMessage message;
            while (!_messages.TryDequeue(out message))
            {                
                await Task.Delay(_settings.ReactionTime, token);
            }
            message.LeaseTime = DateTime.Now;

            lock (_leasedMessages)
            {
                _leasedMessages.Add(message);
            }
            return message;
        }

        public void Accept(Guid id)
        {
            lock (_leasedMessages)
            {
                for (int i = 0; i < _leasedMessages.Count(); i++)
                {
                    if (_leasedMessages[i].Id == id)
                    {
                        _leasedMessages.RemoveAt(i);
                        return;
                    }
                }
            }
        }

        public void Decline(Guid id)
        {
            CoreMessage? message = null;
            lock (_leasedMessages)
            {
                for (int i = 0; i < _leasedMessages.Count(); i++)
                {
                    if (_leasedMessages[i].Id == id)
                    {
                        message = _leasedMessages[i];
                        break;
                    }
                }
            }

            if (message == null)
                return;

            message.LeaseTime = DateTime.MinValue;
            _messages.Enqueue(message);
        }

        public MQMessage? Consume()
        {
            _messages.TryDequeue(out var message);
            return message;
        }

        public async IAsyncEnumerable<CoreMessage> MessagesAsync([EnumeratorCancellation]CancellationToken token = default)
        {
            while (!token.IsCancellationRequested)
            {
                if (_messages.TryDequeue(out var message))
                {
                    yield return message;
                }
                await Task.Delay(_settings.ReactionTime, token);
            }
        }

        public void Publish(string body, MessagePersistence persistence)
        {
            var message = new CoreMessage
            {
                Body = body,
                CreateDate = DateTime.Now,
                Id = Guid.NewGuid(),
                LeaseTime = DateTime.MinValue,
                Persistence = persistence,
                Queue = Name
            };
            _messages.Enqueue(message);
        }

        public void Publish<T>(T body, MessagePersistence persistence) where T:class
        {
            var bodyJson = JsonSerializer.Serialize(body);
            Publish(body, persistence);
        }
    }
}

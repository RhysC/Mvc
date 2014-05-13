using System;
using Newtonsoft.Json;

namespace CodeConverters.PersistentQueues
{
    public class DequeuedMessage<TMessage> where TMessage : class
    {
        private readonly IPersistentMessage _message;
        private readonly Action<Guid> _processed;
      
        public DequeuedMessage(IPersistentMessage message, Action<Guid> processed)
        {
            _message = message;
            _processed = processed;
        }

        public TMessage GetPayload()
        {
            if (_message == null) return null;
            var payload = JsonConvert.DeserializeObject(_message.MessageContent, Type.GetType(_message.MessageType)) as TMessage;
            if (payload == null)
                throw new NullReferenceException(string.Format("The message on deserialization was null which may indicate an invalid payload such as a type mismatch. Type: {0} : content : {1}", _message.MessageType, _message.MessageContent));
            return payload;
        }

        public void MarkAsProcessed()
        {
            _processed(_message.Id);
        }

        public bool HasPayload()
        {
            return _message != null;
        }
    }
}
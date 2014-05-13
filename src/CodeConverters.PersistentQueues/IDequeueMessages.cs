using System;

namespace CodeConverters.PersistentQueues
{
    public interface IDequeueMessages<TMessage> : IDisposable where TMessage : class
    {
        DequeuedMessage<TMessage> Dequeue();
    }
}
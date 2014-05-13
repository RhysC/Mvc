using System;

namespace CodeConverters.PersistentQueues
{
    public interface IEnqueueMessages<in TMessage> : IDisposable where TMessage : class
    {
        void Enqueue(TMessage message);
    }
}
using System;
using System.Data.Entity;
using System.Linq;
using CodeConverters.Core.Persistence;

namespace CodeConverters.PersistentQueues
{
    public abstract class MessageQueueBase<TMessage, TPersistentMessage>
        : IDequeueMessages<TMessage>,
          IEnqueueMessages<TMessage>
        where TMessage : class
        where TPersistentMessage : class, IPersistentMessage
    {
        protected readonly IDbContext DataContext;

        protected MessageQueueBase(IDbContext dataContext)
        {
            DataContext = dataContext;
        }

        protected virtual IDbSet<TPersistentMessage> GetMessageSet()
        {
            return DataContext.Set<TPersistentMessage>();
        }

        protected abstract TPersistentMessage CreateMessageToQueue(TMessage message);

        protected virtual TPersistentMessage GetMessageById(Guid messageId)
        {
            return GetMessageSet().Single(m => m.Id == messageId);
        }

        public void Enqueue(TMessage message)
        {
            var messageToQueue = CreateMessageToQueue(message);
            GetMessageSet().Add(messageToQueue);
            DataContext.SaveChanges();
        }

        public DequeuedMessage<TMessage> Dequeue()
        {
            var message = GetMessageSet().Where(m => !m.ProcessedOn.HasValue)
                .OrderBy(m => m.Sequence)
                .FirstOrDefault();

            return new DequeuedMessage<TMessage>(message, Processed);
        }

        private void Processed(Guid messageId)
        {
            var retrievedMessage = GetMessageById(messageId);
            retrievedMessage.ProcessedOn = DateTimeOffset.UtcNow;
            DataContext.SaveChanges();
        }

        public void Dispose()
        {
            DataContext.Dispose();
        }
    }
}
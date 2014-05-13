using CodeConverters.Core.Persistence;
using CodeConverters.PersistentQueues;

namespace CodeConverters.Worker
{
    public class WorkerTask<T> : IWorkerTask<T> where T : class
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDequeueMessages<T> _messageQueue;
        private readonly IMessageHandler<T> _messageHandler;

        public WorkerTask(
            IUnitOfWork unitOfWork,
            IDequeueMessages<T> messageQueue,
            IMessageHandler<T> messageHandler)
        {
            _unitOfWork = unitOfWork;
            _messageQueue = messageQueue;
            _messageHandler = messageHandler;
        }

        public WorkResult DoWork()
        {
            using (_unitOfWork)
            {
                var dequeuedMessage = _messageQueue.Dequeue();
                if (!dequeuedMessage.HasPayload())
                    return WorkResult.None;

                _messageHandler.Handle(dequeuedMessage.GetPayload());
                dequeuedMessage.MarkAsProcessed();
                _unitOfWork.Commit();
                return WorkResult.Processed;
            }
        }
    }
}
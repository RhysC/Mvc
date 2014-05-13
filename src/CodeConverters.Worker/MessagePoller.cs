using System;
using System.Threading;
using Autofac.Features.OwnedInstances;
using log4net;

namespace CodeConverters.Worker
{
    public class MessagePoller<T> : IStartable where T : class
    {
        //Note that an implementation could decorate the underlying worker task with a stop watch as done in Hoover to send NewRelic metrics
        private readonly Func<Owned<IWorkerTask<T>>> _workerTaskFactory;
        private readonly ILog _logger;

        public MessagePoller(Func<Owned<IWorkerTask<T>>> workerTaskFactory)
        {
            _workerTaskFactory = workerTaskFactory;
            _logger = LogManager.GetLogger(GetType());
        }

        public void Start()
        {
            while (true)
            {
                try
                {
                    using (var scope = _workerTaskFactory())
                    {
                        scope.Value
                             .DoWork()
                             .HandleResult(_logger);
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error("Failed to process message:", ex);
                    Thread.Sleep(WorkResult.SleepTime);
                }
            }
            // ReSharper disable once FunctionNeverReturns
        }
    }
}
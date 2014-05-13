using System;
using System.Threading;
using log4net;

namespace CodeConverters.PersistentQueues
{
    public abstract class MessageHandlerBase<T> : IMessageHandler<T>
    {
        protected readonly ILog Logger;
        private const int MaxAttempts = 3;

        protected MessageHandlerBase()
        {
            Logger = LogManager.GetLogger(GetType());
        }

        protected abstract void Process(T message);

        public void Handle(T message)
        {
            //retry logic goes here - Handle is a (blocking) command - 
            // be aware that throwing below means other handlers won't handle this message
            // but if we can't handle it then...

            for (var i = 0; i < MaxAttempts; i++)
            {
                try
                {
                    Process(message);
                    return;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                    Thread.Sleep(5000 * i + 1);

                    if (i != MaxAttempts - 1) continue;
                    Logger.FatalFormat("Could not process message : {0}", message);
                    throw;
                }
            }
        }
    }
}
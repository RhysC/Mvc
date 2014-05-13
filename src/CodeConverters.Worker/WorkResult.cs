using System;
using System.Threading;
using log4net;

namespace CodeConverters.Worker
{
    public abstract class WorkResult
    {
        public static TimeSpan SleepTime = TimeSpan.FromSeconds(10);

        public static readonly WorkResult None = new NoneWorkResult();
        public static readonly WorkResult Processed = new ProcessedWorkResult();

        public abstract void HandleResult(ILog logger);

        public class NoneWorkResult : WorkResult
        {
            public override void HandleResult(ILog logger)
            {
                logger.Debug("No messages to process. SleepTime: " + SleepTime);
                Thread.Sleep(SleepTime);
            }
        }

        public class ProcessedWorkResult : WorkResult
        {
            public override void HandleResult(ILog logger)
            {
                logger.Info("Processed message. Looking for more work");
            }
        }
    }
}
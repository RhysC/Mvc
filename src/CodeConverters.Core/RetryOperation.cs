using System;
using System.Threading;
using log4net;

namespace CodeConverters.Core
{
    public static class RetryOperation
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(RetryOperation));

        public static void For(Action action, int attempts, int waitInSeconds, bool randomiseWait = false)
        {
            var rawWaitInSeconds = waitInSeconds;
            var remainingAttempts = attempts;
            while (remainingAttempts > 0)
            {
                try
                {
                    action();
                    return;
                }
                catch (Exception e)
                {
                    remainingAttempts--;
                    if (remainingAttempts == 0)
                        throw;

                    Logger.WarnFormat("Retry loop failed - {0}. {1} attempts remaining", e.Message, remainingAttempts);
                    if (randomiseWait)
                        waitInSeconds = new Random().Next(0, rawWaitInSeconds);
                    Thread.Sleep(TimeSpan.FromSeconds(waitInSeconds));
                }
            }
        }

        public static T For<T>(Func<T> action, int attempts, int waitInSeconds, bool randomiseWait = false)
        {
            var rawWaitInSeconds = waitInSeconds;
            var remainingAttempts = attempts;
            while (remainingAttempts > 0)
            {
                try
                {
                    return action();
                }
                catch (Exception e)
                {
                    remainingAttempts--;
                    if (remainingAttempts == 0)
                        throw;

                    Logger.WarnFormat("Retry loop failed - {0}. {1} attempts remaining", e.Message, remainingAttempts);
                    if (randomiseWait)
                        waitInSeconds = new Random().Next(0, rawWaitInSeconds);
                    Thread.Sleep(TimeSpan.FromSeconds(waitInSeconds));
                }
            }
            return default(T);
        }
    }
}
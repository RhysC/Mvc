using System;
using log4net.Appender;
using log4net.Core;

namespace CodeConverters.Core.Diagnostics
{
    public class NewRelicAgentErrorAppender : AppenderSkeleton
    {
        //https://github.com/niltz/NewRelicAgentAppender/blob/master/NewRelicAgentAppender/NewRelicAgentAppender.cs
        override protected void Append(LoggingEvent loggingEvent)
        {
            var ex = loggingEvent.ExceptionObject;
            if (ex == null)
            {
                if (loggingEvent.MessageObject is Exception)
                    ex = loggingEvent.MessageObject as Exception;
                else if (loggingEvent.Level == Level.Error)
                    ex = new Exception(loggingEvent.RenderedMessage);
            }
            if (ex != null)
                NewRelic.Api.Agent.NewRelic.NoticeError(ex);
        }
    }
}

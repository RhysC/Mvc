using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Repository.Hierarchy;

namespace CodeConverters.Mvc.Diagnostics
{
    public static class Log4NetConfiguration
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Log4NetConfiguration));
       
        /// <summary>
        /// Use <see cref="Log4NetAppenderFactory"/> for default appenders
        /// </summary>
        /// <param name="appenders"></param>
        public static void InitialiseLog4Net(params IAppender[] appenders)
        {
            var hierarchy = (Hierarchy)LogManager.GetRepository();
            hierarchy.Clear();
            foreach (var appender in appenders)
            {
                hierarchy.Root.AddAppender(appender);    
            }
            hierarchy.Root.Level = Level.Debug;
            hierarchy.Configured = true;
            Logger.Info("Log4Net configuration complete!");
        }
    }
}
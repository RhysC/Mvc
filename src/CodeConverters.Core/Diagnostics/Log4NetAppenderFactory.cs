using System.IO;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;

namespace CodeConverters.Core.Diagnostics
{
    public static class Log4NetAppenderFactory
    {
        private static readonly PatternLayout PatternLayout = new PatternLayout("[%-5level][%date][%thread][%logger]: %message%newline");
        private const long TenMb = (10 * 1024 * 1024);
     
        public static IAppender CreateNewRelicAgentAppender()
        {
            return new NewRelicAgentErrorAppender { Threshold = Level.Error };
        }
    
        public static TraceAppender CreateTraceAppender()
        {
            var tracer = new TraceAppender { Layout = PatternLayout };
            tracer.ActivateOptions();
            PatternLayout.ActivateOptions();
            return tracer;
        }

        public static RollingFileAppender CreateRollingFileAppender(string processName, string loggingPath)
        {
            var fileAppender = new RollingFileAppender
            {
                File = Path.Combine(loggingPath, processName + ".log"),
                PreserveLogFileNameExtension = true,
                AppendToFile = true,
                RollingStyle = RollingFileAppender.RollingMode.Composite,
                MaxSizeRollBackups = 31,
                MaxFileSize = TenMb,
                DatePattern = ".yyyy-MM-dd",
                LockingModel = new FileAppender.MinimalLock(),
                StaticLogFileName = false,
                Layout = PatternLayout,
                ImmediateFlush = true
            };
            fileAppender.ActivateOptions();
            return fileAppender;
        }
    }
}
using System.Web.Mvc;
using log4net;

namespace CodeConverters.Mvc.Diagnostics
{
    public class LogAttribute : ActionFilterAttribute, IExceptionFilter
    {
        public bool Enabled { get; set; }

        public LogAttribute()
        {
            Enabled = true;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (!Enabled) return;
            var logEvent = new MvcLogEvent(context);
            var log = LogManager.GetLogger(context.Controller.GetType());
            if (logEvent.IsHttpGet)
            {
                log.Debug(logEvent);
            }
            else
            {
                log.Info(logEvent);
            }
            base.OnActionExecuted(context);
        }

        public void OnException(ExceptionContext context)
        {
            var logErrorEvent = new MvcErrorLogEvent(context);
            var log = LogManager.GetLogger(context.Controller.GetType());
            log.Error(logErrorEvent, context.Exception);
        }
    }
}
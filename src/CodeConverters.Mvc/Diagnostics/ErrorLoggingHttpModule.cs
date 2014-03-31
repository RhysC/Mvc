using System;
using System.Web;
using log4net;

namespace CodeConverters.Mvc.Diagnostics
{
    /// <summary>
    /// Register this module in your web config under the system.webServer -> modules node
    /// </summary>
    public class ErrorLoggingHttpModule : IHttpModule
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ErrorLoggingHttpModule));

        public void Init(HttpApplication context)
        {
            context.Error += On_Error;
        }

        private static void On_Error(object sender, EventArgs e)
        {
            var lastError = HttpContext.Current.Server.GetLastError();
            if (Logger != null) LogError(lastError);
        }

        private static void LogError(Exception lastError)
        {
            if (lastError is HttpException && ((HttpException)lastError).GetHttpCode() == 404)
            {
                Logger.Warn("A 404 occurred", lastError);
            }
            else
            {
                Logger.Error(lastError);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) { }
    }
}
using System.Web.Mvc;
using CodeConverters.Core.Persistence;

namespace CodeConverters.Mvc.Persistence
{
    public class UnitOfWorkAttribute : ActionFilterAttribute, IExceptionFilter
    {
        public IUnitOfWork UnitOfWork { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var httpMethod = filterContext.HttpContext.Request.HttpMethod;
            if (httpMethod == "GET")
            {
                UnitOfWork.NoTracking = true;
            }
            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var httpMethod = filterContext.HttpContext.Request.HttpMethod;
            if (httpMethod == "POST")
            {
                UnitOfWork.Commit();
            }
            UnitOfWork.Dispose();
            base.OnActionExecuted(filterContext);
        }

        public void OnException(ExceptionContext filterContext)
        {
            UnitOfWork.Dispose();
        }
    }
}

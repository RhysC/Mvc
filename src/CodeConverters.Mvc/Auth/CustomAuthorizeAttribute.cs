using System;
using System.Web;
using System.Web.Mvc;

namespace CodeConverters.Mvc.Auth
{
    public class CustomAuthorizeAttribute : FilterAttribute, IAuthorizationFilter
    {
         public object RequiredPermission { get; set; }

        /// <summary>
        /// Originally copied from the System.Web.Mvc.AuthorizeAttribute class with modifications to allow for 
        /// - Redirect on authenticated but unauthorized access
        /// - Permission based Authorization
        /// </summary>
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            if (OutputCacheAttribute.IsChildActionCacheActive(filterContext))
            {
                // NOTES BELOW FROM THE ORIGINAL M$ CODE :
                // If a child action cache block is active, we need to fail immediately, even if authorization
                // would have succeeded. The reason is that there's no way to hook a callback to rerun
                // authorization before the fragment is served from the cache, so we can't guarantee that this
                // filter will be re-run on subsequent requests.
                throw new InvalidOperationException("CannotUseWithinChildActionCache");//MvcResources.AuthorizeAttribute_CannotUseWithinChildActionCache);
            }

            var skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true)
                                    || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true);

            if (skipAuthorization) return;

            if (AuthorizeCore(filterContext.HttpContext))
            {
                var cachePolicy = filterContext.HttpContext.Response.Cache;
                cachePolicy.SetProxyMaxAge(new TimeSpan(0));
                cachePolicy.AddValidationCallback(CacheValidateHandler, null /* data */);
            }
            //Custom Code : Is user logged in?
            else if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                // Redirect to custom Unauthorized page
                filterContext.Result = new RedirectResult("/Error/Unauthorized");
            }
            else
            {
                HandleUnauthorizedRequest(filterContext);
            }
        }

        protected bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException("httpContext");
            return httpContext.User.Can(RequiredPermission);
        }

        private void CacheValidateHandler(HttpContext context, object data, ref HttpValidationStatus validationStatus)
        {
            validationStatus = OnCacheAuthorization(new HttpContextWrapper(context));
        }

        protected virtual void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            // Returns HTTP 401 - see comment in HttpUnauthorizedResult.cs.
            filterContext.Result = new HttpUnauthorizedResult();
        }

        // This method must be thread-safe since it is called by the caching module.
        protected virtual HttpValidationStatus OnCacheAuthorization(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }

            bool isAuthorized = AuthorizeCore(httpContext);
            return (isAuthorized) ? HttpValidationStatus.Valid : HttpValidationStatus.IgnoreThisRequest;
        }
    }
}
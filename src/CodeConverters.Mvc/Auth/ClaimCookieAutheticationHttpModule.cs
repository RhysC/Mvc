using System;
using System.Web;

namespace CodeConverters.Mvc.Auth
{
    /// <summary>
    /// Register this module in your web config.
    /// Inherit from me or register the generic type 
    /// For Registering a Managed-code Module (like this) see http://msdn.microsoft.com/en-us/library/bb763179(v=vs.100).aspx 
    /// See here for generic registetraion : http://stackoverflow.com/questions/4138794/how-can-i-register-generic-class-as-ihttpmodule
    /// </summary>
    public class ClaimCookieAutheticationHttpModule<T> : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.AuthenticateRequest += OnAuthenticateRequest;
        }

        protected virtual void OnAuthenticateRequest(object sender, EventArgs eventArgs)
        {
            var claimsPrincipal = FormAuthCookieManager<T>.GetClaimPrincipal(new HttpRequestWrapper(HttpContext.Current.Request));
            if (claimsPrincipal == null)
                return;
            HttpContext.Current.User = claimsPrincipal;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) { }
    }
}
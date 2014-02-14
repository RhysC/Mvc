using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Security;
using log4net;
using Newtonsoft.Json;

namespace CodeConverters.Mvc.Auth
{
    /// <summary>
    /// Allows you to use your own custom permission type and serilise in secure https cookie. 
    /// Note the permission will be in every connection so large payload will impact request/reponse 
    /// Be s
    /// </summary>
    /// <typeparam name="T">The Permission type your are using. this is a cusm type that can be an enum</typeparam>
    public class FormAuthCookieManager<T>
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(FormAuthCookieManager<>));

        public static HttpCookie CreateFormAuthCookie(string userName, IEnumerable<T> permissions)
        {
            var json = JsonConvert.SerializeObject(permissions, Formatting.None);
            var authTicket = new FormsAuthenticationTicket(1, userName, DateTime.UtcNow, DateTime.UtcNow.AddDays(1), true, json);
            var encTicket = FormsAuthentication.Encrypt(authTicket);
            var faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket)
            {
                Secure = true,
                HttpOnly = true,
                Shareable = false,
                Expires = authTicket.Expiration
            };
            return faCookie;
        }

        public static ClaimsPrincipal GetClaimPrincipal(HttpRequestBase request)
        {
            var authCookie = request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie == null) return null;

            var ticket = FormsAuthentication.Decrypt(authCookie.Value);
            var formsIdentity = new FormsIdentity(ticket);
            var claimsIdentity = new ClaimsIdentity(formsIdentity);

            foreach (var permission in GetPermissions(ticket))
            {
                claimsIdentity.AddClaim(new Claim(CustomClaimTypes.Permission, permission.ToString()));
            }

            return new ClaimsPrincipal(claimsIdentity);
        }

        private static IEnumerable<T> GetPermissions(FormsAuthenticationTicket ticket)
        {
            var payload = ticket.UserData;
            try
            {
                return JsonConvert.DeserializeObject<IEnumerable<T>>(payload);
            }
            catch (JsonSerializationException ex)
            {
                Log.Error(string.Format("Could not get permission from Forms Authentication Ticket. Payload in raw for is : {0}", ticket.UserData), ex);
                return Enumerable.Empty<T>();
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using CodeConverters.Mvc.Auth;

namespace CodeConverters.MvcSamples.Auth
{
    public class FormsAuthenticationSessionManager
    {
        public void Logout()
        {
            FormsAuthentication.SignOut();
        }

        public bool Login(string userName, string password)
        {
            var user = GetUser(userName, password);
            if (user == null)
                return false;
            LogUserIn(user);
            return true;
        }

        private IUser GetUser(string userName, string password)
        {
            //Get user somehow
            throw new NotImplementedException();
        }

        private static void LogUserIn(IUser user)
        {
            var faCookie = FormAuthCookieManager<CustomPermissionType>.CreateFormAuthCookie(user.Username, user.Permissions);
            HttpContext.Current.Response.Cookies.Add(faCookie);
        }

        private interface IUser
        {
            string Username { get; }
            IEnumerable<CustomPermissionType> Permissions { get; set; }
        }
        
    }
}
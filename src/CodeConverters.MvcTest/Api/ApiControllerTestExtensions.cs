using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Web.Http;
using System.Web.Http.Hosting;

namespace CodeConverters.MvcTest.Api
{
    public static class ApiControllerTestExtensions
    {
        public static T SetUser<T>(this T controller, string userName) where T : ApiController
        {
            if (controller.Request == null)
            {
                controller.Request = new HttpRequestMessage
                {
                    Properties = {{HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration()}}
                };
            }
            controller.User = new ClaimsPrincipal(new GenericPrincipal(new GenericIdentity(userName), null));
            return controller;
        }
    }
}
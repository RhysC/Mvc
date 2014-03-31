using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using CodeConverters.Mvc.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace CodeConverters.WebApi.Auth
{
    public class RequiresApiKeyAttribute : AuthorizeAttribute
    {
        private const string ApiKeyHeader = "CC-ApiKey";

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (!actionContext.Request.Headers.Contains(ApiKeyHeader)
             || actionContext.Request.Headers.GetValues(ApiKeyHeader) == null)
            {
                actionContext.Response = CreateUnauthorizedResponse(actionContext);
                return;
            }

            var suppliedApiKey = actionContext.Request.Headers.GetValues(ApiKeyHeader).FirstOrDefault();
            if (suppliedApiKey != LoadExpectedApiKey())
            {
                actionContext.Response = CreateUnauthorizedResponse(actionContext);
            }
        }

        private static string LoadExpectedApiKey()
        {
            RoleConfiguration.ThrowIfUnavailable();
            return RoleEnvironment.GetConfigurationSettingValue("ApiKey");
        }

        private static HttpResponseMessage CreateUnauthorizedResponse(HttpActionContext actionContext)
        {
            return actionContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Invalid or missing ApiKey");
        }
    }
}

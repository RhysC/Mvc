using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NSubstitute;

namespace CodeConverters.MvcTest.Mvc
{
    public static class ControllerSetupExtensions
    {
        public static T SetUser<T>(this T controller, string userName) where T : Controller
        {
            controller.EnsureControllerTestContext();
            var principal = new ClaimsPrincipal(new GenericPrincipal(new GenericIdentity(userName), null));
            controller.ControllerContext.HttpContext.User.Returns(principal);
            return controller;
        }

        public static T SetCurrentUrl<T>(this T controller, string currentUrl = "http://localhost:123") where T : Controller
        {
            controller.EnsureControllerTestContext();
            controller.ControllerContext.HttpContext.Request.Url.Returns(new Uri(currentUrl));
            var requestContext = new RequestContext(controller.ControllerContext.HttpContext, controller.ControllerContext.RouteData);
            controller.Url = new UrlHelper(requestContext);
            return controller;
        }

        private static void EnsureControllerTestContext<T>(this T controller) where T : Controller
        {
            if (controller.ControllerContext != null) return;
            var currentHttpContext = Substitute.For<HttpContextBase>();
            currentHttpContext.Request.Returns(Substitute.For<HttpRequestBase>());
            controller.ControllerContext = new ControllerContext(currentHttpContext, new RouteData(), controller);
        }
    }
}

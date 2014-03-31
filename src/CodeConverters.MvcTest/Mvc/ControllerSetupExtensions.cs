using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;

namespace CodeConverters.MvcTest.Mvc
{
    public static class ControllerSetupExtensions
    {
        public static T SetUser<T>(this T controller, string userName) where T : Controller
        {
            controller.EnsureControllerTestContext();
            var principal = new ClaimsPrincipal(new GenericPrincipal(new GenericIdentity(userName), null));
            Mock.Get(controller.ControllerContext.HttpContext).SetupGet(r => r.User).Returns(principal);
            return controller;
        }

        public static T SetCurrentUrl<T>(this T controller, string currentUrl = "http://localhost:123") where T : Controller
        {
            controller.EnsureControllerTestContext();
            Mock.Get(controller.ControllerContext.HttpContext.Request).SetupGet(r => r.Url).Returns(new Uri(currentUrl));
            var requestContext = new RequestContext(controller.ControllerContext.HttpContext, controller.ControllerContext.RouteData);
            controller.Url = new UrlHelper(requestContext);
            return controller;
        }

        private static void EnsureControllerTestContext<T>(this T controller) where T : Controller
        {
            if (controller.ControllerContext != null) return;
            var currentHttpContext = new Mock<HttpContextBase> { DefaultValue = DefaultValue.Mock };
            currentHttpContext.SetupAllProperties();
            controller.ControllerContext = new ControllerContext(currentHttpContext.Object, new RouteData(), controller);
        }
    }
}

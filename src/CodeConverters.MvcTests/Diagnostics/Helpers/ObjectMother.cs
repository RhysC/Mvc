using System.Collections.Specialized;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;

namespace CodeConverters.MvcTests.Diagnostics.Helpers
{
    public static class ObjectMother
    {
        internal static ExceptionContext CreateExceptionContextFake()
        {
            var expectedHeaders = new NameValueCollection { { "header1", "value1" }, { "header2", "value2" }, { "secret", "IAmBatman" } };
            var expectedFormData = new NameValueCollection { { "Form1", "valueA" }, { "form2", "valueB" }, { "password", "123qwe" } };

            return Mock.Of<ExceptionContext>(ctx =>
                ctx.HttpContext.Request.RawUrl == "http://mytesturl.com/mycontroller/myaction/123" &&
                ctx.HttpContext.Request.HttpMethod == "POST" &&
                ctx.HttpContext.Request.Headers == expectedHeaders &&
                ctx.HttpContext.Request.Form == expectedFormData &&
                ctx.Controller == new DummyController());
        }
        public static ActionExecutedContext CreateActionActionExecutedContextFake()
        {
            var routeData = new RouteData();
            routeData.Values.Add("id", "123");

            return Mock.Of<ActionExecutedContext>(ctx =>
                ctx.HttpContext.Request.RawUrl == "http://mytesturl.com/mycontroller/myaction/123" &&
                ctx.ActionDescriptor.ControllerDescriptor.ControllerName == "mycontroller" &&
                ctx.ActionDescriptor.ActionName == "myaction" &&
                ctx.HttpContext.Request.HttpMethod == "GET" &&
                ctx.HttpContext.Request.Headers == new NameValueCollection { { "header1", "value1" }, { "header2", "value2" }, { "secret", "IAmBatman" } } &&
                ctx.HttpContext.Request.Form == new NameValueCollection { { "Form1", "valueA" }, { "form2", "valueB" }, { "password", "123qwe" } } &&
                ctx.RouteData == routeData &&
                ctx.Controller == new DummyController());
        }
    }
}
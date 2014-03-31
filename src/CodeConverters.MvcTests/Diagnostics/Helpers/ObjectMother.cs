using System.Collections.Specialized;
using System.Web;
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
            //var actionExecutedContext = new Mock<ActionExecutedContext>();
            //actionExecutedContext.SetupGet(a => a.HttpContext).Returns(CreateHttpContextMock().Object);
            //actionExecutedContext.SetupGet(a => a.ActionDescriptor).Returns(CreateActionDescriptorMock().Object);

            //var routeData = new RouteData();
            //routeData.Values.Add("id", "123");
            //actionExecutedContext.SetupGet(a => a.RouteData).Returns(routeData);

            //actionExecutedContext.SetupGet(a => a.Controller).Returns(new DummyController());

            //return actionExecutedContext.Object;

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

        private static Mock<ActionDescriptor> CreateActionDescriptorMock()
        {
            var actionDescriptor = new Mock<ActionDescriptor>();
            actionDescriptor.Setup(a => a.ControllerDescriptor.ControllerName).Returns("mycontroller");
            actionDescriptor.Setup(a => a.ActionName).Returns("myaction");
            return actionDescriptor;
        }

        private static Mock<HttpContextBase> CreateHttpContextMock()
        {
            var httpContext = new Mock<HttpContextBase>();
            httpContext.Setup(c => c.Request.RawUrl).Returns("http://mytesturl.com/mycontroller/myaction/123");
            httpContext.Setup(c => c.Request.HttpMethod).Returns("GET");
            httpContext.Setup(c => c.Request.Headers).Returns(new NameValueCollection
            {
                {"header1", "value1"},
                {"header2", "value2"},
                {"secret", "IAmBatman"}
            });
            httpContext.Setup(c => c.Request.Form)
                .Returns(new NameValueCollection { { "Form1", "valueA" }, { "form2", "valueB" }, { "password", "123qwe" } });
            return httpContext;
        }
    }
}
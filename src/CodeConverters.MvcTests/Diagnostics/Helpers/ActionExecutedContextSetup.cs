using System.Collections.Specialized;
using System.Web.Mvc;
using System.Web.Routing;
using NSubstitute;

namespace CodeConverters.MvcTests.Diagnostics
{
    public class ActionExecutedContextSetup
    {
        public ActionExecutedContext CreateActionActionExecutedContextFake()
        {
            var routeData = new RouteData();
            routeData.Values.Add("id", "123");

            var context = Substitute.For<ActionExecutedContext>();
            context.RequestContext.HttpContext.Request.RawUrl.Returns("http://mytesturl.com/mycontroller/myaction/123");
            context.ActionDescriptor.ControllerDescriptor.ControllerName.Returns("mycontroller");
            context.ActionDescriptor.ActionName.Returns("myaction");
            context.HttpContext.Request.HttpMethod.Returns("GET");
            context.HttpContext.Request.Headers.Returns(new NameValueCollection { { "header1", "value1" }, { "header2", "value2" }, { "secret", "IAmBatman" } });
            context.HttpContext.Request.Form.Returns(new NameValueCollection { { "Form1", "valueA" }, { "form2", "valueB" }, { "password", "123qwe" } });
            context.RouteData.Returns(routeData);
            context.Controller.Returns(new DummyController());

            return context;
        }
    }
}
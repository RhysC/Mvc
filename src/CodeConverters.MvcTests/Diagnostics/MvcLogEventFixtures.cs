using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using CodeConverters.Mvc.Diagnostics;
using CodeConverters.MvcTests.Diagnostics.Helpers;
using Xunit;
using NSubstitute;

namespace CodeConverters.MvcTests.Diagnostics
{
    public class MvcLogEventFixtures
    {
        private readonly MvcLogEvent _sut;
        private readonly string _expectedUrl;
        private readonly string _expectedControllerName;
        private readonly string _expectedActionName;
        private readonly ActionExecutedContext _context;

        public MvcLogEventFixtures()
        {
            _expectedUrl = "http://mytesturl.com/mycontroller/myaction/123";
            _expectedControllerName = "mycontroller";
            _expectedActionName = "myaction";

            var expectedHeaders = new NameValueCollection { { "header1", "value1" }, { "header2", "value2" }, { "secret", "IAmBatman" } };
            var expectedFormData = new NameValueCollection { { "Form1", "valueA" }, { "form2", "valueB" }, { "password", "123qwe" } };
            var routeData = new RouteData();
            routeData.Values.Add("id", "123");

            _context = Substitute.For<ActionExecutedContext>();
            _context.RequestContext.HttpContext.Request.RawUrl.Returns(_expectedUrl);
            _context.ActionDescriptor.ControllerDescriptor.ControllerName.Returns(_expectedControllerName);
            _context.ActionDescriptor.ActionName.Returns(_expectedActionName);
            _context.HttpContext.Request.HttpMethod.Returns("GET");
            _context.HttpContext.Request.Headers.Returns(expectedHeaders);
            _context.HttpContext.Request.Form.Returns(expectedFormData);
            _context.RouteData.Returns(routeData);

            _sut = new MvcLogEvent(_context);
        }
        [Fact]
        public void LogsUrl()
        {
            var url = _sut.ToString().Split('|').GetLogValue("URL");
            Assert.Equal(_expectedUrl, url);
        }
        [Fact]
        public void LogsControllerActionAndRouteId()
        {
            var logEntries = _sut.ToString().Split('|');
            var controller = logEntries.GetLogValue("Controller");
            var action = logEntries.GetLogValue("Action");
            var id = logEntries.GetLogValue("RouteId");
            Assert.Equal(_expectedControllerName, controller);
            Assert.Equal(_expectedActionName, action);
            Assert.Equal("123", id);
        }
        [Fact]
        public void LogsScrubbedHeaders()
        {
            var headers = _sut.ToString().Split('|').GetLogValue("Headers").Split(',').ToDictionary(k => k.Split(':')[0], v => v.Split(':')[1]);
            Assert.Equal(headers, new Dictionary<string, string> { { "header1", "value1" }, { "header2", "value2" }, { "secret", "*********" } });
        }
        [Fact]
        public void LogsScrubbedFormData()
        {
            var formData = _sut.ToString().Split('|').GetLogValue("FormData").Split(',').ToDictionary(k => k.Split(':')[0], v => v.Split(':')[1]);
            Assert.Equal(formData, new Dictionary<string, string> { { "Form1", "valueA" }, { "form2", "valueB" }, { "password", "******" } });
        }
        [Fact]
        public void IsGetIsTrueOnHttpGet()
        {
            Assert.True(_sut.IsHttpGet);
            _context.HttpContext.Request.HttpMethod.Returns("POST");
            Assert.False(new MvcLogEvent(_context).IsHttpGet);
        }
    }
}

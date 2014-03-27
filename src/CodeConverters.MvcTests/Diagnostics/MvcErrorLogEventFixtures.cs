using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Mvc;
using CodeConverters.Mvc.Diagnostics;
using NSubstitute;
using Xunit;

namespace CodeConverters.MvcTests.Diagnostics
{
    public class MvcErrorLogEventFixtures
    {
        private readonly MvcErrorLogEvent _sut;
        private const string ExpectedUrl = "http://mytesturl.com/mycontroller/myaction/123";

        public MvcErrorLogEventFixtures()
        {
            var context = Substitute.For<ExceptionContext>();
            
            var expectedHeaders = new NameValueCollection { { "header1", "value1" }, { "header2", "value2" }, { "secret", "IAmBatman" } };
            var expectedFormData = new NameValueCollection { { "Form1", "valueA" }, { "form2", "valueB" }, { "password", "123qwe" } };
          

            context.RequestContext.HttpContext.Request.RawUrl.Returns(ExpectedUrl);
            context.HttpContext.Request.HttpMethod.Returns("POST");
            context.HttpContext.Request.Headers.Returns(expectedHeaders);
            context.HttpContext.Request.Form.Returns(expectedFormData);
          
            _sut = new MvcErrorLogEvent(context);
        }
        [Fact]
        public void LogsUrl()
        {
            var url = _sut.ToString().Split('|').GetLogValue("URL");
            Assert.Equal(ExpectedUrl, url);
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
    }
}
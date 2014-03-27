using System.Collections.Specialized;
using System.Linq;
using System.Web.Mvc;
using CodeConverters.Mvc.Diagnostics;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using NSubstitute;
using Xunit;

namespace CodeConverters.MvcTests.Diagnostics
{
    public class LogAttributeOnExceptionFixture
    {
        private readonly MemoryAppender _memoryAppender;

        public LogAttributeOnExceptionFixture()
        {
            _memoryAppender = new MemoryAppender();
            BasicConfigurator.Configure(_memoryAppender);
            var sut = new LogAttribute();
            var context = CreateExceptionContextFake();
            sut.OnException(context);
        }

        [Fact]
        public void ErrorsAreLoggedAtErrorLevel()
        {
            Assert.True(_memoryAppender.GetEvents().Count() == 1, "Expected single messages in the logs");
            Assert.True(_memoryAppender.GetEvents().Count(le => le.Level == Level.Error) == 1, "Expected single Error messages in the logs");
        }

        [Fact]
        public void UsesTheGivenControllerAsTheLoggerName()
        {
            Assert.True(_memoryAppender.GetEvents().All(e => e.LoggerName == typeof(DummyController).FullName), "Expecting logger name to be that of the controller type");
        }

        private static ExceptionContext CreateExceptionContextFake()
        {
            var context = Substitute.For<ExceptionContext>();

            var expectedHeaders = new NameValueCollection { { "header1", "value1" }, { "header2", "value2" }, { "secret", "IAmBatman" } };
            var expectedFormData = new NameValueCollection { { "Form1", "valueA" }, { "form2", "valueB" }, { "password", "123qwe" } };

            context.RequestContext.HttpContext.Request.RawUrl.Returns("http://mytesturl.com/mycontroller/myaction/123");
            context.HttpContext.Request.HttpMethod.Returns("POST");
            context.HttpContext.Request.Headers.Returns(expectedHeaders);
            context.HttpContext.Request.Form.Returns(expectedFormData);
            context.Controller.Returns(new DummyController());
            return context;
        }
    }
}
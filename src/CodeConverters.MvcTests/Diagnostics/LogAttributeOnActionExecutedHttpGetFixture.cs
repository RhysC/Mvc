using System.Linq;
using System.Web.Mvc;
using CodeConverters.Mvc.Diagnostics;
using CodeConverters.MvcTests.Diagnostics.Helpers;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using NSubstitute;
using Xunit;

namespace CodeConverters.MvcTests.Diagnostics
{
    public class LogAttributeOnActionExecutedHttpGetFixture : IUseFixture<ActionExecutedContextSetup>
    {
        private readonly LogAttribute _sut;
        private readonly MemoryAppender _memoryAppender;
        private ActionExecutedContext _context;

        public LogAttributeOnActionExecutedHttpGetFixture()
        {
            _memoryAppender = new MemoryAppender();
            BasicConfigurator.Configure(_memoryAppender);
            _sut = new LogAttribute();
        }

        public void SetFixture(ActionExecutedContextSetup data)
        {
            _context = data.CreateActionActionExecutedContextFake();
            _context.HttpContext.Request.HttpMethod.Returns("GET");
        }

        [Fact]
        public void UsesTheGivenControllerAsTheLoggerName()
        {
            _sut.OnActionExecuted(_context);

            Assert.True(_memoryAppender.GetEvents().All(e => e.LoggerName == typeof(DummyController).FullName), "Expecting logger name to be that of the controller type");
        }
        [Fact]
        public void GetsAreLoggedAtDebugLevel()
        {
            _sut.OnActionExecuted(_context);

            Assert.True(_memoryAppender.GetEvents().Count() == 1, "Expected single messages in the logs");
            Assert.True(_memoryAppender.GetEvents().Count(le => le.Level == Level.Debug) == 1, "Expected single debug messages in the logs");
        }

        [Fact]
        public void GetsAreNotLoggedIfFilterDisabled()
        {
            _sut.Enabled = false;
            _sut.OnActionExecuted(_context);

            Assert.False(_memoryAppender.GetEvents().Any(), "Expected no messages in the logs");
        }
    }
}

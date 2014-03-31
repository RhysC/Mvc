using System.Linq;
using CodeConverters.Mvc.Diagnostics;
using CodeConverters.MvcTests.Diagnostics.Helpers;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
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
            var context = ObjectMother.CreateExceptionContextFake();
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
    }
}
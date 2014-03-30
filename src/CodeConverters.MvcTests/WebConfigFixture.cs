using CodeConverters.Mvc.Diagnostics;
using CodeConverters.MvcTest;
using Xunit;

namespace CodeConverters.MvcTests
{
    public class WebConfigFixture
    {
        private readonly WebConfig _sut;

        public WebConfigFixture()
        {
            //Web configed is copied as a linked file to this test project. 
            _sut = new WebConfig();
        }
        [Fact]
        public void DebugIsNotSetToTrue()
        {
            Assert.False(_sut.IsDebugSet());
        }
        [Fact]
        public void CookiesAreSecureAndHttpOnly()
        {
            Assert.True(_sut.AllowOnlyCookiesOverSsl());
            Assert.True(_sut.AllowOnlyHttpCookies());
        }
        [Fact]
        public void HasFrameworkVersion451()
        {
            Assert.Equal("4.5.1", _sut.GetFrameworkVersion());
        }
        [Fact]
        public void HasErrorLoggingModuleRegistered()
        {
            Assert.True(_sut.HasHttpModuleRegistered<ErrorLoggingHttpModule>());
        }
    }
}

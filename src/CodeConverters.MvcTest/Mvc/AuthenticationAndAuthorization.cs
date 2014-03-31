using System.Linq;
using System.Security.Principal;
using Moq;

namespace CodeConverters.MvcTest.Mvc
{
    public class AuthenticationAndAuthorization
    {
        public static IPrincipal GetFakePrinciple(string[] roles)
        {
            return GetFakePrinciple(true, roles);
        }

        public static IPrincipal GetFakePrinciple(bool isLoggedIn, string[] roles = null)
        {
            if (roles == null) roles = new string[] { };
            var mock = new Mock<IPrincipal>();

            var fakeIdentity = GetFakeIdentity(isLoggedIn);
            mock.SetupGet(m => m.Identity).Returns(fakeIdentity);
            mock.Setup(m => m.IsInRole(It.Is<string>(x => roles.Contains(x)))).Returns(true);
            return mock.Object;
        }

        public static IIdentity GetFakeIdentity(bool isLoggedIn)
        {
            var mock = new Mock<IIdentity>();
            mock.SetupGet(m => m.AuthenticationType).Returns(isLoggedIn ? "Mock Identity" : null);
            mock.SetupGet(m => m.IsAuthenticated).Returns(isLoggedIn);
            mock.SetupGet(m => m.Name).Returns(isLoggedIn ? "testuser" : null);
            return mock.Object;
        }
    }
}
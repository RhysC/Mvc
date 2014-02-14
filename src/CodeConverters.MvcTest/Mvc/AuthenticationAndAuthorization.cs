using System.Linq;
using System.Security.Principal;
using NSubstitute;

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
            var mock = Substitute.For<IPrincipal>();

            var fakeIdentity = GetFakeIdentity(isLoggedIn);
            mock.Identity.Returns(fakeIdentity);
            mock.IsInRole(Arg.Is<string>(x => roles.Contains(x)))
                .Returns(true);
            return mock;
        }

        public static IIdentity GetFakeIdentity(bool isLoggedIn)
        {
            var mock = Substitute.For<IIdentity>();

            mock.AuthenticationType.Returns(isLoggedIn ? "Mock Identity" : null);
            mock.IsAuthenticated.Returns(isLoggedIn);
            mock.Name.Returns(isLoggedIn ? "testuser" : null);
            return mock;
        }
    }
}
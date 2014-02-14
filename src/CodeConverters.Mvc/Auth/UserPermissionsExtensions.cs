using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace CodeConverters.Mvc.Auth
{
    public static class UserPermissionsExtensions
    {
        public static bool HasAnyPermissions<T>(this IPrincipal principal, T[] requiredPermission)
        {
            var user = principal as ClaimsPrincipal;
            if (user == null || !user.Identity.IsAuthenticated)
            {
                return false;
            }
            var requiredPermissionStr = requiredPermission.Select(x => x.ToString());
            var userPermissions = user.Claims
                .Where(c => c.Type == CustomClaimTypes.Permission)
                .Select(c => c.Value);
            return userPermissions.Any(requiredPermissionStr.Contains);
        }

        public static bool Can<T>(this IPrincipal principal, T requiredPermission)
        {
            var user = principal as ClaimsPrincipal;
            if (user == null || !user.Identity.IsAuthenticated)
            {
                return false;
            }
            var userPermissions = user.Claims
                .Where(c => c.Type == CustomClaimTypes.Permission)
                .Select(c => c.Value);
            return userPermissions.Contains(requiredPermission.ToString());
        }
    }
}
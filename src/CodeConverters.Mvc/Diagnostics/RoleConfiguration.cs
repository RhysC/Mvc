using System;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace CodeConverters.Mvc.Diagnostics
{
    public static class RoleConfiguration
    {
        public static void ThrowIfUnavailable()
        {
            if (!RoleEnvironment.IsAvailable || RoleEnvironment.CurrentRoleInstance == null)
                throw new InvalidOperationException("Must be running in azure to access role configuration");
        }
    }
}
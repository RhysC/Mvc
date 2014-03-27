using System;
using System.Linq;

namespace CodeConverters.MvcTests.Diagnostics
{
    public static class LogEntryExtensions
    {
        public static string GetLogValue(this string[] logentries, string key)
        {
            return logentries.Single(s => s.StartsWith(key, StringComparison.CurrentCultureIgnoreCase))
                .Split('=')
                .Skip(1)
                .First();
        }
    }
}
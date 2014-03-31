using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace CodeConverters.MvcTest
{
    public class WebConfig
    {
        private readonly XDocument _doc;
        private const string WebConfigFileName = "web.config";

        /// <summary>
        /// Test wrapper for web config
        /// </summary>
        /// <param name="subfolderName">the sub folder name if the web config is copied to a child folder (e.g. you are testing multiple web configs in one test project)</param>
        public WebConfig(string subfolderName = null)
        {
            var filePath = string.Format("{0}{1}", subfolderName, WebConfigFileName);
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(string.Format("Could not find a web.config file in the following path {0}. Please make sure the web.config is added to the test project as a linked file", filePath));
            }
            var config = File.ReadAllLines(filePath);
            _doc = XDocument.Parse(string.Join(Environment.NewLine, config));
        }

        public bool IsDebugSet()
        {
            var debugValue = _doc.GetNode("compilation").GetAttributeValue("debug");
            bool isdebugEnabled;
            return Boolean.TryParse(debugValue, out isdebugEnabled) && isdebugEnabled;
        }
        public string GetFrameworkVersion()
        {
            return _doc.GetNode("compilation").GetAttributeValue("targetFramework");
        }

        public bool HasHttpModuleRegistered<T>() where T : IHttpModule
        {
            var httpModules = _doc.GetNode("system.webServer", "modules")
                .Descendants("add");
            var assemblyQualifiedName = typeof(T).AssemblyQualifiedName;
            return httpModules.Any(d => assemblyQualifiedName.StartsWith(d.Attribute("type").Value));
        }

        /// <summary>
        /// Check to enure client side script can not read the cookies
        /// </summary>
        /// <returns></returns>
        public bool AllowOnlyHttpCookies()
        {
            return _doc.GetNode("system.web", "httpCookies")
                .IsAttributeTrue("httpOnlyCookies");
        }
        /// <summary>
        /// Check to ensure the cookies are only transported on SSL connections. Note : This usually requires your site to be SSL only or you will have pages that will not be able to access cookies
        /// </summary>
        /// <returns></returns>
        public bool AllowOnlyCookiesOverSsl()
        {
            return _doc.GetNode("system.web", "httpCookies")
                .IsAttributeTrue("requireSSL");
        }

        public string GetAppSetting(string key)
        {
            return _doc.Descendants("appSettings")
                .Descendants("add")
                .Where(a => a.Attribute("key").Value == key)
                .Select(a => a.Attribute("value").Value)
                .FirstOrDefault();
        }
    }
    public static class ConfigExtensions
    {
        public static XElement GetNode(this XDocument doc, params string[] xNames)
        {
            if (!xNames.Any())
                return null;
            var currentNode = doc.Descendants(xNames.First()).FirstOrDefault();

            foreach (var xName in xNames.Skip(1))
            {
                if (currentNode == null)
                    return null;
                currentNode = currentNode.Descendants(xName).FirstOrDefault();
            }
            return currentNode;
        }

        public static bool IsAttributeTrue(this XElement xElement, string attributeName)
        {
            if (xElement == null)
                return false;
            return string.Equals(xElement.Attribute(attributeName).Value,
                true.ToString(),
                StringComparison.CurrentCultureIgnoreCase);
        }
        public static string GetAttributeValue(this XElement xElement, string attributeName)
        {
            if (xElement == null)
                return null;
            var attribute = xElement.Attribute(attributeName);
            if (attribute == null)
                return null;
            return attribute.Value;
        }
    }
}

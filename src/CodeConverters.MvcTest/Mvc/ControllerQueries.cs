using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace CodeConverters.MvcTest.Mvc
{
    public static class ControllerQueries
    {
        public static IEnumerable<MethodInfo> GetAllPostMethodsThatDoNotValidateAntiForgery(Assembly assembly)
        {
            var controllerTypes = assembly.GetTypes().Where(t => typeof(IController).IsAssignableFrom(t));
            var postActions =
                controllerTypes.SelectMany(
                    t => t.GetMethods().Where(m => m.GetCustomAttributes<HttpPostAttribute>().Any()));
            return postActions.Where(a => !a.GetCustomAttributes<ValidateAntiForgeryTokenAttribute>().Any());
        }

        /// <summary>
        /// pass in the action you use to gloabal register your filetes eg return ControllerQueries.GloballyRegisteredFilters(App_Start.MvcConfig.RegisterGlobalFilters);
        /// </summary>
        /// <param name="registerGlobalFilters"></param>
        /// <returns></returns>
        public static IEnumerable<Filter> GloballyRegisteredFilters(Action<GlobalFilterCollection> registerGlobalFilters)
        {
            var filters = new GlobalFilterCollection();
            registerGlobalFilters(filters);
            return filters;
        }

    }
}

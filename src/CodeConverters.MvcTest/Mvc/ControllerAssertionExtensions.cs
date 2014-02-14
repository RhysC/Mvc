using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using Xunit;

namespace CodeConverters.MvcTest.Mvc
{
    public static class ControllerAssertionExtensions
    {
        public static TAttribute Has<TAttribute>(this Controller controller) where TAttribute : Attribute
        {
            var attribute = controller.GetType().GetCustomAttributes<TAttribute>().Single();
            Assert.NotNull(attribute);
            return attribute;
        }

        public static TAttribute With<TAttribute>(this TAttribute attribute, Func<TAttribute, bool> predicate) where TAttribute : Attribute
        {
            Assert.True(predicate(attribute));
            return attribute;
        }
        public static MethodInfo Action<TController>(this TController controller, Expression<Func<TController, ActionResult>> expression)
        {
            var methodCall = expression.Body as MethodCallExpression;
            if (methodCall == null) throw new InvalidOperationException("Expression body is expected to be a MethodCallExpression. This is design for Action on Controllers which must be methods.");
            return methodCall.Method;
        }
    }
}
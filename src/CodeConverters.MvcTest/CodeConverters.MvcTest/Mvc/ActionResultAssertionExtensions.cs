using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using ExpectedObjects;
using Xunit;

namespace CodeConverters.MvcTest.Mvc
{
    public static class ActionResultAssertionExtensions
    {
        public static T Has<T>(this MethodInfo methodInfo) where T : Attribute
        {
            var att = methodInfo.GetCustomAttributes(typeof(T), true).Single();
            Assert.NotNull(att);
            return att as T;
        }

        public static TResult As<TResult>(this ActionResult actionResult) where TResult : ActionResult
        {
            Assert.IsAssignableFrom<TResult>(actionResult);
            return actionResult as TResult;
        }
        public static TResult HasViewName<TResult>(this TResult viewResult, string viewName) where TResult : ViewResult
        {
            Assert.Equal(viewName, viewResult.ViewName);
            return viewResult;
        }
        public static TResult HasViewModel<TResult>(this TResult viewResult, object viewModel) where TResult : ViewResult
        {
            if (viewModel is ExpectedObject)
            {
                viewResult.Model.ShouldMatch(viewModel as ExpectedObject);
                return viewResult;
            }
            Assert.Equal(viewModel, viewResult.Model);
            return viewResult;
        }
        public static TResult HasViewBagProperty<TResult>(this TResult viewResult, string viewBagPropertyName, object expectedPropertyValue) where TResult : ViewResult
        {
            return viewResult.HasViewDataProperty(viewBagPropertyName, expectedPropertyValue);//Viewdata is just the string dict of the view bag
        }
        public static TResult HasViewDataProperty<TResult>(this TResult viewResult, string viewBagPropertyName, object expectedPropertyValue) where TResult : ViewResult
        {
            if (expectedPropertyValue is ExpectedObject)
            {
                viewResult.ViewData[viewBagPropertyName].ShouldMatch(expectedPropertyValue as ExpectedObject);
                return viewResult;
            }

            Assert.Equal(expectedPropertyValue, viewResult.ViewData[viewBagPropertyName]);
            return viewResult;
        }

        public static TResult HasModelStateErrorMessage<TResult>(this TResult viewResult, string modelStateErrorKey, object expectedErrorMessage) where TResult : ViewResult
        {
            Assert.Equal(expectedErrorMessage, viewResult.ViewData.ModelState.Single(msd => msd.Key == modelStateErrorKey).Value.Errors.Single().ErrorMessage);
            return viewResult;
        }

        public static TResult HasRedirectUrl<TResult>(this TResult redirectResult, string expectedUrl) where TResult : RedirectResult
        {
            Assert.Equal(expectedUrl, redirectResult.Url);
            return redirectResult;
        }

        public static TResult IsPermanent<TResult>(this TResult redirectResult) where TResult : RedirectResult
        {
            Assert.True(redirectResult.Permanent);
            return redirectResult;
        }

        public static TResult HasActionName<TResult>(this TResult redirectResult, string expectedName) where TResult : RedirectToRouteResult
        {
            Assert.Equal(expectedName, redirectResult.RouteValues["action"]);
            return redirectResult;
        }
        public static TResult HasRouteValues<TResult>(this TResult redirectResult, object expectedValues) where TResult : RedirectToRouteResult
        {
            //Assume this is an Anon object
            var properties = expectedValues.GetType().GetProperties();
            foreach (var propertyInfo in properties)
            {
                var expectedValue = propertyInfo.GetValue(expectedValues, null);
                Assert.Equal(expectedValue, redirectResult.RouteValues[propertyInfo.Name]);
            }
            return redirectResult;
        }

    }
}
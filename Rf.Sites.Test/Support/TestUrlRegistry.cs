using System;
using System.Linq.Expressions;
using System.Reflection;
using FubuMVC.Core.Registration.Routes;
using FubuMVC.Core.Urls;
using Rf.Sites.Features.Models;

namespace Rf.Sites.Test.Support
{
    internal class TestUrlRegistry : IUrlRegistry 
    {
        public string UrlFor(object model, string categoryOrHttpMethod = null)
        {
            if (model as ContentId != null)
                return "/go/" + ((ContentId) model).Id;
            if (model as TagPaging != null)
                return "/tag/" + ((TagPaging)model).Tag;
            if (model as YearPaging != null)
                return "/year/" + ((YearPaging)model).Year;
            throw new ArgumentException("Cannot deal with " + model, "model");
        }
        # region dont care
        public string UrlFor<T>(string categoryOrHttpMethod = null) where T : class
        {
            throw new NotImplementedException();
        }

        public string UrlFor(Type handlerType, MethodInfo method = null, string categoryOrHttpMethodOrHttpMethod = null)
        {
            throw new NotImplementedException();
        }

        public string UrlFor<TController>(Expression<Action<TController>> expression, string categoryOrHttpMethod = null)
        {
            throw new NotImplementedException();
        }

        public string UrlForNew(Type entityType)
        {
            throw new NotImplementedException();
        }

        public bool HasNewUrl(Type type)
        {
            throw new NotImplementedException();
        }

        public string TemplateFor(object model, string categoryOrHttpMethod = null)
        {
            throw new NotImplementedException();
        }

        public string TemplateFor<TModel>(params Func<object, object>[] hash) where TModel : class, new()
        {
            throw new NotImplementedException();
        }

        public string UrlFor(Type modelType, RouteParameters parameters, string categoryOrHttpMethod = null)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
using System;
using FubuCore.Binding;
using FubuMVC.Core.Runtime;
using System.Linq;

namespace Rf.Sites.Frame.SiteInfrastructure
{
    public class DynamicBinding : IModelBinder
    {
        public bool Matches(Type type)
        {
            return type == typeof(object);
        }

        public void Bind(Type type, object instance, IBindingContext context)
        {

        }

        public object Bind(Type type, IBindingContext context)
        {
            var rd = context.Service<IRequestData>();
            var cType = (string)rd.Value("Content-Type");

            if (!new[] { "application/json", "application/jsonrequest", "application/x-javascript" }.Any(ct => ct.Equals(cType)))
                throw new DynamicBindException("Content-Type");
            var data = context.Service<IStreamingData>().InputText();
            try
            {
                return DynamicJson.Parse(data);
            }
            catch
            {
                // DynamicJson explodes when parsing faulty json, and the exception is rather cryptic, so we just say it didn't work
                throw new DynamicBindException("Parse");
            }
        }
    }

    public class DynamicBindException : Exception
    {
        public DynamicBindException(string message) : base(message)
        {
        }
    }
}
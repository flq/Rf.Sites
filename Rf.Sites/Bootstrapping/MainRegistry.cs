using System;
using System.Web;
using FubuCore.Binding;
using Rf.Sites.Features.Searching;
using Rf.Sites.Frame;
using Rf.Sites.Frame.SiteInfrastructure;
using StructureMap.Configuration.DSL;

namespace Rf.Sites.Bootstrapping
{
    public class MainRegistry : Registry
    {
        public MainRegistry()
        {
            ForSingletonOf<ICache>().Use<WebBasedCache>();

            For<IBindingLogger>().Use<NulloBindingLogger>();

            For<ServerVariables>().Use(ctx => new ServerVariables(ctx.GetInstance<HttpContextBase>().Request.ServerVariables));
            Scan(s =>
                     {
                         s.TheCallingAssembly();
                         s.AddAllTypesOf<ISearchPlugin>();
                         s.ConnectImplementationsToTypesClosing(typeof(IObjectConverter<,>));
                     });
        }
    }
}
using System.Web;
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

            For<ServerVariables>().Use(ctx => new ServerVariables(ctx.GetInstance<HttpContextBase>().Request.ServerVariables));
            For<RequestHeaders>().Use(ctx => new RequestHeaders(ctx.GetInstance<HttpContextBase>().Request.Headers));

            Scan(s =>
                     {
                         s.TheCallingAssembly();
                         s.AddAllTypesOf<ISearchPlugin>();
                         s.ConnectImplementationsToTypesClosing(typeof(IObjectConverter<,>));
                     });
        }
    }
}
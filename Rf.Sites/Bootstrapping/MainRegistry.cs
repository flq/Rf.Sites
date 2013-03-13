using System.Web;
using Bottles;
using NHibernate;
using Rf.Sites.Features.Searching;
using Rf.Sites.Frame;
using Rf.Sites.Frame.Persistence;
using Rf.Sites.Frame.SiteInfrastructure;
using StructureMap.Configuration.DSL;
using WebBackgrounder;

namespace Rf.Sites.Bootstrapping
{
    public class MainRegistry : Registry
    {
        public MainRegistry()
        {
            ForSingletonOf<ICache>().Use<WebBasedCache>();

            For<ServerVariables>().Use(ctx => new ServerVariables(ctx.GetInstance<HttpContextBase>().Request.ServerVariables));
            For<RequestHeaders>().Use(ctx => new RequestHeaders(ctx.GetInstance<HttpContextBase>().Request.Headers));

            var maker = new SessionFactoryMaker();
            ForSingletonOf<ISessionFactory>().Use(maker.CreateFactory);

            For<ISession>()
                .HybridHttpOrThreadLocalScoped()
                .Use(ctx => ctx.GetInstance<ISessionFactory>().OpenSession());

            For<IStatelessSession>()
                .HybridHttpOrThreadLocalScoped()
                .Use(ctx => ctx.GetInstance<ISessionFactory>().OpenStatelessSession());

            For(typeof(IRepository<>)).Use(typeof(Repository<>));

            ForSingletonOf<IActivator>().Use<JobsBootstrapper>();

            Scan(s =>
                     {
                         s.TheCallingAssembly();
                         s.AddAllTypesOf<ISearchPlugin>();
                         s.AddAllTypesOf<IJob>();
                         s.ConnectImplementationsToTypesClosing(typeof(IObjectConverter<,>));
                     });
        }
    }
}
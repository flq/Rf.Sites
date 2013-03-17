using System.Web;
using Bottles;
using NHibernate;
using Rf.Sites.Features.Searching;
using Rf.Sites.Frame.CloudStorageSupport;
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

            DbAccessBits();

            ForSingletonOf<IActivator>().Use<JobsBootstrapper>();

            #if DEBUG
              For<ICloudStorageFacade>().Use<FileSystemFacade>();
            #else
              For<ICloudStorageFacade>().Use<DropboxFacade>();
            #endif

            Scan(s =>
            {
                s.TheCallingAssembly();
                s.AddAllTypesOf<ISearchPlugin>();
                s.AddAllTypesOf<IJob>();
            });
        }

        private void DbAccessBits()
        {
            ForSingletonOf<ISessionFactory>().Use(new SessionFactoryMaker().CreateFactory);

            For<ISession>()
                .HybridHttpOrThreadLocalScoped()
                .Use(ctx => ctx.GetInstance<ISessionFactory>().OpenSession());

            For(typeof (IRepository<>)).Use(typeof (Repository<>));
        }
    }
}
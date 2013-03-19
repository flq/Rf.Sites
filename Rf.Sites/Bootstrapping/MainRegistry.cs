using System;
using System.Web;
using Bottles;
using DropNet;
using FubuCore.Configuration;
using NHibernate;
using Rf.Sites.Features.Searching;
using Rf.Sites.Frame.CloudStorageSupport;
using Rf.Sites.Frame.Persistence;
using Rf.Sites.Frame.SiteInfrastructure;
using StructureMap;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;
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
            For<DropNetClient>().Use(ctx =>
            {
                var cfg = ctx.GetInstance<DropboxSettings>();
                var dropnet = new DropNetClient(cfg.ApiKey, cfg.AppSecret, cfg.UserToken, cfg.UserSecret)
                {
                    UseSandbox = true
                };
                return dropnet;
            });
            For<ICloudStorageFacade>().Use<DropboxFacade>();
#endif

            Scan(s =>
            {
                s.TheCallingAssembly();
                s.AddAllTypesOf<ISearchPlugin>();
                s.AddAllTypesOf<IJob>();
                s.Convention<WireUpSettings>();
            });
        }

        private void DbAccessBits()
        {
            ForSingletonOf<ISessionFactory>().Use(new SessionFactoryMaker().CreateFactory);

            For<ISession>()
                .HybridHttpOrThreadLocalScoped()
                .Use(ctx => ctx.GetInstance<ISessionFactory>().OpenSession());

            For(typeof(IRepository<>)).Use(typeof(Repository<>));
        }

        public class WireUpSettings : IRegistrationConvention
        {
            public void Process(Type type, Registry registry)
            {
                if (!type.IsAbstract && type.Name.EndsWith("Settings"))
                {
                    registry.For(type)
                        .LifecycleIs(InstanceScope.Singleton)
                        .Use(ctx => ctx.GetInstance<ISettingsProvider>().SettingsFor(type));
                }
            }
        }
    }


}
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;
using NHibernate.Dialect;
using NHibernate.Driver;
using Rf.Sites.Frame;
using Rf.Sites.Frame.Persistence;

namespace Rf.Sites.Test.Frame
{
    public class SessionfactoryMakerForTest : SessionFactoryMaker
    {
        protected override IPersistenceConfigurer DbConfig()
        {
            return SQLiteConfiguration.Standard
              .Dialect(typeof(SQLiteDialect).AssemblyQualifiedName)
              .Driver(typeof(SQLite20Driver).AssemblyQualifiedName)
              .InMemory()
              .Raw(Environment.ReleaseConnections, "on_close");
        }

        protected override void AdditionalModelConfig(AutoPersistenceModel model)
        {
            //No specials in testing
        }

        protected override void InspectConfig(Configuration cfg)
        {
            base.InspectConfig(cfg);
            cfg.SetProperty("show_sql", "true");
        }

        protected override System.Type currentSessionContextType()
        {
            return typeof(SessionContextForTest);
        }

        public class SessionContextForTest : ICurrentSessionContext
        {
            private readonly ISessionFactory factory;
            private ISession session;

            public SessionContextForTest(ISessionFactory factory)
            {
                this.factory = factory;
            }

            public ISession CurrentSession()
            {
                return session ?? (session = factory.OpenSession());
            }
        }
    }
}
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg.Db;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using Rf.Sites.Domain.Frame;

namespace Rf.Sites.Tests.Frame
{
  public class InMemorySessions : SessionFactoryMaker
  {
    protected override IPersistenceConfigurer dbConfig()
    {
      return SQLiteConfiguration.Standard
        .Dialect(typeof (SQLiteDialect).AssemblyQualifiedName)
        .Driver(typeof (SQLite20Driver).AssemblyQualifiedName)
        .InMemory()
        .Raw(Environment.ReleaseConnections, "on_close");
    }

    protected override void additionalModelConfig(AutoPersistenceModel model)
    {
      //No specials in testing
    }

    protected override void inspectConfig(Configuration cfg)
    {
      base.inspectConfig(cfg);
      cfg.SetProperty("show_sql", "true");
    }

    protected override System.Type currentSessionContextType()
    {
      return typeof (TestNHSessionContext);
    }
  }
}
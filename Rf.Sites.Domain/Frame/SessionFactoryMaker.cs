using System;
using System.Data;
using System.IO;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NHibernate.Validator.Cfg;
using NHibernate.Validator.Engine;

namespace Rf.Sites.Domain.Frame
{
  public class SessionFactoryMaker
  {
    private static Configuration config;
    private ISessionFactory factory;

    public ISessionFactory CreateFactory()
    {
      factory = Fluently.Configure()
        .Database(dbConfig())
        .Mappings(m => m.AutoMappings.Add(modelConfig()))
        .ExposeConfiguration(inspectConfig)
        .BuildSessionFactory();
      return factory;
    }

    public void DropAndRecreateSchema(TextWriter w, IDbConnection con)
    {
      initializeValidationFramework(config);
      var se = new SchemaExport(config);
      se.Execute(false, true, false, con, w);
    }

    protected virtual IPersistenceConfigurer dbConfig()
    {
      //return MsSqlConfiguration.MsSql2005
      //  .ConnectionString(b => b.FromConnectionStringWithKey("Rf.Sites"));
      return MsSqlConfiguration.MsSql2005
        .ConnectionString(b => b.Server(".\\SQLEXPRESS")
                            .Database("RfSite")
                            .TrustedConnection());
    }

    protected virtual Type currentSessionContextType()
    {
      return typeof(WebNHSessionContext);
    }

    protected virtual void additionalModelConfig(AutoPersistenceModel model)
    {
      model.Override<Content>(a => a.Map(c => c.Body).CustomSqlType("nvarchar(MAX)"));
    }

    private AutoPersistenceModel modelConfig()
    {
      var model = AutoMap.AssemblyOf<Content>(t => t.BaseType == typeof (Entity))
        .IgnoreBase(typeof (Entity))
        .Override<Content>(a => a.HasMany(c => c.Comments).Cascade.AllDeleteOrphan());
      additionalModelConfig(model);
      return model;
    }

    protected virtual void inspectConfig(Configuration cfg)
    {
      config = cfg;
      var sessionContextType = currentSessionContextType();
      
      if (sessionContextType != null)
        config.SetProperty("current_session_context_class", sessionContextType.AssemblyQualifiedName);
      initializeValidationFramework(cfg);
    }

    private static void initializeValidationFramework(Configuration cfg)
    {
      var engine = new ValidatorEngine();
      engine.Configure(new NHVConfigurationBase());
      ValidatorInitializer.Initialize(cfg, engine);
    }
  }
}
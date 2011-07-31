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
using Rf.Sites.Entities;
using FluentConfiguration = NHibernate.Validator.Cfg.Loquacious.FluentConfiguration;
using System.Linq;
using NHibernate.Validator.Cfg.Loquacious;

namespace Rf.Sites.Frame
{
    public class SessionFactoryMaker
    {
        private static Configuration config;
        private static ValidatorEngine engine;
        private ISessionFactory factory;

        public ISessionFactory CreateFactory()
        {
            factory = Fluently.Configure()
              .Database(dbConfig())
              .Mappings(m => m.AutoMappings.Add(ModelConfig()))
              .ExposeConfiguration(inspectConfig)
              .BuildSessionFactory();
            return factory;
        }

        public void DropAndRecreateSchema(TextWriter w, IDbConnection con)
        {
            CreateValidationEngine();
            config.Initialize(engine);
            var se = new SchemaExport(config);
            se.Execute(false, true, false, con, w);
        }

        public ValidatorEngine GetValidationEngine()
        {
            return engine;
        }

        protected virtual IPersistenceConfigurer dbConfig()
        {
            return MsSqlConfiguration.MsSql2008
              .ConnectionString(b => b.FromConnectionStringWithKey("RfSite"));
        }

        protected virtual Type currentSessionContextType()
        {
            return typeof(WebNhSessionContext);
        }

        protected virtual void additionalModelConfig(AutoPersistenceModel model)
        {
            model.Override<Content>(a => a.Map(c => c.Body).CustomSqlType("nvarchar(MAX)"));
        }

        private AutoPersistenceModel ModelConfig()
        {
            var model = AutoMap.Assemblies(typeof(Content).Assembly)
              .Where(t => t.BaseType == typeof(Entity))
              .IgnoreBase(typeof(Entity))
              .Override<Content>(a => a.HasMany(c => c.Attachments).Cascade.AllDeleteOrphan());
            additionalModelConfig(model);
            return model;
        }

        protected virtual void inspectConfig(Configuration cfg)
        {
            config = cfg;
            var sessionContextType = currentSessionContextType();

            if (sessionContextType != null)
                config.SetProperty("current_session_context_class", sessionContextType.AssemblyQualifiedName);
            var engine = CreateValidationEngine();
            cfg.Initialize(engine);
        }

        private static ValidatorEngine CreateValidationEngine()
        {
            var valCfg = new FluentConfiguration();
            valCfg.Register(
                typeof(Content).Assembly.GetTypes().Where(t => t.BaseType == typeof(Entity))
                .ValidationDefinitions())
                .SetDefaultValidatorMode(ValidatorMode.UseAttribute);

            engine = new ValidatorEngine();
            engine.Configure(valCfg);
            return engine;
        }
    }
}
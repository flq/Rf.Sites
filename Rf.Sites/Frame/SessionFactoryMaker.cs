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
        private static Configuration _config;
        private static ValidatorEngine _engine;
        private ISessionFactory _factory;

        public ISessionFactory CreateFactory()
        {
            _factory = Fluently.Configure()
              .Database(DbConfig())
              .Mappings(m => m.AutoMappings.Add(ModelConfig()))
              .ExposeConfiguration(InspectConfig)
              .BuildSessionFactory();
            return _factory;
        }

        public void DropAndRecreateSchema(TextWriter w, IDbConnection con)
        {
            CreateValidationEngine();
            _config.Initialize(_engine);
            var se = new SchemaExport(_config);
            se.Execute(false, true, false, con, w);
        }

        public ValidatorEngine GetValidationEngine()
        {
            return _engine;
        }

        protected virtual IPersistenceConfigurer DbConfig()
        {
            return MsSqlConfiguration.MsSql2008
              .ConnectionString(b => b.FromConnectionStringWithKey("RfSite"));
        }

        protected virtual Type currentSessionContextType()
        {
            return typeof(WebNhSessionContext);
        }

        protected virtual void AdditionalModelConfig(AutoPersistenceModel model)
        {
            model.Override<Content>(a => a.Map(c => c.Body).CustomSqlType("nvarchar(MAX)"));
        }

        private AutoPersistenceModel ModelConfig()
        {
            var model = AutoMap.Assemblies(typeof(Content).Assembly)
              .Where(t => t.BaseType == typeof(Entity))
              .IgnoreBase(typeof(Entity))
              .Override<Content>(a => a.HasMany(c => c.Attachments).Cascade.AllDeleteOrphan());
            AdditionalModelConfig(model);
            return model;
        }

        protected virtual void InspectConfig(Configuration cfg)
        {
            _config = cfg;
            var sessionContextType = currentSessionContextType();

            if (sessionContextType != null)
                _config.SetProperty("current_session_context_class", sessionContextType.AssemblyQualifiedName);
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

            _engine = new ValidatorEngine();
            _engine.Configure(valCfg);
            return _engine;
        }
    }
}
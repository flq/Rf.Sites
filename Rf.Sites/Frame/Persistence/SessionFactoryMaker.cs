using System;
using System.Data;
using System.IO;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using Rf.Sites.Entities;

namespace Rf.Sites.Frame.Persistence
{
    public class SessionFactoryMaker
    {
        private static Configuration _config;
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
            var se = new SchemaExport(_config);
            se.Execute(false, true, false, con, w);
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
            // 29.12.2011 - Who would have thought that apparently only know I managed to surpass 4000 chars.
            // seems crazy but anyways, here's a fix for NH cutting off my text
            model.Override<Content>(
                a => a.Map(c => c.Body).Length(4001).CustomSqlType("nvarchar(MAX)"));
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
        }
    }
}
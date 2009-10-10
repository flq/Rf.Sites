using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NHibernate.Validator.Cfg;
using NHibernate.Validator.Engine;
using StructureMap.Attributes;
using StructureMap.Configuration.DSL;

namespace Rf.Sites.Domain.Frame
{
  public class DomainRegistry : Registry
  {
    public DomainRegistry()
    {
      
      ForRequestedType<ISessionFactory>()
        .CacheBy(InstanceScope.Singleton)
        .TheDefault.Is.ConstructedBy(()=>new SessionFactoryMaker().CreateFactory());

      ForRequestedType<ISession>()
        .CacheBy(InstanceScope.Hybrid)
        .TheDefault.Is.ConstructedBy(
        ctx => ctx.GetInstance<ISessionFactory>().OpenSession());

      ForRequestedType<IStatelessSession>()
        .CacheBy(InstanceScope.PerRequest)
        .TheDefault.Is.ConstructedBy(
        ctx => ctx.GetInstance<ISessionFactory>().OpenStatelessSession());

      ForRequestedType(typeof (IRepository<>))
        .TheDefaultIsConcreteType(typeof (Repository<>));
    }

    
  }
}
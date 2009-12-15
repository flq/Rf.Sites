using NHibernate;
using StructureMap.Attributes;
using StructureMap.Configuration.DSL;

namespace Rf.Sites.Domain.Frame
{
  public class DomainRegistry : Registry
  {
    public DomainRegistry()
    {
      var maker = new SessionFactoryMaker();
      
      ForRequestedType<ISessionFactory>()
        .CacheBy(InstanceScope.Singleton)
        .TheDefault.Is.ConstructedBy(maker.CreateFactory);

      ForRequestedType<IValidator>()
        .CacheBy(InstanceScope.Singleton)
        .TheDefault.Is.ConstructedBy(()=>new NHBasedValidator(maker.GetValidationEngine()));

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
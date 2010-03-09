using System;
using NHibernate;
using StructureMap;
using StructureMap.Attributes;
using StructureMap.Configuration.DSL;
using StructureMap.Configuration.DSL.Expressions;
using StructureMap.Pipeline;

namespace Rf.Sites.Domain.Frame
{
  public class DomainRegistry : Registry
  {
    public DomainRegistry()
    {
      var maker = new SessionFactoryMaker();
      ForSingletonOf<ISessionFactory>().Use(maker.CreateFactory);

      For<IValidator>()
        .Use(()=>new NHBasedValidator(maker.GetValidationEngine()));

      For<ISession>()
        .HybridHttpOrThreadLocalScoped()
        .Use(ctx => ctx.GetInstance<ISessionFactory>().OpenSession());

      For<IStatelessSession>()
        .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
        .Use(ctx => ctx.GetInstance<ISessionFactory>().OpenStatelessSession());

      For(typeof (IRepository<>)).Use(typeof (Repository<>));
    }
  }

  public static class StrMapExtensions
  {
    public static void Use<T>(this CreatePluginFamilyExpression<T> expression, Func<T> func)
    {
      expression.Use(c => func());
    }
  }
}
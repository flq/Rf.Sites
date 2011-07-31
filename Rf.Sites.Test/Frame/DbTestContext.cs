using System;
using NHibernate;

namespace Rf.Sites.Test.Frame
{
  public class DbTestContext : IDisposable
  {
    private readonly ISessionFactory factory;
    private readonly EntityMaker maker = new EntityMaker();
    
    public ISession Session
    {
      get { return factory.GetCurrentSession(); }
    }

    public ISessionFactory Factory
    {
      get { return factory; }
    }

    public EntityMaker Maker
    {
      get { return maker; }
    }

    public DbTestContext()
    {
      var factoryMaker = new SessionfactoryMakerForTest();
      factory = factoryMaker.CreateFactory();
      factoryMaker.DropAndRecreateSchema(Console.Out, Session.Connection);
    }

    public void Dispose()
    {
      Session.Dispose();
    }
  }
}
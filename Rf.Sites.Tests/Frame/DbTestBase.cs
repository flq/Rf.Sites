using System;
using System.IO;
using System.Text;
using NHibernate;

namespace Rf.Sites.Tests.Frame
{
  public class DbTestBase : IDisposable
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

    public DbTestBase()
    {
      var memorySessions = new InMemorySessions();
      factory = memorySessions.CreateFactory();

      var sb = new StringBuilder();
      
      memorySessions.DropAndRecreateSchema(Console.Out, Session.Connection);
    }

    public void Dispose()
    {
      Session.Dispose();
    }
  }
}
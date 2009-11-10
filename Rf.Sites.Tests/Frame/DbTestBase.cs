using System;
using System.IO;
using System.Text;
using NHibernate;

namespace Rf.Sites.Tests.Frame
{
  public class DbTestBase : IDisposable
  {
    protected readonly ISessionFactory factory;
    protected readonly EntityMaker maker = new EntityMaker();
    
    public ISession Session
    {
      get
      {
        return factory.GetCurrentSession();
      }
    }

    public DbTestBase()
    {
      var memorySessions = new InMemorySessions();
      factory = memorySessions.CreateFactory();

      var sb = new StringBuilder();
      
      memorySessions.DropAndRecreateSchema(new StringWriter(sb), Session.Connection);
    }

    public void Dispose()
    {
      Session.Dispose();
    }
  }
}
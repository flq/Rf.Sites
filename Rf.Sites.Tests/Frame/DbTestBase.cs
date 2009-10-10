using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using NHibernate;

namespace Rf.Sites.Tests
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
      var maker = new InMemorySessions();
      factory = maker.CreateFactory();

      var sb = new StringBuilder();
      
      maker.DropAndRecreateSchema(new StringWriter(sb), Session.Connection);
      //Debug.WriteLine(sb.ToString());
    }

    public void Dispose()
    {
      Session.Dispose();
    }
  }
}

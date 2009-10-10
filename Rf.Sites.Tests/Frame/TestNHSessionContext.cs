using System;
using NHibernate;
using NHibernate.Context;

namespace Rf.Sites.Tests
{
  public class TestNHSessionContext : ICurrentSessionContext
  {
    private readonly ISessionFactory factory;
    private ISession session;

    public TestNHSessionContext(ISessionFactory factory)
    {
      this.factory = factory;
    }

    public ISession CurrentSession()
    {
      return session ?? (session = factory.OpenSession());
    }
  }
}
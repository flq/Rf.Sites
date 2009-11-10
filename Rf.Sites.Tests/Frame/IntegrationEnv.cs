using System;
using NHibernate;
using Rf.Sites.Frame;
using StructureMap;

namespace Rf.Sites.Tests.Frame
{
  public abstract class IntegrationEnv
  {
    protected Container container;
    protected IContainer nestedContainer;
    private DbTestBase inMemoryDb;

    protected IntegrationEnv()
    {
      container = new Container(ce => ce.AddRegistry<SiteRegistry>());
    }

    public IContainer Container
    {
      get { return nestedContainer ?? container; }
    }

    public DbTestBase InMemoryDB
    {
      get { return inMemoryDb; }
    }

    public void OverloadContainer(Action<ConfigurationExpression> action)
    {
      getNestedContainer();
      nestedContainer.Configure(action);
    }

    public void UseInMemoryDb()
    {
      inMemoryDb = new DbTestBase();
      getNestedContainer();
      // Providing the sessionFactory does not work
      // as the same session must be used throughout
      nestedContainer.Configure(c => c.ForRequestedType<ISession>().TheDefault.Is.ConstructedBy(()=>inMemoryDb.Session));
    }


    public void ReleaseResources()
    {
      if (nestedContainer != null)
        nestedContainer.Dispose();
      nestedContainer = null;
      if (inMemoryDb != null)
        inMemoryDb.Dispose();
      inMemoryDb = null;
    }

    private void getNestedContainer()
    {
      nestedContainer = nestedContainer ?? container.GetNestedContainer();
    }
  }
}
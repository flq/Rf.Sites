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

    protected IntegrationEnv()
    {
      container = new Container(ce => ce.AddRegistry<SiteRegistry>());
    }

    public IContainer Container
    {
      get { return nestedContainer ?? container; }
    }

    public DbTestBase InMemoryDB { get; private set; }

    public void OverloadContainer(Action<ConfigurationExpression> action)
    {
      getNestedContainer();
      nestedContainer.Configure(action);
    }

    public void UseInMemoryDb()
    {
      InMemoryDB = new DbTestBase();
      getNestedContainer();
      // Providing the sessionFactory does not work
      // as the same session must be used throughout
      nestedContainer.Configure(c => c.ForRequestedType<ISession>().TheDefault.Is.ConstructedBy(()=>InMemoryDB.Session));
    }


    public void ReleaseResources()
    {
      if (nestedContainer != null)
        nestedContainer.Dispose();
      nestedContainer = null;
      if (InMemoryDB != null)
        InMemoryDB.Dispose();
      InMemoryDB = null;
    }

    private void getNestedContainer()
    {
      nestedContainer = nestedContainer ?? container.GetNestedContainer();
    }
  }
}
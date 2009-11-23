using System;
using Moq;
using NHibernate;
using Rf.Sites.Frame;
using Rf.Sites.Tests.DataScenarios;
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
      nestedContainer
        .Configure(c =>
                     {
                       c.ForRequestedType<ISession>().TheDefault.Is.ConstructedBy(() => InMemoryDB.Session);
                       c.ForRequestedType<ISessionFactory>().TheDefault.Is.ConstructedBy(() => InMemoryDB.Factory);
                     });
    }

    public ISessionFactory FactoryForStatelessSession()
    {
      var sF = new Mock<ISessionFactory>();
      sF.Setup(f => f.OpenStatelessSession())
        .Returns(InMemoryDB.Factory.OpenStatelessSession(InMemoryDB.Session.Connection));
      return sF.Object;
    }

    public T DataScenario<T>() where T : AbstractDataScenario
    {
      if (InMemoryDB == null)
        throw new InvalidOperationException("Data Scneario currently only supported for in memory DB");
      var s = Container.GetInstance<T>();
      s.ExecuteScenario(InMemoryDB.Session);

      return s;
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
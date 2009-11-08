using System;
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

    public Container Container
    {
      get { return container; }
    }

    public void OverloadContainer(Action<ConfigurationExpression> action)
    {
      nestedContainer = container.GetNestedContainer();
      nestedContainer.Configure(action);
    }

    public void ReleaseResources()
    {
      if (nestedContainer != null)
        nestedContainer.Dispose();
      nestedContainer = null;

    }
  }
}
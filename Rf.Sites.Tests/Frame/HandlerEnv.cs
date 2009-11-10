using System.Web;
using Rf.Sites.Handlers;

namespace Rf.Sites.Tests.Frame
{
  public class HandlerEnv : IntegrationEnv
  {
    public TestHttpContext UsedContext { get; private set; }

    public void ExecuteHandler<T>() where T : IHttpHandler
    {
      var h = Container.GetInstance<T>();
      ExecuteHandler(h);
    }

    public void ExecuteHandler(IHttpHandler handler)
    {
      UsedContext = new TestHttpContext();
      ((ISetMockHttpContext)handler).SetMockContext(UsedContext);
      handler.ProcessRequest(null);
    }
  }
}
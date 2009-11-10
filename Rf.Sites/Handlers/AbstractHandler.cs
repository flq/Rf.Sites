using System;
using System.Web;

namespace Rf.Sites.Handlers
{
  public abstract class AbstractHandler : IHttpHandler, ISetMockHttpContext
  {
    public virtual bool IsReusable { get { return false; } }

    public void ProcessRequest(HttpContext context)
    {
      Context = Context ?? new WebHttpContext(context);
      processRequest();
    }

    protected abstract void processRequest();

    protected IHttpContext Context { get; set; }

    void ISetMockHttpContext.SetMockContext(IHttpContext context)
    {
      Context = context;
    }
  }
}
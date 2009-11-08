using System;
using System.Web;
using StructureMap;

namespace Rf.Sites.Frame
{
  public class HttpHandlerDispatcher : IHttpHandlerFactory
  {
    public IContainer Container { set; private get; }

    public HttpHandlerDispatcher()
    {
      ObjectFactory.BuildUp(this);
    }

    public IHttpHandler GetHandler(HttpContext context, string requestType, string url, string pathTranslated)
    {
      int startIndex = url.StartsWith("/") ? 1 : 0;
      return Container.GetInstance<IHttpHandler>(url.Substring(startIndex).ToLowerInvariant());
    }

    public void ReleaseHandler(IHttpHandler handler)
    {
      // Nothing to do.
    }
  }
}
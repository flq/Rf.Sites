using System;
using System.Web;
using Rf.Sites.Frame;

namespace Rf.Sites.Handlers
{
  [HandlerUrl(Url="unknown")]
  public class Handler404 : IHttpHandler
  {
    public void ProcessRequest(HttpContext context)
    {
      context.Response.StatusCode = 404;
      context.Response.End();
    }

    public bool IsReusable
    {
      get { return true; }
    }
  }
}
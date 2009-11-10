using System;
using System.IO;
using System.Web;

namespace Rf.Sites.Handlers
{
  /// <summary>
  /// Wrapper for the standard <see cref="HttpContext"/>.
  /// </summary>
  public class WebHttpContext : IHttpContext
  {
    private readonly HttpContext context;

    public WebHttpContext(HttpContext context)
    {
      this.context = context;
    }

    public string ResponseContentType
    {
      get { return context.Response.ContentType; }
      set { context.Response.ContentType = value; }
    }

    public TextWriter ResponseOutput
    {
      get { return context.Response.Output; }
    }

    public int ResponseStatusCode
    {
      get { return context.Response.StatusCode; }
      set { context.Response.StatusCode = value; }
    }

    public void EndResponse()
    {
      context.Response.End();
    }
  }
}
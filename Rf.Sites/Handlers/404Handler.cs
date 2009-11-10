using System;
using System.Web;
using Rf.Sites.Frame;

namespace Rf.Sites.Handlers
{
  [HandlerUrl(Url="unknown")]
  public class Handler404 : AbstractHandler
  {

    protected override void processRequest()
    {
      Context.ResponseStatusCode = 404;
      Context.EndResponse();
    }

    public override bool IsReusable
    {
      get { return true; }
    }
  }
}
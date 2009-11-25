using System.Web.Mvc;

namespace Rf.Sites.Actions.Args
{
  public class ChronicsPostArgs
  {
    public string RawRequestId { get; set; }

    public ChronicsPostArgs()
    {
    }

    public ChronicsPostArgs(ControllerContext context)
    {
      if (!context.RequestContext.HttpContext.Request.IsAjaxRequest()) return;
      RawRequestId = context.RequestContext.HttpContext.Request.QueryString["root"];
    }

  }
}
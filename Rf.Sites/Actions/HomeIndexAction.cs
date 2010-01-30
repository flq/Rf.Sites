using System;
using System.Web.Mvc;
using Rf.Sites.Frame;

namespace Rf.Sites.Actions
{
  public class HomeIndexAction : AbstractAction
  {
    private readonly DrupalUrlMap map;
    private readonly string drupalQString;

    public HomeIndexAction(ControllerContext ctx, DrupalUrlMap map)
    {
      this.map = map;
      drupalQString = ctx.RequestContext.HttpContext.Request.QueryString["q"];
    }

    public override ActionResult Execute()
    {
      if (drupalQString == null)
        return redirectTo<ContentIndexAction>();
      var url = map[drupalQString];
      return url != null ? redirectTo(url) : redirectTo<UnknownAction>();
    }
  }
}
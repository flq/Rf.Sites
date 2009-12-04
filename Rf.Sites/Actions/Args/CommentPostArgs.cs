using System;
using System.Web.Mvc;

namespace Rf.Sites.Actions.Args
{
  public class CommentPostArgs
  {
    public CommentPostArgs(ControllerContext ctx)
    {
      var r = ctx.RequestContext.HttpContext.Request;
      Email = r.Form["email"];
      Website = r.Form["website"];
      Comment = r.Form["comment_text"];
    }

    public string Email { get; set; }
    public string Website { get; set; }
    public string Comment { get; set;}
  }
}
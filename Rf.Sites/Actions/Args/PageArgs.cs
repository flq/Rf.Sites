using System.Web.Mvc;
using Rf.Sites.Frame;

namespace Rf.Sites.Actions.Args
{
  public class PageArgs : ActionArgs, IPageArgs
  {
    public PageArgs() { }

    public PageArgs(ControllerContext ctx)
    {
      var page = ctx.GetValue1();
      Page = string.IsNullOrEmpty(page) ? 0 : int.Parse(page);
    }

    protected override object makeObject()
    {
      return makeObject(Page);
    }

    public int Page { get; set; }
  }
}
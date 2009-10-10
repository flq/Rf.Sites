using System;
using System.Web.Mvc;
using Rf.Sites.Frame;

namespace Rf.Sites.Actions.Args
{
  public class YearArgs : ActionArgs, IPageArgs
  {
    public YearArgs() { }

    public YearArgs(ControllerContext ctx)
    {
      Year = ctx.GetValue1().SafeCast(DateTime.Now.Year);
      Page = ctx.GetValue2().SafeCast(0);
    }

    public int Year { get; set; }
    public int Page { get; set; }

    protected override object makeObject()
    {
      return makeObject(Year, Page);
    }
  }
}
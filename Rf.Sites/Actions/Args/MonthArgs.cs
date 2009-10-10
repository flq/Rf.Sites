using System;
using System.Web.Mvc;
using Rf.Sites.Frame;

namespace Rf.Sites.Actions.Args
{
  public class MonthArgs : ActionArgs, IPageArgs
  {
    public MonthArgs(){ }
    public MonthArgs(ControllerContext ctx)
    {
      Year = ctx.GetValue1().SafeCast(DateTime.Now.Year);
      Month = ctx.GetValue2().SafeCast(DateTime.Now.Month);
      Page = ctx.GetValue3().SafeCast(0);
    }

    protected override object makeObject()
    {
      return makeObject(Year, Month, Page);
    }

    public int Year { get; set; }
    public int Month { get; set; }
    public int Page { get; set; }
  }
}
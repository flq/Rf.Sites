using System.Web.Mvc;
using Rf.Sites.Frame;

namespace Rf.Sites.Actions.Args
{
  public class ValueArgs : ActionArgs, IPageArgs
  {
    public string Value { get; set;}
    public int Page { get; set; }

    public ValueArgs() { }

    public ValueArgs(ControllerContext ctx)
    {
      Value = ctx.GetValue1();
      Page = ctx.GetValue2().SafeCast(0);
    }

    protected override object makeObject()
    {
      return makeObject(Value, Page);
    }
  }
}
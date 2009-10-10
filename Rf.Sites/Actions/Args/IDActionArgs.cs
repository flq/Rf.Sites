using System.Web.Mvc;
using Rf.Sites.Frame;

namespace Rf.Sites.Actions.Args
{
  public class IDActionArgs : ActionArgs
  {
    public int Id { get; set; }

    public IDActionArgs() { }

    public IDActionArgs(ControllerContext ctx)
    {
      Id = ctx.GetValue1().SafeCast(1);
    }

    protected override object makeObject()
    {
      return makeObject(Id);
    }
  }
}
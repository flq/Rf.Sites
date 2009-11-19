using System.Web.Mvc;
using NHibernate.Cache;
using Rf.Sites.Actions.Args;
using Rf.Sites.Frame;
using Rf.Sites.Models;

namespace Rf.Sites.Actions
{
  public class ChronicsIndexAction : AbstractAction
  {
    private readonly ChronicsPostArgs args;

    public ChronicsIndexAction(ChronicsPostArgs args, ICache cache)
    {
      this.args = args;
    }

    public override ActionResult Execute()
    {
      var ret = new object[]
                  {
                    new ChronicsNode() { hasChildren = true, id = "1", text = "A"},
                    new ChronicsNode() { hasChildren = true, id = "2", text = "B"}
                  };

      return new JsonResult() {Data = ret};
    }
  }
}
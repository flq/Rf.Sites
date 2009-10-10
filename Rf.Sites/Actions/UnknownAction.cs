using System.Web.Mvc;
using Rf.Sites.Frame;

namespace Rf.Sites.Actions
{
  public class UnknownAction : AbstractAction
  {
    public override ActionResult Execute()
    {
      return createResult(null, "404");
    }
  }
}
using System;
using System.Web.Mvc;
using Rf.Sites.Frame;

namespace Rf.Sites.Actions
{
  public class HomeIndexAction : AbstractAction
  {
    public override ActionResult Execute()
    {
      return redirectTo<ContentIndexAction>();
    }
  }
}
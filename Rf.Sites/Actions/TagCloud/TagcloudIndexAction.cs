using Rf.Sites.Frame;

namespace Rf.Sites.Actions
{
  public class TagcloudIndexAction : AbstractAction
  {
    //public TagcloudIndexAction(ISes)
    //{
      
    //}

    public override System.Web.Mvc.ActionResult Execute()
    {
      return createPartialView(null);
    }
  }
}
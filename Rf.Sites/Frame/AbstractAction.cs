using System.Web.Mvc;

namespace Rf.Sites.Frame
{
  public abstract class AbstractAction : IAction
  {
    public virtual ActionResult Execute()
    {
      return createResult();
    }

    protected ViewResult createResult()
    {
      return createResult(null, null);
    }

    protected ViewResult createResult(object model)
    {
      return createResult(model, null);
    }

    protected ViewResult createResult(object model, string viewName)
    {
      var result = new ViewResult();
      if (model != null)
        result.ViewData.Model = model;
      if (viewName != null)
        result.ViewName = viewName;
      return result;
    }

    protected ActionResult redirectTo<T>() where T : IAction
    {
      var tokens = typeof (T).Name.PasCalCaseTokenization();
      return new RedirectResult(string.Format("/{0}/{1}", tokens[0], tokens[1]));
    }
  }
}
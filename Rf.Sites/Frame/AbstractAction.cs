using System.Web.Mvc;
using StructureMap;

namespace Rf.Sites.Frame
{
  public abstract class AbstractAction : IAction
  {
    public virtual ActionResult Execute()
    {
      return createResult();
    }

    public IContainer Container { get; set; }
    public Environment Environment { get; set; }

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
      return (ViewResult)createResult(result, model, viewName);
    }

    protected ViewResultBase createResult(ViewResultBase viewResult, object model, string viewName)
    {
      if (model != null)
        viewResult.ViewData.Model = model;
      if (viewName != null)
        viewResult.ViewName = viewName;
      return viewResult;
    }

    protected ViewResultBase createPartialView(object model)
    {
      var result = new PartialViewResult();
      return createResult(result, model, null);
    }

    protected ActionResult redirectTo<T>() where T : IAction
    {
      var tokens = typeof (T).Name.PasCalCaseTokenization();
      return new RedirectResult(string.Format("/{0}/{1}", tokens[0], tokens[1]));
    }
  }
}
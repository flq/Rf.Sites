using System.Web.Mvc;

namespace Rf.Sites.Frame
{
  /// <summary>
  /// This controller just introduces a new action invoker to the controller
  /// pipeline. The implementation for Rf.Sites is the 
  /// <see cref="UrlToClassActionInvoker"/>.
  /// </summary>
  public class ActionDispatcher : Controller
  {
    public ActionDispatcher(IActionInvoker invoker)
    {
      ActionInvoker = invoker;
    }
  }
}
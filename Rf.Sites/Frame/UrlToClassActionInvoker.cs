using System;
using System.Web.Mvc;
using Rf.Sites.Domain.Frame;
using StructureMap;

namespace Rf.Sites.Frame
{
  /// <summary>
  /// Map the Url parts to an implementation of <see cref="IAction"/>.
  /// </summary>
  public class UrlToClassActionInvoker : IActionInvoker
  {
    private readonly IContainer container;

    public UrlToClassActionInvoker(IContainer container)
    {
      this.container = container;
    }

    public bool InvokeAction(ControllerContext controllerContext, string actionName)
    {
      string instanceKey = formTheName(controllerContext);

      try
      {
        var action = container.With(controllerContext)
          .GetInstance<IAction>(instanceKey);
        var result = action.Execute();
        result.ExecuteResult(controllerContext);
      }
      catch (Exception x)
      {
        var log = container.GetInstance<ILog>();
        log.Error(x);
      }
      return true;
    }

    private string formTheName(ControllerContext context)
    {
      var values = context.RouteData.Values;
      var name = values["controller"] + values["action"].ToString();
      return name.ToLowerInvariant();
    }
  }
}
using System.Web.Mvc;
using System.Web.Routing;
using StructureMap;

namespace Rf.Sites.Frame
{
  public class ControllerFactory : DefaultControllerFactory
  {
    public override IController CreateController(
      RequestContext requestContext,
      string controllerName)
    {
      return ObjectFactory.GetInstance<IController>();
    }
  }
}
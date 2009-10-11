using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;
using Rf.Sites.Frame;
using StructureMap;

namespace Rf.Sites.Tests.Frame
{
  public class ActionEnv
  {
    private readonly Container container;

    public ActionEnv()
    {
      container =
        new Container(
          ce =>
            {
              ce.AddRegistry<SiteRegistry>();
              ce.ForRequestedType<ControllerContext>()
                .TheDefault.Is.ConstructedBy(getControllerContext);
            });
    }

    private ControllerContext getControllerContext()
    {
      var request = new Mock<HttpRequestBase>();
      request.Setup(r => r.HttpMethod).Returns("GET");
      var mockHttpContext = new Mock<HttpContextBase>();
      mockHttpContext.Setup(c => c.Request).Returns(request.Object);
      var controllerContext = new ControllerContext(mockHttpContext.Object
      , new RouteData(), new Mock<ControllerBase>().Object);
      return controllerContext;
    }

    public IAction GetAction(string actionKey)
    {
      return container.GetInstance<IAction>(actionKey);
    }
  }
}
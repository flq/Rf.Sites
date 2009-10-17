using System.Web.Mvc;
using Rf.Sites.Frame;
using StructureMap;

namespace Rf.Sites.Tests.Frame
{
  public class ActionEnv
  {
    private readonly Container container;

    public ActionEnv()
    {
      ControllerCtxMock = TestHelpExtensions.StartControllerContextMock();

      container =
        new Container(
          ce =>
            {
              ce.AddRegistry<SiteRegistry>();
              ce.ForRequestedType<ControllerContext>()
                .TheDefault.Is.ConstructedBy(() => ControllerCtxMock.ControllerContext);
            });
    }

    public ControllerCtxMock ControllerCtxMock { get; private set; }

    public Container Container
    {
      get { return container; }
    }

    public IAction GetAction(string actionKey)
    {
      return Container.GetInstance<IAction>(actionKey);
    }

    public IAction GetAction<T>() where T : IAction
    {
      return Container.GetInstance<IAction>(typeof(T).Name.ToLowerInvariant());
    }

    public IAction GetAction(AbstractAction action)
    {
      action.Container = Container;
      action.Environment = Container.GetInstance<Environment>();
      return action;
    }
  }
}
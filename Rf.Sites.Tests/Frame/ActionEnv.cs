using System;
using System.Web.Mvc;
using Rf.Sites.Frame;
using Environment=Rf.Sites.Frame.Environment;

namespace Rf.Sites.Tests.Frame
{

  public class ActionEnv : IntegrationEnv
  {
    public ActionEnv()
    {
      ControllerCtxMock = TestHelpExtensions.StartControllerContextMock();
      container.Configure(ce => ce.For<ControllerContext>()
                                  .TheDefault.Is.ConstructedBy(() => ControllerCtxMock.ControllerContext));
    }

    public ControllerCtxMock ControllerCtxMock { get; private set; }

    public IAction GetAction(string actionKey)
    {
      return Container.GetInstance<IAction>(actionKey);
    }

    public IAction GetAction<T,U>(U value) where T : IAction
    {
      return Container.With(value).GetInstance<IAction>(getActionName<T>());
    }

    public IAction GetAction<T>() where T : IAction
    {
      return Container.GetInstance<IAction>(getActionName<T>());
    }

    private static string getActionName<T>()
    {
      return typeof(T).Name.ToLowerInvariant().Replace("action", "");
    }

    public IAction GetAction(AbstractAction action)
    {
      action.Container = Container;
      action.Environment = Container.GetInstance<Environment>();
      return action;
    }
  }
}
using System.Web.Mvc;

namespace Rf.Sites.Tests.Frame
{
  public static class TestHelpExtensions
  {
    public static T GetModelFromAction<T>(this ActionResult actionResult)
    {
      return (T) ((ViewResultBase) actionResult).ViewData.Model;
    }
  }
}
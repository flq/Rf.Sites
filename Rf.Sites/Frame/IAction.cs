using System.Web.Mvc;

namespace Rf.Sites.Frame
{
  /// <summary>
  /// Describes a single action towards a web request.
  /// There is a convenience base class, <see cref="AbstractAction"/>.
  /// </summary>
  public interface IAction
  {
    ActionResult Execute();
  }
}
using System.Web.Mvc;
using Rf.Sites.Models;

namespace Rf.Sites.Frame
{
  public abstract class AbstractFeed : AbstractAction
  {
    public override sealed ActionResult Execute()
    {
      var feedResponse = Container.With(produceFragments())
        .GetInstance<ContentToFeedResponse>();
      return new CustomActionResult(feedResponse);
    }

    protected abstract ContentFragments produceFragments();
  }
}
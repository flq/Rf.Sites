using System.Linq;
using System.Web.Mvc;
using Rf.Sites.Domain;
using Rf.Sites.Domain.Frame;
using Rf.Sites.Frame;
using Rf.Sites.Queries;

namespace Rf.Sites.Actions
{
  public class FutureIndexAction : AbstractAction
  {
    private readonly IRepository<Content> repository;

    public FutureIndexAction(IRepository<Content> repository)
    {
      this.repository = repository;
    }

    public override ActionResult Execute()
    {
      var l = new FutureContentQuery()
          .Query(repository)
          .Take(10)
          .ToList();

      return createPartialView(l.ToList());
    }
  }
}
using System.Web.Mvc;
using Rf.Sites.Actions.Args;
using Rf.Sites.Domain;
using Rf.Sites.Domain.Frame;
using Rf.Sites.Frame;
using Rf.Sites.Models;

namespace Rf.Sites.Actions
{
  public class ContentEntryAction : AbstractAction
  {
    private readonly IDActionArgs args;
    private readonly IRepository<Content> repository;

    public ContentEntryAction(IDActionArgs args, IRepository<Content> repository)
    {
      this.args = args;
      this.repository = repository;
    }

    public override ActionResult Execute()
    {
      var content = repository[args.Id];
      return createResult(content != null ? new ContentViewModel(content) : null);
    }
  }
}
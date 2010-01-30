using System.Web.Mvc;
using Rf.Sites.Actions.Args;
using Rf.Sites.Domain;
using Rf.Sites.Domain.Frame;
using Rf.Sites.Frame;
using Rf.Sites.Models;
using Rf.Sites.Models.Extender;

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
      if (args.Id == -1)
        return redirectTo<UnknownAction>();
      var content = repository[args.Id];
      return content != null
               ? createResult(Container.With(content).GetInstance<ContentViewModel>())
               : redirectTo<UnknownAction>();
    }
  }
}
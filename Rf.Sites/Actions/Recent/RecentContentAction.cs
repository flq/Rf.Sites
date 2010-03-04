using System;
using System.Linq;
using System.Web.Mvc;
using Rf.Sites.Domain;
using Rf.Sites.Domain.Frame;
using Rf.Sites.Frame;
using Rf.Sites.Models;

namespace Rf.Sites.Actions.Recent
{
  public class RecentContentAction : AbstractAction
  {
    private readonly IRepository<Content> repository;

    public RecentContentAction(IRepository<Content> repository)
    {
      this.repository = repository;
    }

    public override ActionResult Execute()
    {
      var l =
        (from c in repository
         orderby c.Created descending 
         select new ContentFragmentViewModel(c.Id, c.Title, c.Created, "null"))
          .Take(10)
          .ToList();

      return createPartialView(
        new ContentFragmentList(l, 
          new Uri(Environment.ApplicationBaseUrl, FrameUtilities.RelativeUrlToAction<ContentEntryAction>())));
    }
  }
}
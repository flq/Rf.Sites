  using System;
using System.Web.Mvc;
using NHibernate;
using Rf.Sites.Frame;
  using Rf.Sites.Queries;

namespace Rf.Sites.Actions.Recent
{
  public class RecentCommentsAction : AbstractAction
  {
    private readonly ISessionFactory factory;

    public RecentCommentsAction(ISessionFactory factory)
    {
      this.factory = factory;
    }

    public override ActionResult Execute()
    {
      CommentList cl;

      using (var s = factory.OpenStatelessSession())
      {
        var q = new RecentCommentsQuery().Query(s); 
        cl = new CommentList(
          q.List(),
          new Uri(Environment.ApplicationBaseUrl, FrameUtilities.RelativeUrlToAction<ContentEntryAction>()));
      }

      return createPartialView(cl);
    }
  }
}
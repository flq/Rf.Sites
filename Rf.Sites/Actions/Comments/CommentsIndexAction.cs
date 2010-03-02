using System;
using System.Web.Mvc;
using NHibernate;
using Rf.Sites.Frame;

namespace Rf.Sites.Actions.Comments
{
  public class CommentsIndexAction : AbstractAction
  {
    private readonly ISessionFactory factory;

    public CommentsIndexAction(ISessionFactory factory)
    {
      this.factory = factory;
    }

    public override ActionResult Execute()
    {
      CommentList cl;

      using (var s = factory.OpenStatelessSession())
      {
        var q = s.CreateQuery(
          @"select cnt.Id, c.CommenterName, c.Created from 
            Content cnt join cnt.Comments c
            where c.AwaitsModeration = false
            order by c.Created desc");
        q.SetMaxResults(10);
        cl = new CommentList(
          q.List(),
          new Uri(Environment.ApplicationBaseUrl, FrameUtilities.RelativeUrlToAction<ContentEntryAction>()));

      }

      return createPartialView(cl);
    }
  }
}
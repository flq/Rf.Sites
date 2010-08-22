using System;
using System.Web.Mvc;
using NHibernate;
using Rf.Sites.Frame;
using Rf.Sites.Queries;

namespace Rf.Sites.Actions.TagCloud
{
  public class TagcloudIndexAction : AbstractAction
  {
    private readonly ISessionFactory sessionFactory;

    public TagcloudIndexAction(ISessionFactory sessionFactory)
    {
      this.sessionFactory = sessionFactory;
    }

    public override ActionResult Execute()
    {
      TagList tl;

      using (var s = sessionFactory.OpenStatelessSession())
      {
        var q = new WeightedTagsQuery(Environment.TagsToIgnoreAsArray).Query(s);
        tl = new TagList(
          q.List(), 
          new Uri(Environment.ApplicationBaseUrl, FrameUtilities.RelativeUrlToAction<ContentTagAction>()));
      }

      tl.Segment(Environment.TagcloudSegments);

      return createPartialView(tl);
    }
  }
}
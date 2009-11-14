using System;
using System.Web.Mvc;
using NHibernate;
using Rf.Sites.Frame;

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

      TagList tl = null;

      using (var s = sessionFactory.OpenStatelessSession())
      {
        var q = s.CreateQuery(
          @"select tag.Name, count(cnt.Id) from 
            Tag tag join tag.RelatedContent cnt
            group by tag.Name");
        
        tl = new TagList(
          q.List(), 
          new Uri(Environment.AbsoluteBaseUrl, FrameUtilities.RelativeUrlToAction<ContentTagAction>()));

      }

      tl.Segment(Environment.TagcloudSegments);

      return createPartialView(tl);
    }
  }
}
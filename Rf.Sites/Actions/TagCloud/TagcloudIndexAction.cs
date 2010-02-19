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
      TagList tl;

      using (var s = sessionFactory.OpenStatelessSession())
      {
        var q = s.CreateQuery(
          @"select tag.Name, count(cnt.Id) from 
            Tag tag join tag.RelatedContent cnt
            where tag.Name not in (:taglist)
            group by tag.Name");
        q.SetParameterList("taglist", Environment.TagsToIgnoreAsArray);
        
        tl = new TagList(
          q.List(), 
          new Uri(Environment.ApplicationBaseUrl, FrameUtilities.RelativeUrlToAction<ContentTagAction>()));

      }

      tl.Segment(Environment.TagcloudSegments);

      return createPartialView(tl);
    }
  }
}
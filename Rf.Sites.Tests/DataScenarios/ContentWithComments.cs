using System;
using System.Collections.Generic;
using NHibernate;
using Rf.Sites.Domain;

namespace Rf.Sites.Tests.DataScenarios
{
  public class ContentWithComments : AbstractDataScenario
  {
    public List<Content> Contents { get; private set; }

    protected override void inflateScenario(ISession session)
    {
      Contents = new List<Content>();

      var cnt = entityMaker.CreateContent("A");
      cnt.Created = DateTime.Now.Subtract(TimeSpan.FromDays(1));
      Contents.Add(cnt);

      cnt = entityMaker.CreateContent("B");
      cnt.AddComment(entityMaker.CreateComment());
      cnt.AddComment(entityMaker.CreateComment());
      cnt.AddComment(entityMaker.CreateComment());
      cnt.Created = DateTime.Now.Subtract(TimeSpan.FromDays(60));
      Contents.Add(cnt);

      cnt = entityMaker.CreateContent("C");
      cnt.Created = new DateTime(2005, 5, 5);
      var comment = entityMaker.CreateComment();
      comment.Created = new DateTime(2006, 6, 6);
      cnt.AddComment(comment);
      Contents.Add(cnt);

      foreach (var c in Contents)
        session.Save(c);
    }
  }
}